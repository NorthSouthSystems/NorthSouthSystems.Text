namespace Kangarooper.Text
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class StringExtensions
    {
        #region Construction

        /// <summary>
        /// Creates and returns a new string containing all of the characters in the provided enumeration.
        /// </summary>
        public static string ToNewString(this IEnumerable<char> chars) { return new string(chars.ToArray()); }

        #endregion

        #region Empty / Null

        /// <summary>
        /// VIOLATES NULL REFERENCE SEMANTICS! Will return null if string.IsNullOrEmpty. This simplifies code that treats
        /// null string values the same as it treats Empty string values, and wants only to deal with null.
        /// </summary>
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
        /// <returns>Returns string.Empty when value == null; else, returns the original string.</returns>
        public static string NullToEmpty(this string value) { return value ?? string.Empty; }

        #endregion

        #region Cleaning

        /// <inheritdoc cref="NormalizeWhiteSpace(IEnumerable{char}, string)"/>
        public static string NormalizeWhiteSpace(this string value) { return NormalizeWhiteSpace((IEnumerable<char>)value, Environment.NewLine).ToNewString(); }

        /// <inheritdoc cref="NormalizeWhiteSpace(IEnumerable{char}, string)"/>
        public static string NormalizeWhiteSpace(this string value, string respectNewLine) { return NormalizeWhiteSpace((IEnumerable<char>)value, respectNewLine).ToNewString(); }

        /// <inheritdoc cref="NormalizeWhiteSpace(IEnumerable{char}, string)"/>
        public static IEnumerable<char> NormalizeWhiteSpace(this IEnumerable<char> chars) { return NormalizeWhiteSpace(chars, Environment.NewLine); }

        /// <summary>
        /// Trims the front and back sides of a sequence of chars while replacing any repeating instances of whitespace with a
        /// single instance of whitespace.  Newlines consisting of more than one whitespace character (Windows Newlines = \r\n) are
        /// treated as a single whitespace instance.
        /// </summary>
        /// <param name="respectNewLine">The Newline string to treat as a single whitespace instance. (default = Environment.NewLine)</param>
        /// <example>
        /// <code>
        /// Console.WriteLine(" A  B C   D   ".NormalizeWhiteSpace());
        /// Console.WriteLine(("Lots\tOf" + Environment.NewLine + "Changes").NormalizeWhiteSpace());
        /// </code>
        /// Console Output:<br/>
        /// A B C D<br/>
        /// Lots Of<br/>
        /// Changes<br/>
        /// </example>
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
        /// <param name="maxLength">The maximum length allowed for the return string. Additional chars from value are truncated.</param>
        /// <returns>The original value truncated to maxLength if necessary.</returns>
        /// <example>
        /// <code>
        /// Console.WriteLine("abc".SubstringToFit(1));
        /// Console.WriteLine("abc".SubstringToFit(2));
        /// Console.WriteLine("abc".SubstringToFit(3));
        /// Console.WriteLine("abc".SubstringToFit(4));
        /// </code>
        /// Console Output:<br/>
        /// a<br/>
        /// ab<br/>
        /// abc<br/>
        /// abc<br/>
        /// </example>
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
        public static string ToLowerCamelCase(this string value) { return ToCamelCaseImplementation(value, true).ToNewString(); }

        /// <summary>
        /// Process a sequence of characters and returns its lower camel case representation.
        /// </summary>
        /// <example>
        /// <code>
        /// Console.WriteLine("FooBar".ToLowerCamelCase());
        /// Console.WriteLine(" FooBar".ToLowerCamelCase());
        /// </code>
        /// Console Output:<br/>
        /// fooBar<br/>
        ///  fooBar<br/>
        /// </example>
        public static IEnumerable<char> ToLowerCamelCase(this IEnumerable<char> chars) { return ToCamelCaseImplementation(chars, true); }

        /// <inheritdoc cref="ToUpperCamelCase(IEnumerable{char})"/>
        public static string ToUpperCamelCase(this string value) { return ToCamelCaseImplementation(value, false).ToNewString(); }

        /// <summary>
        /// Process a sequence of characters and returns its upper camel case representation.
        /// </summary>
        /// <example>
        /// <code>
        /// Console.WriteLine("fooBar".ToUpperCamelCase());
        /// Console.WriteLine(" fooBar".ToUpperCamelCase());
        /// </code>
        /// Console Output:<br/>
        /// FooBar<br/>
        ///  FooBar<br/>
        /// </example>
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
        public static string SpaceCamelCase(this string value) { return SpaceCamelCase((IEnumerable<char>)value).ToNewString(); }

        /// <summary>
        /// Process a sequence of characters and returns the same sequence of characters but with spaces inserted
        /// whenever camel casing indicates the start of a new word.
        /// </summary>
        /// <example>
        /// <code>
        /// Console.WriteLine("FooBarFoo FooBarFoo".SpaceCamelCase());
        /// Console.WriteLine("123A".SpaceCamelCase());
        /// Console.WriteLine("123a".SpaceCamelCase());
        /// Console.WriteLine("A123".SpaceCamelCase());
        /// Console.WriteLine("A123A".SpaceCamelCase());
        /// </code>
        /// Console Output:<br/>
        /// Foo Bar Foo Foo Bar Foo<br/>
        /// 123 A<br/>
        /// 123 a<br/>
        /// A 123<br/>
        /// A 123 A<br/>
        /// </example>
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
        public static string Filter(this string value, CharFilters filters) { return Filter((IEnumerable<char>)value, filters).ToNewString(); }

        /// <summary>
        /// Removes unwanted characters from a sequence of characters.
        /// </summary>
        /// <param name="filters">Bitwise union of 1 or more CharFilters designating which characters to filter.</param>
        /// <returns>The original sequence of chars with unwanted chars omitted.</returns>
        /// <example>
        /// <code>
        /// Console.WriteLine("a1b2c3d".Filter(CharFilters.None));
        /// Console.WriteLine("a1b2c3d".Filter(CharFilters.RemoveLetters));
        /// Console.WriteLine("a1b2c3d".Filter(CharFilters.RemoveLetters | CharFilters.RemoveDigits));
        /// Console.WriteLine("a1b2-c3d".Filter(CharFilters.RemovePunctuation));
        /// Console.WriteLine("a1b2-c3d".Filter(CharFilters.RemoveLetters | CharFilters.RemoveDigits));
        /// </code>
        /// Console Output:<br/>
        /// a1b2c3d<br/>
        /// 123<br/>
        /// <br/>
        /// a1b2c3d<br/>
        /// -<br/>
        /// </example>
        public static IEnumerable<char> Filter(this IEnumerable<char> chars, CharFilters filters)
        {
            if (chars == null)
                throw new ArgumentNullException("chars");

            return filters == CharFilters.None ? chars : chars.Where(c => c.PassesFilters(filters));
        }

        #endregion
    }
}