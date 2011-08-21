using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringSchemaSplitExtensionsTests
    {
        [TestMethod]
        public void Basic()
        {
            var schema = new StringSchema(new StringSchemaEntry[]
            {
                new StringSchemaEntry("A", new[] { 1, 1, 1 }),
                new StringSchemaEntry("B", new[] { 2, 2, 2 }),
                new StringSchemaEntry("CD", new[] { 3, 3, 3 })
            });

            var split = "A123".SplitSchemaRow(schema);
            Assert.AreEqual("A", split.Entry.Header);
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, split.ToArray());

            split = "B123456".SplitSchemaRow(schema);
            Assert.AreEqual("B", split.Entry.Header);
            CollectionAssert.AreEqual(new[] { "12", "34", "56" }, split.ToArray());

            split = "CD123456789".SplitSchemaRow(schema);
            Assert.AreEqual("CD", split.Entry.Header);
            CollectionAssert.AreEqual(new[] { "123", "456", "789" }, split.ToArray());
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThisNull()
        {
            var schema = new StringSchema();
            string value = null;
            value.SplitSchemaRow(schema);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SchemaNull()
        {
            string value = "Afoo";
            value.SplitSchemaRow(null);
        }

        #endregion
    }
}