namespace NorthSouthSystems.Text;

using MoreLinq;

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

        void SplitAndAssert(string arrangeRows, int shouldEqualRowCount)
        {
            var shouldEqualRowsOfSingleField = Enumerable.Repeat(string.Empty, shouldEqualRowCount);

            arrangeRows.SplitQuotedRows(signals).Select(fields => fields.Single())
                .Should().Equal(shouldEqualRowsOfSingleField);
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