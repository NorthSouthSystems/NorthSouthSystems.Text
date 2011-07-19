using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareBotany.Ivy
{
    /// <summary>
    /// Extensions for Splitting and Joining delimited sequences of characters that may possess "quoted" columns in
    /// order for them to contain instances of the delimiter itself.
    /// </summary>
    public static partial class StringQuotedExtensions
    {
        /// <inheritdoc cref="JoinQuotedLine(IEnumerable{string}, StringQuotedSignals, bool)"/>
        public static string JoinQuotedLine(this IEnumerable<string> columns, StringQuotedSignals signals)
        {
            return JoinQuotedLine(columns, signals, false);
        }

        /// <summary>
        /// Joins a sequence of columns, separates them with a delimiter, all the while allowing for instances of the delimiter
        /// to occur within individual columns.  Such columns must be quoted to allow for this behavior.
        /// </summary>
        /// <param name="forceQuotes">
        /// Dictates whether to force every column to be quoted regardless of whether or not the column contains an instance
        /// of the delimiter. Microsoft Excel forces quotes (as far as I remember) when saving spreadsheets to the CSV format.
        /// </param>
        public static string JoinQuotedLine(this IEnumerable<string> columns, StringQuotedSignals signals, bool forceQuotes)
        {
            if (columns == null)
                throw new ArgumentNullException("columns");

            if (signals == null)
                throw new ArgumentNullException("signals");

            StringBuilder result = new StringBuilder();

            foreach (string column in columns)
            {
                bool useQuotes = forceQuotes
                    || column.Contains(signals.Delimiter)
                    || (signals.NewLineIsSpecified && column.Contains(signals.NewLine));

                if (useQuotes && !signals.QuoteIsSpecified)
                    throw new ArgumentException("Quote'ing necessary; therefore, signals.Quote must not be null or empty.");

                result.AppendFormat("{0}{1}{0}{2}",
                    useQuotes ? signals.Quote : null,
                    signals.QuoteIsSpecified ? column.Replace(signals.Quote, signals.Quote + signals.Quote) : column,
                    signals.Delimiter);
            }

            // If any output was generated, it will contain a trailing instance of Delimiter; so, remove it.
            if (result.Length > 0)
                result.Length -= signals.Delimiter.Length;

            return result.ToString();
        }
    }
}