namespace NorthSouthSystems.Text;

internal interface IStringSignalTracker
{
    string Signal { get; }

    void Reset();
    int ProcessCharReturnsTriggeredLength(char value);
}

internal static class StringSignalTracker
{
    internal static IStringSignalTracker Create(string signal)
    {
        if (string.IsNullOrEmpty(signal))
            return _emptyTracker;
        else if (signal.Length == 1)
            return new SingleCharTracker(signal);
        else
            return new MultiCharTracker(signal);
    }

    private static readonly EmptyTracker _emptyTracker = new();

    private class EmptyTracker : IStringSignalTracker
    {
        public string Signal => string.Empty;

        public void Reset() { }

        public int ProcessCharReturnsTriggeredLength(char value) => 0;
    }

    private class SingleCharTracker : IStringSignalTracker
    {
        internal SingleCharTracker(string signal)
        {
            Signal = signal;
            _c = signal[0];
        }

        public string Signal { get; }

        // PERF - Benchmarks show a minor but noticible improvement when storing and using this.
        private readonly char _c;

        public void Reset() { }

        public int ProcessCharReturnsTriggeredLength(char value) => _c == value ? 1 : 0;
    }

    private class MultiCharTracker : IStringSignalTracker
    {
        internal MultiCharTracker(string signal)
        {
            Signal = signal;

            _activeCounters = new List<int>(Signal.Length);
        }

        public string Signal { get; }

        private readonly List<int> _activeCounters;

        public void Reset() => _activeCounters.Clear();

        public int ProcessCharReturnsTriggeredLength(char value)
        {
            // Iterate backwards because we will be removing from the list during the iteration.
            for (int i = _activeCounters.Count - 1; i >= 0; i--)
            {
                if (Signal[_activeCounters[i]] == value)
                {
                    if (_activeCounters[i] == Signal.Length - 1)
                    {
                        _activeCounters.Clear();

                        return Signal.Length;
                    }
                    else
                        _activeCounters[i]++;
                }
                else
                    _activeCounters.RemoveAt(i);
            }

            if (Signal[0] == value)
                _activeCounters.Add(1);

            return 0;
        }
    }
}