namespace Kangarooper.Text
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class StringSchemaJoinExtensionsTests
    {
        [TestMethod]
        public void Basic()
        {
            var a = new StringSchemaEntry("A", new[] { 1, 1, 1 });
            var b = new StringSchemaEntry("B", new[] { 2, 2, 2 });
            var c = new StringSchemaEntry("C", new[] { 2, 2, 2 }, '-');

            string join = new[] { "1", "2", "3" }.JoinSchemaRow(a);
            Assert.AreEqual("A123", join);

            join = new[] { "12", "34", "56" }.JoinSchemaRow(b);
            Assert.AreEqual("B123456", join);

            join = new[] { "1", "2", "3" }.JoinSchemaRow(b);
            Assert.AreEqual("B1 2 3 ", join);

            join = new[] { "1", "2", "3" }.JoinSchemaRow(c);
            Assert.AreEqual("C1-2-3-", join);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThisNull()
        {
            string[] split = null;
            StringSchemaEntry entry = new StringSchemaEntry("A", new[] { 1, 1, 1 });

            split.JoinSchemaRow(entry);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EntryNull()
        {
            string[] split = new[] { "1" };
            split.JoinSchemaRow(null);
        }

        #endregion
    }
}