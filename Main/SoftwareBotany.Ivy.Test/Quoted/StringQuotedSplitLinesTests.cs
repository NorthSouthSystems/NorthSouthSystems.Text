using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringQuotedSplitLinesTests
    {
        [TestMethod]
        public void Basic()
        {
            string[][] splits = new[] { "a,b,c", "d,e,f", "g,h,i" }
                .SplitQuotedLines(StringQuotedSignals.Csv)
                .ToArray();

            Assert.AreEqual(3, splits.Length);

            Assert.AreEqual(3, splits[0].Length);
            Assert.AreEqual("a", splits[0][0]);
            Assert.AreEqual("b", splits[0][1]);
            Assert.AreEqual("c", splits[0][2]);

            Assert.AreEqual(3, splits[1].Length);
            Assert.AreEqual("d", splits[1][0]);
            Assert.AreEqual("e", splits[1][1]);
            Assert.AreEqual("f", splits[1][2]);

            Assert.AreEqual(3, splits[2].Length);
            Assert.AreEqual("g", splits[2][0]);
            Assert.AreEqual("h", splits[2][1]);
            Assert.AreEqual("i", splits[2][2]);

            splits = new[] { "a,b,c", string.Empty }
                .SplitQuotedLines(StringQuotedSignals.Csv)
                .ToArray();

            Assert.AreEqual(2, splits.Length);

            Assert.AreEqual(3, splits[0].Length);
            Assert.AreEqual("a", splits[0][0]);
            Assert.AreEqual("b", splits[0][1]);
            Assert.AreEqual("c", splits[0][2]);

            Assert.AreEqual(0, splits[1].Length);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNull1()
        {
            string[] sa = null;
            sa.SplitQuotedLines(StringQuotedSignals.Csv).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNull2()
        {
            string[] sa = new [] { string.Empty };
            sa.SplitQuotedLines(null).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentInArray()
        {
            string[] sa = new[] { "A", null };
            sa.SplitQuotedLines(StringQuotedSignals.Csv).ToArray();
        }

        #endregion
    }
}