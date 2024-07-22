namespace NorthSouthSystems.Text;

public class StringQuotedSignalsTests
{
    [Fact]
    public void Basic()
    {
        var signals = new StringQuotedSignals([","], [Environment.NewLine], null, string.Empty);

        signals.DelimiterIsSpecified.Should().BeTrue();
        signals.Delimiter.Should().Be(",");

        signals.QuoteIsSpecified.Should().BeFalse();
        signals.Quote.Should().BeEmpty();

        signals.NewRowIsSpecified.Should().BeTrue();
        signals.NewRow.Should().Be(Environment.NewLine);

        signals.EscapeIsSpecified.Should().BeFalse();
        signals.Escape.Should().BeEmpty();
    }
}