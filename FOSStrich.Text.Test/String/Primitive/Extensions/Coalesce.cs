namespace FOSStrich.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

public static partial class StringExtensionsTests
{
    [TestClass]
    public class Coalesce
    {
        [TestMethod]
        public void EmptyToNullNullified()
        {
            Assert.IsNull(((string)null).EmptyToNull());
            Assert.IsNull(string.Empty.EmptyToNull());
            Assert.IsNull("".EmptyToNull());
        }

        [TestMethod]
        public void EmptyToNullEquals()
        {
            foreach (string s in new string[] { " ", "a", "A", "1", "abc", "ABC", "123" })
                Assert.AreEqual(s, s.EmptyToNull());
        }
    }
}