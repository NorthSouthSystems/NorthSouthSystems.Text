namespace Kangarooper.Text
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Wraps the fields in a row and then allows for index or column name based access to them.
    /// </summary>
    public sealed class StringRowWrapper
    {
        internal StringRowWrapper(StringRowWrapperFactory factory, string[] fields)
        {
            _factory = factory;

            _fields = new string[fields.Length];
            Array.Copy(fields, _fields, fields.Length);
        }

        private readonly StringRowWrapperFactory _factory;
        private readonly string[] _fields;

        public StringFieldWrapper this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index", "index must be >= 0.");

                if (index >= _factory.ColumnNames.Length)
                    throw new ArgumentOutOfRangeException("index", "index must be < the number of columns in the StringRowWrapperFactory.");

                return new StringFieldWrapper(_factory.ColumnNames[index], index < _fields.Length ? _fields[index] : null);
            }
        }

        public StringFieldWrapper this[string columnName]
        {
            get
            {
                int index;

                if (!_factory.TryGetIndex(columnName, out index))
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Column not found: {0}.", columnName), "columnName");

                return new StringFieldWrapper(_factory.ColumnNames[index], index < _fields.Length ? _fields[index] : null);
            }
        }

        public IEnumerable<StringFieldWrapper> Fields
        {
            get
            {
                var fillerFields = Enumerable.Range(_fields.Length, Math.Max(0, _factory.ColumnNames.Length - _fields.Length))
                    .Select(fillerFieldIndex => new StringFieldWrapper(_factory.ColumnNames[fillerFieldIndex], null));

                return Enumerable.Range(0, _fields.Length)
                    .Select(fieldIndex => new StringFieldWrapper(_factory.ColumnNames[fieldIndex], _fields[fieldIndex]))
                    .Concat(fillerFields);
            }
        }
    }
}