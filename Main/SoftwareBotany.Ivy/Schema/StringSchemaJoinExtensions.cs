using System;

namespace SoftwareBotany.Ivy
{
    public static partial class StringPositionalJoinExtensions
    {
        public static string JoinSchema(this StringSchemaEntryAndStrings entryAndStrings, char fillCharacter)
        {
            if (entryAndStrings == null)
                throw new ArgumentNullException("entryAndStrings");

            return entryAndStrings.Entry.Header + JoinPositionalImplementation(entryAndStrings.Strings, fillCharacter, entryAndStrings.Entry.Lengths);
        }
    }
}