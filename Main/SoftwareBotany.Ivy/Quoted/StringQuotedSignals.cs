﻿using System;

namespace SoftwareBotany.Ivy
{
    /// <summary>
    /// Represents the Signals that determine how columns of a delimited string are delimited, quoted to allow
    /// themselves to contain an instance of the delimiter, put onto rows in the case of serialization, and how
    /// newlines in a column are substituted in the case of serialization.
    /// </summary>
    public class StringQuotedSignals
    {
        public static readonly StringQuotedSignals Csv = new StringQuotedSignals(",", "\"", Environment.NewLine);
        public static readonly StringQuotedSignals Pipe = new StringQuotedSignals("|", "\"", Environment.NewLine);
        public static readonly StringQuotedSignals Tab = new StringQuotedSignals("\t", "\"", Environment.NewLine);

        /// <summary>
        /// Represents the Signals that determine how columns of a delimited string are delimited, quoted to allow
        /// themselves to contain an instance of the delimiter, put onto rows in the case of serialization, and how
        /// newlines in a column are substituted in the case of serialization.
        /// </summary>
        /// <param name="delimiter">Delimiter between columns of a quoted string.</param>
        /// <param name="quote">
        /// String used to surround (or quote) a column and allow it to contain an instance of the delimiter. E.g. if the
        /// delimiter = "," and the quote = "'", then the string "'some element, that contains a comma',other element"
        /// would split into two strings: "some column, that contains a comma" and "other column".
        /// </param>
        /// <param name="newLine">
        /// Sequence of characters used to separate rows during serialization of multiple quoted strings.
        /// </param>
        public StringQuotedSignals(string delimiter, string quote, string newLine)
        {
            _delimiter = delimiter.NullToEmpty();
            _quote = quote.NullToEmpty();
            _newLine = newLine.NullToEmpty();

            if (!DelimiterIsSpecified)
                throw new ArgumentException("Delimiter must not be null or empty.");

            if (ContainsAny(_delimiter, _quote, _newLine) || ContainsAny(_quote, _newLine))
                throw new ArgumentException("No parameter may be containable within any other.");
        }

        private static bool ContainsAny(string source, params string[] compares)
        {
            if (source.Length > 0)
                foreach (string compare in compares)
                    if (compare.Length > 0)
                        if (source.Contains(compare) || compare.Contains(source))
                            return true;

            return false;
        }

        public bool DelimiterIsSpecified { get { return !string.IsNullOrEmpty(_delimiter); } }
        public string Delimiter { get { return _delimiter; } }
        private readonly string _delimiter;

        public bool QuoteIsSpecified { get { return !string.IsNullOrEmpty(_quote); } }
        public string Quote { get { return _quote; } }
        private readonly string _quote;

        public bool NewLineIsSpecified { get { return !string.IsNullOrEmpty(_newLine); } }
        public string NewLine { get { return _newLine; } }
        private readonly string _newLine;
    }
}