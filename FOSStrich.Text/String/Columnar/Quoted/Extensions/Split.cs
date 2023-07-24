namespace FOSStrich.Text;

using System.Text;

public static partial class StringQuotedExtensions
{
    /// <summary>
    /// Splits a row (sequence of chars) representing delimited fields (separated by Delimiter) while allowing for instances of the
    /// Delimiter to occur within individual fields.  Such fields must be quoted (surrounded by Quote) to allow for this behavior.
    /// A NewRow signal outside of Quotes will cause an exception because multiple rows are not allowed for this method.
    /// Escaping takes precedence over all other evaluation logic. Only individual characters can be Escaped.
    /// </summary>
    /// <example>
    /// <code>
    /// string row = "a,b,c";
    /// string[] fields = row.SplitQuotedRow(StringQuotedSignals.Csv);
    /// 
    /// foreach(string field in fields);
    ///     Console.WriteLine(field);
    /// </code>
    /// Console Output:<br/>
    /// a<br/>
    /// b<br/>
    /// c<br/>
    /// <code>
    /// StringQuotedSignals signals = new StringQuotedSignals(",", "'", Environment.NewLine, null);
    /// string row = "'a,a',b,c";
    /// string[] fields = row.SplitQuotedRow(signals);
    /// 
    /// foreach(string field in fields);
    ///     Console.WriteLine(field);
    /// </code>
    /// Console Output:<br/>
    /// a,a<br/>
    /// b<br/>
    /// c<br/>
    /// <code>
    /// StringQuotedSignals signals = new StringQuotedSignals(",", "'", Environment.NewLine, null);
    /// string row = "a''a,b,c";
    /// string[] fields = row.SplitQuotedRow(signals);
    /// 
    /// foreach(string field in fields);
    ///     Console.WriteLine(field);
    /// </code>
    /// Console Output:<br/>
    /// a'a<br/>
    /// b<br/>
    /// c<br/>
    /// </example>
    public static string[] SplitQuotedRow(this IEnumerable<char> row, StringQuotedSignals signals)
    {
        if (row == null)
            throw new ArgumentNullException(nameof(row));

        if (signals == null)
            throw new ArgumentNullException(nameof(signals));

        SplitQuotedProcessor processor = new SplitQuotedProcessor(signals);
        string[][] rowsFields = processor.Process(row).Take(2).ToArray();

        if (rowsFields.Length > 1)
            throw new ArgumentException("A NewRow signal is not allowed outside of Quotes.", nameof(row));

        return rowsFields.Length == 0 ? new string[0] : rowsFields[0];
    }

    /// <summary>
    /// Splits a row (a sequence of chars) representing delimited fields (separated by Delimiter) while allowing for instances of the
    /// Delimiter to occur within individual fields.  Such fields must be quoted (surrounded by Quote) to allow for this behavior.
    /// A NewRow signal outside of Quotes is allowed and signals that a new row has begun.
    /// Escaping takes precedence over all other evaluation logic. Only individual characters can be Escaped.
    /// </summary>
    /// <returns>A set of fields for each row in the stream as it is identified.</returns>
    /// <example>
    /// <code>
    /// string rows = "a,b,c" + Environment.NewLine + "d,e,f";
    /// var rowsFields = rows.SplitQuotedRows(StringQuotedSignals.Csv);
    /// 
    /// foreach(string[] rowFields in rowsFields)
    /// {
    ///     Console.WriteLine("Row");
    ///     
    ///     foreach(string field in rowFields)
    ///         Console.WriteLine(field);
    /// }
    /// </code>
    /// Console Output:<br/>
    /// Row<br/>
    /// a<br/>
    /// b<br/>
    /// c<br/>
    /// Row<br/>
    /// d<br/>
    /// e<br/>
    /// f<br/>
    /// </example>
    public static IEnumerable<string[]> SplitQuotedRows(this IEnumerable<char> rows, StringQuotedSignals signals)
    {
        if (rows == null)
            throw new ArgumentNullException(nameof(rows));

        if (signals == null)
            throw new ArgumentNullException(nameof(signals));

        SplitQuotedProcessor processor = new SplitQuotedProcessor(signals);
        return processor.Process(rows);
    }

    /// <summary>
    /// Resuable logic for providing the SplitQuoted algorithm.
    /// </summary>
    /// <remarks>
    /// At the moment, processors are only used for Process'ing once; however, the code was trivial to allow
    /// them to be reused. Very likely a premature optimization, but it cost 1 line of code, so we're going to leave it
    /// in for now. (In my defense) Processors were once being reused by a function that has since been YAGNI deleted from the API.
    /// </remarks>
    private class SplitQuotedProcessor
    {
        public SplitQuotedProcessor(StringQuotedSignals signals)
        {
            _signals = signals;

            _delimiterTracker = new StringSignalTracker(_signals.Delimiter);
            _quoteTracker = new StringSignalTracker(_signals.Quote);
            _newRowTracker = new StringSignalTracker(_signals.NewRow);
            _escapeTracker = new StringSignalTracker(_signals.Escape);
        }

        private StringQuotedSignals _signals;

        private StringSignalTracker _delimiterTracker;
        private StringSignalTracker _quoteTracker;
        private StringSignalTracker _newRowTracker;
        private StringSignalTracker _escapeTracker;

