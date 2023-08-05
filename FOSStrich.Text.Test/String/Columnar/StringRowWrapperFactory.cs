namespace FOSStrich.Text;

public class StringRowWrapperFactoryTests
{
    [Fact]
    public void Exceptions()
    {
        Action act;

        act = () => new StringRowWrapperFactory(null);
        act.Should().Throw<ArgumentNullException>();

        act = () => new StringRowWrapperFactory(new[] { "A", "A" });
        act.Should().Throw<ArgumentException>("ConstructionDuplicateColumnNames");

        act = () => new StringRowWrapperFactory(new[] { "A", "B", "B" });
        act.Should().Throw<ArgumentException>("ConstructionDuplicateColumnNames");

        act = () => new StringRowWrapperFactory(new[] { "A", "B", "C", "A" });
        act.Should().Throw<ArgumentException>("ConstructionDuplicateColumnNames");

        act = () => new StringRowWrapperFactory(new[] { "A", "B", "C" }).Wrap(null);
        act.Should().Throw<ArgumentNullException>();

        act = () => new StringRowWrapperFactory(new[] { "A", "B", "C" }).Wrap(new[] { "1", "2", "3", "4" });
        act.Should().Throw<ArgumentException>("WrapFieldsTooMany");
    }
}