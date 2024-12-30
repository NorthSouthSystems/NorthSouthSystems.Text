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
            ActAndAssert(pair.Raw, pair.Parsed);

            foreach (string delimiter in signals.Delimiters)
            {
                foreach (string newRow in signals.NewRows.DefaultIfEmpty(string.Empty))
                {
                    ActAndAssert(pair.Raw + delimiter, pair.Parsed, string.Empty);
                    ActAndAssert(delimiter + pair.Raw, string.Empty, pair.Parsed);
                    ActAndAssert(pair.Raw + newRow, pair.Parsed);
                }
            }
        }

        void ActAndAssert(string arrangeRow, params string[] shouldEqualFields)
        {
            var actualFields = arrangeRow.SplitQuotedRow(signals);
            actualFields.Should().Equal(shouldEqualFields);

            actualFields = arrangeRow.SplitQuotedRows(signals).Single();
            actualFields.Should().Equal(shouldEqualFields);

            bool shouldBeEmpty = shouldEqualFields.Length == 1 && string.IsNullOrEmpty(shouldEqualFields[0]);

            actualFields = actualFields.JoinQuotedRow(signals).SplitQuotedRow(signals);
            actualFields.Should().Equal(shouldBeEmpty ? [] : shouldEqualFields);
        }
    });

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    public void MultiFieldSingleRow(int fieldCount) => StringQuotedFixture.Signals.ForEach(signals =>
    {
        foreach (var permutation in SplitQuotedRawParsedFieldPair.Fuzzing(signals)
            .Subsets(fieldCount)
            .SelectMany(subset => subset.Permutations()))
        {
            var rowBuilder = new StringBuilder();

            permutation.ForEach((pair, index) =>
            {
                if (index > 0)
                    rowBuilder.Append(StringQuotedFixture.Random(signals.Delimiters));

                rowBuilder.Append(pair.Raw);
            });

            string row = rowBuilder.ToString();
            var expectedFields = permutation.Select(pair => pair.Parsed).ToArray();

            ActAndAssert(row, expectedFields);

            foreach (string newRow in signals.NewRows)
                ActAndAssert(row + newRow, expectedFields);
        }

        void ActAndAssert(string arrangeRow, string[] shouldEqualFields)
        {
            var actualFields = arrangeRow.SplitQuotedRow(signals);
            actualFields.Should().Equal(shouldEqualFields);

            actualFields = arrangeRow.SplitQuotedRows(signals).Single();
            actualFields.Should().Equal(shouldEqualFields);

            bool shouldBeEmpty = shouldEqualFields.Length == 1 && string.IsNullOrEmpty(shouldEqualFields[0]);

            actualFields = actualFields.JoinQuotedRow(signals).SplitQuotedRow(signals);
            actualFields.Should().Equal(shouldBeEmpty ? [] : shouldEqualFields);
        }
    });

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    public void SingleFieldMultiRows(int rowCount) =>
        StringQuotedFixture.Signals.Where(signals => signals.NewRowIsSpecified).ForEach(signals =>
        {
            // We ignore string.Empty because of the Fuzzing difficulties caused by \r, \n, and \r\n
            // each representing a single NewRow in the case of IsNewRowTolerant. A string.Empty row
            // followed by a random NewRow can inadvertently create a single NewRow when two were expected.
            foreach (var permutation in SplitQuotedRawParsedFieldPair.Fuzzing(signals)
                .Where(pair => pair.Raw != string.Empty)
                .Subsets(rowCount)
                .SelectMany(subset => subset.Permutations()))
            {
                var rowsBuilder = new StringBuilder();

                permutation.ForEach((pair, index) =>
                {
                    if (index > 0)
                        rowsBuilder.Append(StringQuotedFixture.Random(signals.NewRows));

                    rowsBuilder.Append(pair.Raw);
                });

                string rows = rowsBuilder.ToString();
                var expectedRows = permutation.Select(pair => pair.Parsed).ToArray();

                ActAndAssert(rows, expectedRows);

                foreach (string newRow in signals.NewRows)
                    ActAndAssert(rows + newRow, expectedRows);
            }

            void ActAndAssert(string arrangeRows, string[] shouldEqualRowsOfSingleField)
            {
                var actualRowsOfSingleField = arrangeRows.SplitQuotedRows(signals).Select(fields => fields.Single()).ToArray();
                actualRowsOfSingleField.Should().Equal(shouldEqualRowsOfSingleField);

                bool shouldSkipLastRow = string.IsNullOrEmpty(shouldEqualRowsOfSingleField.Last());

                actualRowsOfSingleField = string.Join(signals.NewRow, actualRowsOfSingleField.Select(field => new[] { field }.JoinQuotedRow(signals)))
                    .SplitQuotedRows(signals).Select(field => field.Single()).ToArray();

                actualRowsOfSingleField.Should().Equal(shouldSkipLastRow ? shouldEqualRowsOfSingleField.SkipLast(1) : shouldEqualRowsOfSingleField);
            }
        });

    [Theory]
    [InlineData(3, 1.00)]
    [InlineData(4, 0.05)]
    public void MultiFieldMultiRows(int totalFieldCount, double samplingPercentage) =>
        StringQuotedFixture.Signals.Where(signals => signals.NewRowIsSpecified).ForEach(signals =>
        {
            var random = new Random(839);

            // See FuzzingSingleFieldRows comment about ignoring string.Empty.
            foreach (var permutation in SplitQuotedRawParsedFieldPair.Fuzzing(signals)
                .Where(pair => pair.Raw != string.Empty)
                .Subsets(totalFieldCount)
                .SelectMany(subset => subset.Permutations())
                .Where(_ => random.NextDouble() < samplingPercentage))
            {
                var rowsBuilder = new StringBuilder();
                var expectedRows = new List<string[]>();
                var expectedRow = new List<string>();

                permutation.ForEach(pair =>
                {
                    if (expectedRow.Count > 0)
                    {
                        // Coin-flip
                        if (random.Next(2) == 0)
                        {
                            rowsBuilder.Append(StringQuotedFixture.Random(signals.NewRows));
                            expectedRows.Add(expectedRow.ToArray());
                            expectedRow.Clear();
                        }
                        else
                            rowsBuilder.Append(StringQuotedFixture.Random(signals.Delimiters));
                    }

                    rowsBuilder.Append(pair.Raw);
                    expectedRow.Add(pair.Parsed);
                });

                expectedRows.Add(expectedRow.ToArray());

                string rows = rowsBuilder.ToString();

                ActAndAssert(rows, expectedRows);

                foreach (string newRow in signals.NewRows)
                    ActAndAssert(rows + newRow, expectedRows);
            }

            void ActAndAssert(string arrangeRows, List<string[]> shouldEqualRowsOfFields)
            {
                var actualRowsOfFields = arrangeRows.SplitQuotedRows(signals).ToArray();
                actualRowsOfFields.Length.Should().Be(shouldEqualRowsOfFields.Count);

                for (int i = 0; i < actualRowsOfFields.Length; i++)
                    actualRowsOfFields[i].Should().Equal(shouldEqualRowsOfFields[i]);

                bool shouldSkipLastRow = shouldEqualRowsOfFields.Last().Length == 1 && string.IsNullOrEmpty(shouldEqualRowsOfFields.Last()[0]);

                actualRowsOfFields = string.Join(signals.NewRow, actualRowsOfFields.Select(s => s.JoinQuotedRow(signals)))
                    .SplitQuotedRows(signals).ToArray();

                actualRowsOfFields.Length.Should().Be(shouldEqualRowsOfFields.Count - (shouldSkipLastRow ? 1 : 0));

                for (int i = 0; i < actualRowsOfFields.Length; i++)
                    actualRowsOfFields[i].Should().Equal(shouldEqualRowsOfFields[i]);
            }
        });
}