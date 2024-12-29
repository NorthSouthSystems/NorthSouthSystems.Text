namespace NorthSouthSystems.Text;

using MoreLinq;
using System.Text;

public class StringQuotedExtensionsTests_Fuzzing
{
    [Fact]
    public void SingleFieldSingleRow() => StringQuotedFixture.Signals.ForEach(signals =>
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
    public void MultiFieldSingleRow(int fieldCount) => StringQuotedFixture.Signals.ForEach(signals =>
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

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    public void SingleFieldMultiRows(int rowCount) =>
        StringQuotedFixture.Signals.Where(signals => signals.NewRowIsSpecified).ForEach(signals =>
        {
            var rowsBuilder = new StringBuilder();

            // We ignore string.Empty because of the Fuzzing difficulties caused by \r, \n, and \r\n
            // each representing a single NewRow in the case of IsNewRowTolerant. A string.Empty row
            // followed by a random NewRow can inadvertently create a single NewRow when two were expected.
            foreach (var permutation in SplitQuotedRawParsedFieldPair.Fuzzing(signals)
                .Where(pair => pair.Raw != string.Empty)
                .Subsets(rowCount)
                .SelectMany(subset => subset.Permutations()))
            {
                rowsBuilder.Clear();

                permutation.ForEach((pair, index) =>
                {
                    if (index > 0)
                        rowsBuilder.Append(StringQuotedFixture.Random(signals.NewRows));

                    rowsBuilder.Append(pair.Raw);
                });

                string rows = rowsBuilder.ToString();

                SplitAndAssert(rows);

                foreach (string newRow in signals.NewRows)
                    SplitAndAssert(rows + newRow);

                void SplitAndAssert(string s)
                {
                    var expectedRows = permutation.Select(pair => pair.Parsed);

                    s.SplitQuotedRows(signals).Select(split => split.Single())
                        .Should().Equal(expectedRows);
                }
            }
        });

    [Theory]
    [InlineData(3, 1.00)]
    [InlineData(4, 0.05)]
    public void MultiFieldMultiRows(int totalFieldCount, double samplingPercentage) =>
        StringQuotedFixture.Signals.Where(signals => signals.NewRowIsSpecified).ForEach(signals =>
        {
            var random = new Random(839);

            var rowsBuilder = new StringBuilder();
            var rowLengths = new List<int>();
            int rowLength;

            // See FuzzingSingleFieldRows comment about ignoring string.Empty.
            foreach (var permutation in SplitQuotedRawParsedFieldPair.Fuzzing(signals)
                .Where(pair => pair.Raw != string.Empty)
                .Subsets(totalFieldCount)
                .SelectMany(subset => subset.Permutations())
                .Where(_ => random.NextDouble() < samplingPercentage))
            {
                rowsBuilder.Clear();
                rowLengths.Clear();
                rowLength = 0;

                permutation.ForEach(pair =>
                {
                    if (rowLength > 0)
                    {
                        // Coin-flip
                        if (random.Next(2) == 0)
                        {
                            rowsBuilder.Append(StringQuotedFixture.Random(signals.NewRows));
                            rowLengths.Add(rowLength);
                            rowLength = 0;
                        }
                        else
                            rowsBuilder.Append(StringQuotedFixture.Random(signals.Delimiters));
                    }

                    rowsBuilder.Append(pair.Raw);
                    rowLength++;
                });

                rowLengths.Add(rowLength);

                string rows = rowsBuilder.ToString();

                SplitAndAssert(rows);

                foreach (string newRow in signals.NewRows)
                    SplitAndAssert(rows + newRow);

                void SplitAndAssert(string s)
                {
                    int skip = 0;

                    s.SplitQuotedRows(signals).ForEach((actualRow, index) =>
                    {
                        int expectedRowLength = rowLengths[index];
                        var expectedRow = permutation.Skip(skip).Take(expectedRowLength).Select(pair => pair.Parsed);

                        actualRow.Should().Equal(expectedRow);

                        skip += expectedRowLength;
                    });
                }
            }
        });
}