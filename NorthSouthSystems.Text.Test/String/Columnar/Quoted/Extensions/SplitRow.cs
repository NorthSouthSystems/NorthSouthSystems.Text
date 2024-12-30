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

        void SplitAndAssert(string arrangeRow, int shouldEqualFieldCount)
        {
            var shouldEqualFields = Enumerable.Repeat(string.Empty, shouldEqualFieldCount);

            arrangeRow.SplitQuotedRow(signals).Should().Equal(shouldEqualFields);

            if (shouldEqualFieldCount > 0)
                arrangeRow.SplitQuotedRows(signals).Single().Should().Equal(shouldEqualFields);
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