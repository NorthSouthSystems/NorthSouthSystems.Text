namespace FreeAndWithBeer.Text
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    public static partial class StringFixedExtensionsTests
    {
        [TestClass]
        public class SplitRow
        {
            [TestMethod]
            public void Basic()
            {
                string[] split = "ABC".SplitFixedRow(new[] { 1, 1, 1 });
                CollectionAssert.AreEqual(new[] { "A", "B", "C" }, split);

                split = "ABC".SplitFixedRow(new[] { 1, 2 });
                CollectionAssert.AreEqual(new[] { "A", "BC" }, split);

                split = "ABCD".SplitFixedRow(new[] { 1, 2, 1 });
                CollectionAssert.AreEqual(new[] { "A", "BC", "D" }, split);

                split = "ABCDEF".SplitFixedRow(new[] { 2, 2, 2 });
                CollectionAssert.AreEqual(new[] { "AB", "CD", "EF" }, split);

                split = "A B C ".SplitFixedRow(new[] { 2, 2, 2 });
                CollectionAssert.AreEqual(new[] { "A", "B", "C" }, split);

                split = "A-B-C-".SplitFixedRow(new[] { 2, 2, 2 });
                CollectionAssert.AreEqual(new[] { "A-", "B-", "C-" }, split);

                split = "A-B-C-".SplitFixedRow(new[] { 2, 2, 2 }, '-');
                CollectionAssert.AreEqual(new[] { "A", "B", "C" }, split);

                split = "A B   ".SplitFixedRow(new[] { 2, 2, 2 });
                CollectionAssert.AreEqual(new[] { "A", "B", "" }, split);

                split = "A-B---".SplitFixedRow(new[] { 2, 2, 2 });
                CollectionAssert.AreEqual(new[] { "A-", "B-", "--" }, split);

                split = "A-B---".SplitFixedRow(new[] { 2, 2, 2 }, '-');
                CollectionAssert.AreEqual(new[] { "A", "B", "" }, split);
            }

            #region Exceptions

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThisNull()
            {
                string s = null;
                s.SplitFixedRow(new[] { 1 });
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void ThisEmpty()
            {
                string.Empty.SplitFixedRow(new[] { 1 });
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void LengthColumnWidthSumMismatch1()
            {
                "1".SplitFixedRow(new[] { 2 });
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void LengthColumnWidthSumMismatch2()
            {
                "12".SplitFixedRow(new[] { 3 });
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void LengthColumnWidthSumMismatch3()
            {
                "12".SplitFixedRow(new[] { 1, 2 });
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void LengthColumnWidthSumMismatch4()
            {
                "1234".SplitFixedRow(new[] { 1, 2 });
            }

            #endregion
        }
    }
}