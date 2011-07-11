using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringSchemaEntryAndStringsTests
    {
        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionEntryNull()
        {
            StringSchemaEntryAndStrings split = new StringSchemaEntryAndStrings(null, new[] { "Dan", "Terry" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionStringsNull()
        {
            StringSchemaEntry entry = new StringSchemaEntry("A", new[] { 10, 10 });
            StringSchemaEntryAndStrings split = new StringSchemaEntryAndStrings(entry, null);
        }

        #endregion
    }
}