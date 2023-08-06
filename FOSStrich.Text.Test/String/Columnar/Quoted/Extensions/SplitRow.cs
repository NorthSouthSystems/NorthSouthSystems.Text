namespace FOSStrich.Text;

public class StringQuotedExtensionsTests_SplitRow
{
    private static string[] Split(string format, StringQuotedSignals signals) =>
        StringQuotedExtensionsTests_Join.Expected(format, signals)
            .SplitQuotedRow(signals);

    [Fact]
    public void NonQuoted()
    {
        NonQuotedBase(StringQuotedSignals.Csv);
        NonQuotedBase(StringQuotedSignals.Pipe);
        NonQuotedBase(StringQuotedSignals.Tab);
    }

    private static void NonQuotedBase(StringQuotedSignals signals)
    {
        Split(string.Empty, signals).Length.Should().Be(0);

        Split("{0}", signals)
            .Should().Equal(new[] { string.Empty, string.Empty });

        Split("{0}{0}", signals)
            .Should().Equal(new[] { string.Empty, string.Empty, string.Empty });

        Split("a", signals)
            .Should().Equal(new[] { "a" });

        Split("a{0}b", signals)
            .Should().Equal(new[] { "a", "b" });

        Split("a{0}b{0}c", signals)
            .Should().Equal(new[] { "a", "b", "c" });

        Split("a{0}b{0}", signals)
            .Should().Equal(new[] { "a", "b", string.Empty });

        Split("a{0}{0}c", signals)
            .Should().Equal(new[] { "a", string.Empty, "c" });

        if (signals.NewRowIsSpecified)
        {
            Split("{2}", signals)
                .Should().Equal(new[] { string.Empty });

            Split("a{2}", signals)
                .Should().Equal(new[] { "a" });

            Split("a{0}b{2}", signals)
                .Should().Equal(new[] { "a", "b" });

            Split("a{0}b{0}c{2}", signals)
                .Should().Equal(new[] { "a", "b", "c" });
        }
    }

    [Fact]
    public void Quoted()
    {
        QuotedBase(StringQuotedSignals.Csv);
        QuotedBase(StringQuotedSignals.Pipe);
        QuotedBase(StringQuotedSignals.Tab);
        QuotedBase(new(",", "'", Environment.NewLine, string.Empty));
    }

    private static void QuotedBase(StringQuotedSignals signals)
    {
        if (!signals.QuoteIsSpecified)
            throw new NotSupportedException();

        QuotedBaseQuotingUnnecessary(signals);
        QuotedBaseQuotingNecessary(signals);
    }

    private static void QuotedBaseQuotingUnnecessary(StringQuotedSignals signals)
    {
        Split("{1}{1}", signals)
            .Should().Equal(new[] { string.Empty });

        Split("{1}a{1}", signals)
            .Should().Equal(new[] { "a" });

        Split("{1}a{1}{0}{1}b{1}", signals)
            .Should().Equal(new[] { "a", "b" });

        Split("{1}a{1}{0}{1}b{1}{0}{1}c{1}", signals)
            .Should().Equal(new[] { "a", "b", "c" });

        Split("{1}a{1}{0}b{0}{1}c{1}", signals)
            .Should().Equal(new[] { "a", "b", "c" });

        Split("a{0}{1}b{1}{0}c", signals)
            .Should().Equal(new[] { "a", "b", "c" });

        Split("a{0}{1}{1}{0}c", signals)
            .Should().Equal(new[] { "a", string.Empty, "c" });

        Split("{1}a{1}{0}{1}b{1}{0}", signals)
            .Should().Equal(new[] { "a", "b", string.Empty });

        Split("{1}a{1}{0}{1}b{1}{0}{1}{1}", signals)
            .Should().Equal(new[] { "a", "b", string.Empty });
    }

