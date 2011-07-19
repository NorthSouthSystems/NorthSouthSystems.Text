using System;
using System.Globalization;
using System.Text;

namespace SoftwareBotany.Ivy
{
    /// <summary>
    /// Extension methods for Splitting and Joining sequences of characters with fixed-width columns.
    /// </summary>
    public static partial class StringFixedExtensions
    {
        /// <inheritdoc cref="JoinFixedLine(string[], char, int[])"/>
        public static string JoinFixedLine(this string[] columns, params int[] widths)
        {
            return JoinFixedLine(columns, ' ', widths);
        }

        /// <summary>
        /// Joins an array of strings together so that each string is "filled" to occupy a column with an exact width.
        /// </summary>
        /// <param name="fillCharacter">When a string is less than its column's width, fillCharacter is added until the width is reached.</param>
        /// <param name="widths">The width of each column. E.g. The first string in strings will be place into a column the size of the first width in widths.</param>
        public static string JoinFixedLine(this string[] columns, char fillCharacter, params int[] widths)
        {
            if (columns == null)
                throw new ArgumentNullException("columns");

            VerifyWidths(widths);

            if (columns.Length != widths.Length)
                throw new ArgumentException("Number of columns to be joined must equal number of widths.");

            return JoinFixedImplementation(columns, fillCharacter, widths);
        }

        internal static string JoinFixedImplementation(string[] columns, char fillCharacter, int[] widths)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < columns.Length; i++)
            {
                string column = columns[i];
                int width = widths[i];

                if (column.Length > width)
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Each column's length must be <= to its corresponding width. Zero-based column: {0}. Column length: {1}. Width: {2}.", i, column.Length, width));

                result.Append(column);

                int fillCount = width - column.Length;

                if (fillCount > 0)
                    result.Append(fillCharacter, fillCount);
            }

            return result.ToString();
        }
    }
}