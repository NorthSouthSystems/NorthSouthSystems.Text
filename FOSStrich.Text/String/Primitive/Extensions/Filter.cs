namespace FreeAndWithBeer.Text
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class StringExtensions
    {
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
    }
}