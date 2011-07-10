using System;

namespace SoftwareBotany.Ivy
{
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

    public static class CharExtensions
    {
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