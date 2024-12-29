namespace NorthSouthSystems.Text;

using MoreLinq;
using System.Text;

public class StringQuotedExtensionsTests_SplitRows
{
    [Fact]
    public void EmptyRows() => StringQuotedFixture.Signals.ForEach(signals =>
    {
        SplitAndAssert(string.Empty, 0);

        foreach (string newRow in signals.NewRows)
        {
            SplitAndAssert(newRow, 1);
            SplitAndAssert(newRow + newRow, 2);
            SplitAndAssert(newRow + newRow + newRow, 3);

            if (signals.QuoteIsSpecified)
            {
                string quotedEmpty = signals.Quote + signals.Quote;

                SplitAndAssert(quotedEmpty, 1);
                SplitAndAssert(quotedEmpty + newRow, 1);

                SplitAndAssert(newRow + quotedEmpty, 2);
                SplitAndAssert(newRow + quotedEmpty + newRow, 2);

                SplitAndAssert(newRow + quotedEmpty + newRow + newRow, 3);
                SplitAndAssert(newRow + quotedEmpty + newRow + quotedEmpty, 3);
            }
        }

        void SplitAndAssert(string rows, int rowCount)
        {
            var expectedRows = Enumerable.Repeat(string.Empty, rowCount);

            rows.SplitQuotedRows(signals).Select(split => split.Single())
                .Should().Equal(expectedRows);
        }
    });

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    public void FuzzingSingleFieldRows(int rowCount) => StringQuotedFixture.Signals.Where(signals => signals.NewRowIsSpecified).ForEach(signals =>
    {
        var rowsBuilder = new StringBuilder();

        // We ignore string.Empty because of the Fuzzing difficulties caused by \r, \n, and \r\n
        // each representing a single NewRow in the case of IsNewRowTolerant. A string.Empty row
        // followed by a random NewRow can inadvertently create a single NewRow when two were expected.
        foreach (var permutation in StringQuotedRawParsedFieldPair.Fuzzing(signals)
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
            var expectedRows = permutation.Select(pair => pair.Parsed);

            rows.SplitQuotedRows(signals).Select(split => split.Single())
                .Should().Equal(expectedRows);

            foreach (string newRow in signals.NewRows)
            {
                (rows + newRow).SplitQuotedRows(signals).Select(split => split.Single())
                    .Should().Equal(expectedRows);
            }
        }
    });

    [Fact]
    public void Exceptions()
    {
        Action act;

        act = () => ((string)null).SplitQuotedRows(StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary).ToArray();
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => string.Empty.SplitQuotedRows(null).ToArray();
        act.Should().ThrowExactly<ArgumentNullException>();
    }
}