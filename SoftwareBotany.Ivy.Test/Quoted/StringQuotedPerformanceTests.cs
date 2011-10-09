using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringQuotedPerformanceTests
    {
        [TestMethod]
        public void SplitQuoted10MBShortRows() { SplitQuotedPerformanceBase(StringQuotedSignals.Csv, 10, 50000, 10); }

        [TestMethod]
        public void SplitQuoted10MBMediumRows() { SplitQuotedPerformanceBase(StringQuotedSignals.Csv, 50, 2000, 50); }

        [TestMethod]
        public void SplitQuoted10MBLongRows() { SplitQuotedPerformanceBase(StringQuotedSignals.Csv, 100, 500, 100); }

        [TestMethod]
        public void SplitQuoted100MBShortRows() { SplitQuotedPerformanceBase(StringQuotedSignals.Csv, 10, 500000, 10); }

        [TestMethod]
        public void SplitQuoted100MBMediumRows() { SplitQuotedPerformanceBase(StringQuotedSignals.Csv, 50, 20000, 50); }

        [TestMethod]
        public void SplitQuoted100MBLongRows() { SplitQuotedPerformanceBase(StringQuotedSignals.Csv, 100, 5000, 100); }

        private static void SplitQuotedPerformanceBase(StringQuotedSignals signals, int columnCount, int rowCount, int columnLength)
        {
            if (!signals.QuoteIsSpecified)
                throw new NotSupportedException();

            foreach (string[] row in SplitQuotedPerformanceChars(signals, columnCount, rowCount, columnLength).SplitQuotedRows(signals))
                Assert.AreEqual(columnCount, row.Length);
        }

        private static IEnumerable<char> SplitQuotedPerformanceChars(StringQuotedSignals signals, int columnCount, int rowCount, int columnLength)
        {
            string column = signals.Quote + signals.Delimiter + Enumerable.Repeat('c', columnLength).ToNewString() + signals.Delimiter + signals.Quote;
            string row = string.Join(signals.Delimiter, Enumerable.Repeat(column, columnCount)) + signals.NewRow;

            return Enumerable.Repeat(row, rowCount)
                .SelectMany(r => r);
        }

        [TestMethod]
        public void JoinQuoted10MBShortRows() { JoinQuotedPerformanceBase(StringQuotedSignals.Csv, 10, 50000, 10); }

        [TestMethod]
        public void JoinQuoted10MBMediumRows() { JoinQuotedPerformanceBase(StringQuotedSignals.Csv, 50, 2000, 50); }

        [TestMethod]
        public void JoinQuoted10MBLongRows() { JoinQuotedPerformanceBase(StringQuotedSignals.Csv, 100, 500, 100); }

        [TestMethod]
        public void JoinQuoted100MBShortRows() { JoinQuotedPerformanceBase(StringQuotedSignals.Csv, 10, 500000, 10); }

        [TestMethod]
        public void JoinQuoted100MBMediumRows() { JoinQuotedPerformanceBase(StringQuotedSignals.Csv, 50, 20000, 50); }

        [TestMethod]
        public void JoinQuoted100MBLongRows() { JoinQuotedPerformanceBase(StringQuotedSignals.Csv, 100, 5000, 100); }

        private static void JoinQuotedPerformanceBase(StringQuotedSignals signals, int columnCount, int rowCount, int columnLength)
        {
            if (!signals.QuoteIsSpecified)
                throw new NotSupportedException();

            string[] columns = JoinQuotedPerformanceColumns(signals, columnCount, columnLength);
            int joinedRowExpectedLength = (columnCount * (columns[0].Length + (signals.Quote.Length * 4) + signals.Delimiter.Length)) - signals.Delimiter.Length;

            for (int i = 0; i < rowCount; i++)
                Assert.AreEqual(joinedRowExpectedLength, columns.JoinQuotedRow(signals, true).Length);
        }

        private static string[] JoinQuotedPerformanceColumns(StringQuotedSignals signals, int columnCount, int columnLength)
        {
            string column = signals.Quote + signals.Delimiter + Enumerable.Repeat('c', columnLength).ToNewString() + signals.Delimiter + signals.Quote;
            return Enumerable.Repeat(column, columnCount).ToArray();
        }
    }
}