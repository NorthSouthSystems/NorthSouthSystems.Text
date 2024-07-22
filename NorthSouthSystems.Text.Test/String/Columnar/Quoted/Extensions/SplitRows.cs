namespace NorthSouthSystems.Text;

public class StringQuotedExtensionsTests_SplitRows
{
    private static string[][] Splits(string format, StringQuotedSignals signals) =>
        StringQuotedExtensionsTests_Join.Expected(format, signals)
            .SplitQuotedRows(signals)
            .ToArray();

    [Fact]
    public void Basic()
    {
        string[][] splits;

        splits = Splits(string.Empty, StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary);

        splits.Length.Should().Be(0);

        splits = Splits("{2}", StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary);

        splits.Length.Should().Be(1);
        splits[0].Length.Should().Be(1);
        splits[0][0].Should().BeEmpty();

        splits = Splits("{1}{1}", StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary);

        splits.Length.Should().Be(1);
        splits[0].Length.Should().Be(1);
        splits[0][0].Should().BeEmpty();

        splits = Splits("{1}{1}{2}", StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary);

        splits.Length.Should().Be(1);
        splits[0].Length.Should().Be(1);
        splits[0][0].Should().BeEmpty();

        splits = Splits("a,b,c", StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary);

        AssertSingleRow();

        splits = Splits("a,b,c{2}", StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary);

        AssertSingleRow();

        splits = Splits("{1}a{1},{1}b{1},{1}c{1}", StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary);

        AssertSingleRow();

        splits = Splits("{1}a{1},{1}b{1},{1}c{1}{2}", StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary);

        AssertSingleRow();

        void AssertSingleRow()
        {
            splits.Length.Should().Be(1);

            splits[0].Length.Should().Be(3);
            splits[0][0].Should().Be("a");
            splits[0][1].Should().Be("b");
            splits[0][2].Should().Be("c");
        }

        splits = Splits("a,b,c{2}d,e,f{2}g,h,i", StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary);

        AssertMultiRow();

        void AssertMultiRow()
        {
            splits.Length.Should().Be(3);

            splits[0].Length.Should().Be(3);
            splits[0][0].Should().Be("a");
            splits[0][1].Should().Be("b");
            splits[0][2].Should().Be("c");

            splits[1].Length.Should().Be(3);
            splits[1][0].Should().Be("d");
            splits[1][1].Should().Be("e");
            splits[1][2].Should().Be("f");

            splits[2].Length.Should().Be(3);
            splits[2][0].Should().Be("g");
            splits[2][1].Should().Be("h");
            splits[2][2].Should().Be("i");
        }
    }

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