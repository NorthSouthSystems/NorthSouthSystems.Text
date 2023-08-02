namespace FOSStrich.Text;

public static partial class StringExtensionsTests
{
    [TestClass]
    public class Left
    {
        [TestMethod]
        public void Basic()
        {
            Helper(string.Empty, 0);

            Helper("a", 0);
            Helper("a", 1);
            Helper("a", 2);

            Helper("ab", 0);
            Helper("ab", 1);
            Helper("ab", 2);
            Helper("ab", 3);

            Helper("abc", 0);
            Helper("abc", 1);
            Helper("abc", 2);
            Helper("abc", 3);
            Helper("abc", 4);

            void Helper(string value, int length) =>
                Assert.AreEqual(StringExtensions.Left(value, length), Microsoft.VisualBasic.Strings.Left(value, length));
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThisNull() => ((string)null).Left(1);

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MaxLengthLessThanZero() => "a".Left(-1);

        #endregion
    }

    [TestClass]
    public class Right
    {
        [TestMethod]
        public void Basic()
        {
            Helper(string.Empty, 0);

            Helper("a", 0);
            Helper("a", 1);
            Helper("a", 2);

            Helper("ab", 0);
            Helper("ab", 1);
            Helper("ab", 2);
            Helper("ab", 3);

            Helper("abc", 0);
            Helper("abc", 1);
            Helper("abc", 2);
            Helper("abc", 3);
            Helper("abc", 4);

            void Helper(string value, int length) =>
                Assert.AreEqual(StringExtensions.Right(value, length), Microsoft.VisualBasic.Strings.Right(value, length));
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThisNull() => ((string)null).Right(1);

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MaxLengthLessThanZero() => "a".Right(-1);

        #endregion
    }
}