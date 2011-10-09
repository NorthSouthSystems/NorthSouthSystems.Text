using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwareBotany.Ivy
{
    [TestClass]
    public class StringSchemaEntryTests
    {
        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructionArgument1()
        {
            StringSchemaEntry entry = new StringSchemaEntry(null, new[] { 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructionArgument2()
        {
            StringSchemaEntry entry = new StringSchemaEntry(string.Empty, new[] { 1 });
        }

        #endregion
    }
}