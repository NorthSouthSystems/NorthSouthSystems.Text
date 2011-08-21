using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareBotany.Ivy
{
    public static partial class StringQuotedExtensions
    {
        /// <summary>
        /// Splits a row, a sequence of chars, representing delimited columns (separated by Delimiter) while allowing for instances of the
        /// Delimiter to occur within individual columns.  Such columns must be quoted (surrounded by Quote) to allow for this behavior.
        /// A NewRow signal outside of Quotes will cause an exception because multiple rows are not allowed for this method.
        /// </summary>
        /// <example>
        /// <code>
        /// foreach(string column in "a,b,c".SplitQuotedRow(StringQuotedSignals.Csv);)
        ///     Console.WriteLine(column);
        /// </code>
        /// Console Output:
        /// <code>
        /// a
        /// b
        /// c
        /// </code>
        /// <code>
        /// StringQuotedSignals signals = new StringQuotedSignals(",", "'", Environment.NewLine);
        /// 
        /// foreach(string column in "'a,a',b,c".SplitQuotedRow(signals);)
        ///     Console.WriteLine(column);
        /// </code>
        /// Console Output:
        /// <code>
        /// a,a
        /// b
        /// c
        /// </code>
        /// <code>
        /// StringQuotedSignals signals = new StringQuotedSignals(",", "'", Environment.NewLine);
        /// 
        /// foreach(string column in "a''a,b,c".SplitQuotedRow(signals);)
        ///     Console.WriteLine(column);
        /// </code>
        /// Console Output:
        /// <code>
        /// a'a
        /// b
        /// c
        /// </code>
        /// </example>
        public static string[] SplitQuotedRow(this IEnumerable<char> row, StringQuotedSignals signals)
        {
            if (row == null)
                throw new ArgumentNullException("row");

            if (signals == null)
                throw new ArgumentNullException("signals");

            SplitQuotedProcessor processor = new SplitQuotedProcessor(signals);
            string[][] splits = processor.Process(row).Take(2).ToArray();

            if (splits.Length > 1)
                throw new ArgumentException("A NewRow signal is not allowed outside of Quotes.", "row");

            return splits.Length == 0 ? new string[0] : splits[0];
        }

        /// <summary>
        /// Splits a row, a sequence of chars, representing delimited columns (separated by Delimiter) while allowing for instances of the
        /// Delimiter to occur within individual columns.  Such columns must be quoted (surrounded by Quote) to allow for this behavior.
        /// A NewRow signal outside of Quotes is allowed and signals that a new row has begun.
        /// </summary>
        /// <returns>A sets of string columns for each row in the stream as it is identified.</returns>
        /// <example>
        /// <code>
        /// foreach(string[] rowColumns in ("a,b,c" + Environment.NewLine + "d,e,f").SplitQuotedRows(StringQuotedSignals.Csv);)
        /// {
        ///     Console.WriteLine("Row");
        ///     
        ///     foreach(string column in rowColumns)
        ///         Console.WriteLine(column);
        /// }
        /// </code>
        /// Console Output:
        /// <code>
        /// Row
        /// a
        /// b
        /// c
        /// Row
        /// d
        /// e
        /// f
        /// </code>
        /// </example>
        public static IEnumerable<string[]> SplitQuotedRows(this IEnumerable<char> rows, StringQuotedSignals signals)
        {
            if (rows == null)
                throw new ArgumentNullException("rows");

            if (signals == null)
                throw new ArgumentNullException("signals");

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
            }

            public IEnumerable<string[]> Process(IEnumerable<char> rows)
            {
                List<string> results = new List<string>();

                foreach (char c in rows)
                {
                    bool triggered = ProcessChar(c);

                    if (triggered)
                    {
                        if (IsNewRow && !InQuotes())
                            yield return ProduceLineResults();

                        ResetTrackers();
                    }
                }

                if (_buffer.Length > 0)
                    yield return ProduceLineResults();

                // This will ensure the Processor is in a reusable state.
                ResetTrackers();
            }

            private bool ProcessChar(char c)
            {
                _buffer.Append(c);
                _categories.Add(CharacterCategory.Char);

                _delimiterTracker.ProcessChar(c);
                _quoteTracker.ProcessChar(c);
                _newRowTracker.ProcessChar(c);

                ReviseCategories();

                return IsDelimiter || IsQuote || IsNewRow;
            }

            private void ReviseCategories()
            {
                if (IsDelimiter)
                {
                    ReviseCategories(CharacterCategory.Delimiter, _signals.Delimiter.Length);
                }
                else if (IsQuote)
                {
                    bool isEndQuote = InQuotes();

                    ReviseCategories(isEndQuote ? CharacterCategory.EndQuote : CharacterCategory.StartQuote, _signals.Quote.Length);

                    int potentialStartQuoteIndex = _categories.Count - (_signals.Quote.Length * 2);

                    if (isEndQuote
                        && potentialStartQuoteIndex >= 0
                        && _categories[potentialStartQuoteIndex] == CharacterCategory.StartQuote)
                    {
                        ReviseCategories(CharacterCategory.EscapedQuote, _signals.Quote.Length * 2);
                    }
                }
                else if (IsNewRow)
                {
                    ReviseCategories(CharacterCategory.NewRow, _signals.NewRow.Length);
                }
            }

            private void ReviseCategories(CharacterCategory category, int length)
            {
                for (int i = length; i > 0; i--)
                    _categories[_categories.Count - i] = i == length ? category : CharacterCategory.NoOp;
            }

            private bool InQuotes()
            {
                return _categories.AsEnumerable()
                    .Reverse()
                    .TakeWhile(category => category != CharacterCategory.EndQuote)
                    .Any(category => category == CharacterCategory.StartQuote);
            }

            private string[] ProduceLineResults()
            {
                List<string> results = new List<string>();
                StringBuilder result = new StringBuilder();

                bool inQuotes = false;
                int index = 0;

                foreach (CharacterCategory category in _categories)
                {
                    switch (category)
                    {
                        case CharacterCategory.Char:
                            result.Append(_buffer[index]);
                            break;

                        case CharacterCategory.Delimiter:
                            if (inQuotes)
                            {
                                result.Append(_signals.Delimiter);
                            }
                            else
                            {
                                results.Add(result.ToString());
                                result.Clear();
                                inQuotes = false;
                            }
                            break;

                        case CharacterCategory.StartQuote:
                            inQuotes = true;
                            break;

                        case CharacterCategory.EndQuote:
                            inQuotes = false;
                            break;

                        case CharacterCategory.EscapedQuote:
                            result.Append(_signals.Quote);
                            break;

                        case CharacterCategory.NewRow:
                            if (inQuotes)
                                result.Append(_signals.NewRow);
                            break;

                        case CharacterCategory.NoOp:
                            break;

                        default:
                            throw new NotImplementedException(category.ToString());
                    }

                    index++;
                }

                results.Add(result.ToString());

                _buffer.Clear();
                _categories.Clear();

                return results.ToArray();
            }

            private void ResetTrackers()
            {
                _delimiterTracker.Reset();
                _quoteTracker.Reset();
                _newRowTracker.Reset();
            }

            private StringQuotedSignals _signals;
            private StringBuilder _buffer = new StringBuilder();
            private List<CharacterCategory> _categories = new List<CharacterCategory>();

            private StringSignalTracker _delimiterTracker;
            private StringSignalTracker _quoteTracker;
            private StringSignalTracker _newRowTracker;

            private bool IsDelimiter { get { return _delimiterTracker.IsTriggered; } }
            private bool IsQuote { get { return _quoteTracker.IsTriggered; } }
            private bool IsNewRow { get { return _newRowTracker.IsTriggered; } }

            private enum CharacterCategory : byte
            {
                Char,
                Delimiter,
                StartQuote,
                EndQuote,
                EscapedQuote,
                NewRow,
                NoOp
            }
        }
    }
}