    private static void QuotedBaseQuotingNecessary(StringQuotedSignals signals)
    {
        Split("{1}{1}{1}{1}", signals)
            .Should().Equal(new[] { signals.Quote });

        Split("{1}a{0}{1}", signals)
            .Should().Equal(new[] { "a" + signals.Delimiter });

        Split("{1}a{0}a{1}", signals)
            .Should().Equal(new[] { "a" + signals.Delimiter + "a" });

        Split("{1}a{0}a{1}{0}b", signals)
            .Should().Equal(new[] { "a" + signals.Delimiter + "a", "b" });

        Split("{1}a{0}a{1}{0}{1}b{0}{1}", signals)
            .Should().Equal(new[] { "a" + signals.Delimiter + "a", "b" + signals.Delimiter });

        Split("{1}a{0}a{1}{0}{1}b{0}b{1}", signals)
            .Should().Equal(new[] { "a" + signals.Delimiter + "a", "b" + signals.Delimiter + "b" });

        Split("{1}a{0}a{1}{0}{1}b{0}b{1}{0}c", signals)
            .Should().Equal(new[] { "a" + signals.Delimiter + "a", "b" + signals.Delimiter + "b", "c" });

        Split("{1}a{0}a{1}{0}{1}b{0}b{1}{0}{1}c{0}{1}", signals)
            .Should().Equal(new[] { "a" + signals.Delimiter + "a", "b" + signals.Delimiter + "b", "c" + signals.Delimiter });

        Split("{1}a{0}a{1}{0}{1}b{0}b{1}{0}{1}c{0}c{1}", signals)
            .Should().Equal(new[] { "a" + signals.Delimiter + "a", "b" + signals.Delimiter + "b", "c" + signals.Delimiter + "c" });

        Split("{1}a{0}{1}{0}b{0}c", signals)
            .Should().Equal(new[] { "a" + signals.Delimiter, "b", "c" });

        Split("a{0}{1}b{0}{1}{0}c", signals)
            .Should().Equal(new[] { "a", "b" + signals.Delimiter, "c" });

        Split("a{0}b{0}{1}c{0}{1}", signals)
            .Should().Equal(new[] { "a", "b", "c" + signals.Delimiter });

        Split("{1}a{0}a{1}{0}b{0}c", signals)
            .Should().Equal(new[] { "a" + signals.Delimiter + "a", "b", "c" });

        Split("a{0}{1}b{0}b{1}{0}c", signals)
            .Should().Equal(new[] { "a", "b" + signals.Delimiter + "b", "c" });

        Split("a{0}b{0}{1}c{0}c{1}", signals)
            .Should().Equal(new[] { "a", "b", "c" + signals.Delimiter + "c" });

        Split("a{0}b{0}c{0}{1}{0}{1}", signals)
            .Should().Equal(new[] { "a", "b", "c", signals.Delimiter });

        if (signals.NewRowIsSpecified)
        {
            Split("{1}a{2}{1}", signals)
                .Should().Equal(new[] { "a" + signals.NewRow });

            Split("{1}a{2}a{1}", signals)
                .Should().Equal(new[] { "a" + signals.NewRow + "a" });

            Split("{1}a{2}a{1}{0}b", signals)
                .Should().Equal(new[] { "a" + signals.NewRow + "a", "b" });

            Split("{1}a{2}a{1}{0}{1}b{2}{1}", signals)
                .Should().Equal(new[] { "a" + signals.NewRow + "a", "b" + signals.NewRow });

            Split("{1}a{2}a{1}{0}{1}b{2}b{1}", signals)
                .Should().Equal(new[] { "a" + signals.NewRow + "a", "b" + signals.NewRow + "b" });

            Split("{1}a{2}a{1}{0}{1}b{2}b{1}{0}{1}{2}{1}", signals)
                .Should().Equal(new[] { "a" + signals.NewRow + "a", "b" + signals.NewRow + "b", signals.NewRow });
        }
    }

    [Fact]
    public void QuotedEscapedQuote()
    {
        QuotedEscapedQuoteBase(StringQuotedSignals.Csv);
        QuotedEscapedQuoteBase(StringQuotedSignals.Pipe);
        QuotedEscapedQuoteBase(StringQuotedSignals.Tab);
        QuotedEscapedQuoteBase(new(",", "'", Environment.NewLine, "\\"));
        QuotedEscapedQuoteBase(new("DELIMITER", "QUOTE", "NEWLINE", "ESCAPE"));
    }

