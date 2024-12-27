namespace NorthSouthSystems.Text;

using MoreLinq;

public class StringQuotedExtensionsTests_SplitRow
{
    [Fact]
    public void EmptyFields() => StringQuotedFixture.Signals.ForEach(signals =>
    {
        SplitAndAssert(string.Empty, 0);

        foreach (string delimiter in signals.Delimiters)
        {
            SplitAndAssert(delimiter, 2);
            SplitAndAssert(delimiter + delimiter, 3);

            if (signals.QuoteIsSpecified)
            {
                string quotedEmpty = signals.Quote + signals.Quote;

                SplitAndAssert(quotedEmpty + delimiter, 2);
                SplitAndAssert(delimiter + quotedEmpty, 2);
                SplitAndAssert(quotedEmpty + delimiter + quotedEmpty, 2);

                SplitAndAssert(quotedEmpty + delimiter + delimiter, 3);
                SplitAndAssert(delimiter + quotedEmpty + delimiter, 3);
                SplitAndAssert(delimiter + delimiter + quotedEmpty, 3);
                SplitAndAssert(quotedEmpty + delimiter + quotedEmpty + delimiter + quotedEmpty, 3);
            }
        }

        void SplitAndAssert(string row, int fieldCount) =>
            row.SplitQuotedRow(signals).Should().Equal(Enumerable.Repeat(string.Empty, fieldCount));
    });

    [Fact]
    public void FullFuzzing() => StringQuotedFixture.Signals.ForEach(signals =>
    {
        var pairs = StringQuotedFixture.RawParsedFieldPairs.Where(p => p.IsRelevant(signals));

        foreach (var pair in pairs)
        {
            if (!string.IsNullOrEmpty(pair.RawFormat))
            {
                pair.Raw(signals).SplitQuotedRow(signals)
                    .Should().Equal(pair.Parsed(signals));

                if (signals.NewRowIsSpecified && pair.RawFormat != "{e}")
                {
                    (pair.Raw(signals) + StringQuotedFixture.Random(signals.NewRows)).SplitQuotedRow(signals)
                        .Should().Equal(pair.Parsed(signals));
                }
            }
        }

        new[] { 2, 3 }
            .SelectMany(subsetSize => pairs.Where(pair => pair.RawFormat != "{e}").Subsets(subsetSize))
            .SelectMany(subset => subset.Permutations())
            .ForEach(subset =>
            {
                string.Join(StringQuotedFixture.Random(signals.Delimiters), subset.Select(pair => pair.Raw(signals))).SplitQuotedRow(signals)
                    .Should().Equal(subset.Select(pair => pair.Parsed(signals)));

                if (signals.NewRowIsSpecified)
                {
                    string.Join(StringQuotedFixture.Random(signals.Delimiters), subset.Select((pair, i) => pair.Raw(signals) + (i == subset.Count - 1 ? StringQuotedFixture.Random(signals.NewRows) : string.Empty))).SplitQuotedRow(signals)
                        .Should().Equal(subset.Select(pair => pair.Parsed(signals)));
                }
            });
    });

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