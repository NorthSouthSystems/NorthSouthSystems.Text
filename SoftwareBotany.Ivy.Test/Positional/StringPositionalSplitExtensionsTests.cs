using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringPositionalSplitExtensionsTests
    {
        [TestMethod]
        public void Single()
        {
            string[] split = "ABC".SplitPositional(1, 1, 1);
            CollectionAssert.AreEqual(new[] { "A", "B", "C" }, split);

            split = "ABC".SplitPositional(1, 2);
            CollectionAssert.AreEqual(new[] { "A", "BC" }, split);

            split = "ABCD".SplitPositional(1, 2, 1);
            CollectionAssert.AreEqual(new[] { "A", "BC", "D" }, split);

            split = "ABCDEF".SplitPositional(2, 2, 2);
            CollectionAssert.AreEqual(new[] { "AB", "CD", "EF" }, split);
        }

        [TestMethod]
        public void Enumerable()
        {
            int iteration = 0;

            foreach (string[] split in new[] { "123" }.SplitPositional(1, 1, 1))
            {
                int offset = 3 * iteration;
                CollectionAssert.AreEqual(new[] { (1 + offset).ToString(), (2 + offset).ToString(), (3 + offset).ToString() }, split);
                iteration++;
            }

            iteration = 0;

            foreach (string[] split in new[] { "123", "456" }.SplitPositional(1, 1, 1))
            {
                int offset = 3 * iteration;
                CollectionAssert.AreEqual(new[] { (1 + offset).ToString(), (2 + offset).ToString(), (3 + offset).ToString() }, split);
                iteration++;
            }

            iteration = 0;

            foreach (string[] split in new[] { "123", "456", "789" }.SplitPositional(1, 1, 1))
            {
                int offset = 3 * iteration;
                CollectionAssert.AreEqual(new[] { (1 + offset).ToString(), (2 + offset).ToString(), (3 + offset).ToString() }, split);
                iteration++;
            }
        }

        [TestMethod]
        public void Repeating()
        {
            int iteration = 0;

            foreach (string[] split in "123".SplitPositionalRepeating(1, 1, 1))
            {
                int offset = 3 * iteration;
                CollectionAssert.AreEqual(new[] { (1 + offset).ToString(), (2 + offset).ToString(), (3 + offset).ToString() }, split);
                iteration++;
            }

            iteration = 0;

            foreach (string[] split in "123456".SplitPositionalRepeating(1, 1, 1))
            {
                int offset = 3 * iteration;
                CollectionAssert.AreEqual(new[] { (1 + offset).ToString(), (2 + offset).ToString(), (3 + offset).ToString() }, split);
                iteration++;
            }

            iteration = 0;

            foreach (string[] split in "123456789".SplitPositionalRepeating(1, 1, 1))
            {
                int offset = 3 * iteration;
                CollectionAssert.AreEqual(new[] { (1 + offset).ToString(), (2 + offset).ToString(), (3 + offset).ToString() }, split);
                iteration++;
            }
        }

        [TestMethod]
        public void SchemaSingle()
        {
            var schema = new StringPositionalSchema(new StringPositionalSchemaEntry[]
            {
                new StringPositionalSchemaEntry("A", new []{1, 1, 1}),
                new StringPositionalSchemaEntry("B", new []{2, 2, 2})
            });

            var split = "A123".SplitPositionalSchema(schema);
            Assert.AreEqual("A", split.Entry.Header);
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, split.ToArray());

            split = "B123456".SplitPositionalSchema(schema);
            Assert.AreEqual("B", split.Entry.Header);
            CollectionAssert.AreEqual(new[] { "12", "34", "56" }, split.ToArray());
        }

        [TestMethod]
        public void SchemaEnumerable()
        {
            var schema = new StringPositionalSchema(new StringPositionalSchemaEntry[]
            {
                new StringPositionalSchemaEntry("A", new []{1, 1, 1}),
                new StringPositionalSchemaEntry("B", new []{2, 2, 2})
            });

            var splits = new[] { "A123", "B123456" }.SplitPositionalSchema(schema).ToArray();
            Assert.AreEqual("A", splits[0].Entry.Header);
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, splits[0].ToArray());
            Assert.AreEqual("B", splits[1].Entry.Header);
            CollectionAssert.AreEqual(new[] { "12", "34", "56" }, splits[1].ToArray());
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SingleArgument1()
        {
            "1".SplitPositional(2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SingleArgument2()
        {
            "12".SplitPositional(3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SingleArgument3()
        {
            "12".SplitPositional(1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SingleArgument4()
        {
            "1234".SplitPositional(1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RepeatingArgument1()
        {
            "1".SplitPositionalRepeating(2).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RepeatingArgument2()
        {
            "12".SplitPositionalRepeating(3).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RepeatingArgument3()
        {
            "12".SplitPositionalRepeating(1, 2).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RepeatingArgument4()
        {
            "123".SplitPositionalRepeating(2).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RepeatingArgument5()
        {
            "123".SplitPositionalRepeating(1, 1).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RepeatingArgument6()
        {
            "12345".SplitPositionalRepeating(1, 2).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RepeatingArgument7()
        {
            "1234567".SplitPositionalRepeating(1, 2).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SchemaValueNull()
        {
            var schema = new StringPositionalSchema();
            string value = null;
            value.SplitPositionalSchema(schema);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SchemaSchemaNull()
        {
            string value = "Afoo";
            value.SplitPositionalSchema(null);
        }

        #endregion
    }
}