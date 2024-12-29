namespace NorthSouthSystems.Text;

using System.Text;

public static partial class StringQuotedExtensions
{
    private sealed class SimpleSplitQuotedProcessor : ISplitQuotedProcessor
    {
        internal SimpleSplitQuotedProcessor(StringQuotedSignals signals)
        {
            _delimiter = signals.Delimiters.Single().Single();
            _newRow = signals.NewRows?.SingleOrDefault()?.SingleOrDefault() ?? default;
            _quote = signals.Quote.SingleOrDefault();
            _escape = signals.Escape.SingleOrDefault();
        }

        private bool _inRow = false;
        private readonly List<string> _fields = new();

        private readonly StringBuilder _fieldBuilder = new();

        private bool InQuotes => (_quoteAnyCount - (2 * _quoteQuoteCount)) % 2 == 1;

        private bool _quoteWasTriggered;

        private int _quoteAnyCount;
        private int _quoteQuoteCount;

        private bool _escaped = false;

        private readonly char _delimiter;
        private readonly char _newRow;
        private readonly char _quote;
        private readonly char _escape;

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

            _escaped = false;
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
            _inRow = true;
            _fieldBuilder.Append(c);

            if (_escaped)
            {
                _escaped = false;

                return false;
            }

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

            if (c == _newRow)
            {
                _quoteWasTriggered = false;

                if (InQuotes)
                    return false;

                RewindField(1);
                FlushField();

                return true;
            }

            if (c == _escape)
            {
                _quoteWasTriggered = false;

                RewindField(1);

                _escaped = true;

                return false;
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