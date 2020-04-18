namespace FreeAndWithBeer.Text
{
    public static partial class StringExtensions
    {
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
    }
}