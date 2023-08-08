namespace FOSStrich.Text;

public class StringQuotedSignalsTests
{
    [Fact]
    public void Basic()
    {
        var signals = new StringQuotedSignals(",", null, Environment.NewLine, string.Empty);

        signals.DelimiterIsSpecified.Should().BeTrue();
        signals.Delimiter.Should().Be(",");

        signals.QuoteIsSpecified.Should().BeFalse();
        signals.Quote.Should().BeEmpty();

        signals.NewRowIsSpecified.Should().BeTrue();
        signals.NewRow.Should().Be(Environment.NewLine);

        signals.EscapeIsSpecified.Should().BeFalse();
        signals.Escape.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null, "\"", "\n", "\\")]
    [InlineData(null, "\"", "\r", "\\")]
    [InlineData(null, "\"", "\r\n", "\\")]
    [InlineData("", "\"", "\n", "\\")]
    [InlineData("", "\"", "\r", "\\")]
    [InlineData("", "\"", "\r\n", "\\")]
    [InlineData("A", "AB", null, null)]
    [InlineData("A", null, "AB", null)]
    [InlineData("A", null, null, "AB")]
    [InlineData("AB", "A", null, null)]
    [InlineData("AB", null, "A", null)]
    [InlineData("AB", null, null, "A")]
    [InlineData(",", "A", "AB", null)]
    [InlineData(",", "A", null, "AB")]
    [InlineData(",", "AB", "A", null)]
    [InlineData(",", "AB", null, "A")]
    [InlineData(",", null, "A", "AB")]
    [InlineData(",", null, "AB", "A")]
    public void Exceptions(string delimiter, string quote, string newRow, string escape)
    {
        Action act;

        act = () => new StringQuotedSignals(delimiter, quote, newRow, escape);
        act.Should().ThrowExactly<ArgumentException>();
    }
}