        private bool _inRow = false;
        private List<string> _fields = new List<string>();
        private List<char> _fieldBuffer = new List<char>();
        private List<CharacterCategory> _fieldCategories = new List<CharacterCategory>();

        private int _consecutiveQuoteCount = 0;
        private bool _inQuotes = false;
        private bool _escaped = false;

        private void Reset()
        {
            ResetTrackers();

            _inRow = false;
            _fields.Clear();
            _fieldBuffer.Clear();
            _fieldCategories.Clear();

            _consecutiveQuoteCount = 0;
            _inQuotes = false;
            _escaped = false;
        }

        private void ResetTrackers()
        {
            _delimiterTracker.Reset();
            _quoteTracker.Reset();
            _newRowTracker.Reset();
            _escapeTracker.Reset();
        }

        public IEnumerable<string[]> Process(IEnumerable<char> rows)
        {
            foreach (char c in rows)
            {
                _inRow = true;
                _fieldBuffer.Add(c);
                _fieldCategories.Add(CharacterCategory.Char);

                if (!_escaped)
                {
                    _delimiterTracker.ProcessChar(c);
                    _quoteTracker.ProcessChar(c);
                    _newRowTracker.ProcessChar(c);
                    _escapeTracker.ProcessChar(c);
                }
                else
                    _escaped = false;

                // Quote needs to be processed first because of it's potential need to rewind a character and FlushConsecutiveQuotes.
                if (_quoteTracker.IsTriggered)
                {
                    ResetTrackers();
                    ReviseCategories(CharacterCategory.NoOp, _signals.Quote.Length);

                    _consecutiveQuoteCount++;
                }
                else if (!_quoteTracker.IsCounting && _consecutiveQuoteCount > 0)
                {
                    // Rewind a character so that our FlushConsecutiveQuotes is accurate.
                    _fieldBuffer.RemoveAt(_fieldBuffer.Count - 1);
                    _fieldCategories.RemoveAt(_fieldCategories.Count - 1);

                    FlushConsecutiveQuotes();

                    // Now place the character being processed back in the buffers.
                    _fieldBuffer.Add(c);
                    _fieldCategories.Add(CharacterCategory.Char);
                }

                if (_delimiterTracker.IsTriggered)
                {
                    ResetTrackers();
                    ReviseCategories(_inQuotes ? CharacterCategory.Delimiter : CharacterCategory.NoOp, _signals.Delimiter.Length);

                    if (!_inQuotes)
                        FlushColumn();
                }

                if (_newRowTracker.IsTriggered)
                {
                    ResetTrackers();
                    ReviseCategories(_inQuotes ? CharacterCategory.NewRow : CharacterCategory.NoOp, _signals.NewRow.Length);

                    if (!_inQuotes)
                    {
                        FlushColumn();
                        yield return _fields.ToArray();
                        Reset();
                    }
                }

                if (_escapeTracker.IsTriggered)
                {
                    ResetTrackers();
                    ReviseCategories(CharacterCategory.NoOp, _signals.Escape.Length);

                    _escaped = true;
                }
            }

            FlushColumn();

            if (_fields.Count > 0)
                yield return _fields.ToArray();

            Reset();
        }

        private void ReviseCategories(CharacterCategory category, int length)
        {
            for (int i = length; i > 0; i--)
                _fieldCategories[_fieldCategories.Count - i] = i == length ? category : CharacterCategory.NoOp;
        }

        private void FlushColumn()
        {
            if (!_inRow)
                return;

            FlushConsecutiveQuotes();

            // An empty field that is Quoted or a Quoted field containing only Quotes will never properly detect that it is Quoted,
            // and therefore it will contain an extra Quote. E.G. a,"",c or a,"""",c
            bool skipAQuote = _fieldCategories.All(category => category == CharacterCategory.NoOp || category == CharacterCategory.Quote);

            StringBuilder field = new StringBuilder();
            int fieldBufferIndex = 0;

            foreach (CharacterCategory category in _fieldCategories)
            {
                switch (category)
                {
                    case CharacterCategory.Char:
                        field.Append(_fieldBuffer[fieldBufferIndex]);
                        break;

                    case CharacterCategory.Delimiter:
                        field.Append(_signals.Delimiter);
                        break;

                    case CharacterCategory.Quote:
                        if (skipAQuote)
                            skipAQuote = false;
                        else
                            field.Append(_signals.Quote);
                        break;

                    case CharacterCategory.NewRow:
                        field.Append(_signals.NewRow);
                        break;

                    case CharacterCategory.NoOp:
                        break;

                    default:
                        throw new NotImplementedException(category.ToString());
                }

                fieldBufferIndex++;
            }

            _fields.Add(field.ToString());
            _fieldBuffer.Clear();
            _fieldCategories.Clear();
        }

        private void FlushConsecutiveQuotes()
        {
            if (_consecutiveQuoteCount == 0)
                return;

            if (_consecutiveQuoteCount % 2 == 1)
                _inQuotes = !_inQuotes;

            for (int i = _consecutiveQuoteCount / 2; i > 0; i--)
                ReviseCategories(CharacterCategory.Quote, i * 2 * _signals.Quote.Length);

            _consecutiveQuoteCount = 0;
        }

        private enum CharacterCategory : byte
        {
            Char,
            Delimiter,
            Quote,
            NewRow,
            NoOp
        }
    }
}