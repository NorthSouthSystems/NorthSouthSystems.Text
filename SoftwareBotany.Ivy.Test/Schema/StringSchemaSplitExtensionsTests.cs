using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringSchemaSplitExtensionsTests
    {
        [TestMethod]
        public void Line()
        {
            var schema = new StringSchema(new StringSchemaEntry[]
            {
                new StringSchemaEntry("A", 1, 1, 1),
                new StringSchemaEntry("B", 2, 2, 2)
            });

            var split = "A123".SplitSchemaLine(schema);
            Assert.AreEqual("A", split.Entry.Header);
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, split.ToArray());

            split = "B123456".SplitSchemaLine(schema);
            Assert.AreEqual("B", split.Entry.Header);
            CollectionAssert.AreEqual(new[] { "12", "34", "56" }, split.ToArray());
        }

        [TestMethod]
        public void Lines()
        {
            var schema = new StringSchema(new StringSchemaEntry[]
            {
                new StringSchemaEntry("A", 1, 1, 1),
                new StringSchemaEntry("B", 2, 2, 2)
            });

            var splits = new[] { "A123", "B123456" }.SplitSchemaLines(schema).ToArray();
            Assert.AreEqual("A", splits[0].Entry.Header);
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, splits[0].ToArray());
            Assert.AreEqual("B", splits[1].Entry.Header);
            CollectionAssert.AreEqual(new[] { "12", "34", "56" }, splits[1].ToArray());
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValueNull()
        {
            var schema = new StringSchema();
            string value = null;
            value.SplitSchemaLine(schema);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SchemaNull()
        {
            string value = "Afoo";
            value.SplitSchemaLine(null);
        }

        #endregion
    }
}