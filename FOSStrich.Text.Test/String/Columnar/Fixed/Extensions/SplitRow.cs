namespace FOSStrich.Text;

public class StringFixedExtensionsTests_SplitRow
{
    [Fact]
    public void Basic()
    {
        "ABC".SplitFixedRow(new[] { 1, 1, 1 })
            .Should().BeEquivalentTo(new[] { "A", "B", "C" });

        "ABC".SplitFixedRow(new[] { 1, 2 })
            .Should().BeEquivalentTo(new[] { "A", "BC" });

        "ABCD".SplitFixedRow(new[] { 1, 2, 1 })
            .Should().BeEquivalentTo(new[] { "A", "BC", "D" });

        "ABCDEF".SplitFixedRow(new[] { 2, 2, 2 })
            .Should().BeEquivalentTo(new[] { "AB", "CD", "EF" });

        "A B C ".SplitFixedRow(new[] { 2, 2, 2 })
            .Should().BeEquivalentTo(new[] { "A", "B", "C" });

        "A-B-C-".SplitFixedRow(new[] { 2, 2, 2 })
            .Should().BeEquivalentTo(new[] { "A-", "B-", "C-" });

        "A-B-C-".SplitFixedRow(new[] { 2, 2, 2 }, '-')
            .Should().BeEquivalentTo(new[] { "A", "B", "C" });

        "A B   ".SplitFixedRow(new[] { 2, 2, 2 })
            .Should().BeEquivalentTo(new[] { "A", "B", "" });

        "A-B---".SplitFixedRow(new[] { 2, 2, 2 })
            .Should().BeEquivalentTo(new[] { "A-", "B-", "--" });

        "A-B---".SplitFixedRow(new[] { 2, 2, 2 }, '-')
            .Should().BeEquivalentTo(new[] { "A", "B", "" });
    }

    [Fact]
    public void Exceptions()
    {
        Action act;

        act = () => ((string)null).SplitFixedRow(new[] { 1 });
        act.Should().Throw<ArgumentNullException>();

        act = () => string.Empty.SplitFixedRow(new[] { 1 });
        act.Should().Throw<ArgumentException>();

        act = () => "1".SplitFixedRow(new[] { 2 });
        act.Should().Throw<ArgumentOutOfRangeException>();

        act = () => "12".SplitFixedRow(new[] { 3 });
        act.Should().Throw<ArgumentOutOfRangeException>();

        act = () => "12".SplitFixedRow(new[] { 1, 2 });
        act.Should().Throw<ArgumentOutOfRangeException>();

        act = () => "1234".SplitFixedRow(new[] { 1, 2 });
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}