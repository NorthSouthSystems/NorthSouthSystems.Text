using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SoftwareBotany.Ivy
{
    public sealed class StringPositionalSchemaEntry : IEnumerable<int>
    {
        public StringPositionalSchemaEntry(string header, int[] lengths)
        {
            if (string.IsNullOrEmpty(header))
                throw new ArgumentException("Must be non-null non-empty.", "header");

            StringPositionalHelpers.SplitOrJoinPositionalVerifyLengths(lengths);

            _header = header;
            _lengths = lengths;
        }

        public string Header { get { return _header; } }
        private readonly string _header;

        internal int[] Lengths { get { return _lengths; } }
        private readonly int[] _lengths;

        public IEnumerator<int> GetEnumerator() { return ((IList<int>)_lengths).GetEnumerator(); }

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator() { return _lengths.GetEnumerator(); }
    }
}