    private static void QuotedEscapedQuoteBase(StringQuotedSignals signals)
    {
        if (!signals.QuoteIsSpecified)
            throw new NotSupportedException();

        Split("{1}{1}a", signals)
            .Should().Equal(new[] { signals.Quote + "a" });

        Split("{1}{1}{1}a{1}", signals)
            .Should().Equal(new[] { signals.Quote + "a" });

        Split("{1}{1}{1}{1}a", signals)
            .Should().Equal(new[] { signals.Quote + signals.Quote + "a" });

        Split("{1}{1}{1}{1}{1}a{1}", signals)
            .Should().Equal(new[] { signals.Quote + signals.Quote + "a" });

        Split("{1}{1}a{0}b", signals)
            .Should().Equal(new[] { signals.Quote + "a", "b" });

        Split("{1}{1}{1}a{1}{0}b", signals)
            .Should().Equal(new[] { signals.Quote + "a", "b" });

        Split("{1}{1}{1}{1}a{0}b", signals)
            .Should().Equal(new[] { signals.Quote + signals.Quote + "a", "b" });

        Split("{1}{1}{1}{1}{1}a{1}{0}b", signals)
            .Should().Equal(new[] { signals.Quote + signals.Quote + "a", "b" });

        Split("a{0}{1}{1}b", signals)
            .Should().Equal(new[] { "a", signals.Quote + "b" });

        Split("a{0}{1}{1}{1}b{1}", signals)
            .Should().Equal(new[] { "a", signals.Quote + "b" });

        Split("a{0}{1}{1}{1}{1}b", signals)
            .Should().Equal(new[] { "a", signals.Quote + signals.Quote + "b" });

        Split("a{0}{1}{1}{1}{1}{1}b{1}", signals)
            .Should().Equal(new[] { "a", signals.Quote + signals.Quote + "b" });

        Split("a{0}{1}{1}b{0}c", signals)
            .Should().Equal(new[] { "a", signals.Quote + "b", "c" });

        Split("a{0}{1}{1}{1}b{1}{0}c", signals)
            .Should().Equal(new[] { "a", signals.Quote + "b", "c" });

        Split("a{0}{1}{1}{1}{1}b{0}c", signals)
            .Should().Equal(new[] { "a", signals.Quote + signals.Quote + "b", "c" });

        Split("a{0}{1}{1}{1}{1}{1}b{1}{0}c", signals)
            .Should().Equal(new[] { "a", signals.Quote + signals.Quote + "b", "c" });

        Split("{1}{1}a{0}{1}{1}b{0}{1}{1}c", signals)
            .Should().Equal(new[] { signals.Quote + "a", signals.Quote + "b", signals.Quote + "c" });

        Split("{1}{1}{1}a{1}{0}{1}{1}{1}b{1}{0}{1}{1}{1}c{1}", signals)
            .Should().Equal(new[] { signals.Quote + "a", signals.Quote + "b", signals.Quote + "c" });

        Split("{1}{1}{1}a{0}{1}{0}{1}{1}{1}b{0}{1}{0}{1}{1}{1}c{0}{1}", signals)
            .Should().Equal(new[] { signals.Quote + "a" + signals.Delimiter, signals.Quote + "b" + signals.Delimiter, signals.Quote + "c" + signals.Delimiter });

        if (signals.EscapeIsSpecified)
        {
            Split("{3}{1}", signals)
                .Should().Equal(new[] { signals.Quote });

            Split("{1}{3}{1}{1}", signals)
                .Should().Equal(new[] { signals.Quote });

            Split("{3}{1}a", signals)
                .Should().Equal(new[] { signals.Quote + "a" });

            Split("{1}{3}{1}a{1}", signals)
                .Should().Equal(new[] { signals.Quote + "a" });

            Split("{3}{1}{3}{1}a", signals)
                .Should().Equal(new[] { signals.Quote + signals.Quote + "a" });

            Split("{1}{3}{1}{3}{1}a{1}", signals)
                .Should().Equal(new[] { signals.Quote + signals.Quote + "a" });

            Split("{1}{3}{1}{3}{1}a{0}{2}{1}{0}b", signals)
                .Should().Equal(new[] { signals.Quote + signals.Quote + "a" + signals.Delimiter + signals.NewRow, "b" });
        }
    }

