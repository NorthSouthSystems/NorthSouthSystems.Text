using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    public static partial class StringFixedExtensions
    {
        public static string[] SplitFixedLine(this IEnumerable<char> line, params int[] widths)
        {
            return SplitFixedLine(line, ' ', widths);
        }

        public static string[] SplitFixedLine(this IEnumerable<char> line, char fillCharacter, params int[] widths)
        {
            if (line == null)
                throw new ArgumentNullException("line");

            VerifyWidths(widths);

            string[] split;

            using (var charEnumerator = line.GetEnumerator())
            {
                split = SplitFixedImplementation(charEnumerator, fillCharacter, widths);

                if (split == null)
                    throw new ArgumentException("Empty enumerable.", "line");

                if (charEnumerator.MoveNext())
                    throw new ArgumentException("line length must equal the sum of all widths.", "line");
            }

            return split;
        }

        public static IEnumerable<string[]> SplitFixedLines(this IEnumerable<IEnumerable<char>> lines, params int[] widths)
        {
            return SplitFixedLines(lines, ' ', widths);
        }

        public static IEnumerable<string[]> SplitFixedLines(this IEnumerable<IEnumerable<char>> lines, char fillCharacter, params int[] widths)
        {
            if (lines == null)
                throw new ArgumentNullException("lines");

            VerifyWidths(widths);

            foreach (string line in lines)
            {
                if (line == null)
                    throw new ArgumentNullException("lines", "All lines must be non-null.");

                yield return SplitFixedLine(line, fillCharacter, widths);
            }
        }

        public static IEnumerable<string[]> SplitFixedLinesStream(this IEnumerable<char> lines, params int[] widths)
        {
            return SplitFixedLinesStream(lines, ' ', widths);
        }

        public static IEnumerable<string[]> SplitFixedLinesStream(this IEnumerable<char> lines, char fillCharacter, params int[] widths)
        {
            if (lines == null)
                throw new ArgumentNullException("lines");

            VerifyWidths(widths);

            using (var charEnumerator = lines.GetEnumerator())
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