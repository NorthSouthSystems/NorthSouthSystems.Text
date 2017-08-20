namespace Kangarooper.Text
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CharExtensionsTest
    {
        [TestMethod]
        public void PassesFiltersNone()
        {
            Assert.IsTrue('a'.PassesFilters(CharFilters.None));
            Assert.IsTrue('1'.PassesFilters(CharFilters.None));
            Assert.IsTrue('.'.PassesFilters(CharFilters.None));
            Assert.IsTrue(' '.PassesFilters(CharFilters.None));
            Assert.IsTrue('\t'.PassesFilters(CharFilters.None));
            Assert.IsTrue('*'.PassesFilters(CharFilters.None));
            Assert.IsTrue('@'.PassesFilters(CharFilters.None));
        }

        [TestMethod]
        public void PassesFiltersSingleFilter()
        {
            Assert.IsFalse('a'.PassesFilters(CharFilters.RemoveLetters));
            Assert.IsFalse('A'.PassesFilters(CharFilters.RemoveLetters));
            Assert.IsTrue('a'.PassesFilters(CharFilters.RemoveDigits));
            Assert.IsTrue('a'.PassesFilters(CharFilters.RemovePunctuation));
            Assert.IsTrue('a'.PassesFilters(CharFilters.RemoveWhiteSpace));
            Assert.IsTrue('a'.PassesFilters(CharFilters.RemoveOther));

            Assert.IsFalse('1'.PassesFilters(CharFilters.RemoveDigits));
            Assert.IsTrue('1'.PassesFilters(CharFilters.RemoveLetters));
            Assert.IsTrue('1'.PassesFilters(CharFilters.RemovePunctuation));
            Assert.IsTrue('1'.PassesFilters(CharFilters.RemoveWhiteSpace));
            Assert.IsTrue('1'.PassesFilters(CharFilters.RemoveOther));

            Assert.IsFalse('.'.PassesFilters(CharFilters.RemovePunctuation));
            Assert.IsFalse('!'.PassesFilters(CharFilters.RemovePunctuation));
            Assert.IsTrue('.'.PassesFilters(CharFilters.RemoveLetters));
            Assert.IsTrue('.'.PassesFilters(CharFilters.RemoveDigits));
            Assert.IsTrue('.'.PassesFilters(CharFilters.RemoveWhiteSpace));
            Assert.IsTrue('.'.PassesFilters(CharFilters.RemoveOther));

            Assert.IsFalse(' '.PassesFilters(CharFilters.RemoveWhiteSpace));
            Assert.IsFalse('\t'.PassesFilters(CharFilters.RemoveWhiteSpace));
            Assert.IsTrue(' '.PassesFilters(CharFilters.RemoveLetters));
            Assert.IsTrue(' '.PassesFilters(CharFilters.RemoveDigits));
            Assert.IsTrue(' '.PassesFilters(CharFilters.RemovePunctuation));
            Assert.IsTrue(' '.PassesFilters(CharFilters.RemoveOther));

            Assert.IsFalse('+'.PassesFilters(CharFilters.RemoveOther));
            Assert.IsFalse('<'.PassesFilters(CharFilters.RemoveOther));
            Assert.IsTrue('+'.PassesFilters(CharFilters.RemoveLetters));
            Assert.IsTrue('+'.PassesFilters(CharFilters.RemoveDigits));
            Assert.IsTrue('+'.PassesFilters(CharFilters.RemovePunctuation));
            Assert.IsTrue('+'.PassesFilters(CharFilters.RemoveWhiteSpace));
        }

        [TestMethod]
        public void PassesFiltersMultiFilter()
        {
            Assert.IsFalse('a'.PassesFilters(CharFilters.RemoveLetters | CharFilters.RemoveDigits));
            Assert.IsFalse('A'.PassesFilters(CharFilters.RemoveLetters | CharFilters.RemoveDigits | CharFilters.RemovePunctuation | CharFilters.RemoveWhiteSpace | CharFilters.RemoveOther));
            Assert.IsTrue('a'.PassesFilters(CharFilters.RemoveDigits | CharFilters.RemovePunctuation));
            Assert.IsTrue('a'.PassesFilters(CharFilters.RemoveDigits | CharFilters.RemovePunctuation | CharFilters.RemoveWhiteSpace));
            Assert.IsTrue('a'.PassesFilters(CharFilters.RemoveDigits | CharFilters.RemovePunctuation | CharFilters.RemoveWhiteSpace | CharFilters.RemoveOther));

            Assert.IsFalse('1'.PassesFilters(CharFilters.RemoveLetters | CharFilters.RemoveDigits));
            Assert.IsTrue('1'.PassesFilters(CharFilters.RemoveLetters | CharFilters.RemovePunctuation));
            Assert.IsTrue('1'.PassesFilters(CharFilters.RemoveLetters | CharFilters.RemovePunctuation | CharFilters.RemoveWhiteSpace));
            Assert.IsTrue('1'.PassesFilters(CharFilters.RemoveLetters | CharFilters.RemovePunctuation | CharFilters.RemoveWhiteSpace | CharFilters.RemoveOther));

            Assert.IsFalse('.'.PassesFilters(CharFilters.RemoveLetters | CharFilters.RemovePunctuation));
            Assert.IsFalse('!'.PassesFilters(CharFilters.RemoveDigits | CharFilters.RemovePunctuation));
            Assert.IsTrue('.'.PassesFilters(CharFilters.RemoveLetters | CharFilters.RemoveDigits));
            Assert.IsTrue('.'.PassesFilters(CharFilters.RemoveLetters | CharFilters.RemoveDigits | CharFilters.RemoveWhiteSpace));
            Assert.IsTrue('.'.PassesFilters(CharFilters.RemoveLetters | CharFilters.RemoveDigits | CharFilters.RemoveWhiteSpace | CharFilters.RemoveOther));

            Assert.IsFalse(' '.PassesFilters(CharFilters.RemoveLetters | CharFilters.RemoveWhiteSpace));
            Assert.IsFalse('\t'.PassesFilters(CharFilters.RemovePunctuation | CharFilters.RemoveWhiteSpace));
            Assert.IsTrue(' '.PassesFilters(CharFilters.RemoveLetters | CharFilters.RemoveDigits));
            Assert.IsTrue(' '.PassesFilters(CharFilters.RemoveLetters | CharFilters.RemoveDigits | CharFilters.RemovePunctuation));
            Assert.IsTrue(' '.PassesFilters(CharFilters.RemoveLetters | CharFilters.RemoveDigits | CharFilters.RemovePunctuation | CharFilters.RemoveOther));

            Assert.IsFalse('+'.PassesFilters(CharFilters.RemovePunctuation | CharFilters.RemoveOther));
            Assert.IsFalse('<'.PassesFilters(CharFilters.RemoveWhiteSpace | CharFilters.RemoveOther));
            Assert.IsTrue('+'.PassesFilters(CharFilters.RemoveLetters | CharFilters.RemoveDigits));
            Assert.IsTrue('+'.PassesFilters(CharFilters.RemoveLetters | CharFilters.RemoveDigits | CharFilters.RemovePunctuation));
            Assert.IsTrue('+'.PassesFilters(CharFilters.RemoveLetters | CharFilters.RemoveDigits | CharFilters.RemovePunctuation | CharFilters.RemoveWhiteSpace));
        }
    }
}