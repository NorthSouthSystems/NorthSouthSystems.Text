namespace NorthSouthSystems.Text;

using System.Text;

public static partial class StringQuotedExtensions
{
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