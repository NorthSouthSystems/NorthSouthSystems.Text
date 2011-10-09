using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringQuotedSignalsTests
    {
        [TestMethod]
        public void Basic()
        {
            var signals = new StringQuotedSignals(",", null, Environment.NewLine, string.Empty);
            Assert.IsTrue(signals.DelimiterIsSpecified);
            Assert.AreEqual(",", signals.Delimiter);

            Assert.IsFalse(signals.QuoteIsSpecified);
            Assert.AreEqual(string.Empty, signals.Quote);

            Assert.IsTrue(signals.NewRowIsSpecified);
            Assert.AreEqual(Environment.NewLine, signals.NewRow);

            Assert.IsFalse(signals.EscapeIsSpecified);
            Assert.AreEqual(string.Empty, signals.Escape);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionNoDelimiter1()
        {
            var signals = new StringQuotedSignals(null, "\"", Environment.NewLine, "\\");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionNoDelimiter2()
        {
            var signals = new StringQuotedSignals(string.Empty, "\"", Environment.NewLine, "\\");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionOverlapArgument1()
        {
            var signals = new StringQuotedSignals("A", "AB", null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionOverlapArgument2()
        {
            var signals = new StringQuotedSignals("A", null, "AB", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionOverlapArgument3()
        {
            var signals = new StringQuotedSignals("A", null, null, "AB");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionOverlapArgument4()
        {
            var signals = new StringQuotedSignals("AB", "A", null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionOverlapArgument5()
        {
            var signals = new StringQuotedSignals("AB", null, "A", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionOverlapArgument6()
        {
            var signals = new StringQuotedSignals("AB", null, null, "A");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionOverlapArgument7()
        {
            var signals = new StringQuotedSignals(",", "A", "AB", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionOverlapArgument8()
        {
            var signals = new StringQuotedSignals(",", "A", null, "AB");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionOverlapArgument9()
        {
            var signals = new StringQuotedSignals(",", "AB", "A", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionOverlapArgument10()
        {
            var signals = new StringQuotedSignals(",", "AB", null, "A");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionOverlapArgument11()
        {
            var signals = new StringQuotedSignals(",", null, "A", "AB");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionOverlapArgument12()
        {
            var signals = new StringQuotedSignals(",", null, "AB", "A");
        }

        #endregion
    }
}