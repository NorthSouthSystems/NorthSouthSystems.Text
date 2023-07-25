namespace FOSStrich.Text;

public static partial class CharExtensionsTests
{
    [TestClass]
    public class Categories
    {
        [TestMethod]
        public void IsInAnyCategoryNone()
        {
            Assert.IsFalse('a'.IsInAnyCategory(CharCategories.None));
            Assert.IsFalse('1'.IsInAnyCategory(CharCategories.None));
            Assert.IsFalse('.'.IsInAnyCategory(CharCategories.None));
            Assert.IsFalse(' '.IsInAnyCategory(CharCategories.None));
            Assert.IsFalse('\t'.IsInAnyCategory(CharCategories.None));
            Assert.IsFalse('*'.IsInAnyCategory(CharCategories.None));
            Assert.IsFalse('@'.IsInAnyCategory(CharCategories.None));
        }

        [TestMethod]
        public void IsInAnyCategoryAll()
        {
            Assert.IsTrue('a'.IsInAnyCategory(CharCategories.All));
            Assert.IsTrue('1'.IsInAnyCategory(CharCategories.All));
            Assert.IsTrue('.'.IsInAnyCategory(CharCategories.All));
            Assert.IsTrue(' '.IsInAnyCategory(CharCategories.All));
            Assert.IsTrue('\t'.IsInAnyCategory(CharCategories.All));
            Assert.IsTrue('*'.IsInAnyCategory(CharCategories.All));
            Assert.IsTrue('@'.IsInAnyCategory(CharCategories.All));
        }

        [TestMethod]
        public void IsInAnyCategoryVarious()
        {
            AssertChar('a', CharCategories.Letter, CharCategories.Lower);
            AssertChar('A', CharCategories.Letter, CharCategories.Upper);

            AssertChar('1', CharCategories.Digit | CharCategories.Number);

            AssertChar('.', CharCategories.Punctuation);
            AssertChar('!', CharCategories.Punctuation);

            AssertChar(' ', CharCategories.Separator | CharCategories.WhiteSpace);
            AssertChar('\t', CharCategories.Control | CharCategories.Separator | CharCategories.WhiteSpace);

            AssertChar('+', CharCategories.Symbol);
            AssertChar('<', CharCategories.Symbol);

            static void AssertChar(char c, params CharCategories[] categories)
            {
                foreach (var category in categories)
                    Assert.IsTrue(c.IsInAnyCategory(category));

                Assert.IsTrue(
                    c.IsInAnyCategory(
                        categories.Aggregate(CharCategories.None, (accumulate, category) => accumulate | category)));

                Assert.IsFalse(
                    c.IsInAnyCategory(
                        categories.Aggregate(CharCategories.All, (accumulate, category) => accumulate ^ category)));
            }
        }
    }
}