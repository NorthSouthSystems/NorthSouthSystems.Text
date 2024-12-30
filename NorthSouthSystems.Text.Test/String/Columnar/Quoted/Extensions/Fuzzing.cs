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

        void ActAndAssert(string row, params string[] expectedFields)
        {
            row.SplitQuotedRow(signals)
                .Should().Equal(expectedFields);

            row.SplitQuotedRows(signals).Single()
                .Should().Equal(expectedFields);

            bool expectEmpty = expectedFields.Length == 1 && string.IsNullOrEmpty(expectedFields[0]);

            row.SplitQuotedRow(signals).JoinQuotedRow(signals).SplitQuotedRow(signals)
                .Should().Equal(expectEmpty ? [] : expectedFields);
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

        void ActAndAssert(string row, string[] expectedFields)
        {
            row.SplitQuotedRow(signals)
                .Should().Equal(expectedFields);

            row.SplitQuotedRows(signals).Single()
                .Should().Equal(expectedFields);

            bool expectEmpty = expectedFields.Length == 1 && string.IsNullOrEmpty(expectedFields[0]);

            row.SplitQuotedRow(signals).JoinQuotedRow(signals).SplitQuotedRow(signals)
                .Should().Equal(expectEmpty ? [] : expectedFields);
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
                var expectedRowsOfSingleField = permutation.Select(pair => pair.Parsed).ToArray();

                ActAndAssert(rows, expectedRowsOfSingleField);

                foreach (string newRow in signals.NewRows)
                    ActAndAssert(rows + newRow, expectedRowsOfSingleField);
            }

            void ActAndAssert(string rows, string[] expectedRowsOfSingleField)
            {
                rows.SplitQuotedRows(signals).Select(fields => fields.Single())
                    .Should().Equal(expectedRowsOfSingleField);

                bool skipLastRow = string.IsNullOrEmpty(expectedRowsOfSingleField.Last());

                string.Join(signals.NewRow, rows.SplitQuotedRows(signals).Select(fields => fields.JoinQuotedRow(signals)))
                    .SplitQuotedRows(signals).Select(field => field.Single())
                    .Should().Equal(expectedRowsOfSingleField.SkipLast(skipLastRow ? 1 : 0));
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
                var expectedRowsOfFields = new List<string[]>();
                var expectedFields = new List<string>();

                permutation.ForEach(pair =>
                {
                    if (expectedFields.Count > 0)
                    {
                        // Coin-flip
                        if (random.Next(2) == 0)
                        {
                            rowsBuilder.Append(StringQuotedFixture.Random(signals.NewRows));
                            expectedRowsOfFields.Add(expectedFields.ToArray());
                            expectedFields.Clear();
                        }
                        else
                            rowsBuilder.Append(StringQuotedFixture.Random(signals.Delimiters));
                    }

                    rowsBuilder.Append(pair.Raw);
                    expectedFields.Add(pair.Parsed);
                });

                expectedRowsOfFields.Add(expectedFields.ToArray());

                string rows = rowsBuilder.ToString();

                ActAndAssert(rows, expectedRowsOfFields);

                foreach (string newRow in signals.NewRows)
                    ActAndAssert(rows + newRow, expectedRowsOfFields);
            }

            void ActAndAssert(string rows, List<string[]> expectedRowsOfFields)
            {
                rows.SplitQuotedRows(signals)
                    .EquiZip(expectedRowsOfFields, (fields, expectedFields) => fields.Should().Equal(expectedFields))
                    .Consume();

                bool skipLastRow = expectedRowsOfFields.Last().Length == 1 && string.IsNullOrEmpty(expectedRowsOfFields.Last()[0]);

                string.Join(signals.NewRow, rows.SplitQuotedRows(signals).Select(fields => fields.JoinQuotedRow(signals)))
                    .SplitQuotedRows(signals)
                    .EquiZip(expectedRowsOfFields.SkipLast(skipLastRow ? 1 : 0), (fields, expectedFields) => fields.Should().Equal(expectedFields))
                    .Consume();
            }
        });
}