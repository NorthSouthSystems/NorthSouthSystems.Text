using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    public sealed class StringSchemaEntryAndColumns : IEnumerable<string>
    {
        public StringSchemaEntryAndColumns(StringSchemaEntry entry, IEnumerable<string> columns)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");

            if (columns == null)
                throw new ArgumentNullException("columns");

            _entry = entry;
            _columns = columns.ToArray();
        }

        public StringSchemaEntry Entry { get { return _entry; } }
        private readonly StringSchemaEntry _entry;

        internal string[] Columns { get { return _columns; } }
        private readonly string[] _columns;

        public IEnumerator<string> GetEnumerator() { return ((IList<string>)_columns).GetEnumerator(); }

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator() { return _columns.GetEnumerator(); }
    }
}