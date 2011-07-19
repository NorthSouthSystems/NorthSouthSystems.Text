using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    public static partial class StringFixedExtensions
    {
        public static string[] SplitFixedLine(this IEnumerable<char> chars, params int[] widths)
        {
            return SplitFixedLine(chars, ' ', widths);
        }

        public static string[] SplitFixedLine(this IEnumerable<char> chars, char fillCharacter, params int[] widths)
        {
            if (chars == null)
                throw new ArgumentNullException("chars");

            VerifyWidths(widths);

            string[] split;

            using (var charEnumerator = chars.GetEnumerator())
            {
                split = SplitFixedImplementation(charEnumerator, fillCharacter, widths);

                if (split == null)
                    throw new ArgumentException("Empty enumerable.", "chars");

                if (charEnumerator.MoveNext())
                    throw new ArgumentException("chars length must equal the sum of all widths.", "chars");
            }

            return split;
        }

        public static IEnumerable<string[]> SplitFixedLines(this IEnumerable<IEnumerable<char>> strings, params int[] widths)
        {
            return SplitFixedLines(strings, ' ', widths);
        }

        public static IEnumerable<string[]> SplitFixedLines(this IEnumerable<IEnumerable<char>> strings, char fillCharacter, params int[] widths)
        {
            if (strings == null)
                throw new ArgumentNullException("strings");

            VerifyWidths(widths);

            foreach (string s in strings)
            {
                if (s == null)
                    throw new ArgumentNullException("strings", "All strings must be non-null.");

                yield return SplitFixedLine(s, fillCharacter, widths);
            }
        }

        public static IEnumerable<string[]> SplitFixedLinesStream(this IEnumerable<char> chars, params int[] widths)
        {
            return SplitFixedLinesStream(chars, ' ', widths);
        }

        public static IEnumerable<string[]> SplitFixedLinesStream(this IEnumerable<char> chars, char fillCharacter, params int[] widths)
        {
            if (chars == null)
                throw new ArgumentNullException("chars");

            VerifyWidths(widths);

            using (var charEnumerator = chars.GetEnumerator())
            {
                string[] split = null;

                do
                {
                    split = SplitFixedImplementation(charEnumerator, fillCharacter, widths);

                    if (split != null)
                        yield return split;
                }
                while (split != null);
            }
        }

        internal static string[] SplitFixedImplementation(IEnumerator<char> charEnumerator, char fillCharacter, int[] widths)
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
                        throw new ArgumentException("chars length must equal the sum of all widths.");
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