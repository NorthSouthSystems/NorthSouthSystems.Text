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
        public void PositionalSchema()
        {
            var schema = new StringPositionalSchema(new KeyValuePair<string, int[]>[]
            {
                new KeyValuePair<string, int[]>("A", new []{1, 1, 1}),
                new KeyValuePair<string, int[]>("B", new []{2, 2, 2})
            });

            var split = new KeyValuePair<string, string[]>("A", new[] { "1", "2", "3" });
            string join = split.JoinPositionalSchema(' ', schema);
            Assert.AreEqual("A123", join);

            split = new KeyValuePair<string, string[]>("B", new[] { "12", "34", "56" });
            join = split.JoinPositionalSchema(' ', schema);
            Assert.AreEqual("B123456", join);

            split = new KeyValuePair<string, string[]>("B", new[] { "1", "2", "3" });
            join = split.JoinPositionalSchema(' ', schema);
            Assert.AreEqual("B1 2 3 ", join);

            split = new KeyValuePair<string, string[]>("B", new[] { "1", "2", "3" });
            join = split.JoinPositionalSchema('-', schema);
            Assert.AreEqual("B1-2-3-", join);
        }

        #region Exceptions

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

        #endregion
    }
}