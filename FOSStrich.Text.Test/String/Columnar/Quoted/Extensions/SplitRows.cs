namespace FOSStrich.Text;

public class StringQuotedExtensionsTests_SplitRows
{
    [Fact]
    public void Basic()
    {
        string[][] splits = string.Format("a,b,c{0}d,e,f{0}g,h,i", StringQuotedSignals.Csv.NewRow)
            .SplitQuotedRows(StringQuotedSignals.Csv)
            .ToArray();

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

        splits = string.Format("a,b,c{0}", StringQuotedSignals.Csv.NewRow)
            .SplitQuotedRows(StringQuotedSignals.Csv)
            .ToArray();

        splits.Length.Should().Be(1);

        splits[0].Length.Should().Be(3);
        splits[0][0].Should().Be("a");
        splits[0][1].Should().Be("b");
        splits[0][2].Should().Be("c");
    }

    [Fact]
    public void Exceptions()
    {
        Action act;

        act = () => ((string)null).SplitQuotedRows(StringQuotedSignals.Csv).ToArray();
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => string.Empty.SplitQuotedRows(null).ToArray();
        act.Should().ThrowExactly<ArgumentNullException>();
    }
}