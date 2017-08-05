using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Kangarooper.Text
{
    /// <summary>
    /// Extensions for Splitting and Joining delimited sequences of characters (rows) that may possess quoted (surrounded by Quote)
    /// fields in order for them to contain instances of Delimiter.
    /// </summary>
    public static partial class StringQuotedExtensions
    {
        /// <summary>
        /// Joins a sequence of fields, separates them with Delimiter, and allows for instances of Delimiter (or the NewRow signal)
        /// to occur within individual fields.  Such fields will be quoted (surrounded by Quote) to allow for this behavior. Instances
        /// of the Quote signal within fields will be escaped by doubling (Quote + Quote).
        /// </summary>
        /// <param name="forceQuotes">
        /// Dictates whether to force every field to be quoted regardless of whether or not the field contains an instance
        /// of Delimiter or NewRow. (default = false)
        /// </param>
        /// <example>
        /// <code>
        /// string[] fields = new string[] { "a", "b", "c" };
        /// string result = fields.JoinQuotedRow(StringQuotedSignals.Csv, false);
        /// Console.WriteLine(result);
        /// </code>
        /// Console Output:<br/>
        /// a,b,c<br/>
        /// <code>
        /// string[] fields = new string[] { "a,a", "b", "c" };
        /// string result = fields.JoinQuotedRow(StringQuotedSignals.Csv, false);
        /// Console.WriteLine(result);
        /// </code>
        /// Console Output:<br/>
        /// "a,a",b,c<br/>
        /// <code>
        /// string[] fields = new string[] { "a", "b" + Environment.NewLine + "b", "c" };
        /// string result = fields.JoinQuotedRow(StringQuotedSignals.Csv, true);
        /// Console.WriteLine(result);
        /// </code>
        /// Console Output:<br/>
        /// "a","b<br/>
        /// b","c"<br/>
        /// <code>
        /// string[] fields = new string[] { "a\"a", "b", "c" };
        /// string result = fields.JoinQuotedRow(StringQuotedSignals.Csv, false);
        /// Console.WriteLine(result);
        /// </code>
        /// Console Output:<br/>
        /// a""a,b,c<br/>
        /// </example>
        public static string JoinQuotedRow(this IEnumerable<string> fields, StringQuotedSignals signals, bool forceQuotes = false)
        {
            if (fields == null)
                throw new ArgumentNullException("fields");

            if (signals == null)
                throw new ArgumentNullException("signals");

            if (forceQuotes && !signals.QuoteIsSpecified)
                throw new ArgumentException("Quote'ing forced; therefore, signals.Quote must not be null or empty.");

            return string.Join(signals.Delimiter, fields.Select(field => QuoteAndEscapeField(field, signals, forceQuotes)));
        }

        private static string QuoteAndEscapeField(string field, StringQuotedSignals signals, bool forceQuotes)
        {
            bool containsDelimiter = field.Contains(signals.Delimiter);
            bool containsQuote = signals.QuoteIsSpecified && field.Contains(signals.Quote);
            bool containsNewRow = signals.NewRowIsSpecified && field.Contains(signals.NewRow);
            bool containsEscape = signals.EscapeIsSpecified && field.Contains(signals.Escape);

            bool requiresQuotingOrEscaping = containsDelimiter || containsQuote || containsNewRow || containsEscape;

            if (requiresQuotingOrEscaping && !signals.QuoteIsSpecified && !signals.EscapeIsSpecified)
                throw new ArgumentException("Quoting or Escaping is required; therefore, either signals.Quote or signals.Escape must not be null or empty.");

            bool useQuoting = forceQuotes || (requiresQuotingOrEscaping && signals.QuoteIsSpecified);
            bool useEscaping = !useQuoting && requiresQuotingOrEscaping && signals.EscapeIsSpecified;

            string escapedField = field;

            if (containsEscape)
                escapedField = escapedField.Replace(signals.Escape, signals.Escape + signals.Escape);

            if (useQuoting)
            {
                if (containsQuote)
                    escapedField = escapedField.Replace(signals.Quote, (signals.EscapeIsSpecified ? signals.Escape : signals.Quote) + signals.Quote);

                escapedField = string.Format(CultureInfo.InvariantCulture, "{0}{1}{0}", signals.Quote, escapedField);
            }
            else if (useEscaping)
            {
                if (containsDelimiter)
                    escapedField = escapedField.Replace(signals.Delimiter, signals.Escape + signals.Delimiter);

                if (containsNewRow)
                    escapedField = escapedField.Replace(signals.NewRow, signals.Escape + signals.NewRow);
            }

            return escapedField;
        }
    }
}