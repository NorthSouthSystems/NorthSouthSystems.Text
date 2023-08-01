namespace FOSStrich.Text;

public static partial class StringExtensionsTests
{
    [TestClass]
    public class Overwrite
    {
        [TestMethod]
        public void Basic()
        {
            string result;

            result = "abc".Overwrite(0, "z");
            Assert.AreEqual("zbc", result);

            result = "abc".Overwrite(1, "z");
            Assert.AreEqual("azc", result);

            result = "abc".Overwrite(1, "zy");
            Assert.AreEqual("azy", result);

            result = "abc".Overwrite(2, "zy");
            Assert.AreEqual("abzy", result);

            result = "abc".Overwrite(3, "zy");
            Assert.AreEqual("abczy", result);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThisNull()
        {
            string s = null;
            s.Overwrite(0, "z");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void StartIndexLessThanZero() => "a".Overwrite(-1, "z");

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void StartIndexGreaterThanValueLength() => "a".Overwrite(2, "z");

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewValueNull() => "a".Overwrite(0, null);

        #endregion
    }
}