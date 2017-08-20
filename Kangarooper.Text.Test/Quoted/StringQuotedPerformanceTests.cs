namespace Kangarooper.Text
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class StringQuotedPerformanceTests
    {
        [TestMethod]
        [TestCategory(TestCategories.Performance)]
        public void SplitQuoted10MBShortRows() { SplitQuotedPerformanceBase(StringQuotedSignals.Csv, 10, 50000, 10); }

        [TestMethod]
        [TestCategory(TestCategories.Performance)]
        public void SplitQuoted10MBMediumRows() { SplitQuotedPerformanceBase(StringQuotedSignals.Csv, 50, 2000, 50); }

        [TestMethod]
        [TestCategory(TestCategories.Performance)]
        public void SplitQuoted10MBLongRows() { SplitQuotedPerformanceBase(StringQuotedSignals.Csv, 100, 500, 100); }

        [TestMethod]
        [TestCategory(TestCategories.Performance)]
        public void SplitQuoted100MBShortRows() { SplitQuotedPerformanceBase(StringQuotedSignals.Csv, 10, 500000, 10); }

        [TestMethod]
        [TestCategory(TestCategories.Performance)]
        public void SplitQuoted100MBMediumRows() { SplitQuotedPerformanceBase(StringQuotedSignals.Csv, 50, 20000, 50); }

        [TestMethod]
        [TestCategory(TestCategories.Performance)]
        public void SplitQuoted100MBLongRows() { SplitQuotedPerformanceBase(StringQuotedSignals.Csv, 100, 5000, 100); }

        private static void SplitQuotedPerformanceBase(StringQuotedSignals signals, int columnCount, int rowCount, int columnWidth)
        {
            if (!signals.QuoteIsSpecified)
                throw new NotSupportedException();

            foreach (string[] fields in SplitQuotedPerformanceChars(signals, columnCount, rowCount, columnWidth).SplitQuotedRows(signals))
                Assert.AreEqual(columnCount, fields.Length);
        }

        private static IEnumerable<char> SplitQuotedPerformanceChars(StringQuotedSignals signals, int columnCount, int rowCount, int columnWidth)
        {
            string field = signals.Quote + signals.Delimiter + Enumerable.Repeat('c', columnWidth).ToNewString() + signals.Delimiter + signals.Quote;
            string row = string.Join(signals.Delimiter, Enumerable.Repeat(field, columnCount)) + signals.NewRow;

            return Enumerable.Repeat(row, rowCount)
                .SelectMany(r => r);
        }

        [TestMethod]
        [TestCategory(TestCategories.Performance)]
        public void JoinQuoted10MBShortRows() { JoinQuotedPerformanceBase(StringQuotedSignals.Csv, 10, 50000, 10); }

        [TestMethod]
        [TestCategory(TestCategories.Performance)]
        public void JoinQuoted10MBMediumRows() { JoinQuotedPerformanceBase(StringQuotedSignals.Csv, 50, 2000, 50); }

        [TestMethod]
        [TestCategory(TestCategories.Performance)]
        public void JoinQuoted10MBLongRows() { JoinQuotedPerformanceBase(StringQuotedSignals.Csv, 100, 500, 100); }

        [TestMethod]
        [TestCategory(TestCategories.Performance)]
        public void JoinQuoted100MBShortRows() { JoinQuotedPerformanceBase(StringQuotedSignals.Csv, 10, 500000, 10); }

        [TestMethod]
        [TestCategory(TestCategories.Performance)]
        public void JoinQuoted100MBMediumRows() { JoinQuotedPerformanceBase(StringQuotedSignals.Csv, 50, 20000, 50); }

        [TestMethod]
        [TestCategory(TestCategories.Performance)]
        public void JoinQuoted100MBLongRows() { JoinQuotedPerformanceBase(StringQuotedSignals.Csv, 100, 5000, 100); }

        private static void JoinQuotedPerformanceBase(StringQuotedSignals signals, int columnCount, int rowCount, int columnWidth)
        {
            if (!signals.QuoteIsSpecified)
                throw new NotSupportedException();

            string[] fields = JoinQuotedPerformanceFields(signals, columnCount, columnWidth);
            int joinedRowExpectedLength = (columnCount * (fields[0].Length + (signals.Quote.Length * 4) + signals.Delimiter.Length)) - signals.Delimiter.Length;

            for (int i = 0; i < rowCount; i++)
                Assert.AreEqual(joinedRowExpectedLength, fields.JoinQuotedRow(signals, true).Length);
        }

        private static string[] JoinQuotedPerformanceFields(StringQuotedSignals signals, int columnCount, int columnWidth)
        {
            string field = signals.Quote + signals.Delimiter + Enumerable.Repeat('c', columnWidth).ToNewString() + signals.Delimiter + signals.Quote;
            return Enumerable.Repeat(field, columnCount).ToArray();
        }
    }
}