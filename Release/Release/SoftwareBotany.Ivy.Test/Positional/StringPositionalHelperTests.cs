using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringPositionalHelperTests
    {
        [TestMethod]
        public void SplitOrJoinPositionalVerifyLengths()
        {
            foreach (int count in Enumerable.Range(1, 10))
                StringPositionalHelpers.SplitOrJoinPositionalVerifyLengths(Enumerable.Range(1, count).ToArray());
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SplitOrJoinPositionalVerifyLengthsArgumentNull()
        {
            StringPositionalHelpers.SplitOrJoinPositionalVerifyLengths(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SplitOrJoinPositionalVerifyLengthsArgumentEmptyLengths()
        {
            StringPositionalHelpers.SplitOrJoinPositionalVerifyLengths(new int[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SplitOrJoinPositionalVerifyLengthsArgumentBadLength1()
        {
            StringPositionalHelpers.SplitOrJoinPositionalVerifyLengths(new[] { 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SplitOrJoinPositionalVerifyLengthsArgumentBadLength2()
        {
            StringPositionalHelpers.SplitOrJoinPositionalVerifyLengths(new[] { -1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SplitOrJoinPositionalVerifyLengthsArgumentExceptionBadLength3()
        {
            StringPositionalHelpers.SplitOrJoinPositionalVerifyLengths(new[] { 0, 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SplitOrJoinPositionalVerifyLengthsArgumentBadLength4()
        {
            StringPositionalHelpers.SplitOrJoinPositionalVerifyLengths(new[] { -1, 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SplitOrJoinPositionalVerifyLengthsArgumentBadLength5()
        {
            StringPositionalHelpers.SplitOrJoinPositionalVerifyLengths(new[] { 1, 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SplitOrJoinPositionalVerifyLengthsArgumentBadLength6()
        {
            StringPositionalHelpers.SplitOrJoinPositionalVerifyLengths(new[] { 1, -1 });
        }

        #endregion
    }
}