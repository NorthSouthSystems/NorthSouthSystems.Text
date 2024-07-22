namespace NorthSouthSystems.Text;

internal interface IStringSignalTracker
{
    void Reset();
    int ProcessCharReturnsTriggeredLength(char value);
}

internal static class StringSignalTracker
{
    internal static IStringSignalTracker Create(params string[] signals) =>
        Create((IReadOnlyList<string>)signals);

    internal static IStringSignalTracker Create(IReadOnlyList<string> signals)
    {
        if ((signals?.Count ?? 0) == 0)
            return EmptyTracker.Singleton;
        else if (signals.Count == 1)
            return Create(signals[0]);
        else
            return new CompositeTracker(signals.OrderByDescending(s => s?.Length ?? 0).Select(Create).ToArray());
    }

    private static IStringSignalTracker Create(string signal)
    {
        if (string.IsNullOrEmpty(signal))
            return EmptyTracker.Singleton;
        else if (signal.Length == 1)
            return new SingleCharTracker(signal);
        else
            return new MultiCharTracker(signal);
    }

    private class EmptyTracker : IStringSignalTracker
    {
        internal static EmptyTracker Singleton { get; } = new();

        private EmptyTracker() { }

        public void Reset() { }

        public int ProcessCharReturnsTriggeredLength(char value) => 0;
    }

    private class SingleCharTracker : IStringSignalTracker
    {
        internal SingleCharTracker(string signal) =>
            _c = signal[0];

        private readonly char _c;

        public void Reset() { }

        public int ProcessCharReturnsTriggeredLength(char value) => _c == value ? 1 : 0;
    }

    private class MultiCharTracker : IStringSignalTracker
    {
        internal MultiCharTracker(string signal)
        {
            _signal = signal;
            _activeCounters = new List<int>(_signal.Length);
        }

        private readonly string _signal;
        private readonly List<int> _activeCounters;

        public void Reset() => _activeCounters.Clear();

        public int ProcessCharReturnsTriggeredLength(char value)
        {
            // Iterate backwards because we will be removing from the list during the iteration.
            for (int i = _activeCounters.Count - 1; i >= 0; i--)
            {
                if (_signal[_activeCounters[i]] == value)
                {
                    if (_activeCounters[i] == _signal.Length - 1)
                    {
                        _activeCounters.Clear();

                        return _signal.Length;
                    }
                    else
                        _activeCounters[i]++;
                }
                else
                    _activeCounters.RemoveAt(i);
            }

            if (_signal[0] == value)
                _activeCounters.Add(1);

            return 0;
        }
    }

    private class CompositeTracker : IStringSignalTracker
    {
        internal CompositeTracker(IStringSignalTracker[] trackersLengthDescending) =>
            _trackers = trackersLengthDescending;

        private readonly IStringSignalTracker[] _trackers;

        public void Reset()
        {
            foreach (var tracker in _trackers)
                tracker.Reset();
        }

        public int ProcessCharReturnsTriggeredLength(char value)
        {
            int length = 0;

            foreach (var tracker in _trackers)
                if ((length = tracker.ProcessCharReturnsTriggeredLength(value)) > 0)
                    return length;

            return length;
        }
    }
}