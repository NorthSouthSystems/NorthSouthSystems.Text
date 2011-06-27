using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringPositionalSchemaTests
    {
        [TestMethod]
        public void Basic()
        {
            var schema = new StringPositionalSchema(new KeyValuePair<string, int[]>[]
            {
                new KeyValuePair<string, int[]>("A", new [] { 1 }),
                new KeyValuePair<string, int[]>("B", new [] { 2 }),
            });

            CollectionAssert.AreEqual(new[] { 1 }, schema["A"]);
            CollectionAssert.AreEqual(new[] { 2 }, schema["B"]);

            var entry = schema.GetEntryForValue("Afoo");

            Assert.AreEqual("A", entry.Key);
            CollectionAssert.AreEqual(new[] { 1 }, entry.Value);

            entry = schema.GetEntryForValue("Bfoo");

            Assert.AreEqual("B", entry.Key);
            CollectionAssert.AreEqual(new[] { 2 }, entry.Value);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetEntryForValueArgument()
        {
            var schema = new StringPositionalSchema();
            KeyValuePair<string, int[]> entry = new KeyValuePair<string, int[]>("A", new[] { 1 });
            schema.AddEntry(entry);
            schema.GetEntryForValue("Bfoo");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void VerifyEntryArgumentBadKey()
        {
            var schema = new StringPositionalSchema();
            KeyValuePair<string, int[]> entry = new KeyValuePair<string, int[]>(string.Empty, new[] { 1 });
            schema.AddEntry(entry);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void VerifyEntryArgumentOverlappedKey1()
        {
            var schema = new StringPositionalSchema();
            KeyValuePair<string, int[]> entry = new KeyValuePair<string, int[]>("A", new[] { 1 });
            schema.AddEntry(entry);
            entry = new KeyValuePair<string, int[]>("AB", new[] { 1 });
            schema.AddEntry(entry);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void VerifyEntryArgumentOverlappedKey2()
        {
            var schema = new StringPositionalSchema();
            KeyValuePair<string, int[]> entry = new KeyValuePair<string, int[]>("AB", new[] { 1 });
            schema.AddEntry(entry);
            entry = new KeyValuePair<string, int[]>("A", new[] { 1 });
            schema.AddEntry(entry);
        }

        #endregion
    }
}