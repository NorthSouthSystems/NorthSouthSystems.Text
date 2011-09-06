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
            NonQuotedBase(StringQuotedSignals.Csv);
            NonQuotedBase(StringQuotedSignals.Pipe);
            NonQuotedBase(StringQuotedSignals.Tab);
        }

        private void NonQuotedBase(StringQuotedSignals signals)
        {
            string[] split = string.Empty.SplitQuotedRow(signals);
            Assert.AreEqual(0, split.Length);

            split = signals.Delimiter.SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { string.Empty, string.Empty }, split);

            split = (signals.Delimiter + signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { string.Empty, string.Empty, string.Empty }, split);

            split = string.Format("a{0}b{0}c", signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, split);

            split = string.Format("a{0}{0}c", signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", string.Empty, "c" }, split);

            if (signals.NewRowIsSpecified)
            {
                split = string.Format("a{0}b{0}c{1}", signals.Delimiter, signals.NewRow).SplitQuotedRow(signals);
                CollectionAssert.AreEqual(new[] { "a", "b", "c" }, split);
            }
        }

        [TestMethod]
        public void Quoted()
        {
            QuotedBase(StringQuotedSignals.Csv);
            QuotedBase(StringQuotedSignals.Pipe);
            QuotedBase(StringQuotedSignals.Tab);
            QuotedBase(new StringQuotedSignals(",", "'", Environment.NewLine));
        }

        private void QuotedBase(StringQuotedSignals signals)
        {
            if (!signals.QuoteIsSpecified)
                throw new NotSupportedException();

            string[] split = string.Format("{0}a{1}{0}{1}b{1}c", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a" + signals.Delimiter, "b", "c" }, split);

            split = string.Format("{0}a{1}a{0}{1}b{1}c", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a" + signals.Delimiter + "a", "b", "c" }, split);

            split = string.Format("a{1}{0}b{1}{0}{1}c", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b" + signals.Delimiter, "c" }, split);

            split = string.Format("a{1}{0}b{1}b{0}{1}c", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b" + signals.Delimiter + "b", "c" }, split);

            split = string.Format("a{1}b{1}{0}c{1}{0}", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" + signals.Delimiter }, split);

            split = string.Format("a{1}b{1}{0}c{1}c{0}", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" + signals.Delimiter + "c" }, split);

            split = string.Format("{0}a{0}{1}{0}b{0}{1}{0}c{0}", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, split);

            split = string.Format("{0}a{1}{0}{1}{0}b{1}{0}{1}{0}c{1}{0}", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a" + signals.Delimiter, "b" + signals.Delimiter, "c" + signals.Delimiter }, split);

            split = string.Format("a{1}b{1}{0}c{2}d{0}", signals.Quote, signals.Delimiter, signals.NewRow).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" + signals.NewRow + "d" }, split);
        }

        [TestMethod]
        public void QuotedEscapedQuote()
        {
            QuotedEscapedQuoteBase(StringQuotedSignals.Csv);
            QuotedEscapedQuoteBase(StringQuotedSignals.Pipe);
            QuotedEscapedQuoteBase(StringQuotedSignals.Tab);
            QuotedEscapedQuoteBase(new StringQuotedSignals(",", "'", Environment.NewLine));
        }

        private void QuotedEscapedQuoteBase(StringQuotedSignals signals)
        {
            string[] split = string.Format("a{1}b{1}{0}{0}c", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", signals.Quote + "c" }, split);

            split = string.Format("{0}a{0}{1}{0}b{0}{1}{0}{0}{0}c{0}", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", signals.Quote + "c" }, split);

            split = string.Format("a{1}b{1}{0}{0}c{1}d", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", signals.Quote + "c", "d" }, split);

            split = string.Format("{0}a{0}{1}{0}b{0}{1}{0}{0}{0}c{0}{1}{0}d{0}", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", signals.Quote + "c", "d" }, split);

            split = string.Format("a{1}b{1}{0}{0}", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", string.Empty }, split);

            split = string.Format("{0}a{0}{1}{0}b{0}{1}{0}{0}", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", string.Empty }, split);

            split = string.Format("a{1}b{1}{0}{0}{1}d", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", string.Empty, "d" }, split);

            split = string.Format("{0}a{0}{1}{0}b{0}{1}{0}{0}{1}{0}d{0}", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", string.Empty, "d" }, split);

            split = string.Format("a{1}b{1}{0}{0}{0}c{1}{0}", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", signals.Quote + "c" + signals.Delimiter }, split);

            split = string.Format("{0}a{0}{1}{0}b{0}{1}{0}{0}{0}c{1}{0}", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", signals.Quote + "c" + signals.Delimiter }, split);

            split = string.Format("a{1}b{1}{0}{0}{0}c{1}{0}{1}d", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", signals.Quote + "c" + signals.Delimiter, "d" }, split);

            split = string.Format("{0}a{0}{1}{0}b{0}{1}{0}{0}{0}c{1}{0}{1}{0}d{0}", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", signals.Quote + "c" + signals.Delimiter, "d" }, split);

            split = string.Format("a{1}b{1}{0}{0}{0}{0}", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", signals.Quote }, split);

            split = string.Format("{0}a{0}{1}{0}b{0}{1}{0}{0}{0}{0}", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", signals.Quote }, split);

            split = string.Format("a{1}b{1}{0}{0}{0}{0}{1}d", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", signals.Quote, "d" }, split);

            split = string.Format("{0}a{0}{1}{0}b{0}{1}{0}{0}{0}{0}{1}{0}d{0}", signals.Quote, signals.Delimiter).SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "a", "b", signals.Quote, "d" }, split);
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