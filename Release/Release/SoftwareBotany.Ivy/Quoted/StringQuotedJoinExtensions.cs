using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    /// <summary>
    /// Extensions for Splitting and Joining delimited sequences of characters (rows) that may possess quoted (surrounded by Quote)
    /// columns in order for them to contain instances of Delimiter.
    /// </summary>
    public static partial class StringQuotedExtensions
    {
        /// <inheritdoc cref="JoinQuotedRow(IEnumerable{string}, StringQuotedSignals, bool)"/>
        public static string JoinQuotedRow(this IEnumerable<string> columns, StringQuotedSignals signals) { return JoinQuotedRow(columns, signals, false); }

        /// <summary>
        /// Joins a sequence of columns, separates them with Delimiter, and allows for instances of Delimiter (or the NewRow signal)
        /// to occur within individual columns.  Such columns will be quoted (surrounded by Quote) to allow for this behavior. Instances
        /// of the Quote signal within columns will be escaped by doubling (Quote + Quote).
        /// </summary>
        /// <param name="forceQuotes">
        /// Dictates whether to force every column to be quoted regardless of whether or not the column contains an instance
        /// of Delimiter or NewRow. (default = false)
        /// </param>
        /// <example>
        /// <code>
        /// string[] columns = new string[] { "a", "b", "c" };
        /// string result = columns.JoinQuotedRow(StringQuotedSignals.Csv);
        /// Console.WriteLine(result);
        /// </code>
        /// Console Output:
        /// <code>
        /// a,b,c
        /// </code>
        /// <code>
        /// string[] columns = new string[] { "a,a", "b", "c" };
        /// string result = columns.JoinQuotedRow(StringQuotedSignals.Csv);
        /// Console.WriteLine(result);
        /// </code>
        /// Console Output:
        /// <code>
        /// "a,a",b,c
        /// </code>
        /// <code>
        /// string[] columns = new string[] { "a", "b" + Environment.NewLine + "b", "c" };
        /// string result = columns.JoinQuotedRow(StringQuotedSignals.Csv, true);
        /// Console.WriteLine(result);
        /// </code>
        /// Console Output:
        /// <code>
        /// "a","b
        /// b","c"
        /// </code>
        /// <code>
        /// string[] columns = new string[] { "a\"a", "b", "c" };
        /// string result = columns.JoinQuotedRow(StringQuotedSignals.Csv);
        /// Console.WriteLine(result);
        /// </code>
        /// Console Output:
        /// <code>
        /// a""a,b,c
        /// </code>
        /// </example>
        public static string JoinQuotedRow(this IEnumerable<string> columns, StringQuotedSignals signals, bool forceQuotes)
        {
            if (columns == null)
                throw new ArgumentNullException("columns");

            if (signals == null)
                throw new ArgumentNullException("signals");

            if (forceQuotes && !signals.QuoteIsSpecified)
                throw new ArgumentException("Quote'ing forced; therefore, signals.Quote must not be null or empty.");

            return string.Join(signals.Delimiter, columns.Select(column => QuoteAndEscapeColumn(column, signals, forceQuotes)));
        }

        private static string QuoteAndEscapeColumn(string column, StringQuotedSignals signals, bool forceQuotes)
        {
            bool containsDelimiter = column.Contains(signals.Delimiter);
            bool containsQuote = signals.QuoteIsSpecified && column.Contains(signals.Quote);
            bool containsNewRow = signals.NewRowIsSpecified && column.Contains(signals.NewRow);
            bool containsEscape = signals.EscapeIsSpecified && column.Contains(signals.Escape);

            bool requiresQuotingOrEscaping = containsDelimiter || containsQuote || containsNewRow || containsEscape;

            if (requiresQuotingOrEscaping && !signals.QuoteIsSpecified && !signals.EscapeIsSpecified)
                throw new ArgumentException("Quoting or Escaping is required; therefore, either signals.Quote or signals.Escape must not be null or empty.");

            bool useQuoting = forceQuotes || (requiresQuotingOrEscaping && signals.QuoteIsSpecified);
            bool useEscaping = !useQuoting && requiresQuotingOrEscaping && signals.EscapeIsSpecified;

            string escapedColumn = column;

            if (containsEscape)
                escapedColumn = escapedColumn.Replace(signals.Escape, signals.Escape + signals.Escape);

            if (useQuoting)
            {
                if (containsQuote)
                    escapedColumn = escapedColumn.Replace(signals.Quote, (signals.EscapeIsSpecified ? signals.Escape : signals.Quote) + signals.Quote);

                escapedColumn = string.Format(CultureInfo.InvariantCulture, "{0}{1}{0}", signals.Quote, escapedColumn);
            }
            else if (useEscaping)
            {
                if (containsDelimiter)
                    escapedColumn = escapedColumn.Replace(signals.Delimiter, signals.Escape + signals.Delimiter);

                if (containsNewRow)
                    escapedColumn = escapedColumn.Replace(signals.NewRow, signals.Escape + signals.NewRow);
            }

            return escapedColumn;
        }
    }
}