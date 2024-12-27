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
    public void FuzzingSingleField() => StringQuotedFixture.Signals.ForEach(signals =>
    {
        // A row with a single empty field results in an empty collection as desired. That special case is addressed
        // in the EmptyFields Fact.
        foreach (var pair in StringQuotedRawParsedFieldPair.Fuzzing(signals)
            .Where(p => !string.IsNullOrEmpty(p.Raw)))
        {
            pair.Raw.SplitQuotedRow(signals)
                .Should().Equal(pair.Parsed);

            if (signals.NewRowIsSpecified)
            {
                (pair.Raw + StringQuotedFixture.Random(signals.NewRows)).SplitQuotedRow(signals)
                    .Should().Equal(pair.Parsed);
            }
        }
    });

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    public void FuzzingMultiField(int subsetSize) => StringQuotedFixture.Signals.ForEach(signals =>
    {
        foreach (var permutation in StringQuotedRawParsedFieldPair.Fuzzing(signals)
            .Subsets(subsetSize)
            .SelectMany(subset => subset.Permutations()))
        {
            string.Join(StringQuotedFixture.Random(signals.Delimiters), permutation.Select(pair => pair.Raw)).SplitQuotedRow(signals)
                .Should().Equal(permutation.Select(pair => pair.Parsed));

            if (signals.NewRowIsSpecified)
            {
                string.Join(StringQuotedFixture.Random(signals.Delimiters), permutation.Select((pair, i) => pair.Raw + (i == permutation.Count - 1 ? StringQuotedFixture.Random(signals.NewRows) : string.Empty))).SplitQuotedRow(signals)
                    .Should().Equal(permutation.Select(pair => pair.Parsed));
            }
        }
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