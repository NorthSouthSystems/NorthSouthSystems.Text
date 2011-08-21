using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SoftwareBotany.Ivy
{
    /// <summary>
    /// StringSchemaEntry is a class to hold the immutable data for a single Entry in a StringSchema with Widths
    /// being the property return when enumerated.
    /// <see cref="SoftwareBotany.Ivy.StringSchemaExtensions">StringSchemaExtensions</see> explains a "schema'd" sequence of characters.
    /// </summary>
    public sealed class StringSchemaEntry : IEnumerable<int>
    {
        /// <inheritdoc cref="StringSchemaEntry(string, int[], char)"/>
        public StringSchemaEntry(string header, int[] widths)
            : this(header, widths, ' ')
        { }

        /// <param name="header">
        /// The text used in determining if a row is of this Entry's "type". A row that StartsWith this Header will
        /// be processed using the Widths and FillCharacter found in this StringSchemaEntry.
        /// </param>
        /// <param name="widths">The width of each column in the fixed-width column row.</param>
        /// <param name="fillCharacter">The character used to pad a string so that its width reaches its column's width. Trimmed from the split results. (default = ' ')</param>
        public StringSchemaEntry(string header, int[] widths, char fillCharacter)
        {
            if (string.IsNullOrEmpty(header))
                throw new ArgumentException("Must be non-null non-empty.", "header");

            StringFixedExtensions.VerifyWidths(widths);

            _header = header;
            _widths = widths;
            _fillCharacter = fillCharacter;
        }

        public string Header { get { return _header; } }
        private readonly string _header;

        internal int[] Widths { get { return _widths; } }
        private readonly int[] _widths;

        public char FillCharacter { get { return _fillCharacter; } }
        private readonly char _fillCharacter;

        public IEnumerator<int> GetEnumerator() { return ((IList<int>)_widths).GetEnumerator(); }

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator() { return _widths.GetEnumerator(); }
    }
}