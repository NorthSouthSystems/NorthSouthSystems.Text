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

        tracker.IsCounting.Should().BeFalse();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('a');
        tracker.IsCounting.Should().BeFalse();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('a');
        tracker.IsCounting.Should().BeFalse();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('a');
        tracker.IsCounting.Should().BeFalse();
        tracker.IsTriggered.Should().BeFalse();
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

        tracker.IsCounting.Should().BeFalse();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('b');
        tracker.IsCounting.Should().BeFalse();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('a');
        tracker.IsCounting.Should().BeFalse();
        tracker.IsTriggered.Should().BeTrue();
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

        tracker.IsCounting.Should().BeFalse();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('b');
        tracker.IsCounting.Should().BeFalse();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('a');
        tracker.IsCounting.Should().BeTrue();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('c');
        tracker.IsCounting.Should().BeFalse();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('a');
        tracker.IsCounting.Should().BeTrue();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('b');
        tracker.IsCounting.Should().BeFalse();
        tracker.IsTriggered.Should().BeTrue();
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

        tracker.IsCounting.Should().BeFalse();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('b');
        tracker.IsCounting.Should().BeFalse();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('a');
        tracker.IsCounting.Should().BeTrue();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('c');
        tracker.IsCounting.Should().BeFalse();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('a');
        tracker.IsCounting.Should().BeTrue();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('b');
        tracker.IsCounting.Should().BeTrue();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('a');
        tracker.IsCounting.Should().BeTrue();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('b');
        tracker.IsCounting.Should().BeTrue();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('a');
        tracker.IsCounting.Should().BeTrue();
        tracker.IsTriggered.Should().BeFalse();

        tracker.ProcessChar('c');
        tracker.IsCounting.Should().BeFalse();
        tracker.IsTriggered.Should().BeTrue();
    }

    [Fact]
    public void Exceptions()
    {
        Action act;

        act = () =>
        {
            var tracker = StringSignalTracker.Create("a");
            tracker.ProcessChar('a');
            tracker.ProcessChar(' ');
        };
        act.Should().ThrowExactly<InvalidOperationException>("ProcessCharAlreadyTriggered");
    }
}