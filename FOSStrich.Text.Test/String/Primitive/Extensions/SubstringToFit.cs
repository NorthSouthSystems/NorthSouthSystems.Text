namespace FOSStrich.Text;

public static partial class StringExtensionsTests
{
    [TestClass]
    public class SubstringToFit
    {
        [TestMethod]
        public void Basic()
        {
            string result;

            result = "a".SubstringToFit(0);
            Assert.AreEqual(result, string.Empty);

            result = "a".SubstringToFit(1);
            Assert.AreEqual(result, "a");

            result = "a".SubstringToFit(2);
            Assert.AreEqual(result, "a");

            result = "ab".SubstringToFit(1);
            Assert.AreEqual(result, "a");

            result = "ab".SubstringToFit(2);
            Assert.AreEqual(result, "ab");

            result = "ab".SubstringToFit(3);
            Assert.AreEqual(result, "ab");

            result = "abc".SubstringToFit(2);
            Assert.AreEqual(result, "ab");

            result = "abc".SubstringToFit(3);
            Assert.AreEqual(result, "abc");

            result = "abc".SubstringToFit(4);
            Assert.AreEqual(result, "abc");
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThisNull()
        {
            string s = null;
            s.SubstringToFit(1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MaxLengthLessThanZero()
        {
            "a".SubstringToFit(-1);
        }

        #endregion
    }
}