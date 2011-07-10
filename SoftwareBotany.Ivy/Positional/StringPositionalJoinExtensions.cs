using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SoftwareBotany.Ivy
{
    public static class StringPositionalJoinExtensions
    {
        public static string JoinPositional(this string[] strings, char fillCharacter, params int[] lengths)
        {
            if (strings == null)
                throw new ArgumentNullException("strings");

            StringPositionalHelpers.SplitOrJoinPositionalVerifyLengths(lengths);

            if (strings.Length != lengths.Length)
                throw new ArgumentException("Number of strings to be joined must equal number of lengths.");

            return JoinPositionalImplementation(strings, fillCharacter, lengths);
        }

        public static string JoinPositionalSchema(this StringPositionalSchemaEntryAndStrings entryAndStrings, char fillCharacter)
        {
            if (entryAndStrings == null)
                throw new ArgumentNullException("entryAndStrings");

            return entryAndStrings.Entry.Header + JoinPositionalImplementation(entryAndStrings.Strings, fillCharacter, entryAndStrings.Entry.Lengths);
        }

        private static string JoinPositionalImplementation(string[] strings, char fillCharacter, int[] lengths)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < strings.Length; i++)
            {
                string s = strings[i];
                int length = lengths[i];

                if (s.Length > length)
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Each string's length must be <= to its corresponding position's length. Zero-based position: {0}. String length: {1}. Max length: {2}.", i, s.Length, length));

                result.Append(s);

                int fillCount = length - s.Length;

                if (fillCount > 0)
                    result.Append(fillCharacter, fillCount);
            }

            return result.ToString();
        }
    }
}