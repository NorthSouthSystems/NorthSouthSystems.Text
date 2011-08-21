using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringQuotedJoinExtensionsTests
    {
        [TestMethod]
        public void Simple()
        {
            string[] columns = new string[] { "a", "b", "c" };
            string result = columns.JoinQuotedRow(StringQuotedSignals.Csv);
            Assert.AreEqual("a,b,c", result);

            columns = new string[] { "aa", "bb", "cc" };
            result = columns.JoinQuotedRow(StringQuotedSignals.Csv);
            Assert.AreEqual("aa,bb,cc", result);

            columns = new string[] { "aa", "bb", "cc" };
            result = columns.JoinQuotedRow(StringQuotedSignals.Csv, true);
            Assert.AreEqual(string.Format("{0}aa{0},{0}bb{0},{0}cc{0}", StringQuotedSignals.Csv.Quote), result);

            columns = new string[] { "a" + StringQuotedSignals.Csv.Delimiter, "b", "c" };
            result = columns.JoinQuotedRow(StringQuotedSignals.Csv);
            Assert.AreEqual(string.Format("{0}a{1}{0},b,c", StringQuotedSignals.Csv.Quote, StringQuotedSignals.Csv.Delimiter), result);

            columns = new string[] { "a" + StringQuotedSignals.Csv.NewRow + "a", "b", "c" };
            result = columns.JoinQuotedRow(StringQuotedSignals.Csv);
            Assert.AreEqual(string.Format("{0}a{1}a{0},b,c", StringQuotedSignals.Csv.Quote, StringQuotedSignals.Csv.NewRow), result);

            columns = new string[] { "a" + StringQuotedSignals.Csv.Quote, "b", "c" };
            result = columns.JoinQuotedRow(StringQuotedSignals.Csv);
            Assert.AreEqual(string.Format("a{0}{0},b,c", StringQuotedSignals.Csv.Quote), result);

            columns = new string[] { "a", "b", "c" };
            result = columns.JoinQuotedRow(new StringQuotedSignals("FOOBAR", null, null));
            Assert.AreEqual("aFOOBARbFOOBARc", result);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ColumnsNull()
        {
            string[] columns = null;
            columns.JoinQuotedRow(StringQuotedSignals.Csv);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SignalsNull()
        {
            string[] columns = new[] { "A" };
            columns.JoinQuotedRow(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void QuoteNotSpecified1()
        {
            string[] columns = new[] { "A" };
            columns.JoinQuotedRow(new StringQuotedSignals(",", null, null), true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void QuoteNotSpecified2()
        {
            string[] columns = new[] { "A," };
            columns.JoinQuotedRow(new StringQuotedSignals(",", null, null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void QuoteNotSpecified3()
        {
            string[] columns = new[] { "A" + Environment.NewLine };
            columns.JoinQuotedRow(new StringQuotedSignals(",", null, Environment.NewLine));
        }

        #endregion
    }
}