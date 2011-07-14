using System;

namespace SoftwareBotany.Ivy
{
    /// <summary>
    /// Flags used to determine which characters to filter out of an enumeration of chars.
    /// </summary>
    [Flags]
    public enum CharFilters
    {
        None = 0,
        RemoveLetters = 1,
        RemoveDigits = 2,
        RemovePunctuation = 4,
        RemoveWhiteSpace = 8,
        RemoveOther = 16,
    }

    /// <summary>
    /// Extensions for the System.Char primitive data type.
    /// </summary>
    public static class CharExtensions
    {
        /// <summary>
        /// Determines whether or not a char can pass through a given set of CharFilters.
        /// </summary>
        /// <param name="value">The char to test.</param>
        /// <param name="filters">Bitwise union of 1 or more CharFilters designating which characters to filter.</param>
        /// <returns>Returns true if the char passes through the filters.</returns>
        public static bool PassesFilters(this char value, CharFilters filters)
        {
            if (filters == CharFilters.None)
                return true;

            return (!filters.HasFlag(CharFilters.RemoveLetters) || !char.IsLetter(value))
                && (!filters.HasFlag(CharFilters.RemoveDigits) || !char.IsDigit(value))
                && (!filters.HasFlag(CharFilters.RemovePunctuation) || !char.IsPunctuation(value))
                && (!filters.HasFlag(CharFilters.RemoveWhiteSpace) || !char.IsWhiteSpace(value))
                && (!filters.HasFlag(CharFilters.RemoveOther) || char.IsLetter(value) || char.IsDigit(value) || char.IsPunctuation(value) || char.IsWhiteSpace(value));
        }
    }
}