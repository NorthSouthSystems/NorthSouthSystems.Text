using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringQuotedSignalsTests
    {
        [TestMethod]
        public void ConstructionAndGetters()
        {
            var signals = new StringQuotedSignals(",", null, Environment.NewLine);
            Assert.IsTrue(signals.DelimiterIsSpecified);
            Assert.AreEqual(",", signals.Delimiter);

            Assert.IsFalse(signals.QuoteIsSpecified);
            Assert.AreEqual(string.Empty, signals.Quote);

            Assert.IsTrue(signals.NewLineIsSpecified);
            Assert.AreEqual(Environment.NewLine, signals.NewLine);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionNoDelimiterArgument()
        {
            var signals = new StringQuotedSignals(string.Empty, "\"", Environment.NewLine);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionOverlapArgument1()
        {
            var signals = new StringQuotedSignals("A", "AB", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionOverlapArgument2()
        {
            var signals = new StringQuotedSignals("AB", "A", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructionOverlapArgument3()
        {
            var signals = new StringQuotedSignals(null, "A", "AB");
        }

        #endregion
    }
}