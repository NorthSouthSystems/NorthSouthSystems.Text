using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringFixedHelperVerifyWidthsTests
    {
        [TestMethod]
        public void Basic()
        {
            foreach (int count in Enumerable.Range(1, 10))
                StringFixedExtensions.VerifyWidths(Enumerable.Range(1, count).ToArray());
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNull()
        {
            StringFixedExtensions.VerifyWidths(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentEmptyLengths()
        {
            StringFixedExtensions.VerifyWidths(new int[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ArgumentBadLength1()
        {
            StringFixedExtensions.VerifyWidths(new[] { 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ArgumentBadLength2()
        {
            StringFixedExtensions.VerifyWidths(new[] { -1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ArgumentBadLength3()
        {
            StringFixedExtensions.VerifyWidths(new[] { 0, 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ArgumentBadLength4()
        {
            StringFixedExtensions.VerifyWidths(new[] { -1, 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ArgumentBadLength5()
        {
            StringFixedExtensions.VerifyWidths(new[] { 1, 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ArgumentBadLength6()
        {
            StringFixedExtensions.VerifyWidths(new[] { 1, -1 });
        }

        #endregion
    }
}