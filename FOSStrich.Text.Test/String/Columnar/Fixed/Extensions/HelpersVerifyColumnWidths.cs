namespace FOSStrich.Text;

public static partial class StringFixedExtensionsTests
{
    [TestClass]
    public class VerifyColumnWidths
    {
        [TestMethod]
        public void Basic()
        {
            foreach (int count in Enumerable.Range(1, 10))
                StringFixedExtensions.VerifyColumnWidths(Enumerable.Range(1, count).ToArray());
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null() => StringFixedExtensions.VerifyColumnWidths(null);

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Empty() => StringFixedExtensions.VerifyColumnWidths(Array.Empty<int>());

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WidthEqualToZero1() => StringFixedExtensions.VerifyColumnWidths(new[] { 0 });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WidthEqualToZero2() => StringFixedExtensions.VerifyColumnWidths(new[] { 0, 1 });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WidthEqualToZero3() => StringFixedExtensions.VerifyColumnWidths(new[] { 1, 0 });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WidthLessThanZero1() => StringFixedExtensions.VerifyColumnWidths(new[] { -1 });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WidthLessThanZero2() => StringFixedExtensions.VerifyColumnWidths(new[] { -1, 1 });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WidthLessThanZero3() => StringFixedExtensions.VerifyColumnWidths(new[] { 1, -1 });

        #endregion
    }
}