using System;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    /// <summary>
    /// Represents the Signals that determine how columns of a row (sequence of chars) are delimited
    /// (separated by Delimiter), quoted (surrounded by Quote) or escaped (contain signals preceeded by Escape)
    /// to allow themselves to contain an instance of Delimiter, and put onto new rows (rows separated by NewRow) in the
    /// case of serialization.
    /// </summary>
    /// <remarks>
    /// Although the functionality is referred to as StringQuoted, a Quote signal is not required; a Delimiter is the
    /// only required signal. Escaping is not a feature that is mutally exclusive to Quoting; however, it can entirely
    /// obsolete Quoting if the Escape and Delimiter are both single character signals. Combining Escaping and Quoting
    /// can result in unexpected behavior; however, there are several rules of thumb.
    /// <list type="bullet">
    /// <item>
    /// <description>When Splitting, Escape behaves identically both outside of and inside of Quotes.</description>   
    /// </item>
    /// <item>
    /// <description>
    /// When Joining, a Quote signal will be Escaped while inside of Quotes. If the Escape signal
    /// is specified, it will be used. If not, a preceeding Quote will be used as Escape.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// When Joining, an Escape within a column is always Escaped by a second Escape.
    /// </description>
    /// </item>
    /// </list>
    /// One other important thing to note is that Escaping multiple character signals can have unexpected results.
    /// </remarks>
    public sealed class StringQuotedSignals
    {
        public static readonly StringQuotedSignals Csv = new StringQuotedSignals(",", "\"", Environment.NewLine, string.Empty);
        public static readonly StringQuotedSignals Pipe = new StringQuotedSignals("|", "\"", Environment.NewLine, string.Empty);
        public static readonly StringQuotedSignals Tab = new StringQuotedSignals("\t", "\"", Environment.NewLine, string.Empty);

        /// <summary>
        /// Constructor with params for all signal values.
        /// </summary>
        /// <param name="delimiter">String used to separate columns of a row. It is the only param that cannot be null or empty.</param>
        /// <param name="quote">String used to surround (or quote) a column and allow it to contain an instance of Delimiter.</param>
        /// <param name="newRow">String used to separate rows during serialization.</param>
        /// <param name="escape">String used to escape the meaning of the immediately following character.</param>
        public StringQuotedSignals(string delimiter, string quote, string newRow, string escape)
        {
            _delimiter = delimiter.NullToEmpty();
            _quote = quote.NullToEmpty();
            _newRow = newRow.NullToEmpty();
            _escape = escape.NullToEmpty();

            if (!DelimiterIsSpecified)
                throw new ArgumentException("Delimiter must be non-null and non-empty.");

            if (ContainsAny(_delimiter, _quote, _newRow, _escape) || ContainsAny(_quote, _newRow, _escape) || ContainsAny(_newRow, _escape))
                throw new ArgumentException("No parameter may be containable within any other.");
        }

        private static bool ContainsAny(string source, params string[] compares)
        {
            return source.Length > 0
                && compares.Where(compare => compare.Length > 0).Any(compare => source.Contains(compare) || compare.Contains(source));
        }

        public bool DelimiterIsSpecified { get { return !string.IsNullOrEmpty(_delimiter); } }
        public string Delimiter { get { return _delimiter; } }
        private readonly string _delimiter;

        public bool QuoteIsSpecified { get { return !string.IsNullOrEmpty(_quote); } }
        public string Quote { get { return _quote; } }
        private readonly string _quote;

        public bool NewRowIsSpecified { get { return !string.IsNullOrEmpty(_newRow); } }
        public string NewRow { get { return _newRow; } }
        private readonly string _newRow;

        public bool EscapeIsSpecified { get { return !string.IsNullOrEmpty(_escape); } }
        public string Escape { get { return _escape; } }
        private readonly string _escape;
    }
}