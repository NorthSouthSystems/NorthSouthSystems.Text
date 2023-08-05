namespace FOSStrich.Text;

public class StringExtensionsTests_IsNotNullAndNotX
{
    [Fact]
    public void IsNotNullAndNotEmpty()
    {
        StringExtensions.IsNotNullAndNotEmpty(null).Should().BeFalse();
        StringExtensions.IsNotNullAndNotEmpty(string.Empty).Should().BeFalse();
        StringExtensions.IsNotNullAndNotEmpty("a").Should().BeTrue();
    }

    [Fact]
    public void IsNotNullAndNotWhiteSpace()
    {
        StringExtensions.IsNotNullAndNotWhiteSpace(null).Should().BeFalse();
        StringExtensions.IsNotNullAndNotWhiteSpace(string.Empty).Should().BeFalse();
        StringExtensions.IsNotNullAndNotWhiteSpace(" ").Should().BeFalse();
        StringExtensions.IsNotNullAndNotWhiteSpace(Environment.NewLine).Should().BeFalse();
        StringExtensions.IsNotNullAndNotWhiteSpace("a").Should().BeTrue();
    }
}