namespace NorthSouthSystems.Text;

public class StringQuotedExtensionsTests_Join
{
    private static string Expected(string format, StringQuotedSignals signals) =>
        StringQuotedTestHelper.Replace(format, signals);

    [Fact]
    public void Quoting()
    {
        QuotingBase(StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary);
        QuotingBase(new(["\t"], ["\n"], "\"", string.Empty));
        QuotingBase(new([","], [Environment.NewLine], "'", "\\"));
        QuotingBase(new(["DELIMITER"], ["NEWLINE"], "QUOTE", "ESCAPE"));
    }

    private static void QuotingBase(StringQuotedSignals signals)
    {
        if (!signals.QuoteIsSpecified)
            throw new NotSupportedException();

        new string[] { "a", "b", "c" }.JoinQuotedRow(signals)
            .Should().Be(
                Expected("a{d}b{d}c", signals));

        new string[] { null, "b", "c" }.JoinQuotedRow(signals)
            .Should().Be(
                Expected("{d}b{d}c", signals));

        new string[] { "a", "b", string.Empty }.JoinQuotedRow(signals)
            .Should().Be(
                Expected("a{d}b{d}", signals));

        new string[] { "aa", "bb", "cc" }.JoinQuotedRow(signals)
            .Should().Be(
                Expected("aa{d}bb{d}cc", signals));

        new string[] { "a" + signals.Delimiter, "b", "c" }.JoinQuotedRow(signals)
            .Should().Be(
                Expected("{q}a{d}{q}{d}b{d}c", signals));

        new string[] { "a" + signals.Quote, "b", "c" }.JoinQuotedRow(signals)
            .Should().Be(
                Expected(signals.EscapeIsSpecified
                    ? "{q}a{e}{q}{q}{d}b{d}c"
                    : "{q}a{q}{q}{q}{d}b{d}c", signals));

        new string[] { "aa", "bb", "cc" }.JoinQuotedRow(signals, true)
            .Should().Be(
                Expected("{q}aa{q}{d}{q}bb{q}{d}{q}cc{q}", signals));

        if (signals.NewRowIsSpecified)
        {
            new string[] { "a" + signals.NewRow + "a", "b", "c" }.JoinQuotedRow(signals)
                .Should().Be(
                    Expected("{q}a{n}a{q}{d}b{d}c", signals));
        }
    }

    [Fact]
    public void Escaping()
    {
        EscapingBase(new([","], ["\r\n"], null, "\\"));
        EscapingBase(new(["|"], ["\n"], null, "\\"));
        EscapingBase(new(["\t"], [Environment.NewLine], null, "\\"));
        EscapingBase(new(["DELIMITER"], ["NEWLINE"], null, "ESCAPE"));
    }

    private static void EscapingBase(StringQuotedSignals signals)
    {
        if (!signals.EscapeIsSpecified || signals.QuoteIsSpecified)
            throw new NotSupportedException();

        new string[] { "a" + signals.Delimiter, "b", "c" }.JoinQuotedRow(signals)
            .Should().Be(
                Expected("a{e}{d}{d}b{d}c", signals));

        if (signals.NewRowIsSpecified)
        {
            new string[] { "a" + signals.NewRow, "b", "c" }.JoinQuotedRow(signals)
            .Should().Be(
                Expected("a{e}{n}{d}b{d}c", signals));
        }

        new string[] { "a" + signals.Escape, "b", "c" }.JoinQuotedRow(signals)
            .Should().Be(
                Expected("a{e}{e}{d}b{d}c", signals));
    }

    [Fact]
    public void Exceptions()
    {
        Action act = null;

        act = () => ((string[])null).JoinQuotedRow(StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => new[] { "A" }.JoinQuotedRow(null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => new[] { "A" }.JoinQuotedRow(new([","], null, null, null), true);
        act.Should().ThrowExactly<ArgumentException>("QuoteNotSpecified");

        act = () => new[] { "A," }.JoinQuotedRow(new([","], null, null, null));
        act.Should().ThrowExactly<ArgumentException>("QuoteNotSpecified");

        act = () => new[] { "A" + Environment.NewLine }.JoinQuotedRow(new([","], [Environment.NewLine], null, null));
        act.Should().ThrowExactly<ArgumentException>("QuoteNotSpecified");
    }
}