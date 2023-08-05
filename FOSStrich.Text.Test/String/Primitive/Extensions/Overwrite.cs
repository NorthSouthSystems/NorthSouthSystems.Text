namespace FOSStrich.Text;

public class StringExtensionsTests_Overwrite
{
    [Fact]
    public void Basic()
    {
        "abc".Overwrite(0, "z").Should().Be("zbc");
        "abc".Overwrite(1, "z").Should().Be("azc");
        "abc".Overwrite(1, "zy").Should().Be("azy");
        "abc".Overwrite(2, "zy").Should().Be("abzy");
        "abc".Overwrite(3, "zy").Should().Be("abczy");
    }

    [Fact]
    public void Exceptions()
    {
        Action act;

        act = () => ((string)null).Overwrite(0, "z");
        act.Should().Throw<ArgumentNullException>();

        act = () => "a".Overwrite(-1, "z");
        act.Should().Throw<ArgumentOutOfRangeException>();

        act = () => "a".Overwrite(2, "z");
        act.Should().Throw<ArgumentOutOfRangeException>();

        act = () => "a".Overwrite(0, null);
        act.Should().Throw<ArgumentNullException>();
    }
}