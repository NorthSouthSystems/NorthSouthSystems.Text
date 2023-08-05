namespace FOSStrich.Text;

public class CharExtensionsTests_ToNewString
{
    [Fact]
    public void Basic()
    {
        char[] chars = new[] { 'f', 'o', 'o', 'b', 'a', 'r' };
        chars.ToNewString().Should().Be(new string(chars));
    }
}