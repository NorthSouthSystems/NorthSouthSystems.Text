namespace FOSStrich.Text;

public class StringExtensionsTests_VisualBasic
{
    [Theory]
    [InlineData("", 0)]
    [InlineData("a", 0)]
    [InlineData("a", 1)]
    [InlineData("a", 2)]
    [InlineData("ab", 0)]
    [InlineData("ab", 1)]
    [InlineData("ab", 2)]
    [InlineData("ab", 3)]
    [InlineData("abc", 0)]
    [InlineData("abc", 1)]
    [InlineData("abc", 2)]
    [InlineData("abc", 3)]
    [InlineData("abc", 4)]
    public void Left(string value, int length) =>
        StringExtensions.Left(value, length).Should().Be(Microsoft.VisualBasic.Strings.Left(value, length));

    [Theory]
    [InlineData("", 0)]
    [InlineData("a", 0)]
    [InlineData("a", 1)]
    [InlineData("a", 2)]
    [InlineData("ab", 0)]
    [InlineData("ab", 1)]
    [InlineData("ab", 2)]
    [InlineData("ab", 3)]
    [InlineData("abc", 0)]
    [InlineData("abc", 1)]
    [InlineData("abc", 2)]
    [InlineData("abc", 3)]
    [InlineData("abc", 4)]
    public void Right(string value, int length) =>
        StringExtensions.Right(value, length).Should().Be(Microsoft.VisualBasic.Strings.Right(value, length));

    [Fact]
    public void Exceptions()
    {
        Action act;

        act = () => ((string)null).Left(1);
        act.Should().Throw<ArgumentNullException>();

        act = () => "a".Left(-1);
        act.Should().Throw<ArgumentOutOfRangeException>();

        act = () => ((string)null).Right(1);
        act.Should().Throw<ArgumentNullException>();

        act = () => "a".Right(-1);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}