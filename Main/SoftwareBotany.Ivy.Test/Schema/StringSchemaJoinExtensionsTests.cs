using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringSchemaJoinExtensionsTests
    {
        [TestMethod]
        public void Basic()
        {
            var a = new StringSchemaEntry("A", 1, 1, 1);
            var b = new StringSchemaEntry("B", 2, 2, 2);

            var split = new StringSchemaEntryAndStrings(a, new[] { "1", "2", "3" });
            string join = split.JoinSchemaLine();
            Assert.AreEqual("A123", join);

            split = new StringSchemaEntryAndStrings(b, new[] { "12", "34", "56" });
            join = split.JoinSchemaLine();
            Assert.AreEqual("B123456", join);

            split = new StringSchemaEntryAndStrings(b, new[] { "1", "2", "3" });
            join = split.JoinSchemaLine();
            Assert.AreEqual("B1 2 3 ", join);

            var c = new StringSchemaEntry("C", '-', 2, 2, 2);

            split = new StringSchemaEntryAndStrings(c, new[] { "1", "2", "3" });
            join = split.JoinSchemaLine();
            Assert.AreEqual("C1-2-3-", join);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThisNull()
        {
            StringSchemaEntryAndStrings split = null;
            split.JoinSchemaLine();
        }

        #endregion
    }
}