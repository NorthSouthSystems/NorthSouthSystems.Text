namespace FOSStrich.Text
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class StringRowWrapperFactoryTests
    {
        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructionColumnNamesNull()
        {
            var factory = new StringRowWrapperFactory(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructionDuplicateColumnNames1()
        {
            var factory = new StringRowWrapperFactory(new[] { "A", "A" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructionDuplicateColumnNames2()
        {
            var factory = new StringRowWrapperFactory(new[] { "A", "B", "B" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructionDuplicateColumnNames3()
        {
            var factory = new StringRowWrapperFactory(new[] { "A", "B", "C", "A" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WrapFieldsNull()
        {
            var factory = new StringRowWrapperFactory(new[] { "A", "B", "C" });
            factory.Wrap(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrapFieldsTooMany()
        {
            var factory = new StringRowWrapperFactory(new[] { "A", "B", "C" });
            factory.Wrap(new[] { "1", "2", "3", "4" });
        }

        #endregion
    }
}