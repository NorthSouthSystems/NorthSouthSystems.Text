namespace NorthSouthSystems.Text;

using MoreLinq;
using System.Text;

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

        void SplitAndAssert(string row, int fieldCount)
        {
            var expectedFields = Enumerable.Repeat(string.Empty, fieldCount);

            row.SplitQuotedRow(signals).Should().Equal(expectedFields);

            if (fieldCount > 0)
                row.SplitQuotedRows(signals).Single().Should().Equal(expectedFields);
        }
    });

    [Fact]
    public void FuzzingSingleField() => StringQuotedFixture.Signals.ForEach(signals =>
    {
        // A row with a single empty field results in an empty collection as desired. That special case is addressed
        // in the EmptyFields Fact.
        foreach (var pair in SplitQuotedRawParsedFieldPair.Fuzzing(signals)
            .Where(p => !string.IsNullOrEmpty(p.Raw)))
        {
            pair.Raw.SplitQuotedRow(signals)
                .Should().Equal(pair.Parsed);

            pair.Raw.SplitQuotedRows(signals).Single()
                .Should().Equal(pair.Parsed);

            foreach (string delimiter in signals.Delimiters)
            {
                foreach (string newRow in signals.NewRows.DefaultIfEmpty(string.Empty))
                {
                    (pair.Raw + delimiter).SplitQuotedRow(signals)
                        .Should().Equal(pair.Parsed, string.Empty);

                    (delimiter + pair.Raw).SplitQuotedRow(signals)
                        .Should().Equal(string.Empty, pair.Parsed);

                    (pair.Raw + newRow).SplitQuotedRow(signals)
                        .Should().Equal(pair.Parsed);

                    (pair.Raw + delimiter).SplitQuotedRows(signals).Single()
                        .Should().Equal(pair.Parsed, string.Empty);

                    (delimiter + pair.Raw).SplitQuotedRows(signals).Single()
                        .Should().Equal(string.Empty, pair.Parsed);

                    (pair.Raw + newRow).SplitQuotedRows(signals).Single()
                        .Should().Equal(pair.Parsed);
                }
            }
        }
    });

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    public void FuzzingMultiField(int fieldCount) => StringQuotedFixture.Signals.ForEach(signals =>
    {
        var rowBuilder = new StringBuilder();

        foreach (var permutation in SplitQuotedRawParsedFieldPair.Fuzzing(signals)
            .Subsets(fieldCount)
            .SelectMany(subset => subset.Permutations()))
        {
            rowBuilder.Clear();

            permutation.ForEach((pair, index) =>
            {
                if (index > 0)
                    rowBuilder.Append(StringQuotedFixture.Random(signals.Delimiters));

                rowBuilder.Append(pair.Raw);
            });

            string row = rowBuilder.ToString();
            var expectedFields = permutation.Select(pair => pair.Parsed);

            row.SplitQuotedRow(signals)
                .Should().Equal(expectedFields);

            row.SplitQuotedRows(signals).Single()
                .Should().Equal(expectedFields);

            foreach (string newRow in signals.NewRows)
            {
                (row + newRow).SplitQuotedRow(signals)
                    .Should().Equal(expectedFields);

                (row + newRow).SplitQuotedRows(signals).Single()
                    .Should().Equal(expectedFields);
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