namespace NorthSouthSystems.Text;

using MoreLinq;

public class StringQuotedExtensionsTests_SplitRow
{
    private static string Random(IReadOnlyList<string> signals) =>
        signals.Count > 0 ? signals[_random.Next(signals.Count)] : string.Empty;

    private static readonly Random _random = new(22);

    private static readonly IEnumerable<StringQuotedSignals> _signals =
    [
        StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary,
        new(["|"], ["\r\n"], "'", string.Empty),
        new(["\t"], ["\n"], "~", "\\"),
        new(["DELIMITER"], ["NEWLINE"], "QUOTE", "ESCAPE"),
        new([",", "|", "\t", "DELIMITER"], ["\r\n", "\n"], "\"", null)
    ];

    [Fact]
    public void EmptyFields() => _signals.ForEach(signals =>
    {
        string.Empty.SplitQuotedRow(signals)
            .Length.Should().Be(0);

        foreach (string delimiter in signals.Delimiters)
        {
            delimiter.SplitQuotedRow(signals)
                .Should().Equal(string.Empty, string.Empty);

            (delimiter + delimiter).SplitQuotedRow(signals)
                .Should().Equal(string.Empty, string.Empty, string.Empty);
        }
    });

    [Fact]
    public void FullFuzzing() => _signals.ForEach(signals =>
    {
        var pairs = _rawParsedFieldPairs.Where(p => p.IsRelevant(signals));

        foreach (var pair in pairs)
        {
            if (!string.IsNullOrEmpty(pair.RawFormat))
            {
                pair.Raw(signals).SplitQuotedRow(signals)
                    .Should().Equal(pair.Parsed(signals));

                if (signals.NewRowIsSpecified && pair.RawFormat != "{e}")
                {
                    (pair.Raw(signals) + Random(signals.NewRows)).SplitQuotedRow(signals)
                        .Should().Equal(pair.Parsed(signals));
                }
            }
        }

        new[] { 2, 3 }
            .SelectMany(subsetSize => pairs.Where(pair => pair.RawFormat != "{e}").Subsets(subsetSize))
            .SelectMany(subset => subset.Permutations())
            .Where(subset => subset.All(pair => pair.IsRelevant(signals)))
            .ForEach(subset =>
            {
                string.Join(Random(signals.Delimiters), subset.Select(pair => pair.Raw(signals))).SplitQuotedRow(signals)
                    .Should().Equal(subset.Select(pair => pair.Parsed(signals)));

                if (signals.NewRowIsSpecified)
                {
                    string.Join(Random(signals.Delimiters), subset.Select((pair, i) => pair.Raw(signals) + (i == subset.Count - 1 ? Random(signals.NewRows) : string.Empty))).SplitQuotedRow(signals)
                        .Should().Equal(subset.Select(pair => pair.Parsed(signals)));
                }
            });
    });

