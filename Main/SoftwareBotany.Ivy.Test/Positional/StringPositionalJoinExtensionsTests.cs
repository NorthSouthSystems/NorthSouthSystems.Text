using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringPositionalJoinExtensionsTests
    {
        [TestMethod]
        public void Basic()
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

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThisNull()
        {
            string[] strings = null;
            strings.JoinPositional(' ', 1, 1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Argument1()
        {
            new[] { "A", "B" }.JoinPositional(' ', 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Argument2()
        {
            new[] { "AB", "C" }.JoinPositional(' ', 1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Argument3()
        {
            new[] { "A", "BC" }.JoinPositional(' ', 1, 1);
        }

        #endregion
    }
}