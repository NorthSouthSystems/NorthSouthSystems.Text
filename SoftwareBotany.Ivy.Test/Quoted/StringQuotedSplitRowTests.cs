using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringQuotedSplitRowTests
    {
        [TestMethod]
        public void NonQuoted()
        {
            string[] split = string.Empty.SplitQuotedRow(StringQuotedSignals.Csv);
            Assert.AreEqual(0, split.Length);

            split = "a,b,c".SplitQuotedRow(StringQuotedSignals.Csv);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, split);

            split = ("a,b,c" + StringQuotedSignals.Csv.NewRow).SplitQuotedRow(StringQuotedSignals.Csv);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, split);
        }

        [TestMethod]
        public void Quoted()
        {
            StringQuotedSignals signals = new StringQuotedSignals(",", "'", Environment.NewLine);
            string[] split;

            split = "'a,',b,c".SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a,", "b", "c" }, split);

            split = "'a,a',b,c".SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a,a", "b", "c" }, split);

            split = "a,'b,',c".SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b,", "c" }, split);

            split = "a,'b,b',c".SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b,b", "c" }, split);

            split = "a,b,'c,'".SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", "c," }, split);

            split = "a,b,'c,c'".SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", "c,c" }, split);

            split = "'a','b','c'".SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, split);

            split = "'a,','b,','c,'".SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a,", "b,", "c," }, split);

            split = "a,b,''c".SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", "'c" }, split);

            split = "a,b,'''c,'".SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", "'c," }, split);

            split = ("a,b,'c" + signals.NewRow + "d'").SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" + signals.NewRow + "d" }, split);
        }

        [TestMethod]
        public void ComplicatedSeparator()
        {
            StringQuotedSignals signals = new StringQuotedSignals("ab", null, null);
            string[] split = "1aab2aab3a".SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "1a", "2a", "3a" }, split);

            signals = new StringQuotedSignals("ababb", null, null);
            split = "1abababb2abababb3ab".SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "1ab", "2ab", "3ab" }, split);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RowNull()
        {
            string s = null;
            s.SplitQuotedRow(StringQuotedSignals.Csv);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SignalsNull()
        {
            string s = string.Empty;
            s.SplitQuotedRow(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NewLineInArgument1()
        {
            string s = string.Format("a,b,c{0}d", StringQuotedSignals.Csv.NewRow);
            s.SplitQuotedRow(StringQuotedSignals.Csv);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NewLineInArgument2()
        {
            string s = string.Format("a,b,c{0}d,e,f", StringQuotedSignals.Csv.NewRow);
            s.SplitQuotedRow(StringQuotedSignals.Csv);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NewLineInArgument3()
        {
            string s = string.Format("a,b,c{0}d,e,f{0}g,h,i", StringQuotedSignals.Csv.NewRow);
            s.SplitQuotedRow(StringQuotedSignals.Csv);
        }

        #endregion
    }
}