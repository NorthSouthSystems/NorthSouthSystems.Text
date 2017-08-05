namespace Kangarooper.Text
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;

    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void ToNewString()
        {
            char[] chars = new[] { 'f', 'o', 'o', 'b', 'a', 'r' };
            string s = chars.ToNewString();
            Assert.AreEqual("foobar", s);
        }

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

        [TestMethod]
        public void NormalizeWhiteSpaceNoChanges()
        {
            Assert.AreEqual("", "".NormalizeWhiteSpace());
            Assert.AreEqual("A", "A".NormalizeWhiteSpace());
            Assert.AreEqual("No Changes", "No Changes".NormalizeWhiteSpace());
            Assert.AreEqual("No Changes At All", "No Changes At All".NormalizeWhiteSpace());
        }

        [TestMethod]
        public void NormalizeWhiteSpaceChanges()
        {
            Assert.AreEqual("", " ".NormalizeWhiteSpace());
            Assert.AreEqual("A", " A".NormalizeWhiteSpace());
            Assert.AreEqual("A", "A ".NormalizeWhiteSpace());
            Assert.AreEqual("A", " A ".NormalizeWhiteSpace());
            Assert.AreEqual("A", "  A  ".NormalizeWhiteSpace());
            Assert.AreEqual("Changes", " Changes ".NormalizeWhiteSpace());
            Assert.AreEqual("Lots Of Changes", "Lots  Of   Changes".NormalizeWhiteSpace());
            Assert.AreEqual("Lots Of" + Environment.NewLine + "Changes", ("Lots\tOf" + Environment.NewLine + "Changes").NormalizeWhiteSpace());
            Assert.AreEqual("Lots Of Changes", ("Lots\tOf" + Environment.NewLine + "Changes").NormalizeWhiteSpace(null));
            Assert.AreEqual("Lots Of" + Environment.NewLine + "Changes", (" Lots \t Of " + Environment.NewLine + " Changes ").NormalizeWhiteSpace());
            Assert.AreEqual("Lots Of Changes", (" Lots \t Of " + Environment.NewLine + " Changes ").NormalizeWhiteSpace(null));
        }

        [TestMethod]
        public void NormalizeWhiteSpaceIEnumerableChanges()
        {
            Assert.AreEqual("", new string(" ".AsEnumerable().NormalizeWhiteSpace().ToArray()));
            Assert.AreEqual("A", new string(" A".AsEnumerable().NormalizeWhiteSpace().ToArray()));
            Assert.AreEqual("A", new string("A ".AsEnumerable().NormalizeWhiteSpace().ToArray()));
            Assert.AreEqual("A", new string(" A ".AsEnumerable().NormalizeWhiteSpace().ToArray()));
            Assert.AreEqual("A", new string("  A  ".AsEnumerable().NormalizeWhiteSpace().ToArray()));
            Assert.AreEqual("Changes", new string(" Changes ".AsEnumerable().NormalizeWhiteSpace().ToArray()));
            Assert.AreEqual("Lots Of Changes", new string("Lots  Of   Changes".AsEnumerable().NormalizeWhiteSpace().ToArray()));
            Assert.AreEqual("Lots Of" + Environment.NewLine + "Changes", new string(("Lots\tOf" + Environment.NewLine + "Changes").AsEnumerable().NormalizeWhiteSpace().ToArray()));
            Assert.AreEqual("Lots Of Changes", new string(("Lots\tOf" + Environment.NewLine + "Changes").AsEnumerable().NormalizeWhiteSpace(null).ToArray()));
            Assert.AreEqual("Lots Of" + Environment.NewLine + "Changes", new string((" Lots \t Of " + Environment.NewLine + " Changes ").AsEnumerable().NormalizeWhiteSpace().ToArray()));
            Assert.AreEqual("Lots Of Changes", new string((" Lots \t Of " + Environment.NewLine + " Changes ").AsEnumerable().NormalizeWhiteSpace(null).ToArray()));
        }

        [TestMethod]
        public void NormalizeWhiteSpaceNewLineRespect()
        {
            Assert.AreEqual("a" + Environment.NewLine + "b", ("a" + Environment.NewLine + "b").NormalizeWhiteSpace());
            Assert.AreEqual("a" + Environment.NewLine + "b", ("a " + Environment.NewLine + "b").NormalizeWhiteSpace());
            Assert.AreEqual("a" + Environment.NewLine + "b", ("a" + Environment.NewLine + " b").NormalizeWhiteSpace());
            Assert.AreEqual("a" + Environment.NewLine + "b", ("a " + Environment.NewLine + " b").NormalizeWhiteSpace());
        }

        [TestMethod]
        public void NormalizeWhiteSpaceNewLineNoRespect()
        {
            Assert.AreEqual("", "\r".NormalizeWhiteSpace(null));
            Assert.AreEqual("", "\n".NormalizeWhiteSpace(null));
            Assert.AreEqual("", "\r\n".NormalizeWhiteSpace(null));
            Assert.AreEqual("", Environment.NewLine.NormalizeWhiteSpace(null));

            Assert.AreEqual("a b", "a\rb".NormalizeWhiteSpace(null));
            Assert.AreEqual("a b", "a\r b".NormalizeWhiteSpace(null));
            Assert.AreEqual("a b", "a \rb".NormalizeWhiteSpace(null));
            Assert.AreEqual("a b", "a \r b".NormalizeWhiteSpace(null));

            Assert.AreEqual("a b", "a\nb".NormalizeWhiteSpace(null));
            Assert.AreEqual("a b", "a\n b".NormalizeWhiteSpace(null));
            Assert.AreEqual("a b", "a \nb".NormalizeWhiteSpace(null));
            Assert.AreEqual("a b", "a \n b".NormalizeWhiteSpace(null));

            Assert.AreEqual("a b", "a\r\nb".NormalizeWhiteSpace(null));
            Assert.AreEqual("a b", "a\r\n b".NormalizeWhiteSpace(null));
            Assert.AreEqual("a b", "a \r\nb".NormalizeWhiteSpace(null));
            Assert.AreEqual("a b", "a \r\n b".NormalizeWhiteSpace(null));

            Assert.AreEqual("a b", ("a" + Environment.NewLine + "b").NormalizeWhiteSpace(null));
            Assert.AreEqual("a b", ("a " + Environment.NewLine + "b").NormalizeWhiteSpace(null));
            Assert.AreEqual("a b", ("a" + Environment.NewLine + " b").NormalizeWhiteSpace(null));
            Assert.AreEqual("a b", ("a " + Environment.NewLine + " b").NormalizeWhiteSpace(null));
        }

        [TestMethod]
        public void SubstringToFit()
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

        [TestMethod]
        public void ToLowerCamelCase()
        {
            Assert.AreEqual("danTerry", "DanTerry".ToLowerCamelCase());
            Assert.AreEqual(" danTerry", " DanTerry".ToLowerCamelCase());

            Assert.AreEqual("danTerry", new string("DanTerry".AsEnumerable().ToLowerCamelCase().ToArray()));
            Assert.AreEqual(" danTerry", new string(" DanTerry".AsEnumerable().ToLowerCamelCase().ToArray()));
        }

        [TestMethod]
        public void ToUpperCamelCase()
        {
            Assert.AreEqual("DanTerry", "danTerry".ToUpperCamelCase());
            Assert.AreEqual(" DanTerry", " danTerry".ToUpperCamelCase());

            Assert.AreEqual("DanTerry", new string("danTerry".AsEnumerable().ToUpperCamelCase().ToArray()));
            Assert.AreEqual(" DanTerry", new string(" danTerry".AsEnumerable().ToUpperCamelCase().ToArray()));
        }

        [TestMethod]
        public void SpaceCamelCase()
        {
            Assert.AreEqual(string.Empty, string.Empty.SpaceCamelCase());
            Assert.AreEqual("Dan Terry", "DanTerry".SpaceCamelCase());
            Assert.AreEqual("Dan Terry Dan Terry", "DanTerry DanTerry".SpaceCamelCase());
            Assert.AreEqual("Dan Terry Dan Dan Terry Dan", "DanTerryDan DanTerryDan".SpaceCamelCase());
            Assert.AreEqual("1 A", "1A".SpaceCamelCase());
            Assert.AreEqual("123 A", "123A".SpaceCamelCase());
            Assert.AreEqual("123 a", "123a".SpaceCamelCase());
            Assert.AreEqual("A 1", "A1".SpaceCamelCase());
            Assert.AreEqual("A 123", "A123".SpaceCamelCase());
            Assert.AreEqual("a 123", "a123".SpaceCamelCase());
            Assert.AreEqual("A 1 A", "A1A".SpaceCamelCase());
            Assert.AreEqual("A 123 A", "A123A".SpaceCamelCase());
            Assert.AreEqual("a 123 a", "a123a".SpaceCamelCase());
        }

        [TestMethod]
        public void Filter()
        {
            Assert.AreEqual("a1b2c3d", "a1b2c3d".Filter(CharFilters.None));
            Assert.AreEqual("123", "a1b2c3d".Filter(CharFilters.RemoveLetters));
            Assert.AreEqual(string.Empty, "a1b2c3d".Filter(CharFilters.RemoveLetters | CharFilters.RemoveDigits));
            Assert.AreEqual("a1b2c3d", "a1b2-c3d".Filter(CharFilters.RemovePunctuation));
            Assert.AreEqual("-", "a1b2-c3d".Filter(CharFilters.RemoveLetters | CharFilters.RemoveDigits));
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NormalizeWhiteSpaceThisNull()
        {
            string s = null;
            s.NormalizeWhiteSpace();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubstringToFitThisNull()
        {
            string s = null;
            s.SubstringToFit(1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SubstringToFitMaxLengthLessThanZero()
        {
            "a".SubstringToFit(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToCamelCaseThisNull()
        {
            string s = null;
            s.ToLowerCamelCase();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SpaceCamelCaseThisNull()
        {
            string s = null;
            s.SpaceCamelCase();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FilterThisNull()
        {
            string s = null;
            s.Filter(CharFilters.None);
        }

        #endregion
    }
}