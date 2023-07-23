namespace FOSStrich.Text;

public static partial class CharExtensionsTests
{
    [TestClass]
    public class ToNewString
    {
        [TestMethod]
        public void Basic()
        {
            char[] chars = new[] { 'f', 'o', 'o', 'b', 'a', 'r' };
            string s = chars.ToNewString();
            Assert.AreEqual("foobar", s);
        }
    }
}