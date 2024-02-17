namespace NorthSouthSystems.Text;

public static class WithColumnHeadersExtensions
{
    /// <summary>
    /// Provides an extension method for converting rows of fields (IEnumerable<string[]>) to
    /// StringRowWrappers (IEnumerable<StringRowWrapper) by using the first row to construct
    /// a StringRowWrapperFactory used to wrap subsequent rows. This is intended to be used fluently
    /// after a call to any of the Split*Rows extensions methods.
    /// </summary>
    /// <param name="rowsOfFields">The rows of fields resulting from a Split*Rows invocation.</param>
    /// <param name="expectedColumnNames">An optional parameter indicating that the caller would like
    /// an ArgumentException thrown if the first row is not this exact set of fields.
    /// (default = null)</param>
    /// <param name="enforceExpectedColumnNamesOrder">An optional parameter indicating that the caller
    /// would like an ArgumentException thrown if the first row is not the exact sequence of fields
    /// specified by expectedColumnNames. (default = false)</param>
    /// <returns>The rows of fields converted to StringRowWrappers excluding the first row representing
    /// column headers.</returns>
    public static IEnumerable<StringRowWrapper> WithColumnHeaders(this IEnumerable<string[]> rowsOfFields,
        IEnumerable<string> expectedColumnNames = null, bool enforceExpectedColumnNamesOrder = false)
    {
        if (rowsOfFields == null)
            throw new ArgumentNullException(nameof(rowsOfFields));

        return WithColumnHeadersIterator(rowsOfFields, expectedColumnNames, enforceExpectedColumnNamesOrder);
    }

    private static IEnumerable<StringRowWrapper> WithColumnHeadersIterator(IEnumerable<string[]> rowsOfFields,
        IEnumerable<string> expectedColumnNames, bool enforceExpectedColumnNamesOrder)
    {
        StringRowWrapperFactory rowWrapperFactory = null;
        int rowIndex = 0;

        foreach (string[] rowFields in rowsOfFields)
        {
            if (rowWrapperFactory == null)
            {
                if (expectedColumnNames?.Any() ?? false)
                    ThrowIfIncorrectColumnNames(rowFields, expectedColumnNames, enforceExpectedColumnNamesOrder);

                rowWrapperFactory = new StringRowWrapperFactory(rowFields);
            }
            else
            {
                StringRowWrapper rowWrapper;

                try { rowWrapper = rowWrapperFactory.Wrap(rowFields); }
                catch (Exception exception)
                {
                    throw new ArgumentException(
                        FormattableString.Invariant($"Error wrapping fields for row with 0-based index {rowIndex}."),
                        exception);
                }

                yield return rowWrapper;
            }

            rowIndex++;
        }
    }

    private static void ThrowIfIncorrectColumnNames(string[] rowFields,
        IEnumerable<string> expectedColumnNames, bool enforceExpectedColumnNamesOrder)
    {
        var errors = new List<string>();

        errors.AddRange(expectedColumnNames.Except(rowFields).Select(c => "Expected: " + c));
        errors.AddRange(rowFields.Except(expectedColumnNames).Select(c => "Unexpected: " + c));

        if (errors.Count == 0 && enforceExpectedColumnNamesOrder && !rowFields.SequenceEqual(expectedColumnNames))
            errors.Add("Column names are not in the expected order.");

        if (errors.Count > 0)
            throw new ArgumentException(string.Join(Environment.NewLine, errors));
    }
}