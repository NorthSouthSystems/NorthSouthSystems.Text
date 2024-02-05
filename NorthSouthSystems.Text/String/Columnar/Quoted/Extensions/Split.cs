namespace NorthSouthSystems.Text;

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

        var processor = new SplitQuotedProcessor(signals);
        string[][] rowsFields = processor.Process(row).Take(2).ToArray();

        if (rowsFields.Length > 1)
            throw new ArgumentException("A NewRow signal is not allowed outside of Quotes.", nameof(row));

        return rowsFields.Length == 0 ? Array.Empty<string>() : rowsFields[0];
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

        var processor = new SplitQuotedProcessor(signals);
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

            _delimiterTracker = new(_signals.Delimiter);
            _quoteTracker = new(_signals.Quote);
            _newRowTracker = new(_signals.NewRow);
            _escapeTracker = new(_signals.Escape);

            _quoteQuoteTracker = new(_signals.Quote + _signals.Quote);
        }

        private bool _inRow = false;
        private readonly List<string> _fields = new();

        private readonly StringBuilder _fieldBuilder = new();

        private bool _inQuotes = false;
        private bool _inQuotesEverCurrentField = false;
        private bool _escaped = false;
        private bool _escapedEverCurrentField = false;

        private readonly StringQuotedSignals _signals;

        private readonly StringSignalTracker _delimiterTracker;
        private readonly StringSignalTracker _quoteTracker;
        private readonly StringSignalTracker _newRowTracker;
        private readonly StringSignalTracker _escapeTracker;

        private readonly StringSignalTracker _quoteQuoteTracker;

        private void Reset()
        {
            _inRow = false;
            _fields.Clear();

            ResetField();
            ResetTrackers();
        }

        private void ResetField()
        {
            _fieldBuilder.Clear();

            _inQuotes = false;
            _inQuotesEverCurrentField = false;
            _escaped = false;
            _escapedEverCurrentField = false;
        }

        private void ResetTrackers(bool wasQuoteTrackerTriggered = false)
        {
            _delimiterTracker.Reset();
            _quoteTracker.Reset();
            _newRowTracker.Reset();
            _escapeTracker.Reset();

            if (!wasQuoteTrackerTriggered)
                _quoteQuoteTracker.Reset();
        }

        public IEnumerable<string[]> Process(IEnumerable<char> rows)
        {
            foreach (char c in rows)
            {
                _inRow = true;
                _fieldBuilder.Append(c);

                if (_escaped)
                {
                    _escaped = false;
                    continue;
                }

                _delimiterTracker.ProcessChar(c);
                _quoteTracker.ProcessChar(c);
                _newRowTracker.ProcessChar(c);
                _escapeTracker.ProcessChar(c);

                _quoteQuoteTracker.ProcessChar(c);

                if (_delimiterTracker.IsTriggered)
                {
                    ResetTrackers();

                    if (!_inQuotes)
                    {
                        RewindField(_signals.Delimiter.Length);
                        FlushField();
                        ResetField();
                    }
                }
                else if (_quoteQuoteTracker.IsTriggered)
                {
                    ResetTrackers();

                    _inQuotes = false;
                }
                else if (_quoteTracker.IsTriggered)
                {
                    ResetTrackers(wasQuoteTrackerTriggered: true);
                    RewindField(_signals.Quote.Length);

                    _inQuotes = !_inQuotes;
                    _inQuotesEverCurrentField = true;
                }
                else if (_newRowTracker.IsTriggered)
                {
                    ResetTrackers();

                    if (!_inQuotes)
                    {
                        RewindField(_signals.NewRow.Length);
                        FlushField();
                        yield return _fields.ToArray();
                        Reset();
                    }
                }
                else if (_escapeTracker.IsTriggered)
                {
                    ResetTrackers();
                    RewindField(_signals.Escape.Length);

                    _escaped = true;
                    _escapedEverCurrentField = true;
                }
            }

            FlushField();

            if (_fields.Count > 0)
                yield return _fields.ToArray();

            Reset();
        }

        private void RewindField(int length) => _fieldBuilder.Length -= length;

        private void FlushField()
        {
            if (!_inRow)
                return;

            string field = _fieldBuilder.ToString();

            // An empty field that is Quoted or a Quoted field containing only Quotes will never properly detect that the field
            // itself is Quoted because two consecutive quotes results in a Quote at the end of _fieldBuilder and _inQuotes == false;
            // therefore, _fieldBuilder will contain an extra Quote. E.G.
            //     a,"",c
            //     a,"""",c
            //     a,"""""",c
            if (_inQuotesEverCurrentField
                && !_escapedEverCurrentField
                && (field == _signals.Quote || field.SequenceEqual(RepeatQuote())))
            {
                RewindField(_signals.Quote.Length);
                field = _fieldBuilder.ToString();
            }

            IEnumerable<char> RepeatQuote()
            {
                for (int i = 0; i < field.Length / _signals.Quote.Length; i++)
                    foreach (char c in _signals.Quote)
                        yield return c;
            }

            _fields.Add(field);
        }
    }
}