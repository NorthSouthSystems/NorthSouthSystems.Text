using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringPositionalJoinExtensionsTests
    {
        [TestMethod]
        public void Positional()
        {
            string join = new [] { "A", "B", "C"}.JoinPositional(' ', 1, 1, 1);
            Assert.AreEqual("ABC", join);

            join = new[] { "A", "B", "C" }.JoinPositional(' ', 2, 1, 1);
            Assert.AreEqual("A BC", join);

            join = new[] { "A", "B", "C" }.JoinPositional('1', 2, 1, 1);
            Assert.AreEqual("A1BC", join);

            join = new[] { "A", "B", "C" }.JoinPositional(' ', 2, 2, 2);
            Assert.AreEqual("A B C ", join);

            join = new[] { "AB", "CD", "EF" }.JoinPositional(' ', 2, 2, 2);
            Assert.AreEqual("ABCDEF", join);

            join = new[] { "AB", "CD", "EF" }.JoinPositional('1', 3, 2, 2);
            Assert.AreEqual("AB1CDEF", join);
        }

        [TestMethod]
        public void Schema()
        {
            var a = new StringPositionalSchemaEntry("A", new []{1, 1, 1});
            var b = new StringPositionalSchemaEntry("B", new []{2, 2, 2});

            var split = new StringPositionalSchemaEntryAndStrings(a, new[] { "1", "2", "3" });
            string join = split.JoinPositionalSchema(' ');
            Assert.AreEqual("A123", join);

            split = new StringPositionalSchemaEntryAndStrings(b, new[] { "12", "34", "56" });
            join = split.JoinPositionalSchema(' ');
            Assert.AreEqual("B123456", join);

            split = new StringPositionalSchemaEntryAndStrings(b, new[] { "1", "2", "3" });
            join = split.JoinPositionalSchema(' ');
            Assert.AreEqual("B1 2 3 ", join);

            split = new StringPositionalSchemaEntryAndStrings(b, new[] { "1", "2", "3" });
            join = split.JoinPositionalSchema('-');
            Assert.AreEqual("B1-2-3-", join);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PositionalNull()
        {
            string[] strings = null;
            strings.JoinPositional(' ', 1, 1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PositionalArgument1()
        {
            new[] { "A", "B" }.JoinPositional(' ', 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PositionalArgument2()
        {
            new[] { "AB", "C" }.JoinPositional(' ', 1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PositionalArgument3()
        {
            new[] { "A", "BC" }.JoinPositional(' ', 1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SchemaNull()
        {
            StringPositionalSchemaEntryAndStrings split = null;
            split.JoinPositionalSchema(' ');
        }

        #endregion
    }
}