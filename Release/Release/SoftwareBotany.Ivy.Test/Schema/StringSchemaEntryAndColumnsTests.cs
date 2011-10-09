using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringSchemaEntryAndColumnsTests
    {
        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionEntryNull()
        {
            StringSchemaEntryAndColumns split = new StringSchemaEntryAndColumns(null, new[] { "Dan", "Terry" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionColumnsNull()
        {
            StringSchemaEntry entry = new StringSchemaEntry("A", new[] { 10, 10 });
            StringSchemaEntryAndColumns split = new StringSchemaEntryAndColumns(entry, null);
        }

        #endregion
    }
}