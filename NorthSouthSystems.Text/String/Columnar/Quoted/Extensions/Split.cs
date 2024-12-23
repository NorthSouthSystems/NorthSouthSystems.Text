﻿namespace NorthSouthSystems.Text;

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

        var processor = CreateSplitQuotedProcessor(signals);
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

        var processor = CreateSplitQuotedProcessor(signals);
        return processor.Process(rows);
    }

    private static ISplitQuotedProcessor CreateSplitQuotedProcessor(StringQuotedSignals signals) =>
        signals.IsNewRowTolerantSimple
            ? new NewRowTolerantSimpleSplitQuotedProcessor(signals)
            : new FullSplitQuotedProcessor(signals);

    private interface ISplitQuotedProcessor { IEnumerable<string[]> Process(IEnumerable<char> rows); }

    private sealed class FullSplitQuotedProcessor : ISplitQuotedProcessor
    {
        internal FullSplitQuotedProcessor(StringQuotedSignals signals)
        {
            _signals = signals;

            _isNewRowTolerant = signals.IsNewRowTolerant;

            _delimiterTracker = StringSignalTracker.Create(_signals.Delimiters);
            _newRowTracker = StringSignalTracker.Create(_isNewRowTolerant ? null : _signals.NewRows);
            _quoteTracker = StringSignalTracker.Create(_signals.Quote);
            _escapeTracker = StringSignalTracker.Create(_signals.Escape);

            _quoteQuoteTracker = StringSignalTracker.Create(_signals.Quote + _signals.Quote);
        }

        private bool _inRow = false;
        private readonly List<string> _fields = new();

        private readonly StringBuilder _fieldBuilder = new();

        private bool InQuotes => (_quoteAnyCount - (2 * _quoteQuoteCount)) % 2 == 1;

        private int _quoteAnyCount;
        private int _quoteQuoteCount;

        private bool _escaped = false;

        private readonly StringQuotedSignals _signals;

        private readonly IStringSignalTracker _delimiterTracker;
        private readonly IStringSignalTracker _newRowTracker;
        private readonly IStringSignalTracker _quoteTracker;
        private readonly IStringSignalTracker _escapeTracker;

        private readonly IStringSignalTracker _quoteQuoteTracker;

        // Do NOT Reset _newRowTolerantWasCarriageReturn because it needs to survive the "premature"
        // yielding of rows upon seeing the \r of a \r\n (Windows NewRow) in the case that the \r is
        // not followed by the \n.
        private readonly bool _isNewRowTolerant;
        private bool _newRowTolerantWasCarriageReturn;

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

            _quoteAnyCount = 0;
            _quoteQuoteCount = 0;

            _escaped = false;
        }

        private void ResetTrackers(bool wasQuoteTrackerTriggered = false)
        {
            _delimiterTracker.Reset();
            _newRowTracker.Reset();
            _quoteTracker.Reset();
            _escapeTracker.Reset();

            if (!wasQuoteTrackerTriggered)
                _quoteQuoteTracker.Reset();
        }

        public IEnumerable<string[]> Process(IEnumerable<char> rows)
        {
            foreach (char c in rows)
            {
                if (ProcessReturnsYieldRow(c))
                {
                    yield return _fields.ToArray();
                    Reset();
                }
            }

            FlushField();

            if (_fields.Count > 0)
                yield return _fields.ToArray();

            Reset();
        }

        private bool ProcessReturnsYieldRow(char c)
        {
            // Because we immediately yield a row upon seeing a \r, we need to conditionally
            // ignore the immediately following \n in the case of a Windows NewRow (\r\n).
            if (_newRowTolerantWasCarriageReturn)
            {
                _newRowTolerantWasCarriageReturn = false;

                if (c == '\n')
                    return false;
            }

            _inRow = true;
            _fieldBuilder.Append(c);

            if (_escaped)
            {
                _escaped = false;

                return false;
            }

            int triggeredLength;

            if ((triggeredLength = _delimiterTracker.ProcessCharReturnsTriggeredLength(c)) > 0)
            {
                ResetTrackers();

                if (InQuotes)
                    return false;

                RewindField(triggeredLength);
                FlushField();
                ResetField();

                return false;
            }

            if (_quoteQuoteTracker.ProcessCharReturnsTriggeredLength(c) > 0)
            {
                ResetTrackers();

                _quoteAnyCount++;
                _quoteQuoteCount++;

                return false;
            }

            if ((triggeredLength = _quoteTracker.ProcessCharReturnsTriggeredLength(c)) > 0)
            {
                ResetTrackers(wasQuoteTrackerTriggered: true);
                RewindField(triggeredLength);

                _quoteAnyCount++;

                return false;
            }

            if (_isNewRowTolerant && (c == '\n' || c == '\r'))
            {
                ResetTrackers();

                if (InQuotes)
                    return false;

                RewindField(1);
                FlushField();

                _newRowTolerantWasCarriageReturn = c == '\r';

                return true;
            }

            if ((triggeredLength = _newRowTracker.ProcessCharReturnsTriggeredLength(c)) > 0)
            {
                ResetTrackers();

                if (InQuotes)
                    return false;

                RewindField(triggeredLength);
                FlushField();

                return true;
            }

            if ((triggeredLength = _escapeTracker.ProcessCharReturnsTriggeredLength(c)) > 0)
            {
                ResetTrackers();
                RewindField(triggeredLength);

                _escaped = true;

                return false;
            }

            return false;
        }

        private void RewindField(int length) => _fieldBuilder.Length -= length;

        private void FlushField()
        {
            if (!_inRow)
                return;

            // An empty field that is Quoted or a Quoted field containing only Quotes will never properly detect that the field
            // itself is Quoted because two consecutive quotes results in a Quote at the end of _fieldBuilder with InQuotes false;
            // therefore, _fieldBuilder will contain an extra Quote. E.G.
            //     a,"",c
            //     a,"""",c
            //     a,"""""",c
            if (_fieldBuilder.Length > 0 && _fieldBuilder.Length == _signals.Quote.Length * _quoteQuoteCount)
                RewindField(_signals.Quote.Length);

            _fields.Add(_fieldBuilder.ToString());
        }
    }

    private sealed class NewRowTolerantSimpleSplitQuotedProcessor : ISplitQuotedProcessor
    {
        internal NewRowTolerantSimpleSplitQuotedProcessor(StringQuotedSignals signals)
        {
            _delimiter = signals.Delimiters.Single().Single();
            _quote = signals.Quote.Single();
        }

        private bool _inRow = false;
        private readonly List<string> _fields = new();

        private readonly StringBuilder _fieldBuilder = new();

        private bool InQuotes => (_quoteAnyCount - (2 * _quoteQuoteCount)) % 2 == 1;

        private bool _quoteWasTriggered;

        private int _quoteAnyCount;
        private int _quoteQuoteCount;

        private readonly char _delimiter;
        private readonly char _quote;

        // Do NOT Reset _newRowTolerantWasCarriageReturn because it needs to survive the "premature"
        // yielding of rows upon seeing the \r of a \r\n (Windows NewRow) in the case that the \r is
        // not followed by the \n.
        private bool _newRowTolerantWasCarriageReturn;

        private void Reset()
        {
            _inRow = false;
            _fields.Clear();

            ResetField();
        }

        private void ResetField()
        {
            _fieldBuilder.Clear();

            _quoteWasTriggered = false;

            _quoteAnyCount = 0;
            _quoteQuoteCount = 0;
        }

        public IEnumerable<string[]> Process(IEnumerable<char> rows)
        {
            foreach (char c in rows)
            {
                if (ProcessReturnsYieldRow(c))
                {
                    yield return _fields.ToArray();
                    Reset();
                }
            }

            FlushField();

            if (_fields.Count > 0)
                yield return _fields.ToArray();

            Reset();
        }

        private bool ProcessReturnsYieldRow(char c)
        {
            // Because we immediately yield a row upon seeing a \r, we need to conditionally
            // ignore the immediately following \n in the case of a Windows NewRow (\r\n).
            if (_newRowTolerantWasCarriageReturn)
            {
                _newRowTolerantWasCarriageReturn = false;

                if (c == '\n')
                    return false;
            }

            _inRow = true;
            _fieldBuilder.Append(c);

            if (c == _delimiter)
            {
                _quoteWasTriggered = false;

                if (InQuotes)
                    return false;

                RewindField(1);
                FlushField();
                ResetField();

                return false;
            }

            if (_quoteWasTriggered && c == _quote)
            {
                _quoteWasTriggered = false;

                _quoteAnyCount++;
                _quoteQuoteCount++;

                return false;
            }

            if (c == _quote)
            {
                _quoteWasTriggered = true;

                RewindField(1);

                _quoteAnyCount++;

                return false;
            }

            if (c == '\n' || c == '\r')
            {
                _quoteWasTriggered = false;

                if (InQuotes)
                    return false;

                RewindField(1);
                FlushField();

                _newRowTolerantWasCarriageReturn = (c == '\r');

                return true;
            }

            _quoteWasTriggered = false;

            return false;
        }

        private void RewindField(int length) => _fieldBuilder.Length -= length;

        private void FlushField()
        {
            if (!_inRow)
                return;

            // An empty field that is Quoted or a Quoted field containing only Quotes will never properly detect that the field
            // itself is Quoted because two consecutive quotes results in a Quote at the end of _fieldBuilder with InQuotes false;
            // therefore, _fieldBuilder will contain an extra Quote. E.G.
            //     a,"",c
            //     a,"""",c
            //     a,"""""",c
            if (_fieldBuilder.Length > 0 && _fieldBuilder.Length == _quoteQuoteCount)
                RewindField(1);

            _fields.Add(_fieldBuilder.ToString());
        }
    }
}