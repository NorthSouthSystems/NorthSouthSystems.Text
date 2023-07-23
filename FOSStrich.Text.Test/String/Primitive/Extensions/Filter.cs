namespace FreeAndWithBeer.Text
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    public static partial class StringExtensionsTests
    {
        [TestClass]
        public class Filter
        {
            [TestMethod]
            public void Basic()
            {
                Assert.AreEqual("a1b2c3d", "a1b2c3d".Filter(CharFilters.None));
                Assert.AreEqual("123", "a1b2c3d".Filter(CharFilters.RemoveLetters));
                Assert.AreEqual(string.Empty, "a1b2c3d".Filter(CharFilters.RemoveLetters | CharFilters.RemoveDigits));
                Assert.AreEqual("a1b2c3d", "a1b2-c3d".Filter(CharFilters.RemovePunctuation));
                Assert.AreEqual("-", "a1b2-c3d".Filter(CharFilters.RemoveLetters | CharFilters.RemoveDigits));
            }

            #region Exceptions

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThisNull()
            {
                string s = null;
                s.Filter(CharFilters.None);
            }

            #endregion
        }
    }
}