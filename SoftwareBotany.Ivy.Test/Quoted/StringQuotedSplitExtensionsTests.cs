using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringQuotedSplitExtensionsTests
    {
        [TestMethod]
        public void NonQuoted()
        {
            string[] split = string.Empty.SplitQuoted(StringQuotedSignals.Csv);
            Assert.AreEqual(0, split.Length);

            split = "a,b,c".SplitQuoted(StringQuotedSignals.Csv);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a", split[0]);
            Assert.AreEqual("b", split[1]);
            Assert.AreEqual("c", split[2]);

            split = ("a,b,c" + StringQuotedSignals.Csv.NewLine).SplitQuoted(StringQuotedSignals.Csv);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a", split[0]);
            Assert.AreEqual("b", split[1]);
            Assert.AreEqual("c", split[2]);
        }

        [TestMethod]
        public void Quoted()
        {
            StringQuotedSignals signals = new StringQuotedSignals(",", "'", Environment.NewLine);
            string[] split;

            split = "'a,',b,c".SplitQuoted(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a,", split[0]);
            Assert.AreEqual("b", split[1]);
            Assert.AreEqual("c", split[2]);

            split = "'a,a',b,c".SplitQuoted(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a,a", split[0]);
            Assert.AreEqual("b", split[1]);
            Assert.AreEqual("c", split[2]);

            split = "a,'b,',c".SplitQuoted(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a", split[0]);
            Assert.AreEqual("b,", split[1]);
            Assert.AreEqual("c", split[2]);

            split = "a,'b,b',c".SplitQuoted(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a", split[0]);
            Assert.AreEqual("b,b", split[1]);
            Assert.AreEqual("c", split[2]);

            split = "a,b,'c,'".SplitQuoted(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a", split[0]);
            Assert.AreEqual("b", split[1]);
            Assert.AreEqual("c,", split[2]);

            split = "a,b,'c,c'".SplitQuoted(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a", split[0]);
            Assert.AreEqual("b", split[1]);
            Assert.AreEqual("c,c", split[2]);

            split = "a,b,''c".SplitQuoted(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a", split[0]);
            Assert.AreEqual("b", split[1]);
            Assert.AreEqual("'c", split[2]);

            split = ("a,b,'c" + signals.NewLine + "d'").SplitQuoted(signals);
            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a", split[0]);
            Assert.AreEqual("b", split[1]);
            Assert.AreEqual("c" + signals.NewLine + "d", split[2]);
        }

        [TestMethod]
        public void Triple() { TripleBase(",", "", "", "a", "b", "c"); }

        private void TripleBase(string separator, string quote, string newline, string value0, string value1, string value2)
        {
            StringQuotedSignals signals = new StringQuotedSignals(separator, quote, newline);
            string s;
            string[] split;

            s = string.Format("{1}{0}{2}{0}{3}", separator, value0, value1, value2);
            split = s.SplitQuoted(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual(value0, split[0]);
            Assert.AreEqual(value1, split[1]);
            Assert.AreEqual(value2, split[2]);
        }

        [TestMethod]
        public void ComplicatedSeparator()
        {
            StringQuotedSignals signals = new StringQuotedSignals("ab", null, null);
            string[] split = "1aab2aab3a".SplitQuoted(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("1a", split[0]);
            Assert.AreEqual("2a", split[1]);
            Assert.AreEqual("3a", split[2]);

            signals = new StringQuotedSignals("ababb", null, null);
            split = "1abababb2abababb3ab".SplitQuoted(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("1ab", split[0]);
            Assert.AreEqual("2ab", split[1]);
            Assert.AreEqual("3ab", split[2]);
        }

        [TestMethod]
        public void MultilineChars()
        {
            string[][] splits = string.Format("a,b,c{0}d,e,f{0}g,h,i", StringQuotedSignals.Csv.NewLine)
                .SplitQuotedMultiline(StringQuotedSignals.Csv)
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

            splits = string.Format("a,b,c{0}", StringQuotedSignals.Csv.NewLine)
                .SplitQuotedMultiline(StringQuotedSignals.Csv)
                .ToArray();

            Assert.AreEqual(1, splits.Length);

            Assert.AreEqual(3, splits[0].Length);
            Assert.AreEqual("a", splits[0][0]);
            Assert.AreEqual("b", splits[0][1]);
            Assert.AreEqual("c", splits[0][2]);
        }

        [TestMethod]
        public void MultilineStrings()
        {
            string[][] splits = new[] { "a,b,c", "d,e,f", "g,h,i" }
                .SplitQuotedMultiline(StringQuotedSignals.Csv)
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
                .SplitQuotedMultiline(StringQuotedSignals.Csv)
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
        public void ArgumentNull()
        {
            string s = null;
            s.SplitQuoted(StringQuotedSignals.Csv);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SingleOrDefaultInvalidOperation()
        {
            string s = string.Format("a,b,c{0}d,e,f{0}g,h,i", StringQuotedSignals.Csv.NewLine);
            s.SplitQuoted(StringQuotedSignals.Csv);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MultilineCharsArgumentNull()
        {
            string s = null;
            s.SplitQuotedMultiline(StringQuotedSignals.Csv).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MultilineStringsArgumentNull()
        {
            string[] sa = null;
            sa.SplitQuotedMultiline(StringQuotedSignals.Csv).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MultilineStringsArgument()
        {
            string[] sa = new[] { "A", null };
            sa.SplitQuotedMultiline(StringQuotedSignals.Csv).ToArray();
        }

        #endregion
    }
}