using System;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    public static partial class StringFixedExtensions
    {
        internal static void VerifyWidths(int[] widths)
        {
            if (widths == null)
                throw new ArgumentNullException("widths");

            if (widths.Length == 0)
                throw new ArgumentException("widths.Length must be > 0", "widths");

            if (widths.Any(i => i <= 0))
                throw new ArgumentOutOfRangeException("widths", "Each width must be > 0");
        }
    }
}