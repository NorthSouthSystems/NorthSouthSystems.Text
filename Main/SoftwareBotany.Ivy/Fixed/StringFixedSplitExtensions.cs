using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    public static partial class StringFixedExtensions
    {
        /// <inheritdoc cref="SplitFixedRow(IEnumerable{char}, int[], char)"/>
        public static string[] SplitFixedRow(this IEnumerable<char> row, int[] widths) { return SplitFixedRow(row, widths, ' '); }

        /// <summary>
        /// Splits a row, sequence of chars, that are arranged in columns of fixed widths. The fillCharacter used to pad
        /// strings to fill their column's width are trimmed from the split results.
        /// </summary>
        /// <param name="widths">The width of each column. E.g. The first string in the result will be found in a column the size of the first width in widths.</param>
        /// <param name="fillCharacter">The character used to pad a string so that its width reaches its column's width. Trimmed from the split results. (default = ' ')</param>
        /// <example>
        /// <code>
        /// string row = "ABC";
        /// string[] columns = row.SplitFixedRow(new [] { 1, 1, 1 });
        /// 
        /// foreach(string column in columns)
        ///     Console.WriteLine(column);
        /// 
        /// columns = row.SplitFixedRow(new [] { 1, 1, 1 }, '-');
        /// 
        /// foreach(string column in columns)
        ///     Console.WriteLine(column);
        /// </code>
        /// Console Output:<br/>
        /// A<br/>
        /// B<br/>
        /// C<br/>
        /// A<br/>
        /// B<br/>
        /// C<br/>
        /// <code>
        /// string row = "A-BC";
        /// string[] columns = row.SplitFixedRow(new [] { 2, 1, 1 });
        /// 
        /// foreach(string column in columns)
        ///     Console.WriteLine(column);
        /// 
        /// columns = row.SplitFixedRow(new [] { 2, 1, 1 }, '-');
        /// 
        /// foreach(string column in columns)
        ///     Console.WriteLine(column);
        /// </code>
        /// Console Output:<br/>
        /// A-<br/>
        /// B<br/>
        /// C<br/>
        /// A<br/>
        /// B<br/>
        /// C<br/>
        /// <code>
        /// string row = "A-B-C";
        /// string[] columns = row.SplitFixedRow(new [] { 2, 2, 1 });
        /// 
        /// foreach(string column in columns)
        ///     Console.WriteLine(column);
        /// 
        /// columns = row.SplitFixedRow(new [] { 2, 2, 1 }, '-');
        /// 
        /// foreach(string column in columns)
        ///     Console.WriteLine(column);
        /// </code>
        /// Console Output:<br/>
        /// A-<br/>
        /// B-<br/>
        /// C<br/>
        /// A<br/>
        /// B<br/>
        /// C<br/>
        /// </example>
        public static string[] SplitFixedRow(this IEnumerable<char> row, int[] widths, char fillCharacter)
        {
            if (row == null)
                throw new ArgumentNullException("row");

            VerifyWidths(widths);

            string[] split;

            using (var charEnumerator = row.GetEnumerator())
            {
                split = SplitFixedImplementation(charEnumerator, widths, fillCharacter);

                if (split == null)
                    throw new ArgumentException("Empty row.", "row");

                if (charEnumerator.MoveNext())
                    throw new ArgumentOutOfRangeException("row", "row length must equal the sum of all widths.");
            }

            return split;
        }

        /// <inheritdoc cref="SplitFixedRepeating(IEnumerable{char}, int[], char)"/>
        public static IEnumerable<string[]> SplitFixedRepeating(this IEnumerable<char> rows, int[] widths) { return SplitFixedRepeating(rows, widths, ' '); }

        /// <summary>
        /// See <see cref="SplitFixedRow(IEnumerable{char}, int[], char)"/>.  This method is identical, except that it allow rows to repeat one after another.
        /// As soon as the numbers of characters taken reaches the sum of all widths, a new row is started.
        /// </summary>
        /// <example>
        /// <code>
        /// string rows = "ABCDEF";
        /// string[][] rowsColumns = row.SplitFixedRepeating(new [] { 1, 1, 1 });
        /// 
        /// foreach(string[] rowColumns in rowsColumns)
        /// {
        ///     Console.WriteLine("Row");
        ///     
        ///     foreach(string column in rowColumns)
        ///         Console.WriteLine(column);
        /// }
        /// </code>
        /// Console Output:<br/>
        /// Row<br/>
        /// A<br/>
        /// B<br/>
        /// C<br/>
        /// Row<br/>
        /// D<br/>
        /// E<br/>
        /// F<br/>
        /// <code>
        /// string rows = "A-BCD-EF";
        /// string[][] rowsColumns = row.SplitFixedRepeating(new [] { 2, 1, 1 }, '-');
        /// 
        /// foreach(string[] rowColumns in rowsColumns)
        /// {
        ///     Console.WriteLine("Row");
        ///     
        ///     foreach(string column in rowColumns)
        ///         Console.WriteLine(column);
        /// }
        /// </code>
        /// Console Output:<br/>
        /// Row<br/>
        /// A<br/>
        /// B<br/>
        /// C<br/>
        /// Row<br/>
        /// D<br/>
        /// E<br/>
        /// F<br/>
        /// </example>
        public static IEnumerable<string[]> SplitFixedRepeating(this IEnumerable<char> rows, int[] widths, char fillCharacter)
        {
            if (rows == null)
                throw new ArgumentNullException("rows");

            VerifyWidths(widths);

            using (var charEnumerator = rows.GetEnumerator())
            {
                string[] split = null;

                do
                {
                    split = SplitFixedImplementation(charEnumerator, widths, fillCharacter);

                    if (split != null)
                        yield return split;
                }
                while (split != null);
            }
        }

        internal static string[] SplitFixedImplementation(IEnumerator<char> charEnumerator, int[] widths, char fillCharacter)
        {
            string[] results = new string[widths.Length];

            for (int i = 0; i < widths.Length; i++)
            {
                int charsToTake = widths[i];
                char[] charsTaken = new char[charsToTake];

                while (charsToTake > 0)
                {
                    if (charEnumerator.MoveNext())
                    {
                        charsTaken[charsTaken.Length - charsToTake] = charEnumerator.Current;
                        charsToTake--;
                    }
                    else if (i == 0 && charsToTake == widths[0]) // Empty enumerator
                        return null;
                    else
                        throw new ArgumentOutOfRangeException("charEnumerator", "row length must equal the sum of all widths.");
                }

                int charsToKeep = widths[i];

                while (charsToKeep > 0 && charsTaken[charsToKeep - 1] == fillCharacter)
                    charsToKeep--;

                results[i] = charsToKeep > 0 ? charsTaken.Take(charsToKeep).ToNewString() : string.Empty;
            }

            return results;
        }
    }
}