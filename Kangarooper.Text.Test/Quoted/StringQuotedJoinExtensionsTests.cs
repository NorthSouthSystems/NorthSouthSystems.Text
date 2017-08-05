using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kangarooper.Text
{
    [TestClass]
    public class StringQuotedJoinExtensionsTests
    {
        private static void AssertAreEqual(StringQuotedSignals signals, string format, string result)
        {
            Assert.AreEqual(string.Format(format, signals.Delimiter, signals.Quote, signals.NewRow, signals.Escape), result);
        }

        [TestMethod]
        public void Quoting()
        {
            QuotingBase(StringQuotedSignals.Csv);
            QuotingBase(StringQuotedSignals.Pipe);
            QuotingBase(StringQuotedSignals.Tab);
            QuotingBase(new StringQuotedSignals(",", "'", Environment.NewLine, "\\"));
            QuotingBase(new StringQuotedSignals("DELIMITER", "QUOTE", "NEWLINE", "ESCAPE"));
        }

        private static void QuotingBase(StringQuotedSignals signals)
        {
            if (!signals.QuoteIsSpecified)
                throw new NotSupportedException();

            string[] fields = new string[] { "a", "b", "c" };
            string result = fields.JoinQuotedRow(signals);
            AssertAreEqual(signals, "a{0}b{0}c", result);

            fields = new string[] { "aa", "bb", "cc" };
            result = fields.JoinQuotedRow(signals);
            AssertAreEqual(signals, "aa{0}bb{0}cc", result);

            fields = new string[] { "a" + signals.Delimiter, "b", "c" };
            result = fields.JoinQuotedRow(signals);
            AssertAreEqual(signals, "{1}a{0}{1}{0}b{0}c", result);

            fields = new string[] { "a" + signals.Quote, "b", "c" };
            result = fields.JoinQuotedRow(signals);

            if (signals.EscapeIsSpecified)
                AssertAreEqual(signals, "{1}a{3}{1}{1}{0}b{0}c", result);
            else               
                AssertAreEqual(signals, "{1}a{1}{1}{1}{0}b{0}c", result);

            fields = new string[] { "aa", "bb", "cc" };
            result = fields.JoinQuotedRow(signals, true);
            AssertAreEqual(signals, "{1}aa{1}{0}{1}bb{1}{0}{1}cc{1}", result);

            if (signals.NewRowIsSpecified)
            {
                fields = new string[] { "a" + signals.NewRow + "a", "b", "c" };
                result = fields.JoinQuotedRow(signals);
                AssertAreEqual(signals, "{1}a{2}a{1}{0}b{0}c", result);
            }
        }

        [TestMethod]
        public void Escaping()
        {
            EscapingBase(new StringQuotedSignals(",", null, Environment.NewLine, "\\"));
            EscapingBase(new StringQuotedSignals("|", null, Environment.NewLine, "\\"));
            EscapingBase(new StringQuotedSignals("\t", null, Environment.NewLine, "\\"));
            EscapingBase(new StringQuotedSignals("DELIMITER", null, "NEWLINE", "ESCAPE"));
        }

        private static void EscapingBase(StringQuotedSignals signals)
        {
            if (!signals.EscapeIsSpecified || signals.QuoteIsSpecified)
                throw new NotSupportedException();

            string[] fields = null;
            string result = null;

            fields = new string[] { "a" + signals.Delimiter, "b", "c" };
            result = fields.JoinQuotedRow(signals);
            AssertAreEqual(signals, "a{3}{0}{0}b{0}c", result);

            if (signals.NewRowIsSpecified)
            {
                fields = new string[] { "a" + signals.NewRow, "b", "c" };
                result = fields.JoinQuotedRow(signals);
                AssertAreEqual(signals, "a{3}{2}{0}b{0}c", result);
            }

            fields = new string[] { "a" + signals.Escape, "b", "c" };
            result = fields.JoinQuotedRow(signals);
            AssertAreEqual(signals, "a{3}{3}{0}b{0}c", result);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FieldsNull()
        {
            string[] fields = null;
            fields.JoinQuotedRow(StringQuotedSignals.Csv);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SignalsNull()
        {
            string[] fields = new[] { "A" };
            fields.JoinQuotedRow(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void QuoteNotSpecified1()
        {
            string[] fields = new[] { "A" };
            fields.JoinQuotedRow(new StringQuotedSignals(",", null, null, null), true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void QuoteNotSpecified2()
        {
            string[] fields = new[] { "A," };
            fields.JoinQuotedRow(new StringQuotedSignals(",", null, null, null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void QuoteNotSpecified3()
        {
            string[] fields = new[] { "A" + Environment.NewLine };
            fields.JoinQuotedRow(new StringQuotedSignals(",", null, Environment.NewLine, null));
        }

        #endregion
    }
}