using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SoftwareBotany.Ivy
{
    public sealed class StringSchemaEntry : IEnumerable<int>
    {
        public StringSchemaEntry(string header, params int[] widths)
            : this(header, ' ', widths)
        { }

        public StringSchemaEntry(string header, char fillCharacter, params int[] widths)
        {
            if (string.IsNullOrEmpty(header))
                throw new ArgumentException("Must be non-null non-empty.", "header");

            StringFixedExtensions.VerifyWidths(widths);

            _header = header;
            _fillCharacter = fillCharacter;
            _widths = widths;
        }

        public string Header { get { return _header; } }
        private readonly string _header;

        public char FillCharacter { get { return _fillCharacter; } }
        private readonly char _fillCharacter;

        internal int[] Widths { get { return _widths; } }
        private readonly int[] _widths;

        public IEnumerator<int> GetEnumerator() { return ((IList<int>)_widths).GetEnumerator(); }

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator() { return _widths.GetEnumerator(); }
    }
}