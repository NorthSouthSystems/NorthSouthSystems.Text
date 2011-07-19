using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringFixedSplitLinesTests
    {
        [TestMethod]
        public void Basic()
        {
            string[][] splits = new[] { "123" }.SplitFixedLines(1, 1, 1).ToArray();

            for (int i = 0; i < splits.Length; i++)
                CollectionAssert.AreEqual(new[] { (1 + (3 * i)).ToString(), (2 + (3 * i)).ToString(), (3 + (3 * i)).ToString() }, splits[i]);

            splits = new[] { "123", "456" }.SplitFixedLines(1, 1, 1).ToArray();

            for (int i = 0; i < splits.Length; i++)
                CollectionAssert.AreEqual(new[] { (1 + (3 * i)).ToString(), (2 + (3 * i)).ToString(), (3 + (3 * i)).ToString() }, splits[i]);

            splits = new[] { "123", "456", "789" }.SplitFixedLines(1, 1, 1).ToArray();

            for (int i = 0; i < splits.Length; i++)
                CollectionAssert.AreEqual(new[] { (1 + (3 * i)).ToString(), (2 + (3 * i)).ToString(), (3 + (3 * i)).ToString() }, splits[i]);

            splits = new[] { " 23", " 56", " 89" }.SplitFixedLines(1, 1, 1).ToArray();

            for (int i = 0; i < splits.Length; i++)
                CollectionAssert.AreEqual(new[] { "", (2 + (3 * i)).ToString(), (3 + (3 * i)).ToString() }, splits[i]);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNull1()
        {
            string[] s = null;
            s.SplitFixedLines(1).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNull2()
        {
            string[] s = new string[] { null };
            s.SplitFixedLines(1).ToArray();
        }

        #endregion
    }
}