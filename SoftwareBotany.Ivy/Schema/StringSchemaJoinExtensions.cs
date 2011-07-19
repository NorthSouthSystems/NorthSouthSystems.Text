using System;

namespace SoftwareBotany.Ivy
{
    public static partial class StringSchemaExtensions
    {
        public static string JoinSchemaLine(this StringSchemaEntryAndColumns entryAndColumns)
        {
            if (entryAndColumns == null)
                throw new ArgumentNullException("entryAndColumns");

            return entryAndColumns.Entry.Header
                + StringFixedExtensions.JoinFixedImplementation(entryAndColumns.Columns, entryAndColumns.Entry.FillCharacter, entryAndColumns.Entry.Widths);
        }
    }
}