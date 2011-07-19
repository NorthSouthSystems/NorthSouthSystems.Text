using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringSchemaTests
    {
        [TestMethod]
        public void Basic()
        {
            var schema = new StringSchema(new StringSchemaEntry[]
            {
                new StringSchemaEntry("A", 1),
                new StringSchemaEntry("B", 2),
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
            var schema = new StringSchema(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddEntryNull()
        {
            var schema = new StringSchema();
            schema.AddEntry(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetEntryForValueArgument()
        {
            var schema = new StringSchema();
            StringSchemaEntry entry = new StringSchemaEntry("A", 1);
            schema.AddEntry(entry);
            schema.GetEntryForValue("Bfoo");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void VerifyEntryArgumentOverlappedKey1()
        {
            var schema = new StringSchema();
            StringSchemaEntry entry = new StringSchemaEntry("A", 1);
            schema.AddEntry(entry);
            entry = new StringSchemaEntry("AB", 1);
            schema.AddEntry(entry);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void VerifyEntryArgumentOverlappedKey2()
        {
            var schema = new StringSchema();
            StringSchemaEntry entry = new StringSchemaEntry("AB", 1);
            schema.AddEntry(entry);
            entry = new StringSchemaEntry("A", 1);
            schema.AddEntry(entry);
        }

        #endregion
    }
}