using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringFixedRepeatingTests
    {
        [TestMethod]
        public void Basic()
        {
            string[][] rowColumns = "123".SplitFixedRepeating(new[] { 1, 1, 1 }).ToArray();
            Assert.AreEqual(1, rowColumns.Length);
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, rowColumns[0]);

            rowColumns = "123456".SplitFixedRepeating(new[] { 1, 1, 1 }).ToArray();
            Assert.AreEqual(2, rowColumns.Length);
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, rowColumns[0]);
            CollectionAssert.AreEqual(new[] { "4", "5", "6" }, rowColumns[1]);

            rowColumns = "123456789".SplitFixedRepeating(new[] { 1, 1, 1 }).ToArray();
            Assert.AreEqual(3, rowColumns.Length);
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, rowColumns[0]);
            CollectionAssert.AreEqual(new[] { "4", "5", "6" }, rowColumns[1]);
            CollectionAssert.AreEqual(new[] { "7", "8", "9" }, rowColumns[2]);
        }

        [TestMethod]
        public void FillTrim()
        {
            string[][] rowColumns = "1 34 67 9".SplitFixedRepeating(new[] { 1, 1, 1 }).ToArray();
            Assert.AreEqual(3, rowColumns.Length);
            CollectionAssert.AreEqual(new[] { "1", "", "3" }, rowColumns[0]);
            CollectionAssert.AreEqual(new[] { "4", "", "6" }, rowColumns[1]);
            CollectionAssert.AreEqual(new[] { "7", "", "9" }, rowColumns[2]);

            rowColumns = "1-34-67-9".SplitFixedRepeating(new[] { 1, 1, 1 }).ToArray();
            Assert.AreEqual(3, rowColumns.Length);
            CollectionAssert.AreEqual(new[] { "1", "-", "3" }, rowColumns[0]);
            CollectionAssert.AreEqual(new[] { "4", "-", "6" }, rowColumns[1]);
            CollectionAssert.AreEqual(new[] { "7", "-", "9" }, rowColumns[2]);

            rowColumns = "1-34-67-9".SplitFixedRepeating(new[] { 1, 1, 1 }, '-').ToArray();
            Assert.AreEqual(3, rowColumns.Length);
            CollectionAssert.AreEqual(new[] { "1", "", "3" }, rowColumns[0]);
            CollectionAssert.AreEqual(new[] { "4", "", "6" }, rowColumns[1]);
            CollectionAssert.AreEqual(new[] { "7", "", "9" }, rowColumns[2]);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThisNull()
        {
            string s = null;
            s.SplitFixedRepeating(new[] { 1 }).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LengthWidthSumMismatch1()
        {
            "1".SplitFixedRepeating(new[] { 2 }).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LengthWidthSumMismatch2()
        {
            "12".SplitFixedRepeating(new[] { 3 }).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LengthWidthSumMismatch3()
        {
            "12".SplitFixedRepeating(new[] { 1, 2 }).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LengthWidthSumMismatch4()
        {
            "123".SplitFixedRepeating(new[] { 2 }).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LengthWidthSumMismatch5()
        {
            "123".SplitFixedRepeating(new[] { 1, 1 }).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LengthWidthSumMismatch6()
        {
            "12345".SplitFixedRepeating(new[] { 1, 2 }).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LengthWidthSumMismatch7()
        {
            "1234567".SplitFixedRepeating(new[] { 1, 2 }).ToArray();
        }

        #endregion
    }
}