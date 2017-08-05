using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kangarooper.Text
{
    [TestClass]
    public class StringFixedJoinExtensionsTests
    {
        [TestMethod]
        public void Basic()
        {
            string join = new[] { "A", "B", "C" }.JoinFixedRow(new[] { 1, 1, 1 });
            Assert.AreEqual("ABC", join);

            join = new[] { "A", "B", "C" }.JoinFixedRow(new[] { 1, 1, 1 }, '-', false);
            Assert.AreEqual("ABC", join);

            join = new[] { "A", "B", "C" }.JoinFixedRow(new[] { 2, 1, 1 });
            Assert.AreEqual("A BC", join);

            join = new[] { "A", "B", "C" }.JoinFixedRow(new[] { 2, 1, 1 }, '-', false);
            Assert.AreEqual("A-BC", join);

            join = new[] { "A", "B", "C" }.JoinFixedRow(new[] { 2, 2, 2 });
            Assert.AreEqual("A B C ", join);

            join = new[] { "AB", "CD", "EF" }.JoinFixedRow(new[] { 2, 2, 2 });
            Assert.AreEqual("ABCDEF", join);

            join = new[] { "AB", "CD", "EF" }.JoinFixedRow(new[] { 3, 2, 2 }, '-', false);
            Assert.AreEqual("AB-CDEF", join);

            join = new[] { "AB", "CD", "EF" }.JoinFixedRow(new[] { 4, 4, 4 }, '-', false);
            Assert.AreEqual("AB--CD--EF--", join);
        }

        [TestMethod]
        public void SubstringToFit()
        {
            string join = new[] { "AB", "CD", "EF" }.JoinFixedRow(new[] { 1, 1, 1 }, '-', true);
            Assert.AreEqual("ACE", join);

            join = new[] { "AB", "CD", "EF" }.JoinFixedRow(new[] { 3, 3, 1 }, '-', true);
            Assert.AreEqual("AB-CD-E", join);
        }
    }
}