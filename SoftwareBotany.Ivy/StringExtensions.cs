using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    /// <summary>
    /// Extensions for the System.String data type.
    /// </summary>
    public static class StringExtensions
    {
        #region Empty / Null

        /// <summary>
        /// VIOLATES NULL REFERENCE SEMANTICS! Will return null if string.IsNullOrEmpty. This simplifies code that treats
        /// null string values the same as it treats Empty string values, and wants only to deal with null.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>Returns null when value == string.Empty; else, returns the original string.</returns>
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
        /// <param name="value">The string to check.</param>
        /// /// <returns>Returns string.Empty when value == null; else, returns the original string.</returns>
        public static string NullToEmpty(this string value) { return value ?? string.Empty; }

        #endregion

        #region Cleaning

        /// <inheritdoc cref="NormalizeWhiteSpace(IEnumerable{char}, string)"/>
        /// /// <param name="value">A string of characters to normalize.</param>
        public static string NormalizeWhiteSpace(this string value) { return new string(NormalizeWhiteSpace((IEnumerable<char>)value, Environment.NewLine).ToArray()); }

        /// <inheritdoc cref="NormalizeWhiteSpace(IEnumerable{char}, string)"/>
        /// <param name="value">A string of characters to normalize.</param>
        public static string NormalizeWhiteSpace(this string value, string respectNewLine) { return new string(NormalizeWhiteSpace((IEnumerable<char>)value, respectNewLine).ToArray()); }

        /// <inheritdoc cref="NormalizeWhiteSpace(IEnumerable{char}, string)"/>
        public static IEnumerable<char> NormalizeWhiteSpace(this IEnumerable<char> chars) { return NormalizeWhiteSpace(chars, Environment.NewLine); }

        /// <summary>
        /// Trims the front and back sides of a sequence of chars while replacing any repeating instances of whitespace with a
        /// single instance of whitespace.  Newlines consisting of more than 1 whitespace character (Windows Newlines = \r\n) are
        /// treated as a single whitespace instance.
        /// </summary>
        /// <param name="chars">A sequence of chars to normalize.</param>
        /// <param name="respectNewLine">The Newline string to treat as a single whitespace instance.</param>
        /// <returns>A normalized sequence of characters.</returns>
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
        /// <param name="value">The string to process.</param>
        /// <param name="maxLength">The maximum length allowed for the return string. Additional chars from value are truncated.</param>
        /// <returns>The original value truncated to maxLength if necessary.</returns>
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

        /// <inheritdoc cref="ToLowerCamelCase(IEnumerable{char})"/>
        /// <param name="value">The string to process.</param>
        public static string ToLowerCamelCase(this string value) { return new string(ToCamelCaseImplementation(value, true).ToArray()); }

        /// <summary>
        /// Process a sequence of characters and returns its lower camel case representation.
        /// </summary>
        /// <param name="chars">The sequence of chars to process.</param>
        /// <returns>A lower camel case representation of the input sequence.</returns>
        public static IEnumerable<char> ToLowerCamelCase(this IEnumerable<char> chars) { return ToCamelCaseImplementation(chars, true); }

        /// <inheritdoc cref="ToUpperCamelCase(IEnumerable{char})"/>
        /// <param name="value">The string to process.</param>
        public static string ToUpperCamelCase(this string value) { return new string(ToCamelCaseImplementation(value, false).ToArray()); }

        /// <summary>
        /// Process a sequence of characters and returns its upper camel case representation.
        /// </summary>
        /// <param name="chars">The sequence of chars to process.</param>
        /// <returns>An upper camel case representation of the input sequence.</returns>
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

        /// <inheritdoc cref="SpaceCamelCase(IEnumerable{char})"/>
        /// <param name="value">The string to process.</param>
        public static string SpaceCamelCase(this string value) { return new string(SpaceCamelCase((IEnumerable<char>)value).ToArray()); }

        /// <summary>
        /// Process a sequence of characters and returns the same sequence of characters but with spaces inserted
        /// whenever camel casing indicates the start of a new word.
        /// </summary>
        /// <param name="chars">The sequence of chars to process.</param>
        /// <returns>The input sequence with single spaces inserted at the start of each word.</returns>
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

        /// <inheritdoc cref="Filter(IEnumerable{char}, CharFilters)"/>
        /// <param name="value">The string to process.</param>
        public static string Filter(this string value, CharFilters filters)
        {
            char[] filteredChars = Filter((IEnumerable<char>)value, filters).ToArray();
            return new string(filteredChars);
        }

        /// <summary>
        /// Removes unwanted characters from a sequence of characters.
        /// </summary>
        /// <param name="chars">Sequence of characters to process.</param>
        /// <param name="filters">Bitwise union of 1 or more CharFilters designating which characters to filter.</param>
        /// <returns>The original sequence of chars with unwanted chars omitted.</returns>
        public static IEnumerable<char> Filter(this IEnumerable<char> chars, CharFilters filters)
        {
            if (chars == null)
                throw new ArgumentNullException("chars");

            return filters == CharFilters.None ? chars : chars.Where(c => c.PassesFilters(filters));
        }

        #endregion
    }
}