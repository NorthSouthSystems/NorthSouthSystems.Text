namespace FreeAndWithBeer.Text
{
    using System;

    public static partial class StringExtensions
    {
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
    }
}