    private static readonly IEnumerable<RawParsedFieldPair> _rawParsedFieldPairs =
    [
        // Simple

        new("", ""),
        new("a", "a"),
        new("b", "b"),
        new("cd", "cd"),
        new(" a", " a"),
        new("a ", "a "),
        new(" a ", " a "),

        // Quoting Unneccessary

        new("{q}{q}", ""),
        new("{q}a{q}", "a"),
        new("{q}b{q}", "b"),
        new("{q}cd{q}", "cd"),
        new("{q} a{q}", " a"),
        new("{q}a {q}", "a "),
        new("{q} a {q}", " a "),

        // Quoting Neccessary

        new("{q}{d}{q}", "{d}"),
        new("{q}{n}{q}", "{n}"),
        new("{q}a{d}{q}", "a{d}"),
        new("{q}a{d}b{q}", "a{d}b"),
        new("{q}a{n}{q}", "a{n}"),
        new("{q}a{n}b{q}", "a{n}b"),

        // Quoting Quote Quote

        new("{q}{q}{q}{q}", "{q}"),
        new("{q}{q}a", "{q}a"),
        new("{q}{q}{q}a{q}", "{q}a"),
        new("{q}{q}{q}{q}a", "{q}{q}a"),
        new("{q}{q}{q}{q}{q}a{q}", "{q}{q}a"),
        new("{q}{q}{q}a{q}{q}{q}", "{q}a{q}"),
        new("{q}a{q}{q}a{q}{q}a{q}", "a{q}a{q}a"),

        // Quoting Complex

        new("{q}{q}{q}a{d}{q}", "{q}a{d}"),
        new("{q}{q}{q}{d}a{q}", "{q}{d}a"),
        new("{q}{q}{q}a{n}{q}", "{q}a{n}"),
        new("{q}{q}{q}{n}a{q}", "{q}{n}a"),
        new("{q}{q}{q}a{d}{n}{q}", "{q}a{d}{n}"),
        new("{q}{q}{q}{d}{n}a{q}", "{q}{d}{n}a"),
        new("{q}{q}{q}a{n}{d}{q}", "{q}a{n}{d}"),
        new("{q}{q}{q}{n}{d}a{q}", "{q}{n}{d}a"),

        // Escaping

        new("{e}", ""),
        new("{e} ", " "),
        new("{e}a", "a"),
        new("{e}{q}", "{q}"),
        new("{e}{d}", "{d}"),
        new("{e}{n}", "{n}"),
        new("{e}{e}", "{e}"),
        new("{e}{q}a", "{q}a"),
        new("{e}{d}a", "{d}a"),
        new("{e}{n}a", "{n}a"),
        new("{e}{e}a", "{e}a"),
        new("a{e}{d}", "a{d}"),
        new("a{e}{n}", "a{n}"),
        new("a{e}{e}", "a{e}"),
        new("a{e}{d}b", "a{d}b"),
        new("a{e}{n}b", "a{n}b"),
        new("a{e}{e}b", "a{e}b"),
        new("{e}{d}{e}{d}", "{d}{d}"),
        new("{e}{d}{e}{d}a", "{d}{d}a"),
        new("{e}{d}a{e}{d}", "{d}a{d}"),

        // Quoting + Escaping

        new("{e}{q}a", "{q}a"),
        new("a{e}{q}", "a{q}"),
        new("{q}{e}{q}{q}", "{q}"),
        new("{q}{e}{q}a{q}", "{q}a"),
        new("{e}{q}{e}{q}", "{q}{q}"),
        new("{e}{q}{e}{q}a", "{q}{q}a"),
        new("{q}{e}{q}{e}{q}{q}", "{q}{q}"),
        new("{q}{e}{q}{e}{q}a{q}", "{q}{q}a"),
        new("{q}{e}{q}a{e}{q}{q}", "{q}a{q}"),
        new("{q}{e}{q}a{d}{e}{q}{n}{q}", "{q}a{d}{q}{n}"),
    ];

    private class RawParsedFieldPair
    {
        internal RawParsedFieldPair(string rawFormat, string parsedFormat)
        {
            RawFormat = rawFormat;
            ParsedFormat = parsedFormat;

            foreach (var signals in _signals)
            {
                string delimiter = Random(signals.Delimiters);
                string newRow = Random(signals.NewRows);

                _raw.Add(signals, Replace(RawFormat));
                _parsed.Add(signals, Replace(ParsedFormat));

                string Replace(string format) =>
                    format.Replace("{d}", delimiter)
                        .Replace("{n}", newRow)
                        .Replace("{q}", signals.Quote)
                        .Replace("{e}", signals.Escape);
            }
        }

        internal string RawFormat { get; }
        internal string ParsedFormat { get; }

        internal string Raw(StringQuotedSignals signals) => _raw[signals];
        internal string Parsed(StringQuotedSignals signals) => _parsed[signals];

        private readonly Dictionary<StringQuotedSignals, string> _raw = new();
        private readonly Dictionary<StringQuotedSignals, string> _parsed = new();

        internal bool IsRelevant(StringQuotedSignals signals)
        {
            return (signals.DelimiterIsSpecified || !Contains("{d}"))
                && (signals.QuoteIsSpecified || !Contains("{q}"))
                && (signals.NewRowIsSpecified || !Contains("{n}"))
                && (signals.EscapeIsSpecified || !Contains("{e}"));

            bool Contains(string format) =>
                RawFormat.Contains(format) || ParsedFormat.Contains(format);
        }
    }

    [Fact]
    public void ComplicatedDelimiter()
    {
        var signals = new StringQuotedSignals(["ab"], null, null, null);
        "1aab2aab3a".SplitQuotedRow(signals)
            .Should().Equal("1a", "2a", "3a");

        signals = new StringQuotedSignals(["ababb"], null, null, null);
        "1abababb2abababb3ab".SplitQuotedRow(signals)
            .Should().Equal("1ab", "2ab", "3ab");
    }

    [Fact]
    public void Exceptions()
    {
        Action act;

        act = () => ((string)null).SplitQuotedRow(StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => string.Empty.SplitQuotedRow(null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => string.Format("a,b,c{0}d", StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary.NewRow).SplitQuotedRow(StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary);
        act.Should().ThrowExactly<ArgumentException>("NewLineInArgument");

        act = () => string.Format("a,b,c{0}d,e,f", StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary.NewRow).SplitQuotedRow(StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary);
        act.Should().ThrowExactly<ArgumentException>("NewLineInArgument");

        act = () => string.Format("a,b,c{0}d,e,f{0}g,h,i", StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary.NewRow).SplitQuotedRow(StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary);
        act.Should().ThrowExactly<ArgumentException>("NewLineInArgument");
    }
}