namespace NorthSouthSystems.Text;

using System.Globalization;

public static partial class StringFixedExtensions
{
    internal static void VerifyColumnWidths(int[] columnWidths)
    {
        if (columnWidths == null)
            throw new ArgumentNullException(nameof(columnWidths));

        if (columnWidths.Length == 0)
            throw new ArgumentException("Length must be > 0", nameof(columnWidths));

        if (columnWidths.Any(width => width <= 0))
            throw new ArgumentOutOfRangeException(nameof(columnWidths), "Each column width must be > 0");
    }

    internal static void VerifyCoalesceAndFitFields(string[] fields, int[] columnWidths, bool leftToFit)
    {
        if (fields == null)
            throw new ArgumentNullException(nameof(fields));

        if (fields.Length != columnWidths.Length)
            throw new ArgumentException("Number of fields must equal number of column widths.");

        var errors = new List<string>();

        for (int i = 0; i < fields.Length; i++)
        {
            fields[i] = fields[i] ?? string.Empty;

            if (leftToFit)
                fields[i] = fields[i].Left(columnWidths[i]);
            else if (fields[i].Length > columnWidths[i])
                errors.Add(string.Format(CultureInfo.InvariantCulture, "Column index (0-based): {0}. Column width: {1}. Field length: {2}.", i, columnWidths[i], fields[i].Length));
        }

        if (errors.Count > 0)
            throw new ArgumentOutOfRangeException(nameof(fields), string.Format(CultureInfo.InvariantCulture, "Each field's length must be <= to its corresponding column's width (use leftToFit = true if truncation is allowed).{0}{1}", Environment.NewLine, string.Join(Environment.NewLine, errors)));
    }
}