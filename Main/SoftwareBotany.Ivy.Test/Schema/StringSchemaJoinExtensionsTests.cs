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
            var a = new StringSchemaEntry("A", new []{1, 1, 1});
            var b = new StringSchemaEntry("B", new []{2, 2, 2});

            var split = new StringSchemaEntryAndStrings(a, new[] { "1", "2", "3" });
            string join = split.JoinSchema(' ');
            Assert.AreEqual("A123", join);

            split = new StringSchemaEntryAndStrings(b, new[] { "12", "34", "56" });
            join = split.JoinSchema(' ');
            Assert.AreEqual("B123456", join);

            split = new StringSchemaEntryAndStrings(b, new[] { "1", "2", "3" });
            join = split.JoinSchema(' ');
            Assert.AreEqual("B1 2 3 ", join);

            split = new StringSchemaEntryAndStrings(b, new[] { "1", "2", "3" });
            join = split.JoinSchema('-');
            Assert.AreEqual("B1-2-3-", join);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThisNull()
        {
            StringSchemaEntryAndStrings split = null;
            split.JoinSchema(' ');
        }

        #endregion
    }
}