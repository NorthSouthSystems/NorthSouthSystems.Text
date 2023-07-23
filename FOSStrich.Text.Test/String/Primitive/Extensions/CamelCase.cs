namespace FOSStrich.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

public static partial class StringExtensionsTests
{
    [TestClass]
    public class CamelCase
    {
        [TestMethod]
        public void ToLower()
        {
            Assert.AreEqual("danTerry", "DanTerry".ToLowerCamelCase());
            Assert.AreEqual(" danTerry", " DanTerry".ToLowerCamelCase());

            Assert.AreEqual("danTerry", new string("DanTerry".AsEnumerable().ToLowerCamelCase().ToArray()));
            Assert.AreEqual(" danTerry", new string(" DanTerry".AsEnumerable().ToLowerCamelCase().ToArray()));
        }

        [TestMethod]
        public void ToUpper()
        {
            Assert.AreEqual("DanTerry", "danTerry".ToUpperCamelCase());
            Assert.AreEqual(" DanTerry", " danTerry".ToUpperCamelCase());

            Assert.AreEqual("DanTerry", new string("danTerry".AsEnumerable().ToUpperCamelCase().ToArray()));
            Assert.AreEqual(" DanTerry", new string(" danTerry".AsEnumerable().ToUpperCamelCase().ToArray()));
        }

        [TestMethod]
        public void Space()
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

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToLowerThisNull()
        {
            string s = null;
            s.ToLowerCamelCase();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SpaceThisNull()
        {
            string s = null;
            s.SpaceCamelCase();
        }

        #endregion
    }
}