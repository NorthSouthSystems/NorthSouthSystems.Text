namespace NorthSouthSystems.Text;

/// <summary>
/// Tracks an individual signal.
/// </summary>
/// <example>
/// <code>
/// var tracker = StringSignalTracker.Create("abc");
/// 
/// tracker.Process('d');
/// Console.WriteLine(tracker.IsCounting);
/// Console.WriteLine(tracker.IsTriggered);
/// Console.WriteLine();
/// 
/// tracker.Process('a');
/// Console.WriteLine(tracker.IsCounting);
/// Console.WriteLine(tracker.IsTriggered);
/// Console.WriteLine();
/// 
/// tracker.Process('d');
/// Console.WriteLine(tracker.IsCounting);
/// Console.WriteLine(tracker.IsTriggered);
/// Console.WriteLine();
/// 
/// tracker.Process('a');
/// Console.WriteLine(tracker.IsCounting);
/// Console.WriteLine(tracker.IsTriggered);
/// Console.WriteLine();
/// 
/// tracker.Process('b');
/// Console.WriteLine(tracker.IsCounting);
/// Console.WriteLine(tracker.IsTriggered);
/// Console.WriteLine();
/// 
/// tracker.Process('c');
/// Console.WriteLine(tracker.IsCounting);
/// Console.WriteLine(tracker.IsTriggered);
/// </code>
/// Console Output:<br/>
/// False<br/>
/// False<br/>
/// <br/>
/// True<br/>
/// False<br/>
/// <br/>
/// False<br/>
/// False<br/>
/// <br/>
/// True<br/>
/// False<br/>
/// <br/>
/// True<br/>
/// False<br/>
/// <br/>
/// False<br/>
/// True<br/>
/// </example>
public interface IStringSignalTracker
{
    string Signal { get; }

    /// <summary>
    /// IsCounting will be true when the Tracker's state represents a partially matched Signal.
    /// </summary>
    bool IsCounting { get; }

    /// <summary>
    /// IsTriggered will be true when the Signal has been matched. No further processing can take place until
    /// the Tracker has been Reset.
    /// </summary>
    bool IsTriggered { get; }

    /// <summary>
    /// Puts the Tracker in a newly created state.
    /// </summary>
    void Reset();

    /// <summary>
    /// Update all state based on the character to Process.
    /// </summary>
    void ProcessChar(char value);
}

public static class StringSignalTracker
{
    /// <summary>
    /// Creates an optimized IStringSignalTracker for Signal based on its Length.
    /// </summary>
    public static IStringSignalTracker Create(string signal)
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

        public bool IsCounting => false;
        public bool IsTriggered => false;

        public void Reset() { }

        public void ProcessChar(char value) { }
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

        public bool IsCounting => false;
        public bool IsTriggered { get; private set; }

        public void Reset() => IsTriggered = false;

        public void ProcessChar(char value)
        {
            if (IsTriggered)
                throw new InvalidOperationException("Cannot process a char when a String Signal Tracker is triggered.");

            if (_c == value)
                IsTriggered = true;
        }
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

        public bool IsCounting => _activeCounters.Count > 0;
        public bool IsTriggered { get; private set; }

        public void Reset()
        {
            IsTriggered = false;
            _activeCounters.Clear();
        }

        public void ProcessChar(char value)
        {
            if (IsTriggered)
                throw new InvalidOperationException("Cannot process a char when a String Signal Tracker is triggered.");

            // Iterate backwards because we will be removing from the list during the iteration.
            for (int i = _activeCounters.Count - 1; i >= 0; i--)
            {
                if (Signal[_activeCounters[i]] == value)
                {
                    if (_activeCounters[i] == Signal.Length - 1)
                    {
                        IsTriggered = true;
                        _activeCounters.Clear();
                        return;
                    }
                    else
                        _activeCounters[i]++;
                }
                else
                    _activeCounters.RemoveAt(i);
            }

            if (Signal[0] == value)
                _activeCounters.Add(1);
        }
    }
}