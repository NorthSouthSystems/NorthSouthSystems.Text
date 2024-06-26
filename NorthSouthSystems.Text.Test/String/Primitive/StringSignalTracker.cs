namespace NorthSouthSystems.Text;

public class StringSignalTrackerTests
{
    [Fact]
    public void Empty()
    {
        IStringSignalTracker tracker;

        tracker = StringSignalTracker.Create(string.Empty);
        EmptyBase(tracker);
        tracker.Reset();
        EmptyBase(tracker);

        tracker = StringSignalTracker.Create(null);
        EmptyBase(tracker);
        tracker.Reset();
        EmptyBase(tracker);
    }

    private void EmptyBase(IStringSignalTracker tracker)
    {
        tracker.Signal.Should().BeEmpty();

        tracker.ProcessCharReturnsTriggeredLength('a').Should().Be(0);
        tracker.ProcessCharReturnsTriggeredLength('a').Should().Be(0);
    }

    [Fact]
    public void SingleChar()
    {
        IStringSignalTracker tracker;

        tracker = StringSignalTracker.Create("a");
        SingleCharBase(tracker);
        tracker.Reset();
        SingleCharBase(tracker);
    }

    private void SingleCharBase(IStringSignalTracker tracker)
    {
        tracker.Signal.Should().Be("a");

        tracker.ProcessCharReturnsTriggeredLength('b').Should().Be(0);
        tracker.ProcessCharReturnsTriggeredLength('a').Should().Be(1);
    }

    [Fact]
    public void MultiCharSimple()
    {
        IStringSignalTracker tracker;

        tracker = StringSignalTracker.Create("ab");
        MultiCharSimpleBase(tracker);
        tracker.Reset();
        MultiCharSimpleBase(tracker);
    }

    private void MultiCharSimpleBase(IStringSignalTracker tracker)
    {
        tracker.Signal.Should().Be("ab");

        tracker.ProcessCharReturnsTriggeredLength('b').Should().Be(0);
        tracker.ProcessCharReturnsTriggeredLength('a').Should().Be(0);
        tracker.ProcessCharReturnsTriggeredLength('c').Should().Be(0);
        tracker.ProcessCharReturnsTriggeredLength('a').Should().Be(0);
        tracker.ProcessCharReturnsTriggeredLength('b').Should().Be(2);
    }

    [Fact]
    public void MultiCharComplex()
    {
        IStringSignalTracker tracker;

        tracker = StringSignalTracker.Create("abac");
        MultiCharComplexBase(tracker);
        tracker.Reset();
        MultiCharComplexBase(tracker);
    }

    private void MultiCharComplexBase(IStringSignalTracker tracker)
    {
        tracker.Signal.Should().Be("abac");

        tracker.ProcessCharReturnsTriggeredLength('b').Should().Be(0);
        tracker.ProcessCharReturnsTriggeredLength('a').Should().Be(0);
        tracker.ProcessCharReturnsTriggeredLength('c').Should().Be(0);
        tracker.ProcessCharReturnsTriggeredLength('a').Should().Be(0);
        tracker.ProcessCharReturnsTriggeredLength('b').Should().Be(0);
        tracker.ProcessCharReturnsTriggeredLength('a').Should().Be(0);
        tracker.ProcessCharReturnsTriggeredLength('b').Should().Be(0);
        tracker.ProcessCharReturnsTriggeredLength('a').Should().Be(0);
        tracker.ProcessCharReturnsTriggeredLength('c').Should().Be(4);
    }
}