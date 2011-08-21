using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    public static partial class StringFixedExtensions
    {
        internal static void VerifyWidths(int[] widths)
        {
            if (widths == null)
                throw new ArgumentNullException("widths");

            if (widths.Length == 0)
                throw new ArgumentException("widths.Length must be > 0", "widths");

            if (widths.Any(width => width <= 0))
                throw new ArgumentOutOfRangeException("widths", "Each width must be > 0");
        }

        internal static void VerifyAndFitColumns(string[] columns, int[] widths, bool substringToFit)
        {
            if (columns == null)
                throw new ArgumentNullException("columns");

            if (columns.Length != widths.Length)
                throw new ArgumentException("Number of columns must equal number of widths.");

            List<string> errors = new List<string>();

            for (int i = 0; i < columns.Length; i++)
            {
                if (substringToFit)
                    columns[i] = columns[i].SubstringToFit(widths[i]);
                else if (columns[i].Length > widths[i])
                    errors.Add(string.Format(CultureInfo.InvariantCulture, "Column index (0-based): {0}. Column length: {1}. Width: {2}.", i, columns[i].Length, widths[i]));
            }

            if (errors.Count > 0)
                throw new ArgumentOutOfRangeException("columns", string.Format(CultureInfo.InvariantCulture, "Each column's length must be <= to its corresponding width (use substringToFit = true if truncation is allowed).{0}{1}", Environment.NewLine, string.Join(Environment.NewLine, errors)));
        }
    }
}