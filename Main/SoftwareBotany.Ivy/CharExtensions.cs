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
        public static bool PassesFilters(this char c, CharFilters filters)
        {
            if (filters == CharFilters.None)
                return true;

            return (!filters.HasFlag(CharFilters.RemoveLetters) || !char.IsLetter(c))
                && (!filters.HasFlag(CharFilters.RemoveDigits) || !char.IsDigit(c))
                && (!filters.HasFlag(CharFilters.RemovePunctuation) || !char.IsPunctuation(c))
                && (!filters.HasFlag(CharFilters.RemoveWhiteSpace) || !char.IsWhiteSpace(c))
                && (!filters.HasFlag(CharFilters.RemoveOther) || char.IsLetter(c) || char.IsDigit(c) || char.IsPunctuation(c) || char.IsWhiteSpace(c));
        }
    }
}