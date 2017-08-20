namespace Kangarooper.Text
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringTests
    {
        /// <summary>
        /// This test method demonstrates that String.Join allows one of the joined values to be null; therefore, our
        /// Join methods should allow the same.
        /// </summary>
        [TestMethod]
        public void JoinWithANullValue() =>
            Assert.AreEqual("a,,b", string.Join(",", new[] { "a", null, "b" }));
    }
}