using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareBotany.Ivy
{
    public static class StringPositionalJoinExtensions
    {
        public static string JoinPositional(this string[] value, char fillCharacter, params int[] lengths)
        {
            StringPositionalHelpers.SplitOrJoinPositionalVerifyLengths(lengths);

            if (value.Length != lengths.Length)
                throw new ArgumentException("Number of strings to be joined must equal number of lengths.");

            return JoinPositionalImplementation(value, fillCharacter, lengths);
        }

        public static string JoinPositionalSchema(this KeyValuePair<string, string[]> value, char fillCharacter, StringPositionalSchema schema)
        {
            return value.Key + JoinPositionalImplementation(value.Value, fillCharacter, schema[value.Key]);
        }

        private static string JoinPositionalImplementation(string[] value, char fillCharacter, int[] lengths)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < value.Length; i++)
            {
                string s = value[i];
                int length = lengths[i];

                if (s.Length > length)
                    throw new ArgumentException("A string's length must be <= to its corresponding position's length. Zero-based position: " + i.ToString());

                result.Append(s);

                int fillCount = length - s.Length;

                if (fillCount > 0)
                    result.Append(fillCharacter, fillCount);
            }

            return result.ToString();
        }
    }
}