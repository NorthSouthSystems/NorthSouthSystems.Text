namespace FreeAndWithBeer.Text
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class StringExtensions
    {
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
    }
}