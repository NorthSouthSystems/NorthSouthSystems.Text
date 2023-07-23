namespace FOSStrich.Text
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;

    [TestClass]
    public class StringRowWrapperTests
    {
        [TestMethod]
        public void Basic()
        {
            var factory = new StringRowWrapperFactory(new[] { "C0", "C1", "C2" });
            var row = factory.Wrap(new[] { "F0", "F1" });

            Assert.AreEqual("F0", row[0].Value);
            Assert.AreEqual("F1", row[1].Value);
            Assert.AreEqual(null, row[2].Value);

            Assert.AreEqual("F0", row["C0"].Value);
            Assert.AreEqual("F1", row["C1"].Value);
            Assert.AreEqual(null, row["C2"].Value);

            var fields = row.Fields.ToArray();

            Assert.AreEqual(3, fields.Length);
            Assert.AreEqual("F0", fields[0].Value);
            Assert.AreEqual("F1", fields[1].Value);
            Assert.AreEqual(null, fields[2].Value);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexOutOfRange1()
        {
            var factory = new StringRowWrapperFactory(new[] { "C0", "C1", "C2" });
            var row = factory.Wrap(new[] { "F0", "F1" });
            var field = row[-1];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexOutOfRange2()
        {
            var factory = new StringRowWrapperFactory(new[] { "C0", "C1", "C2" });
            var row = factory.Wrap(new[] { "F0", "F1" });
            var field = row[3];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ColumnNotFound()
        {
            var factory = new StringRowWrapperFactory(new[] { "C0", "C1", "C2" });
            var row = factory.Wrap(new[] { "F0", "F1" });
            var field = row["C3"];
        }

        #endregion
    }
}