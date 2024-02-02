namespace NorthSouthSystems.Text;

public class StringRowWrapperTests
{
    [Fact]
    public void Basic()
    {
        var factory = new StringRowWrapperFactory(new[] { "C0", "C1", "C2" });
        var row = factory.Wrap(new[] { "F0", "F1" });

        row[0].Value.Should().Be("F0");
        row[1].Value.Should().Be("F1");
        row[2].Value.Should().Be(null);

        row["C0"].Value.Should().Be("F0");
        row["C1"].Value.Should().Be("F1");
        row["C2"].Value.Should().Be(null);

        var fields = row.Fields.ToArray();

        fields.Length.Should().Be(3);
        fields[0].Value.Should().Be("F0");
        fields[1].Value.Should().Be("F1");
        fields[2].Value.Should().Be(null);
    }

    [Fact]
    public void Exceptions()
    {
        Action act;

        act = () =>
        {
            var factory = new StringRowWrapperFactory(new[] { "C0", "C1", "C2" });
            var row = factory.Wrap(new[] { "F0", "F1" });
            var field = row[-1];
        };
        act.Should().ThrowExactly<ArgumentOutOfRangeException>("IndexOutOfRange");

        act = () =>
        {
            var factory = new StringRowWrapperFactory(new[] { "C0", "C1", "C2" });
            var row = factory.Wrap(new[] { "F0", "F1" });
            var field = row[3];
        };
        act.Should().ThrowExactly<ArgumentOutOfRangeException>("IndexOutOfRange");

        act = () =>
        {
            var factory = new StringRowWrapperFactory(new[] { "C0", "C1", "C2" });
            var row = factory.Wrap(new[] { "F0", "F1" });
            var field = row["C3"];
        };
        act.Should().ThrowExactly<ArgumentException>("ColumnNotFound");
    }
}