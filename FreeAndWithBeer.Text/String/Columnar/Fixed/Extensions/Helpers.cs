namespace FreeAndWithBeer.Text
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public static partial class StringFixedExtensions
    {
        internal static void VerifyColumnWidths(int[] columnWidths)
        {
            if (columnWidths == null)
                throw new ArgumentNullException("columnWidths");

            if (columnWidths.Length == 0)
                throw new ArgumentException("columnWidths.Length must be > 0", "columnWidths");

            if (columnWidths.Any(width => width <= 0))
                throw new ArgumentOutOfRangeException("columnWidths", "Each column width must be > 0");
        }

        internal static void VerifyCoalesceAndFitFields(string[] fields, int[] columnWidths, bool substringToFit)
        {
            if (fields == null)
                throw new ArgumentNullException("fields");

            if (fields.Length != columnWidths.Length)
                throw new ArgumentException("Number of fields must equal number of column widths.");

            List<string> errors = new List<string>();

            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = fields[i] ?? string.Empty;

                if (substringToFit)
                    fields[i] = fields[i].SubstringToFit(columnWidths[i]);
                else if (fields[i].Length > columnWidths[i])
                    errors.Add(string.Format(CultureInfo.InvariantCulture, "Column index (0-based): {0}. Column width: {1}. Field length: {2}.", i, columnWidths[i], fields[i].Length));
            }

            if (errors.Count > 0)
                throw new ArgumentOutOfRangeException("fields", string.Format(CultureInfo.InvariantCulture, "Each field's length must be <= to its corresponding column's width (use substringToFit = true if truncation is allowed).{0}{1}", Environment.NewLine, string.Join(Environment.NewLine, errors)));
        }
    }
}