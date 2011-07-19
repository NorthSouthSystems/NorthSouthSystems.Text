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
        public void ConstructionArgument()
        {
            StringSchemaEntry entry = new StringSchemaEntry(string.Empty, 1);
        }

        #endregion
    }
}