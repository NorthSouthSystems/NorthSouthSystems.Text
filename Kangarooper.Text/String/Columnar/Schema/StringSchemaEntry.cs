namespace Kangarooper.Text
{
    using System;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// StringSchemaEntry is a class to hold the immutable data for a single Entry in a StringSchema.
    /// <see cref="Kangarooper.Text.StringSchemaExtensions">StringSchemaExtensions</see> explains a "schema'd" sequence of characters.
    /// </summary>
    public sealed class StringSchemaEntry
    {
        /// <param name="header">
        /// The text used in determining if a row is of this Entry's "type". A row that StartsWith this Header will
        /// be processed using the columnWidths and fillCharacter found in this StringSchemaEntry.
        /// </param>
        /// <param name="columnWidths">The width of each column in the fixed-width column row.</param>
        /// <param name="fillCharacter">The character used to pad a field so that its width reaches its column's width. Trimmed from the split results. (default = ' ')</param>
        /// <param name="columnNames">
        /// The names of all of the columns in the schema if available. Split results will be wrapped in StringRowWrappers which allow
        /// indexed and columnName based access to fields. (default = null)
        /// </param>
        public StringSchemaEntry(string header, int[] columnWidths, char fillCharacter = ' ', string[] columnNames = null)
        {
            if (string.IsNullOrEmpty(header))
                throw new ArgumentException("Must be non-null non-empty.", "header");

            StringFixedExtensions.VerifyColumnWidths(columnWidths);

            if (columnNames != null && columnNames.Length > 0 && columnNames.Length != columnWidths.Length)
                throw new ArgumentException("columnNames, if provided, must be the same length as columnWidths.", "columnNames");

            _header = header;
            _widths = columnWidths;
            _fillCharacter = fillCharacter;

            columnNames = (columnNames != null && columnNames.Length > 0)
                ? columnNames
                : Enumerable.Range(0, columnWidths.Length).Select(index => index.ToString(CultureInfo.InvariantCulture)).ToArray();

            _rowWrapperFactory = new StringRowWrapperFactory(columnNames);
        }

        public string Header { get { return _header; } }
        private readonly string _header;

        internal int[] Widths { get { return _widths; } }
        private readonly int[] _widths;

        public char FillCharacter { get { return _fillCharacter; } }
        private readonly char _fillCharacter;

        internal StringRowWrapperFactory RowWrapperFactory { get { return _rowWrapperFactory; } }
        private readonly StringRowWrapperFactory _rowWrapperFactory;
    }
}