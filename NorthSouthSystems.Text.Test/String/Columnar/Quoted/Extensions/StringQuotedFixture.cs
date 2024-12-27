namespace NorthSouthSystems.Text;

internal static class StringQuotedFixture
{
    internal static IEnumerable<string> Replace(string format, StringQuotedSignals signals)
    {
        foreach (string delimiter in signals.Delimiters)
            foreach (string newRow in signals.NewRows.DefaultIfEmpty(string.Empty))
                yield return Replace(format, signals.Delimiter, signals.NewRow, signals.Quote, signals.Escape);
    }

    internal static string Replace(string format,
            string delimiter, string newRow, string quote, string escape) =>
        format.Replace("{d}", delimiter)
            .Replace("{n}", newRow)
            .Replace("{q}", quote)
            .Replace("{e}", escape);

    internal static string Random(IReadOnlyList<string> signals) =>
        signals.Count > 0 ? signals[_random.Next(signals.Count)] : string.Empty;

    private static readonly Random _random = new(22);

    internal static IEnumerable<StringQuotedSignals> Signals { get; } =
    [
        StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary,
        new(["|"], ["\r\n"], "'", string.Empty),
        new(["\t"], ["\n"], "~", "\\"),
        new(["DELIMITER"], ["NEWLINE"], "QUOTE", "ESCAPE"),
        new([",", "|", "\t", "DELIMITER"], ["\r\n", "\n", "\r"], "\"", null)
    ];
}

internal class StringQuotedRawParsedFieldPair
{
    internal static IEnumerable<(string Raw, string Parsed)> Fuzzing(StringQuotedSignals signals)
    {
        // A standalone escape character in a field causes the SplitQuotedProcessor to ignore the following
        // delimiter and breaks all such Fuzzing tests as expected. Therefore, we ignore it for Fuzzing tests.
        foreach (var pair in _pairs
            .Where(p => p.IsRelevant(signals))
            .Where(p => p._rawFormat != "{e}"))
        {
            string delimiter = StringQuotedFixture.Random(signals.Delimiters);
            string newRow = StringQuotedFixture.Random(signals.NewRows);

            yield return (
                StringQuotedFixture.Replace(pair._rawFormat, delimiter, newRow, signals.Quote, signals.Escape),
                StringQuotedFixture.Replace(pair._parsedFormat, delimiter, newRow, signals.Quote, signals.Escape));
        }
    }

    private static readonly IEnumerable<StringQuotedRawParsedFieldPair> _pairs =
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

    private StringQuotedRawParsedFieldPair(string rawFormat, string parsedFormat)
    {
        _rawFormat = rawFormat;
        _parsedFormat = parsedFormat;
    }

    internal string _rawFormat { get; }
    internal string _parsedFormat { get; }

    internal bool IsRelevant(StringQuotedSignals signals)
    {
        return (signals.DelimiterIsSpecified || !Contains("{d}"))
            && (signals.QuoteIsSpecified || !Contains("{q}"))
            && (signals.NewRowIsSpecified || !Contains("{n}"))
            && (signals.EscapeIsSpecified || !Contains("{e}"));

        bool Contains(string format) =>
            _rawFormat.Contains(format) || _parsedFormat.Contains(format);
    }
}