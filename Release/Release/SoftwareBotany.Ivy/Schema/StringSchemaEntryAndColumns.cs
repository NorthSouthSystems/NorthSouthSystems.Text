using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    /// <summary>
    /// StringSchemaEntryAndColumns holds both Schema data and column data. This allows us to provide a StringSchemaEntryAndColumns
    /// instance to a Join function and be returned a row, or split a row into a StringSchemaEntryAndColumns instance.
    /// When enumerated, the enumeration is delegated to the Columns property.
    /// </summary>
    public sealed class StringSchemaEntryAndColumns : IEnumerable<string>
    {
        /// <inheritdoc cref="StringSchemaEntryAndColumns(StringSchemaEntry, IEnumerable{string}, bool)"/>
        public StringSchemaEntryAndColumns(StringSchemaEntry entry, IEnumerable<string> columns)
            : this(entry, columns, false)
        { }

        /// <param name="entry">The Entry to use when Join'ing; the Entry used when Splitting.</param>
        /// <param name="columns">The columns to Join; the columns resulting for a Split.</param>
        /// <param name="substringToFit">If Join'ing, substring columns to fit into their respective columns' widths.</param>
        public StringSchemaEntryAndColumns(StringSchemaEntry entry, IEnumerable<string> columns, bool substringToFit)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");

            if (columns == null)
                throw new ArgumentNullException("columns");

            _entry = entry;
            _columns = columns.ToArray();

            StringFixedExtensions.VerifyAndFitColumns(_columns, _entry.Widths, substringToFit);
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