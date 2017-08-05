namespace Kangarooper.Text
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;

    [TestClass]
    public class StringQuotedRowsTests
    {
        [TestMethod]
        public void Basic()
        {
            string[][] splits = string.Format("a,b,c{0}d,e,f{0}g,h,i", StringQuotedSignals.Csv.NewRow)
                .SplitQuotedRows(StringQuotedSignals.Csv)
                .ToArray();

            Assert.AreEqual(3, splits.Length);

            Assert.AreEqual(3, splits[0].Length);
            Assert.AreEqual("a", splits[0][0]);
            Assert.AreEqual("b", splits[0][1]);
            Assert.AreEqual("c", splits[0][2]);

            Assert.AreEqual(3, splits[1].Length);
            Assert.AreEqual("d", splits[1][0]);
            Assert.AreEqual("e", splits[1][1]);
            Assert.AreEqual("f", splits[1][2]);

            Assert.AreEqual(3, splits[2].Length);
            Assert.AreEqual("g", splits[2][0]);
            Assert.AreEqual("h", splits[2][1]);
            Assert.AreEqual("i", splits[2][2]);

            splits = string.Format("a,b,c{0}", StringQuotedSignals.Csv.NewRow)
                .SplitQuotedRows(StringQuotedSignals.Csv)
                .ToArray();

            Assert.AreEqual(1, splits.Length);

            Assert.AreEqual(3, splits[0].Length);
            Assert.AreEqual("a", splits[0][0]);
            Assert.AreEqual("b", splits[0][1]);
            Assert.AreEqual("c", splits[0][2]);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RowsNull()
        {
            string s = null;
            s.SplitQuotedRows(StringQuotedSignals.Csv).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SignalsNull()
        {
            string s = string.Empty;
            s.SplitQuotedRows(null).ToArray();
        }

        #endregion
    }
}