namespace FOSStrich.Text;

public static partial class StringFixedExtensionsTests
{
    [TestClass]
    public class SplitRepeating
    {
        [TestMethod]
        public void Basic()
        {
            string[][] rowsFields = "123".SplitFixedRepeating(new[] { 1, 1, 1 }).ToArray();
            Assert.AreEqual(1, rowsFields.Length);
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, rowsFields[0]);

            rowsFields = "123456".SplitFixedRepeating(new[] { 1, 1, 1 }).ToArray();
            Assert.AreEqual(2, rowsFields.Length);
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, rowsFields[0]);
            CollectionAssert.AreEqual(new[] { "4", "5", "6" }, rowsFields[1]);

            rowsFields = "123456789".SplitFixedRepeating(new[] { 1, 1, 1 }).ToArray();
            Assert.AreEqual(3, rowsFields.Length);
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, rowsFields[0]);
            CollectionAssert.AreEqual(new[] { "4", "5", "6" }, rowsFields[1]);
            CollectionAssert.AreEqual(new[] { "7", "8", "9" }, rowsFields[2]);
        }

        [TestMethod]
        public void FillTrim()
        {
            string[][] rowsFields = "1 34 67 9".SplitFixedRepeating(new[] { 1, 1, 1 }).ToArray();
            Assert.AreEqual(3, rowsFields.Length);
            CollectionAssert.AreEqual(new[] { "1", "", "3" }, rowsFields[0]);
            CollectionAssert.AreEqual(new[] { "4", "", "6" }, rowsFields[1]);
            CollectionAssert.AreEqual(new[] { "7", "", "9" }, rowsFields[2]);

            rowsFields = "1-34-67-9".SplitFixedRepeating(new[] { 1, 1, 1 }).ToArray();
            Assert.AreEqual(3, rowsFields.Length);
            CollectionAssert.AreEqual(new[] { "1", "-", "3" }, rowsFields[0]);
            CollectionAssert.AreEqual(new[] { "4", "-", "6" }, rowsFields[1]);
            CollectionAssert.AreEqual(new[] { "7", "-", "9" }, rowsFields[2]);

            rowsFields = "1-34-67-9".SplitFixedRepeating(new[] { 1, 1, 1 }, '-').ToArray();
            Assert.AreEqual(3, rowsFields.Length);
            CollectionAssert.AreEqual(new[] { "1", "", "3" }, rowsFields[0]);
            CollectionAssert.AreEqual(new[] { "4", "", "6" }, rowsFields[1]);
            CollectionAssert.AreEqual(new[] { "7", "", "9" }, rowsFields[2]);
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
        public void LengthColumnWidthSumMismatch1()
        {
            "1".SplitFixedRepeating(new[] { 2 }).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LengthColumnWidthSumMismatch2()
        {
            "12".SplitFixedRepeating(new[] { 3 }).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LengthColumnWidthSumMismatch3()
        {
            "12".SplitFixedRepeating(new[] { 1, 2 }).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LengthColumnWidthSumMismatch4()
        {
            "123".SplitFixedRepeating(new[] { 2 }).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LengthColumnWidthSumMismatch5()
        {
            "123".SplitFixedRepeating(new[] { 1, 1 }).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LengthColumnWidthSumMismatch6()
        {
            "12345".SplitFixedRepeating(new[] { 1, 2 }).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LengthColumnWidthSumMismatch7()
        {
            "1234567".SplitFixedRepeating(new[] { 1, 2 }).ToArray();
        }

        #endregion
    }
}