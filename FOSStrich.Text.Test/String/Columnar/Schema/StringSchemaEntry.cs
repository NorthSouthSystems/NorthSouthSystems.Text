namespace FOSStrich.Text
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class StringSchemaEntryTests
    {
        [TestMethod]
        public void Construction()
        {
            var entry = new StringSchemaEntry("A", new[] { 1 }, ' ', new[] { "Column0" });
            Assert.AreEqual("A", entry.Header);
            Assert.AreEqual(1, entry.Widths.Length);
            Assert.AreEqual(1, entry.Widths[0]);
            Assert.AreEqual(' ', entry.FillCharacter);
            Assert.AreEqual(1, entry.RowWrapperFactory.ColumnNames.Length);
            Assert.AreEqual("Column0", entry.RowWrapperFactory.ColumnNames[0]);

            entry = new StringSchemaEntry("B", new[] { 2 }, '-', null);
            Assert.AreEqual("B", entry.Header);
            Assert.AreEqual(1, entry.Widths.Length);
            Assert.AreEqual(2, entry.Widths[0]);
            Assert.AreEqual('-', entry.FillCharacter);
            Assert.AreEqual(1, entry.RowWrapperFactory.ColumnNames.Length);
            Assert.AreEqual("0", entry.RowWrapperFactory.ColumnNames[0]);

            entry = new StringSchemaEntry("C", new[] { 3, 4 }, '.', new string[0]);
            Assert.AreEqual("C", entry.Header);
            Assert.AreEqual(2, entry.Widths.Length);
            Assert.AreEqual(3, entry.Widths[0]);
            Assert.AreEqual(4, entry.Widths[1]);
            Assert.AreEqual('.', entry.FillCharacter);
            Assert.AreEqual(2, entry.RowWrapperFactory.ColumnNames.Length);
            Assert.AreEqual("0", entry.RowWrapperFactory.ColumnNames[0]);
            Assert.AreEqual("1", entry.RowWrapperFactory.ColumnNames[1]);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructionNullHeader()
        {
            StringSchemaEntry entry = new StringSchemaEntry(null, new[] { 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructionEmptyHeader()
        {
            StringSchemaEntry entry = new StringSchemaEntry(string.Empty, new[] { 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructionColumnWidthsAndNamesLengthMismatch()
        {
            StringSchemaEntry entry = new StringSchemaEntry("A", new[] { 1, 1 }, ' ', new[] { "Column0" });
        }

        #endregion
    }
}