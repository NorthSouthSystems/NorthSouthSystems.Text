namespace FOSStrich.Text;

public class StringSignalTrackerTests
{
    [Fact]
    public void Empty()
    {
        StringSignalTracker tracker;

        tracker = new(string.Empty);
        EmptyBase(tracker);
        tracker.Reset();
        EmptyBase(tracker);

        tracker = new(null);
        EmptyBase(tracker);
        tracker.Reset();
        EmptyBase(tracker);
    }

    private void EmptyBase(StringSignalTracker tracker)
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
        StringSignalTracker tracker;

        tracker = new("a");
        SingleCharBase(tracker);
        tracker.Reset();
        SingleCharBase(tracker);
    }

    private void SingleCharBase(StringSignalTracker tracker)
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
        StringSignalTracker tracker;

        tracker = new("ab");
        MultiCharSimpleBase(tracker);
        tracker.Reset();
        MultiCharSimpleBase(tracker);
    }

    private void MultiCharSimpleBase(StringSignalTracker tracker)
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
        StringSignalTracker tracker;

        tracker = new("abac");
        MultiCharComplexBase(tracker);
        tracker.Reset();
        MultiCharComplexBase(tracker);
    }

    private void MultiCharComplexBase(StringSignalTracker tracker)
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
            var tracker = new StringSignalTracker("a");
            tracker.ProcessChar('a');
            tracker.ProcessChar(' ');
        };
        act.Should().Throw<InvalidOperationException>("ProcessCharAlreadyTriggered");
    }
}