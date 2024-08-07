﻿namespace NorthSouthSystems.Text;

public class StringQuotedExtensionsTests_Join
{
    internal static string Expected(string format, StringQuotedSignals signals) =>
        string.Format(format, signals.Delimiter, signals.Quote, signals.NewRow, signals.Escape);

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
                Expected("a{0}b{0}c", signals));

        new string[] { null, "b", "c" }.JoinQuotedRow(signals)
            .Should().Be(
                Expected("{0}b{0}c", signals));

        new string[] { "a", "b", string.Empty }.JoinQuotedRow(signals)
            .Should().Be(
                Expected("a{0}b{0}", signals));

        new string[] { "aa", "bb", "cc" }.JoinQuotedRow(signals)
            .Should().Be(
                Expected("aa{0}bb{0}cc", signals));

        new string[] { "a" + signals.Delimiter, "b", "c" }.JoinQuotedRow(signals)
            .Should().Be(
                Expected("{1}a{0}{1}{0}b{0}c", signals));

        new string[] { "a" + signals.Quote, "b", "c" }.JoinQuotedRow(signals)
            .Should().Be(
                Expected(signals.EscapeIsSpecified
                    ? "{1}a{3}{1}{1}{0}b{0}c"
                    : "{1}a{1}{1}{1}{0}b{0}c", signals));

        new string[] { "aa", "bb", "cc" }.JoinQuotedRow(signals, true)
            .Should().Be(
                Expected("{1}aa{1}{0}{1}bb{1}{0}{1}cc{1}", signals));

        if (signals.NewRowIsSpecified)
        {
            new string[] { "a" + signals.NewRow + "a", "b", "c" }.JoinQuotedRow(signals)
                .Should().Be(
                    Expected("{1}a{2}a{1}{0}b{0}c", signals));
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
                Expected("a{3}{0}{0}b{0}c", signals));

        if (signals.NewRowIsSpecified)
        {
            new string[] { "a" + signals.NewRow, "b", "c" }.JoinQuotedRow(signals)
            .Should().Be(
                Expected("a{3}{2}{0}b{0}c", signals));
        }

        new string[] { "a" + signals.Escape, "b", "c" }.JoinQuotedRow(signals)
            .Should().Be(
                Expected("a{3}{3}{0}b{0}c", signals));
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