    [Fact]
    public void QuotedEscaped()
    {
        QuotedEscapedBase(new(",", "'", Environment.NewLine, "\\"));
        QuotedEscapedBase(new(",", string.Empty, Environment.NewLine, "\\"));
        QuotedEscapedBase(new("DELIMITER", "QUOTE", "NEWLINE", "ESCAPE"));
    }

    private static void QuotedEscapedBase(StringQuotedSignals signals)
    {
        if (!signals.EscapeIsSpecified)
            throw new NotSupportedException();

        Split("{3}", signals)
            .Should().Equal(new[] { string.Empty });

        Split("{3}{0}a", signals)
            .Should().Equal(new[] { signals.Delimiter + "a" });

        Split("{3}{0}a{0}b", signals)
            .Should().Equal(new[] { signals.Delimiter + "a", "b" });

        Split("{3}{0}a{3}{0}b", signals)
            .Should().Equal(new[] { signals.Delimiter + "a" + signals.Delimiter + "b" });

        Split("{3}a{0}{3}b{0}{3}c", signals)
            .Should().Equal(new[] { "a", "b", "c" });

        Split("a{0}b{0}c{3}{0}", signals)
            .Should().Equal(new[] { "a", "b", "c" + signals.Delimiter });

        Split("a{0}b{0}c{3}{0}{0}d", signals)
            .Should().Equal(new[] { "a", "b", "c" + signals.Delimiter, "d" });

        if (signals.QuoteIsSpecified)
        {
            Split("a{0}b{0}c{3}{1}", signals)
                .Should().Equal(new[] { "a", "b", "c" + signals.Quote });

            Split("a{0}b{0}c{3}{1}{0}d", signals)
                .Should().Equal(new[] { "a", "b", "c" + signals.Quote, "d" });
        }

        if (signals.NewRowIsSpecified)
        {
            Split("a{0}b{0}c{3}{2}", signals)
                .Should().Equal(new[] { "a", "b", "c" + signals.NewRow });

            Split("a{0}b{0}c{3}{2}{0}d", signals)
                .Should().Equal(new[] { "a", "b", "c" + signals.NewRow, "d" });
        }

        Split("a{0}b{0}c{3}{3}", signals)
            .Should().Equal(new[] { "a", "b", "c" + signals.Escape });

        Split("a{0}b{0}c{3}{3}{0}d", signals)
            .Should().Equal(new[] { "a", "b", "c" + signals.Escape, "d" });
    }

    [Fact]
    public void ComplicatedDelimiter()
    {
        var signals = new StringQuotedSignals("ab", null, null, null);
        "1aab2aab3a".SplitQuotedRow(signals)
            .Should().Equal(new[] { "1a", "2a", "3a" });

        signals = new StringQuotedSignals("ababb", null, null, null);
        "1abababb2abababb3ab".SplitQuotedRow(signals)
            .Should().Equal(new[] { "1ab", "2ab", "3ab" });
    }

    [Fact]
    public void Exceptions()
    {
        Action act;

        act = () => ((string)null).SplitQuotedRow(StringQuotedSignals.Csv);
        act.Should().Throw<ArgumentNullException>();

        act = () => string.Empty.SplitQuotedRow(null);
        act.Should().Throw<ArgumentNullException>();

        act = () => string.Format("a,b,c{0}d", StringQuotedSignals.Csv.NewRow).SplitQuotedRow(StringQuotedSignals.Csv);
        act.Should().Throw<ArgumentException>("NewLineInArgument");

        act = () => string.Format("a,b,c{0}d,e,f", StringQuotedSignals.Csv.NewRow).SplitQuotedRow(StringQuotedSignals.Csv);
        act.Should().Throw<ArgumentException>("NewLineInArgument");

        act = () => string.Format("a,b,c{0}d,e,f{0}g,h,i", StringQuotedSignals.Csv.NewRow).SplitQuotedRow(StringQuotedSignals.Csv);
        act.Should().Throw<ArgumentException>("NewLineInArgument");
    }
}