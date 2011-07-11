using System;
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

        #endregion
    }
}