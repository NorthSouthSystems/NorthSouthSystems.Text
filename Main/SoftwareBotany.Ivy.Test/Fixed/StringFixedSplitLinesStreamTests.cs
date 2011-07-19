using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringFixedSplitLinesStreamTests
    {
        [TestMethod]
        public void Basic()
        {
            int iteration = 0;

            foreach (string[] split in "123".SplitFixedLinesStream(1, 1, 1))
            {
                int offset = 3 * iteration;
                CollectionAssert.AreEqual(new[] { (1 + offset).ToString(), (2 + offset).ToString(), (3 + offset).ToString() }, split);
                iteration++;
            }

            iteration = 0;

            foreach (string[] split in "123456".SplitFixedLinesStream(1, 1, 1))
            {
                int offset = 3 * iteration;
                CollectionAssert.AreEqual(new[] { (1 + offset).ToString(), (2 + offset).ToString(), (3 + offset).ToString() }, split);
                iteration++;
            }

            iteration = 0;

            foreach (string[] split in "123456789".SplitFixedLinesStream(1, 1, 1))
            {
                int offset = 3 * iteration;
                CollectionAssert.AreEqual(new[] { (1 + offset).ToString(), (2 + offset).ToString(), (3 + offset).ToString() }, split);
                iteration++;
            }

            iteration = 0;

            foreach (string[] split in "1 34 67 9".SplitFixedLinesStream(1, 1, 1))
            {
                int offset = 3 * iteration;
                CollectionAssert.AreEqual(new[] { (1 + offset).ToString(), "", (3 + offset).ToString() }, split);
                iteration++;
            }
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNull()
        {
            string s = null;
            s.SplitFixedLinesStream(1).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Argument1()
        {
            "1".SplitFixedLinesStream(2).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Argument2()
        {
            "12".SplitFixedLinesStream(3).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Argument3()
        {
            "12".SplitFixedLinesStream(1, 2).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Argument4()
        {
            "123".SplitFixedLinesStream(2).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Argument5()
        {
            "123".SplitFixedLinesStream(1, 1).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Argument6()
        {
            "12345".SplitFixedLinesStream(1, 2).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Argument7()
        {
            "1234567".SplitFixedLinesStream(1, 2).ToArray();
        }

        #endregion
    }
}