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
        public void Null()
        {
            StringFixedExtensions.VerifyWidths(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Empty()
        {
            StringFixedExtensions.VerifyWidths(new int[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WidthEqualToZero1()
        {
            StringFixedExtensions.VerifyWidths(new[] { 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WidthEqualToZero2()
        {
            StringFixedExtensions.VerifyWidths(new[] { 0, 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WidthEqualToZero3()
        {
            StringFixedExtensions.VerifyWidths(new[] { 1, 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WidthLessThanZero1()
        {
            StringFixedExtensions.VerifyWidths(new[] { -1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WidthLessThanZero2()
        {
            StringFixedExtensions.VerifyWidths(new[] { -1, 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WidthLessThanZero3()
        {
            StringFixedExtensions.VerifyWidths(new[] { 1, -1 });
        }

        #endregion
    }
}