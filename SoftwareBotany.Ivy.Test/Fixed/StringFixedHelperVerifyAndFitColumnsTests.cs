using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringFixedHelperVerifyAndFitColumnsTests
    {
        [TestMethod]
        public void Basic()
        {
            string[] columns = new[] { "A", "BC", "DEF" };
            int[] widths = new[] { 1, 2, 3 };

            string[] columnsExpected = new string[columns.Length];
            Array.Copy(columns, columnsExpected, columns.Length);

            StringFixedExtensions.VerifyAndFitColumns(columns, widths, false);
            CollectionAssert.AreEqual(columnsExpected, columns);

            StringFixedExtensions.VerifyAndFitColumns(columns, widths, true);
            CollectionAssert.AreEqual(columnsExpected, columns);
        }

        [TestMethod]
        public void SubstringToFit()
        {
            string[] columns = new[] { "A", "BC", "DEF" };
            int[] widths = new[] { 1, 2, 2 };

            StringFixedExtensions.VerifyAndFitColumns(columns, widths, true);
            CollectionAssert.AreEqual(new[] { "A", "BC", "DE" }, columns);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ColumnsNull()
        {
            string[] columns = null;
            int[] widths = new[] { 1 };
            StringFixedExtensions.VerifyAndFitColumns(columns, widths, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ColumnWidthsLengthMismatch1()
        {
            string[] columns = new[] { "A", "B", "C" };
            int[] widths = new[] { 1, 1 };
            StringFixedExtensions.VerifyAndFitColumns(columns, widths, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ColumnWidthsLengthMismatch2()
        {
            string[] columns = new[] { "A", "B" };
            int[] widths = new[] { 1, 2, 3 };
            StringFixedExtensions.VerifyAndFitColumns(columns, widths, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ColumnBiggerThanCorrespondingWidth1()
        {
            string[] columns = new[] { "AB", "CD" };
            int[] widths = new[] { 1, 2 };
            StringFixedExtensions.VerifyAndFitColumns(columns, widths, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ColumnBiggerThanCorrespondingWidth2()
        {
            string[] columns = new[] { "AB", "CD" };
            int[] widths = new[] { 2, 1 };
            StringFixedExtensions.VerifyAndFitColumns(columns, widths, false);
        }

        #endregion
    }
}