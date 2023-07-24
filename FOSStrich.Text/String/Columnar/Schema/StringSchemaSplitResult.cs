namespace FOSStrich.Text;

/// <summary>
/// StringSchemaSplitResult holds both Schema data and Split result data.
/// </summary>
public sealed class StringSchemaSplitResult
{
    internal StringSchemaSplitResult(StringSchemaEntry entry, string[] fields)
    {
        _entry = entry;
        _result = entry.RowWrapperFactory.Wrap(fields);
    }

    public StringSchemaEntry Entry => _entry;
    private readonly StringSchemaEntry _entry;

    public StringRowWrapper Result => _result;
    private readonly StringRowWrapper _result;
}