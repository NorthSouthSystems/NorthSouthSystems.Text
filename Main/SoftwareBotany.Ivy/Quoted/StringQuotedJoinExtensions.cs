using System;
using System.Collections.Generic;
using System.Text;

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

            StringBuilder row = new StringBuilder();

            foreach (string column in columns)
            {
                bool useQuotes = forceQuotes
                    || column.Contains(signals.Delimiter)
                    || (signals.NewRowIsSpecified && column.Contains(signals.NewRow));

                if (useQuotes && !signals.QuoteIsSpecified)
                    throw new ArgumentException("Quote'ing necessary; therefore, signals.Quote must not be null or empty.");

                row.AppendFormat("{0}{1}{0}{2}",
                    useQuotes ? signals.Quote : null,
                    signals.QuoteIsSpecified ? column.Replace(signals.Quote, signals.Quote + signals.Quote) : column,
                    signals.Delimiter);
            }

            // If any output was generated, it will contain a trailing instance of Delimiter; so, remove it.
            if (row.Length > 0)
                row.Length -= signals.Delimiter.Length;

            return row.ToString();
        }
    }
}