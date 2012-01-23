using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringFixedHelperVerifyAndFitFieldsTests
    {
        [TestMethod]
        public void Basic()
        {
            string[] fields = new[] { "A", "BC", "DEF" };
            int[] columnWidths = new[] { 1, 2, 3 };

            string[] fieldsExpected = new string[fields.Length];
            Array.Copy(fields, fieldsExpected, fields.Length);

            StringFixedExtensions.VerifyAndFitFields(fields, columnWidths, false);
            CollectionAssert.AreEqual(fieldsExpected, fields);

            StringFixedExtensions.VerifyAndFitFields(fields, columnWidths, true);
            CollectionAssert.AreEqual(fieldsExpected, fields);
        }

        [TestMethod]
        public void SubstringToFit()
        {
            string[] fields = new[] { "A", "BC", "DEF" };
            int[] columnWidths = new[] { 1, 2, 2 };

            StringFixedExtensions.VerifyAndFitFields(fields, columnWidths, true);
            CollectionAssert.AreEqual(new[] { "A", "BC", "DE" }, fields);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FieldsNull()
        {
            string[] fields = null;
            int[] columnWidths = new[] { 1 };
            StringFixedExtensions.VerifyAndFitFields(fields, columnWidths, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FieldsAndColumnWidthsLengthMismatch1()
        {
            string[] fields = new[] { "A", "B", "C" };
            int[] columnWidths = new[] { 1, 1 };
            StringFixedExtensions.VerifyAndFitFields(fields, columnWidths, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FieldsAndColumnWidthsLengthMismatch2()
        {
            string[] fields = new[] { "A", "B" };
            int[] columnWidths = new[] { 1, 2, 3 };
            StringFixedExtensions.VerifyAndFitFields(fields, columnWidths, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void FieldBiggerThanCorrespondingColumnWidth1()
        {
            string[] fields = new[] { "AB", "CD" };
            int[] columnWidths = new[] { 1, 2 };
            StringFixedExtensions.VerifyAndFitFields(fields, columnWidths, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void FieldBiggerThanCorrespondingColumnWidth2()
        {
            string[] fields = new[] { "AB", "CD" };
            int[] columnWidths = new[] { 2, 1 };
            StringFixedExtensions.VerifyAndFitFields(fields, columnWidths, false);
        }

        #endregion
    }
}