using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    public sealed class StringSchemaEntryAndStrings : IEnumerable<string>
    {
        public StringSchemaEntryAndStrings(StringSchemaEntry entry, IEnumerable<string> strings)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");

            if (strings == null)
                throw new ArgumentNullException("strings");

            _entry = entry;
            _strings = strings.ToArray();
        }

        public StringSchemaEntry Entry { get { return _entry; } }
        private readonly StringSchemaEntry _entry;

        internal string[] Strings { get { return _strings; } }
        private readonly string[] _strings;

        public IEnumerator<string> GetEnumerator() { return ((IList<string>)_strings).GetEnumerator(); }

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator() { return _strings.GetEnumerator(); }
    }
}