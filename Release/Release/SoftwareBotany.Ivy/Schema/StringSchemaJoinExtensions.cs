using System;

namespace SoftwareBotany.Ivy
{
    /// <summary>
    /// Extension methods for Splitting and Joining rows, sequences of chars, arranged in fixed-width columns.
    /// The number and size of columns is determined by a StringSchema and its StringSchemaEntrys. Rows must StartsWith
    /// the Header of exactly one Entry from a StringSchema. That Entry then contains the Widths
    /// to use when processing that row along with the designated FillCharacter.
    /// </summary>
    public static partial class StringSchemaExtensions
    {
        /// <summary>
        /// Joins the set of string Columns using the StringSchemaEntry for instructions.
        /// Both are contained within the StringSchemaEntryAndColumns parameter.
        /// </summary>
        /// <example>
        /// <code>
        /// var a = new StringSchemaEntry("A", new[] { 1, 1, 1 });
        /// var b = new StringSchemaEntry("B", new[] { 2, 2, 2 });
        /// var c = new StringSchemaEntry("C", new[] { 2, 2, 2 }, '-');
        ///
        /// var split = new StringSchemaEntryAndColumns(a, new[] { "1", "2", "3" });
        /// string join = split.JoinSchemaRow();
        /// Console.WriteLine(join);
        ///
        /// split = new StringSchemaEntryAndColumns(b, new[] { "12", "34", "56" });
        /// join = split.JoinSchemaRow();
        /// Console.WriteLine(join);
        ///
        /// split = new StringSchemaEntryAndColumns(c, new[] { "1", "2", "3" });
        /// join = split.JoinSchemaRow();
        /// Console.WriteLine(join);
        /// </code>
        /// Console Output:
        /// <code>
        /// A123
        /// B123456
        /// C1-2-3-
        /// </code>
        /// </example>
        public static string JoinSchemaRow(this StringSchemaEntryAndColumns entryAndColumns)
        {
            if (entryAndColumns == null)
                throw new ArgumentNullException("entryAndColumns");

            return entryAndColumns.Entry.Header
                + StringFixedExtensions.JoinFixedImplementation(entryAndColumns.Columns, entryAndColumns.Entry.Widths, entryAndColumns.Entry.FillCharacter);
        }
    }
}