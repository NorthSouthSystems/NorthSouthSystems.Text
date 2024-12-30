using MoreLinq;

namespace NorthSouthSystems.Text;

public class StringQuotedExtensionsTests_Join
{
    private static string Expected(string format, StringQuotedSignals signals) =>
        StringQuotedFixture.Replace(format, signals).First();

    private static string Expected(string format, StringQuotedSignals signals, string delimiterOverride, string newRowOverride) =>
        Expected(format, signals)
            .Replace("{do}", delimiterOverride ?? signals.Delimiter)
            .Replace("{no}", newRowOverride ?? signals.NewRow);

    [Fact]
    public void Quoting() =>
        StringQuotedFixture.Signals.Where(signals => signals.QuoteIsSpecified).ForEach(signals =>
    {
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

        foreach (string delimiter in signals.Delimiters)
            new string[] { "a" + delimiter, "b", "c" }.JoinQuotedRow(signals)
                .Should().Be(
                    Expected("{q}a{do}{q}{d}b{d}c", signals, delimiter, null));

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
            foreach (string newRow in signals.NewRows)
                new string[] { "a" + newRow + "a", "b", "c" }.JoinQuotedRow(signals)
                    .Should().Be(
                        Expected("{q}a{no}a{q}{d}b{d}c", signals, null, newRow));
        }
    });

    [Fact]
    public void Escaping() =>
        StringQuotedFixture.Signals.Where(signals => signals.EscapeIsSpecified && !signals.QuoteIsSpecified).ForEach(signals =>
    {
        foreach (string delimiter in signals.Delimiters)
            new string[] { "a" + delimiter, "b", "c" }.JoinQuotedRow(signals)
                .Should().Be(
                    Expected("a{e}{do}{d}b{d}c", signals, delimiter, null));

        if (signals.NewRowIsSpecified)
        {
            foreach (string newRow in signals.NewRows)
                new string[] { "a" + newRow, "b", "c" }.JoinQuotedRow(signals)
                    .Should().Be(
                        Expected("a{e}{no}{d}b{d}c", signals, null, newRow));
        }

        new string[] { "a" + signals.Escape, "b", "c" }.JoinQuotedRow(signals)
            .Should().Be(
                Expected("a{e}{e}{d}b{d}c", signals));
    });

    [Fact]
    public void Exceptions()
    {
        Action act = null;

        act = () => ((string[])null).JoinQuotedRow(StringQuotedSignals.CsvNewRowTolerantWindowsPrimaryRFC4180);
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