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
        public static string JoinFixedLine(this string[] strings, params int[] widths)
        {
            return JoinFixedLine(strings, ' ', widths);
        }

        /// <summary>
        /// Joins an array of strings together so that each string is "filled" to occupy a column with an exact width.
        /// </summary>
        /// <param name="fillCharacter">When a string is less than its column's width, fillCharacter is added until the width is reached.</param>
        /// <param name="widths">The width of each column. E.g. The first string in strings will be place into a column the size of the first width in widths.</param>
        public static string JoinFixedLine(this string[] strings, char fillCharacter, params int[] widths)
        {
            if (strings == null)
                throw new ArgumentNullException("strings");

            VerifyWidths(widths);

            if (strings.Length != widths.Length)
                throw new ArgumentException("Number of strings to be joined must equal number of widths.");

            return JoinFixedImplementation(strings, fillCharacter, widths);
        }

        internal static string JoinFixedImplementation(string[] strings, char fillCharacter, int[] widths)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < strings.Length; i++)
            {
                string s = strings[i];
                int width = widths[i];

                if (s.Length > width)
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Each string's length must be <= to its corresponding column's width. Zero-based column: {0}. String length: {1}. Column width: {2}.", i, s.Length, width));

                result.Append(s);

                int fillCount = width - s.Length;

                if (fillCount > 0)
                    result.Append(fillCharacter, fillCount);
            }

            return result.ToString();
        }
    }
}