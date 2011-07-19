using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringFixedSplitLineTests
    {
        [TestMethod]
        public void Basic()
        {
            string[] split = "ABC".SplitFixedLine(1, 1, 1);
            CollectionAssert.AreEqual(new[] { "A", "B", "C" }, split);

            split = "ABC".SplitFixedLine(1, 2);
            CollectionAssert.AreEqual(new[] { "A", "BC" }, split);

            split = "ABCD".SplitFixedLine(1, 2, 1);
            CollectionAssert.AreEqual(new[] { "A", "BC", "D" }, split);

            split = "ABCDEF".SplitFixedLine(2, 2, 2);
            CollectionAssert.AreEqual(new[] { "AB", "CD", "EF" }, split);

            split = "A B C ".SplitFixedLine(2, 2, 2);
            CollectionAssert.AreEqual(new[] { "A", "B", "C" }, split);

            split = "A B   ".SplitFixedLine(2, 2, 2);
            CollectionAssert.AreEqual(new[] { "A", "B", "" }, split);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNull()
        {
            string s = null;
            s.SplitFixedLine(1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentEmpty()
        {
            string.Empty.SplitFixedLine(1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Argument1()
        {
            "1".SplitFixedLine(2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Argument2()
        {
            "12".SplitFixedLine(3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Argument3()
        {
            "12".SplitFixedLine(1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Argument4()
        {
            "1234".SplitFixedLine(1, 2);
        }

        #endregion
    }
}