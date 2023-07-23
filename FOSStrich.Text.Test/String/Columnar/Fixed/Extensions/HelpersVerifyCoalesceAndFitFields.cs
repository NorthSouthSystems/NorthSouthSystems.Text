namespace FOSStrich.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

public static partial class StringFixedExtensionsTests
{
    [TestClass]
    public class VerifyCoalesceAndFitFields
    {
        [TestMethod]
        public void Basic()
        {
            string[] fields = new[] { "A", "BC", "DEF" };
            int[] columnWidths = new[] { 1, 2, 3 };

            string[] fieldsExpected = new string[fields.Length];
            Array.Copy(fields, fieldsExpected, fields.Length);

            StringFixedExtensions.VerifyCoalesceAndFitFields(fields, columnWidths, false);
            CollectionAssert.AreEqual(fieldsExpected, fields);

            StringFixedExtensions.VerifyCoalesceAndFitFields(fields, columnWidths, true);
            CollectionAssert.AreEqual(fieldsExpected, fields);
        }

        [TestMethod]
        public void Coalesce()
        {
            string[] fields;
            int[] columnWidths = new[] { 1, 1, 1 };

            StringFixedExtensions.VerifyCoalesceAndFitFields(fields = new[] { null, "B", "C" }, columnWidths, false);
            CollectionAssert.AreEqual(new[] { string.Empty, "B", "C" }, fields);

            StringFixedExtensions.VerifyCoalesceAndFitFields(fields = new[] { "A", null, "C" }, columnWidths, false);
            CollectionAssert.AreEqual(new[] { "A", string.Empty, "C" }, fields);

            StringFixedExtensions.VerifyCoalesceAndFitFields(fields = new[] { "A", "B", null }, columnWidths, false);
            CollectionAssert.AreEqual(new[] { "A", "B", string.Empty }, fields);
        }

        [TestMethod]
        public void SubstringToFit()
        {
            string[] fields = new[] { "A", "BC", "DEF" };
            int[] columnWidths = new[] { 1, 2, 2 };

            StringFixedExtensions.VerifyCoalesceAndFitFields(fields, columnWidths, true);
            CollectionAssert.AreEqual(new[] { "A", "BC", "DE" }, fields);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FieldsNull()
        {
            string[] fields = null;
            int[] columnWidths = new[] { 1 };
            StringFixedExtensions.VerifyCoalesceAndFitFields(fields, columnWidths, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FieldsAndColumnWidthsLengthMismatch1()
        {
            string[] fields = new[] { "A", "B", "C" };
            int[] columnWidths = new[] { 1, 1 };
            StringFixedExtensions.VerifyCoalesceAndFitFields(fields, columnWidths, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FieldsAndColumnWidthsLengthMismatch2()
        {
            string[] fields = new[] { "A", "B" };
            int[] columnWidths = new[] { 1, 2, 3 };
            StringFixedExtensions.VerifyCoalesceAndFitFields(fields, columnWidths, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void FieldBiggerThanCorrespondingColumnWidth1()
        {
            string[] fields = new[] { "AB", "CD" };
            int[] columnWidths = new[] { 1, 2 };
            StringFixedExtensions.VerifyCoalesceAndFitFields(fields, columnWidths, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void FieldBiggerThanCorrespondingColumnWidth2()
        {
            string[] fields = new[] { "AB", "CD" };
            int[] columnWidths = new[] { 2, 1 };
            StringFixedExtensions.VerifyCoalesceAndFitFields(fields, columnWidths, false);
        }

        #endregion
    }
}