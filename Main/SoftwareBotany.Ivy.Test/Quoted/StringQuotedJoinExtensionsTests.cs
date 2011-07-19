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
            string[] sa;
            string result;

            sa = new string[] { "a", "b", "c" };
            result = sa.JoinQuotedLine(StringQuotedSignals.Csv);
            Assert.AreEqual("a,b,c", result);

            sa = new string[] { "aa", "bb", "cc" };
            result = sa.JoinQuotedLine(StringQuotedSignals.Csv);
            Assert.AreEqual("aa,bb,cc", result);

            sa = new string[] { "aa", "bb", "cc" };
            result = sa.JoinQuotedLine(StringQuotedSignals.Csv, true);
            Assert.AreEqual(string.Format("{0}aa{0},{0}bb{0},{0}cc{0}", StringQuotedSignals.Csv.Quote), result);

            sa = new string[] { "a" + StringQuotedSignals.Csv.Delimiter, "b", "c" };
            result = sa.JoinQuotedLine(StringQuotedSignals.Csv);
            Assert.AreEqual(string.Format("{0}a{1}{0},b,c", StringQuotedSignals.Csv.Quote, StringQuotedSignals.Csv.Delimiter), result);

            sa = new string[] { "a" + StringQuotedSignals.Csv.NewLine + "a", "b", "c" };
            result = sa.JoinQuotedLine(StringQuotedSignals.Csv);
            Assert.AreEqual(string.Format("{0}a{1}a{0},b,c", StringQuotedSignals.Csv.Quote, StringQuotedSignals.Csv.NewLine), result);

            sa = new string[] { "a" + StringQuotedSignals.Csv.Quote, "b", "c" };
            result = sa.JoinQuotedLine(StringQuotedSignals.Csv);
            Assert.AreEqual(string.Format("a{0}{0},b,c", StringQuotedSignals.Csv.Quote), result);

            sa = new string[] { "a", "b", "c" };
            result = sa.JoinQuotedLine(new StringQuotedSignals("FOOBAR", null, null));
            Assert.AreEqual("aFOOBARbFOOBARc", result);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StringsArgumentNull()
        {
            string[] strings = null;
            strings.JoinQuotedLine(StringQuotedSignals.Csv);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SignalsArgumentNull()
        {
            string[] strings = new[] { "A" };
            strings.JoinQuotedLine(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SignalsQuoteArgument()
        {
            string[] strings = new[] { "A" };
            strings.JoinQuotedLine(new StringQuotedSignals(",", null, null), true);
        }

        #endregion
    }
}