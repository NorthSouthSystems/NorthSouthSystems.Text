namespace FOSStrich.Text;

public class StringExtensionsTests_Coalesce
{
    [Fact]
    public void EmptyToNullNullified()
    {
        ((string)null).EmptyToNull().Should().BeNull();
        string.Empty.EmptyToNull().Should().BeNull();
    }

    [Fact]
    public void EmptyToNullEquals()
    {
        foreach (string s in new string[] { " ", "a", "A", "1", "abc", "ABC", "123" })
            s.EmptyToNull().Should().Be(s);
    }

    [Fact]
    public void ToStringNullToEmpty()
    {
        ((object)null).ToStringNullToEmpty().Should().BeEmpty();
        1.ToStringNullToEmpty().Should().Be("1");
    }
}