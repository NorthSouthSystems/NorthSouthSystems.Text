using System;

namespace SoftwareBotany.Ivy
{
    public static partial class StringSchemaExtensions
    {
        public static string JoinSchemaLine(this StringSchemaEntryAndStrings entryAndStrings)
        {
            if (entryAndStrings == null)
                throw new ArgumentNullException("entryAndStrings");

            return entryAndStrings.Entry.Header
                + StringFixedExtensions.JoinFixedImplementation(entryAndStrings.Strings, entryAndStrings.Entry.FillCharacter, entryAndStrings.Entry.Widths);
        }
    }
}