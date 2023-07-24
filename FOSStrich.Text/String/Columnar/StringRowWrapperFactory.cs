namespace FOSStrich.Text;

using System.Globalization;

/// <summary>
/// StringRowWrapperFactory is the only path for creating StringRowWrappers. Given columnNames, it can then be used
/// to wrap the fields of a row in a StringRowWrapper. StringRowWrappers then allow for index or column name based
/// access to the fields of a row.
/// </summary>
public sealed class StringRowWrapperFactory
{
    public StringRowWrapperFactory(string[] columnNames)
    {
        if (columnNames == null)
            throw new ArgumentNullException(nameof(columnNames));

        string[] duplicateColumnNames = columnNames.GroupBy(columnName => columnName)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToArray();

        if (duplicateColumnNames.Length > 0)
            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Duplicate columnNames are not allowed: {0}.", string.Join(", ", duplicateColumnNames)));

        _columnNames = new string[columnNames.Length];
        Array.Copy(columnNames, _columnNames, columnNames.Length);

        int index = 0;
        _columnNameIndices = _columnNames.ToDictionary(columnName => columnName, columnName => index++);
    }

    internal string[] ColumnNames => _columnNames;
    private readonly string[] _columnNames;

    private readonly Dictionary<string, int> _columnNameIndices;

    public StringRowWrapper Wrap(string[] fields)
    {
        if (fields == null)
            throw new ArgumentNullException(nameof(fields));

        if (fields.Length > _columnNames.Length)
            throw new ArgumentException("The number of fields must be <= the number of columns.");

        return new StringRowWrapper(this, fields);
    }

    internal bool TryGetIndex(string columnName, out int index) => _columnNameIndices.TryGetValue(columnName, out index);
}