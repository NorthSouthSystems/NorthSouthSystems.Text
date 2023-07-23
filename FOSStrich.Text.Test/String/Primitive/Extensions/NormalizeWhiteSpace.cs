namespace FOSStrich.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

public static partial class StringExtensionsTests
{
    [TestClass]
    public class NormalizeWhiteSpace
    {
        [TestMethod]
        public void NoChanges()
        {
            Assert.AreEqual("", "".NormalizeWhiteSpace());
            Assert.AreEqual("A", "A".NormalizeWhiteSpace());
            Assert.AreEqual("No Changes", "No Changes".NormalizeWhiteSpace());
            Assert.AreEqual("No Changes At All", "No Changes At All".NormalizeWhiteSpace());
        }

        [TestMethod]
        public void Changes()
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
        public void IEnumerableChanges()
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
        public void NewLineRespect()
        {
            Assert.AreEqual("a" + Environment.NewLine + "b", ("a" + Environment.NewLine + "b").NormalizeWhiteSpace());
            Assert.AreEqual("a" + Environment.NewLine + "b", ("a " + Environment.NewLine + "b").NormalizeWhiteSpace());
            Assert.AreEqual("a" + Environment.NewLine + "b", ("a" + Environment.NewLine + " b").NormalizeWhiteSpace());
            Assert.AreEqual("a" + Environment.NewLine + "b", ("a " + Environment.NewLine + " b").NormalizeWhiteSpace());
        }

        [TestMethod]
        public void NewLineNoRespect()
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

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThisNull()
        {
            string s = null;
            s.NormalizeWhiteSpace();
        }

        #endregion
    }
}