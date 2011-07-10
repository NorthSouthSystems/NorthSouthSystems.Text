using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringPositionalSchemaTests
    {
        [TestMethod]
        public void Basic()
        {
            var schema = new StringPositionalSchema(new StringPositionalSchemaEntry[]
            {
                new StringPositionalSchemaEntry("A", new [] { 1 }),
                new StringPositionalSchemaEntry("B", new [] { 2 }),
            });

            CollectionAssert.AreEqual(new[] { 1 }, schema["A"].ToArray());
            CollectionAssert.AreEqual(new[] { 2 }, schema["B"].ToArray());

            var entry = schema.GetEntryForValue("Afoo");

            Assert.AreEqual("A", entry.Header);
            CollectionAssert.AreEqual(new[] { 1 }, entry.ToArray());

            entry = schema.GetEntryForValue("Bfoo");

            Assert.AreEqual("B", entry.Header);
            CollectionAssert.AreEqual(new[] { 2 }, entry.ToArray());
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNull()
        {
            var schema = new StringPositionalSchema(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddEntryNull()
        {
            var schema = new StringPositionalSchema();
            schema.AddEntry(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetEntryForValueArgument()
        {
            var schema = new StringPositionalSchema();
            StringPositionalSchemaEntry entry = new StringPositionalSchemaEntry("A", new[] { 1 });
            schema.AddEntry(entry);
            schema.GetEntryForValue("Bfoo");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void VerifyEntryArgumentBadKey()
        {
            var schema = new StringPositionalSchema();
            StringPositionalSchemaEntry entry = new StringPositionalSchemaEntry(string.Empty, new[] { 1 });
            schema.AddEntry(entry);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void VerifyEntryArgumentOverlappedKey1()
        {
            var schema = new StringPositionalSchema();
            StringPositionalSchemaEntry entry = new StringPositionalSchemaEntry("A", new[] { 1 });
            schema.AddEntry(entry);
            entry = new StringPositionalSchemaEntry("AB", new[] { 1 });
            schema.AddEntry(entry);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void VerifyEntryArgumentOverlappedKey2()
        {
            var schema = new StringPositionalSchema();
            StringPositionalSchemaEntry entry = new StringPositionalSchemaEntry("AB", new[] { 1 });
            schema.AddEntry(entry);
            entry = new StringPositionalSchemaEntry("A", new[] { 1 });
            schema.AddEntry(entry);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SchemaEntryAndStringsEntryNull()
        {
            StringPositionalSchemaEntryAndStrings split = new StringPositionalSchemaEntryAndStrings(null, new[] { "Dan", "Terry" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SchemaEntryAndStringsStringsNull()
        {
            StringPositionalSchemaEntry entry = new StringPositionalSchemaEntry("A", new[] { 10, 10 });
            StringPositionalSchemaEntryAndStrings split = new StringPositionalSchemaEntryAndStrings(entry, null);
        }

        #endregion
    }
}