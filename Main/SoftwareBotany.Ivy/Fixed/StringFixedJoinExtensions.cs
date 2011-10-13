using System.Text;

namespace SoftwareBotany.Ivy
{
    /// <summary>
    /// Extension methods for Splitting and Joining rows, sequences of chars, arranged in fixed-width columns.
    /// </summary>
    public static partial class StringFixedExtensions
    {
        /// <inheritdoc cref="JoinFixedRow(string[], int[], char, bool)"/>
        public static string JoinFixedRow(this string[] columns, int[] widths) { return JoinFixedRow(columns, widths, ' ', false); }

        /// <summary>
        /// Joins an array of strings together so that each string is "filled" to occupy a column with an exact width.
        /// </summary>
        /// <param name="widths">The width of each column. E.g. The first string in columns will be place into a column the size of the first width in widths.</param>
        /// <param name="fillCharacter">When a string is less than its column's width, fillCharacter is added until the width is reached. (default = ' ')</param>
        /// <param name="substringToFit">Determines whether to substring a string to fit its column's width or throw an exception when a column exceeds the allowable width. (default = false)</param>
        /// <example>
        /// <code>
        /// string columns = new [] { "A", "B", "C" };
        /// string row = columns.JoinFixedRow(new [] { 1, 1, 1 });
        /// Console.WriteLine(row);
        /// 
        /// row = columns.JoinFixedRow(new [] { 1, 1, 1 }, '-', false);
        /// Console.WriteLine(row);
        /// </code>
        /// Console Output:<br/>
        /// ABC<br/>
        /// ABC<br/>
        /// <code>
        /// string columns = new [] { "A", "B", "C" };
        /// string row = columns.JoinFixedRow(new [] { 2, 1, 1 });
        /// Console.WriteLine(row);
        /// 
        /// row = columns.JoinFixedRow(new [] { 2, 1, 1 }, '-', false);
        /// Console.WriteLine(row);
        /// </code>
        /// Console Output:<br/>
        /// A BC<br/>
        /// A-BC<br/>
        /// <code>
        /// string columns = new [] { "A", "B", "C" };
        /// string row = columns.JoinFixedRow(new [] { 2, 2, 1 });
        /// Console.WriteLine(row);
        /// 
        /// row = columns.JoinFixedRow(new [] { 2, 2, 1 }, '-', false);
        /// Console.WriteLine(row);
        /// </code>
        /// Console Output:<br/>
        /// A B C<br/>
        /// A-B-C<br/>
        /// <code>
        /// string columns = new [] { "ABC", "123" };
        /// string row = columns.JoinFixedRow(new [] { 2, 2 }, ' ', true);
        /// Console.WriteLine(row);
        /// </code>
        /// Console Output:<br/>
        /// AB12<br/>
        /// </example>
        public static string JoinFixedRow(this string[] columns, int[] widths, char fillCharacter, bool substringToFit)
        {
            VerifyWidths(widths);
            VerifyAndFitColumns(columns, widths, substringToFit);

            return JoinFixedImplementation(columns, widths, fillCharacter);
        }

        internal static string JoinFixedImplementation(string[] columns, int[] widths, char fillCharacter)
        {
            StringBuilder row = new StringBuilder();

            for (int i = 0; i < columns.Length; i++)
            {
                string column = columns[i];
                row.Append(column);

                int fillCount = widths[i] - column.Length;

                if (fillCount > 0)
                    row.Append(fillCharacter, fillCount);
            }

            return row.ToString();
        }
    }
}