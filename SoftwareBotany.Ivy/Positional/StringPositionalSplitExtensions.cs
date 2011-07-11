using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    public static partial class StringPositionalSplitExtensions
    {
        public static string[] SplitPositional(this string value, params int[] lengths)
        {
            StringPositionalHelpers.SplitOrJoinPositionalVerifyLengths(lengths);

            return SplitPositionalImplementation(value, lengths);
        }

        public static IEnumerable<string[]> SplitPositional(this IEnumerable<string> strings, params int[] lengths)
        {
            StringPositionalHelpers.SplitOrJoinPositionalVerifyLengths(lengths);

            foreach (string s in strings)
                yield return SplitPositionalImplementation(s, lengths);
        }

        public static IEnumerable<string[]> SplitPositionalRepeating(this string value, params int[] lengths)
        {
            StringPositionalHelpers.SplitOrJoinPositionalVerifyLengths(lengths);

            int lengthsSum = lengths.Sum();

            if (value.Length % lengthsSum != 0)
                throw new ArgumentException("String length must be a multiple the sum of all lengths.", "value");

            int iterations = value.Length / lengthsSum;

            for (int i = 0; i < iterations; i++)
                yield return SplitPositionalImplementation(value.Substring(i * lengthsSum, lengthsSum), lengths);
        }

        private static string[] SplitPositionalImplementation(string value, int[] lengths)
        {
            string[] results = new string[lengths.Length];

            int startIndex = 0;

            for (int i = 0; i < lengths.Length; i++)
            {
                int length = lengths[i];

                if (value.Length < startIndex + length)
                    throw new ArgumentException("String length must equal the sum of all lengths.", "value");

                results[i] = value.Substring(startIndex, length);
                startIndex += length;
            }

            if (value.Length != startIndex)
                throw new ArgumentException("String length must equal the sum of all lengths.", "value");

            return results;
        }
    }
}