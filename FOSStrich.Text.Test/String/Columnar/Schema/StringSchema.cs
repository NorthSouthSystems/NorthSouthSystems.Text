namespace FOSStrich.Text
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class StringSchemaTests
    {
        [TestMethod]
        public void Basic()
        {
            var schema = new StringSchema();
            schema.AddEntry(new StringSchemaEntry("A", new[] { 3 }));
            schema.AddEntry(new StringSchemaEntry("B", new[] { 6 }));
            schema.AddEntry(new StringSchemaEntry("C", new[] { 3, 3 }));
            schema.AddEntry(new StringSchemaEntry("DE", new[] { 4, 4 }));

            var entry = schema.GetEntryForRow("Afoo");

            Assert.AreEqual("A", entry.Header);
            CollectionAssert.AreEqual(new[] { 3 }, entry.Widths);

            entry = schema["A"];

            Assert.AreEqual("A", entry.Header);
            CollectionAssert.AreEqual(new[] { 3 }, entry.Widths);

            entry = schema.GetEntryForRow("Bfoobar");

            Assert.AreEqual("B", entry.Header);
            CollectionAssert.AreEqual(new[] { 6 }, entry.Widths);

            entry = schema["B"];

            Assert.AreEqual("B", entry.Header);
            CollectionAssert.AreEqual(new[] { 6 }, entry.Widths);

            entry = schema.GetEntryForRow("Cfoobar");

            Assert.AreEqual("C", entry.Header);
            CollectionAssert.AreEqual(new[] { 3, 3 }, entry.Widths);

            entry = schema["C"];

            Assert.AreEqual("C", entry.Header);
            CollectionAssert.AreEqual(new[] { 3, 3 }, entry.Widths);

            entry = schema.GetEntryForRow("DEfoosbars");

            Assert.AreEqual("DE", entry.Header);
            CollectionAssert.AreEqual(new[] { 4, 4 }, entry.Widths);

            entry = schema["DE"];

            Assert.AreEqual("DE", entry.Header);
            CollectionAssert.AreEqual(new[] { 4, 4 }, entry.Widths);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddEntryNullEntry()
        {
            var schema = new StringSchema();
            schema.AddEntry(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetEntryForValueNoEntry1()
        {
            var schema = new StringSchema();
            StringSchemaEntry entry = new StringSchemaEntry("A", new[] { 1 });
            schema.AddEntry(entry);
            schema.GetEntryForRow("Bfoo");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetEntryForValueNoEntry2()
        {
            var schema = new StringSchema();
            StringSchemaEntry entry = new StringSchemaEntry("AB", new[] { 1 });
            schema.AddEntry(entry);
            schema.GetEntryForRow("Afoo");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void VerifyEntryOverlappedHeader1()
        {
            var schema = new StringSchema();
            StringSchemaEntry entry = new StringSchemaEntry("A", new[] { 1 });
            schema.AddEntry(entry);
            entry = new StringSchemaEntry("AB", new[] { 1 });
            schema.AddEntry(entry);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void VerifyEntryOverlappedHeader2()
        {
            var schema = new StringSchema();
            StringSchemaEntry entry = new StringSchemaEntry("AB", new[] { 1 });
            schema.AddEntry(entry);
            entry = new StringSchemaEntry("A", new[] { 1 });
            schema.AddEntry(entry);
        }

        #endregion
    }
}