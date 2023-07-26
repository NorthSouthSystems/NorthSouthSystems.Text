namespace FOSStrich.Text;

public static partial class CharExtensionsTests
{
    [TestClass]
    public class Ascii
    {
        [TestMethod]
        public void IsAscii()
        {
            Assert.IsTrue('a'.IsAscii());
            Assert.IsTrue('1'.IsAscii());
            Assert.IsTrue('.'.IsAscii());
            Assert.IsTrue(' '.IsAscii());
            Assert.IsTrue('*'.IsAscii());
            Assert.IsTrue('@'.IsAscii());

            Assert.IsTrue('\t'.IsAscii());
            Assert.IsTrue('\r'.IsAscii());
            Assert.IsTrue('\n'.IsAscii());

            Assert.IsFalse('\u2714'.IsAscii());
        }

        [TestMethod]
        public void IsAsciiPrintable()
        {
            Assert.IsTrue('a'.IsAsciiPrintable());
            Assert.IsTrue('1'.IsAsciiPrintable());
            Assert.IsTrue('.'.IsAsciiPrintable());
            Assert.IsTrue(' '.IsAsciiPrintable());
            Assert.IsTrue('*'.IsAsciiPrintable());
            Assert.IsTrue('@'.IsAsciiPrintable());

            Assert.IsFalse('\t'.IsAsciiPrintable());
            Assert.IsFalse('\r'.IsAsciiPrintable());
            Assert.IsFalse('\n'.IsAsciiPrintable());

            Assert.IsFalse('\u2714'.IsAsciiPrintable());
        }
    }
}