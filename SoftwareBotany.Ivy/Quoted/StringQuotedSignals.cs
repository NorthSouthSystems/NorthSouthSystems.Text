using System;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    /// <summary>
    /// Represents the Signals that determine how columns of a row (sequence of chars) are delimited
    /// (separated by Delimiter), quoted (surrounded by Quote) to allow themselves to contain an instance of Delimiter,
    /// and put onto new rows (rows separated by NewRow) in the case of serialization.
    /// </summary>
    public sealed class StringQuotedSignals
    {
        public static readonly StringQuotedSignals Csv = new StringQuotedSignals(",", "\"", Environment.NewLine);
        public static readonly StringQuotedSignals Pipe = new StringQuotedSignals("|", "\"", Environment.NewLine);
        public static readonly StringQuotedSignals Tab = new StringQuotedSignals("\t", "\"", Environment.NewLine);

        /// <summary>
        /// Constructor with params for all signal values.
        /// </summary>
        /// <param name="delimiter">String used to separate columns of a row. It is the only param that cannot be null or empty.</param>
        /// <param name="quote">String used to surround (or quote) a column and allow it to contain an instance of Delimiter.</param>
        /// <param name="newRow">String used to separate rows during serialization.</param>
        public StringQuotedSignals(string delimiter, string quote, string newRow)
        {
            _delimiter = delimiter.NullToEmpty();
            _quote = quote.NullToEmpty();
            _newRow = newRow.NullToEmpty();

            if (!DelimiterIsSpecified)
                throw new ArgumentException("Delimiter must be non-null and non-empty.");

            if (ContainsAny(_delimiter, _quote, _newRow) || ContainsAny(_quote, _newRow))
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
    }
}