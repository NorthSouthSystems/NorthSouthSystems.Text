using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    internal static class StringPositionalHelpers
    {
        internal static void SplitOrJoinPositionalVerifyLengths(int[] lengths)
        {
            if (lengths == null)
                throw new ArgumentNullException("lengths");

            if (lengths.Length == 0)
                throw new ArgumentException("lengths.Length must be > 0");

            if (lengths.Any(i => i <= 0))
                throw new ArgumentException("Each length must be > 0");
        }
    }
}