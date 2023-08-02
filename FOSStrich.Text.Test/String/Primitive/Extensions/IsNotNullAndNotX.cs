namespace FOSStrich.Text;

public static partial class StringExtensionsTests
{
    [TestClass]
    public class IsNotNullAndNotX
    {
        [TestMethod]
        public void IsNotNullAndNotEmpty()
        {
            Assert.IsFalse(StringExtensions.IsNotNullAndNotEmpty(null));
            Assert.IsFalse(StringExtensions.IsNotNullAndNotEmpty(string.Empty));
            Assert.IsTrue(StringExtensions.IsNotNullAndNotEmpty("a"));
        }

        [TestMethod]
        public void IsNotNullAndNotWhiteSpace()
        {
            Assert.IsFalse(StringExtensions.IsNotNullAndNotWhiteSpace(null));
            Assert.IsFalse(StringExtensions.IsNotNullAndNotWhiteSpace(string.Empty));
            Assert.IsFalse(StringExtensions.IsNotNullAndNotWhiteSpace(" "));
            Assert.IsFalse(StringExtensions.IsNotNullAndNotWhiteSpace(Environment.NewLine));
            Assert.IsTrue(StringExtensions.IsNotNullAndNotWhiteSpace("a"));
        }
    }
}