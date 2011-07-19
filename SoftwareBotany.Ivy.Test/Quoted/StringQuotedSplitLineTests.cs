using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringQuotedSplitLineTests
    {
        [TestMethod]
        public void NonQuoted()
        {
            string[] split = string.Empty.SplitQuotedLine(StringQuotedSignals.Csv);
            Assert.AreEqual(0, split.Length);

            split = "a,b,c".SplitQuotedLine(StringQuotedSignals.Csv);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a", split[0]);
            Assert.AreEqual("b", split[1]);
            Assert.AreEqual("c", split[2]);

            split = ("a,b,c" + StringQuotedSignals.Csv.NewLine).SplitQuotedLine(StringQuotedSignals.Csv);

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

            split = "'a,',b,c".SplitQuotedLine(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a,", split[0]);
            Assert.AreEqual("b", split[1]);
            Assert.AreEqual("c", split[2]);

            split = "'a,a',b,c".SplitQuotedLine(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a,a", split[0]);
            Assert.AreEqual("b", split[1]);
            Assert.AreEqual("c", split[2]);

            split = "a,'b,',c".SplitQuotedLine(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a", split[0]);
            Assert.AreEqual("b,", split[1]);
            Assert.AreEqual("c", split[2]);

            split = "a,'b,b',c".SplitQuotedLine(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a", split[0]);
            Assert.AreEqual("b,b", split[1]);
            Assert.AreEqual("c", split[2]);

            split = "a,b,'c,'".SplitQuotedLine(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a", split[0]);
            Assert.AreEqual("b", split[1]);
            Assert.AreEqual("c,", split[2]);

            split = "a,b,'c,c'".SplitQuotedLine(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a", split[0]);
            Assert.AreEqual("b", split[1]);
            Assert.AreEqual("c,c", split[2]);

            split = "a,b,''c".SplitQuotedLine(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("a", split[0]);
            Assert.AreEqual("b", split[1]);
            Assert.AreEqual("'c", split[2]);

            split = ("a,b,'c" + signals.NewLine + "d'").SplitQuotedLine(signals);
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
            split = s.SplitQuotedLine(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual(value0, split[0]);
            Assert.AreEqual(value1, split[1]);
            Assert.AreEqual(value2, split[2]);
        }

        [TestMethod]
        public void ComplicatedSeparator()
        {
            StringQuotedSignals signals = new StringQuotedSignals("ab", null, null);
            string[] split = "1aab2aab3a".SplitQuotedLine(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("1a", split[0]);
            Assert.AreEqual("2a", split[1]);
            Assert.AreEqual("3a", split[2]);

            signals = new StringQuotedSignals("ababb", null, null);
            split = "1abababb2abababb3ab".SplitQuotedLine(signals);

            Assert.AreEqual(3, split.Length);
            Assert.AreEqual("1ab", split[0]);
            Assert.AreEqual("2ab", split[1]);
            Assert.AreEqual("3ab", split[2]);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNull1()
        {
            string s = null;
            s.SplitQuotedLine(StringQuotedSignals.Csv);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNull2()
        {
            string s = string.Empty;
            s.SplitQuotedLine(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NewLinesInArgument()
        {
            string s = string.Format("a,b,c{0}d,e,f{0}g,h,i", StringQuotedSignals.Csv.NewLine);
            s.SplitQuotedLine(StringQuotedSignals.Csv);
        }

        #endregion
    }
}