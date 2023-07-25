namespace FOSStrich.Text;

public static partial class StringExtensionsTests
{
    [TestClass]
    public class WhereIsInAnyCategory
    {
        [TestMethod]
        public void Basic()
        {
            Assert.AreEqual("a1b2c3d", "a1b2c3d".WhereIsInAnyCategory(CharCategories.All));
            Assert.AreEqual("a1b2c3d", "a1b2c3d".WhereIsInAnyCategory(CharCategories.Digit | CharCategories.Letter));
            Assert.AreEqual("123", "a1b2c3d".WhereIsInAnyCategory(CharCategories.Digit));
            Assert.AreEqual("abcd", "a1b2c3d".WhereIsInAnyCategory(CharCategories.Letter));
            Assert.AreEqual(string.Empty, "a1b2c3d".WhereIsInAnyCategory(CharCategories.Punctuation | CharCategories.WhiteSpace));
            Assert.AreEqual("a1b2c3d", "a1b2-c3d".WhereIsInAnyCategory(CharCategories.Digit | CharCategories.Letter));
            Assert.AreEqual("-", "a1b2-c3d".WhereIsInAnyCategory(CharCategories.Punctuation));
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThisNull()
        {
            string s = null;
            s.WhereIsInAnyCategory(CharCategories.All);
        }

        #endregion
    }
}