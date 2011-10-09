using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringQuotedSplitRowTests
    {
        private static string[] Split(StringQuotedSignals signals, string format)
        {
            return string.Format(format, signals.Delimiter, signals.Quote, signals.NewRow, signals.Escape).SplitQuotedRow(signals);
        }

        [TestMethod]
        public void NonQuoted()
        {
            NonQuotedBase(StringQuotedSignals.Csv);
            NonQuotedBase(StringQuotedSignals.Pipe);
            NonQuotedBase(StringQuotedSignals.Tab);
        }

        private static void NonQuotedBase(StringQuotedSignals signals)
        {
            string[] split = Split(signals, string.Empty);
            Assert.AreEqual(0, split.Length);

            split = Split(signals, "{0}");
            CollectionAssert.AreEqual(new[] { string.Empty, string.Empty }, split);

            split = Split(signals, "{0}{0}");
            CollectionAssert.AreEqual(new[] { string.Empty, string.Empty, string.Empty }, split);

            split = Split(signals, "a");
            CollectionAssert.AreEqual(new[] { "a"}, split);

            split = Split(signals, "a{0}b");
            CollectionAssert.AreEqual(new[] { "a", "b" }, split);

            split = Split(signals, "a{0}b{0}c");
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, split);

            split = Split(signals, "a{0}b{0}");
            CollectionAssert.AreEqual(new[] { "a", "b", string.Empty }, split);

            split = Split(signals, "a{0}{0}c");
            CollectionAssert.AreEqual(new[] { "a", string.Empty, "c" }, split);

            if (signals.NewRowIsSpecified)
            {
                split = Split(signals, "{2}");
                CollectionAssert.AreEqual(new[] { string.Empty }, split);

                split = Split(signals, "a{2}");
                CollectionAssert.AreEqual(new[] { "a" }, split);

                split = Split(signals, "a{0}b{2}");
                CollectionAssert.AreEqual(new[] { "a", "b" }, split);

                split = Split(signals, "a{0}b{0}c{2}");
                CollectionAssert.AreEqual(new[] { "a", "b", "c" }, split);
            }
        }

        [TestMethod]
        public void Quoted()
        {
            QuotedBase(StringQuotedSignals.Csv);
            QuotedBase(StringQuotedSignals.Pipe);
            QuotedBase(StringQuotedSignals.Tab);
            QuotedBase(new StringQuotedSignals(",", "'", Environment.NewLine, string.Empty));
        }

        private static void QuotedBase(StringQuotedSignals signals)
        {
            if (!signals.QuoteIsSpecified)
                throw new NotSupportedException();

            QuotedBaseQuotingUnnecessary(signals);
            QuotedBaseQuotingNecessary(signals);
        }

        private static void QuotedBaseQuotingUnnecessary(StringQuotedSignals signals)
        {
            string[] split = Split(signals, "{1}{1}");
            CollectionAssert.AreEqual(new[] { string.Empty }, split);

            split = Split(signals, "{1}a{1}");
            CollectionAssert.AreEqual(new[] { "a" }, split);

            split = Split(signals, "{1}a{1}{0}{1}b{1}");
            CollectionAssert.AreEqual(new[] { "a", "b" }, split);

            split = Split(signals, "{1}a{1}{0}{1}b{1}{0}{1}c{1}");
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, split);

            split = Split(signals, "{1}a{1}{0}b{0}{1}c{1}");
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, split);

            split = Split(signals, "a{0}{1}b{1}{0}c");
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, split);

            split = Split(signals, "a{0}{1}{1}{0}c");
            CollectionAssert.AreEqual(new[] { "a", string.Empty, "c" }, split);

            split = Split(signals, "{1}a{1}{0}{1}b{1}{0}");
            CollectionAssert.AreEqual(new[] { "a", "b", string.Empty }, split);

            split = Split(signals, "{1}a{1}{0}{1}b{1}{0}{1}{1}");
            CollectionAssert.AreEqual(new[] { "a", "b", string.Empty }, split);
        }

        private static void QuotedBaseQuotingNecessary(StringQuotedSignals signals)
        {
            string[] split = Split(signals, "{1}{1}{1}{1}");
            CollectionAssert.AreEqual(new[] { signals.Quote }, split);

            split = Split(signals, "{1}a{0}{1}");
            CollectionAssert.AreEqual(new[] { "a" + signals.Delimiter}, split);

            split = Split(signals, "{1}a{0}a{1}");
            CollectionAssert.AreEqual(new[] { "a" + signals.Delimiter + "a" }, split);

            split = Split(signals, "{1}a{0}a{1}{0}b");
            CollectionAssert.AreEqual(new[] { "a" + signals.Delimiter + "a", "b" }, split);

            split = Split(signals, "{1}a{0}a{1}{0}{1}b{0}{1}");
            CollectionAssert.AreEqual(new[] { "a" + signals.Delimiter + "a", "b" + signals.Delimiter }, split);

            split = Split(signals, "{1}a{0}a{1}{0}{1}b{0}b{1}");
            CollectionAssert.AreEqual(new[] { "a" + signals.Delimiter + "a", "b" + signals.Delimiter + "b" }, split);

            split = Split(signals, "{1}a{0}a{1}{0}{1}b{0}b{1}{0}c");
            CollectionAssert.AreEqual(new[] { "a" + signals.Delimiter + "a", "b" + signals.Delimiter + "b", "c" }, split);

            split = Split(signals, "{1}a{0}a{1}{0}{1}b{0}b{1}{0}{1}c{0}{1}");
            CollectionAssert.AreEqual(new[] { "a" + signals.Delimiter + "a", "b" + signals.Delimiter + "b", "c" + signals.Delimiter }, split);

            split = Split(signals, "{1}a{0}a{1}{0}{1}b{0}b{1}{0}{1}c{0}c{1}");
            CollectionAssert.AreEqual(new[] { "a" + signals.Delimiter + "a", "b" + signals.Delimiter + "b", "c" + signals.Delimiter + "c" }, split);

            split = Split(signals, "{1}a{0}{1}{0}b{0}c");
            CollectionAssert.AreEqual(new[] { "a" + signals.Delimiter, "b", "c" }, split);

            split = Split(signals, "a{0}{1}b{0}{1}{0}c");
            CollectionAssert.AreEqual(new[] { "a", "b" + signals.Delimiter, "c" }, split);

            split = Split(signals, "a{0}b{0}{1}c{0}{1}");
            CollectionAssert.AreEqual(new[] { "a", "b", "c" + signals.Delimiter }, split);

            split = Split(signals, "{1}a{0}a{1}{0}b{0}c");
            CollectionAssert.AreEqual(new[] { "a" + signals.Delimiter + "a", "b", "c" }, split);

            split = Split(signals, "a{0}{1}b{0}b{1}{0}c");
            CollectionAssert.AreEqual(new[] { "a", "b" + signals.Delimiter + "b", "c" }, split);

            split = Split(signals, "a{0}b{0}{1}c{0}c{1}");
            CollectionAssert.AreEqual(new[] { "a", "b", "c" + signals.Delimiter + "c" }, split);

            split = Split(signals, "a{0}b{0}c{0}{1}{0}{1}");
            CollectionAssert.AreEqual(new[] { "a", "b", "c", signals.Delimiter }, split);

            if (signals.NewRowIsSpecified)
            {
                split = Split(signals, "{1}a{2}{1}");
                CollectionAssert.AreEqual(new[] { "a" + signals.NewRow }, split);

                split = Split(signals, "{1}a{2}a{1}");
                CollectionAssert.AreEqual(new[] { "a" + signals.NewRow + "a" }, split);

                split = Split(signals, "{1}a{2}a{1}{0}b");
                CollectionAssert.AreEqual(new[] { "a" + signals.NewRow + "a", "b" }, split);

                split = Split(signals, "{1}a{2}a{1}{0}{1}b{2}{1}");
                CollectionAssert.AreEqual(new[] { "a" + signals.NewRow + "a", "b" + signals.NewRow }, split);

                split = Split(signals, "{1}a{2}a{1}{0}{1}b{2}b{1}");
                CollectionAssert.AreEqual(new[] { "a" + signals.NewRow + "a", "b" + signals.NewRow + "b" }, split);

                split = Split(signals, "{1}a{2}a{1}{0}{1}b{2}b{1}{0}{1}{2}{1}");
                CollectionAssert.AreEqual(new[] { "a" + signals.NewRow + "a", "b" + signals.NewRow + "b", signals.NewRow }, split);
            }
        }

        [TestMethod]
        public void QuotedEscapedQuote()
        {
            QuotedEscapedQuoteBase(StringQuotedSignals.Csv);
            QuotedEscapedQuoteBase(StringQuotedSignals.Pipe);
            QuotedEscapedQuoteBase(StringQuotedSignals.Tab);
            QuotedEscapedQuoteBase(new StringQuotedSignals(",", "'", Environment.NewLine, "\\"));
            QuotedEscapedQuoteBase(new StringQuotedSignals("DELIMITER", "QUOTE", "NEWLINE", "ESCAPE"));
        }

        private static void QuotedEscapedQuoteBase(StringQuotedSignals signals)
        {
            if (!signals.QuoteIsSpecified)
                throw new NotSupportedException();

            string[] split = Split(signals, "{1}{1}a");
            CollectionAssert.AreEqual(new[] { signals.Quote + "a" }, split);

            split = Split(signals, "{1}{1}{1}a{1}");
            CollectionAssert.AreEqual(new[] { signals.Quote + "a" }, split);

            split = Split(signals, "{1}{1}{1}{1}a");
            CollectionAssert.AreEqual(new[] { signals.Quote + signals.Quote + "a" }, split);

            split = Split(signals, "{1}{1}{1}{1}{1}a{1}");
            CollectionAssert.AreEqual(new[] { signals.Quote + signals.Quote + "a" }, split);

            split = Split(signals, "{1}{1}a{0}b");
            CollectionAssert.AreEqual(new[] { signals.Quote + "a", "b" }, split);

            split = Split(signals, "{1}{1}{1}a{1}{0}b");
            CollectionAssert.AreEqual(new[] { signals.Quote + "a", "b" }, split);

            split = Split(signals, "{1}{1}{1}{1}a{0}b");
            CollectionAssert.AreEqual(new[] { signals.Quote + signals.Quote + "a", "b" }, split);

            split = Split(signals, "{1}{1}{1}{1}{1}a{1}{0}b");
            CollectionAssert.AreEqual(new[] { signals.Quote + signals.Quote + "a", "b" }, split);

            split = Split(signals, "a{0}{1}{1}b");
            CollectionAssert.AreEqual(new[] { "a", signals.Quote + "b" }, split);

            split = Split(signals, "a{0}{1}{1}{1}b{1}");
            CollectionAssert.AreEqual(new[] { "a", signals.Quote + "b" }, split);

            split = Split(signals, "a{0}{1}{1}{1}{1}b");
            CollectionAssert.AreEqual(new[] { "a", signals.Quote + signals.Quote + "b" }, split);

            split = Split(signals, "a{0}{1}{1}{1}{1}{1}b{1}");
            CollectionAssert.AreEqual(new[] { "a", signals.Quote + signals.Quote + "b" }, split);

            split = Split(signals, "a{0}{1}{1}b{0}c");
            CollectionAssert.AreEqual(new[] { "a", signals.Quote + "b", "c" }, split);

            split = Split(signals, "a{0}{1}{1}{1}b{1}{0}c");
            CollectionAssert.AreEqual(new[] { "a", signals.Quote + "b", "c" }, split);

            split = Split(signals, "a{0}{1}{1}{1}{1}b{0}c");
            CollectionAssert.AreEqual(new[] { "a", signals.Quote + signals.Quote + "b", "c" }, split);

            split = Split(signals, "a{0}{1}{1}{1}{1}{1}b{1}{0}c");
            CollectionAssert.AreEqual(new[] { "a", signals.Quote + signals.Quote + "b", "c" }, split);

            split = Split(signals, "{1}{1}a{0}{1}{1}b{0}{1}{1}c");
            CollectionAssert.AreEqual(new[] { signals.Quote + "a", signals.Quote + "b", signals.Quote + "c" }, split);

            split = Split(signals, "{1}{1}{1}a{1}{0}{1}{1}{1}b{1}{0}{1}{1}{1}c{1}");
            CollectionAssert.AreEqual(new[] { signals.Quote + "a", signals.Quote + "b", signals.Quote + "c" }, split);

            split = Split(signals, "{1}{1}{1}a{0}{1}{0}{1}{1}{1}b{0}{1}{0}{1}{1}{1}c{0}{1}");
            CollectionAssert.AreEqual(new[] { signals.Quote + "a" + signals.Delimiter, signals.Quote + "b" + signals.Delimiter, signals.Quote + "c" + signals.Delimiter }, split);

            if (signals.EscapeIsSpecified)
            {
                split = Split(signals, "{3}{1}");
                CollectionAssert.AreEqual(new[] { signals.Quote }, split);

                split = Split(signals, "{1}{3}{1}{1}");
                CollectionAssert.AreEqual(new[] { signals.Quote }, split);

                split = Split(signals, "{3}{1}a");
                CollectionAssert.AreEqual(new[] { signals.Quote + "a" }, split);

                split = Split(signals, "{1}{3}{1}a{1}");
                CollectionAssert.AreEqual(new[] { signals.Quote + "a" }, split);

                split = Split(signals, "{3}{1}{3}{1}a");
                CollectionAssert.AreEqual(new[] { signals.Quote + signals.Quote + "a" }, split);

                split = Split(signals, "{1}{3}{1}{3}{1}a{1}");
                CollectionAssert.AreEqual(new[] { signals.Quote + signals.Quote + "a" }, split);

                split = Split(signals, "{1}{3}{1}{3}{1}a{0}{2}{1}{0}b");
                CollectionAssert.AreEqual(new[] { signals.Quote + signals.Quote + "a" + signals.Delimiter + signals.NewRow, "b" }, split);
            }
        }

        [TestMethod]
        public void QuotedEscaped()
        {
            QuotedEscapedBase(new StringQuotedSignals(",", "'", Environment.NewLine, "\\"));
            QuotedEscapedBase(new StringQuotedSignals(",", string.Empty, Environment.NewLine, "\\"));
            QuotedEscapedBase(new StringQuotedSignals("DELIMITER", "QUOTE", "NEWLINE", "ESCAPE"));
        }

        private static void QuotedEscapedBase(StringQuotedSignals signals)
        {
            if (!signals.EscapeIsSpecified)
                throw new NotSupportedException();

            string[] split = Split(signals, "{3}");
            CollectionAssert.AreEqual(new[] { string.Empty }, split);

            split = Split(signals, "{3}{0}a");
            CollectionAssert.AreEqual(new[] { signals.Delimiter + "a" }, split);

            split = Split(signals, "{3}{0}a{0}b");
            CollectionAssert.AreEqual(new[] { signals.Delimiter + "a", "b" }, split);

            split = Split(signals, "{3}{0}a{3}{0}b");
            CollectionAssert.AreEqual(new[] { signals.Delimiter + "a" + signals.Delimiter + "b" }, split);

            split = Split(signals, "{3}a{0}{3}b{0}{3}c");
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, split);

            split = Split(signals, "a{0}b{0}c{3}{0}");
            CollectionAssert.AreEqual(new[] { "a", "b", "c" + signals.Delimiter }, split);

            split = Split(signals, "a{0}b{0}c{3}{0}{0}d");
            CollectionAssert.AreEqual(new[] { "a", "b", "c" + signals.Delimiter, "d" }, split);

            if (signals.QuoteIsSpecified)
            {
                split = Split(signals, "a{0}b{0}c{3}{1}");
                CollectionAssert.AreEqual(new[] { "a", "b", "c" + signals.Quote }, split);

                split = Split(signals, "a{0}b{0}c{3}{1}{0}d");
                CollectionAssert.AreEqual(new[] { "a", "b", "c" + signals.Quote, "d" }, split);
            }

            if (signals.NewRowIsSpecified)
            {
                split = Split(signals, "a{0}b{0}c{3}{2}");
                CollectionAssert.AreEqual(new[] { "a", "b", "c" + signals.NewRow }, split);

                split = Split(signals, "a{0}b{0}c{3}{2}{0}d");
                CollectionAssert.AreEqual(new[] { "a", "b", "c" + signals.NewRow, "d" }, split);
            }

            split = Split(signals, "a{0}b{0}c{3}{3}");
            CollectionAssert.AreEqual(new[] { "a", "b", "c" + signals.Escape }, split);

            split = Split(signals, "a{0}b{0}c{3}{3}{0}d");
            CollectionAssert.AreEqual(new[] { "a", "b", "c" + signals.Escape, "d" }, split);
        }

        [TestMethod]
        public void ComplicatedDelimiter()
        {
            StringQuotedSignals signals = new StringQuotedSignals("ab", null, null, null);
            string[] split = "1aab2aab3a".SplitQuotedRow(signals);
            CollectionAssert.AreEqual(new[] { "1a", "2a", "3a" }, split);

            signals = new StringQuotedSignals("ababb", null, null, null);
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