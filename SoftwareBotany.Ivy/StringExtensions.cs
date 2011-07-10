using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    public static class StringExtensions
    {
        #region Empty / Null

        /// <summary>
        /// VIOLATES NULL REFERENCE SEMANTICS! Will return null if string.IsNullOrEmpty. This simplifies code that treats
        /// null string values the same as it treats Empty string values, and wants only to deal with null.
        /// </summary>
        public static string EmptyToNull(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return value;
        }

        /// <summary>
        /// VIOLATES NULL REFERENCE SEMANTICS! Will return string.Empty if string.IsNullOrEmpty. This simplifies code that treats
        /// null string values the same as it treats Empty string values, and wants only to deal with string.Empty.
        /// </summary>
        public static string NullToEmpty(this string value) { return value ?? string.Empty; }

        #endregion

        #region Cleaning

        /// <summary>
        /// Trims the left and right sides of a string while replacing any repeating instances of whitespace with a
        /// single instance of whitespace.
        /// </summary>
        public static string NormalizeWhiteSpace(this string value) { return new string(NormalizeWhiteSpace((IEnumerable<char>)value, Environment.NewLine).ToArray()); }

        public static string NormalizeWhiteSpace(this string value, string respectNewLine) { return new string(NormalizeWhiteSpace((IEnumerable<char>)value, respectNewLine).ToArray()); }

        public static IEnumerable<char> NormalizeWhiteSpace(this IEnumerable<char> chars) { return NormalizeWhiteSpace(chars, Environment.NewLine); }

        public static IEnumerable<char> NormalizeWhiteSpace(this IEnumerable<char> chars, string respectNewLine)
        {
            if (chars == null)
                throw new ArgumentNullException("chars");

            bool nonWhiteSpaceHit = false;
            bool bufferedSpace = false;
            bool bufferedNewLine = false;
            StringSignalTracker newLineTracker = new StringSignalTracker(respectNewLine);

            foreach (char c in chars)
            {
                if (char.IsWhiteSpace(c))
                {
                    bufferedSpace = true;
                }
                else
                {
                    if (bufferedNewLine)
                    {
                        if (nonWhiteSpaceHit)
                            foreach (char newLineChar in Environment.NewLine)
                                yield return newLineChar;

                        bufferedNewLine = false;
                        bufferedSpace = false;
                    }

                    if (bufferedSpace)
                    {
                        if (nonWhiteSpaceHit)
                            yield return ' ';

                        bufferedSpace = false;
                    }

                    yield return c;

                    nonWhiteSpaceHit = true;
                }

                newLineTracker.ProcessChar(c);

                if (newLineTracker.IsTriggered)
                {
                    newLineTracker.Reset();

                    bufferedNewLine = true;
                }
            }
        }

        /// <summary>
        /// Provides an adapter for string.Substring to conditionally call Substring only if the value's length exceeds
        /// a certain maxLength. This is helpful for situations where data must fit into the database; however, truncation
        /// is not considered an error and is acceptable.
        /// </summary>
        public static string SubstringToFit(this string value, int maxLength)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (maxLength < 0)
                throw new ArgumentOutOfRangeException("maxLength");

            if (value.Length <= maxLength)
                return value;

            return value.Substring(0, maxLength);
        }

        #endregion

        #region Casing

        public static string ToLowerCamelCase(this string value) { return new string(ToCamelCaseImplementation(value, true).ToArray()); }

        public static IEnumerable<char> ToLowerCamelCase(this IEnumerable<char> chars) { return ToCamelCaseImplementation(chars, true); }

        public static string ToUpperCamelCase(this string value) { return new string(ToCamelCaseImplementation(value, false).ToArray()); }

        public static IEnumerable<char> ToUpperCamelCase(this IEnumerable<char> chars) { return ToCamelCaseImplementation(chars, false); }

        private static IEnumerable<char> ToCamelCaseImplementation(IEnumerable<char> chars, bool isLower)
        {
            if (chars == null)
                throw new ArgumentNullException("chars");

            bool previousWhiteSpace = true;
            int outputIndex = 0;

            foreach (char c in chars)
            {
                if (!char.IsWhiteSpace(c))
                {
                    if (isLower && outputIndex == 0)
                        yield return char.ToLower(c);
                    else if (previousWhiteSpace)
                        yield return char.ToUpper(c);
                    else
                        yield return c;

                    previousWhiteSpace = false;
                    outputIndex++;
                }
                else
                {
                    yield return c;

                    previousWhiteSpace = true;
                }
            }
        }

        public static string SpaceCamelCase(this string value) { return new string(SpaceCamelCase((IEnumerable<char>)value).ToArray()); }

        public static IEnumerable<char> SpaceCamelCase(this IEnumerable<char> chars)
        {
            if (chars == null)
                throw new ArgumentNullException("chars");

            if (!chars.Any())
                yield break;

            bool first = true;

            bool previousIsDigit = false;
            bool previousIsLetter = false;
            bool previousIsLower = false;
            bool previousIsUpper = false;

            foreach (char c in chars)
            {
                bool isDigit = char.IsDigit(c);
                bool isLetter = char.IsLetter(c);
                bool isUpper = char.IsUpper(c);
                bool isLower = char.IsLower(c);

                if (first)
                {
                    yield return c;

                    first = false;
                }
                else
                {
                    if (isDigit || isLetter)
                    {
                        if (previousIsDigit && isLetter)
                            yield return ' ';
                        else if (previousIsLetter && isDigit)
                            yield return ' ';
                        else if (previousIsLower && isUpper)
                            yield return ' ';
                    }

                    yield return c;
                }

                previousIsDigit = isDigit;
                previousIsLetter = isLetter;
                previousIsLower = isLower;
                previousIsUpper = isUpper;
            }
        }

        #endregion

        #region Filtering

        public static string Filter(this string value, CharFilters filters)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return filters == CharFilters.None ? value : new string(value.Where(c => c.PassesFilters(filters)).ToArray());
        }

        #endregion
    }
}