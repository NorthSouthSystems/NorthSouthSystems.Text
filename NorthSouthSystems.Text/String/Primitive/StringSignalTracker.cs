﻿namespace NorthSouthSystems.Text;

/// <summary>
/// Tracks an individual signal. It maintains a List of counts of chars it has processed that have matched a signal
/// for a specific number of sequential characters.  Any count equal to the signal's length means the signal
/// has been triggered (matched).
/// </summary>
/// <remarks>
/// Performance of _activeCounters.Remove may suffer for larger signals. We may want to look into a LinkedList backing
/// structure to avoid this penalty.
/// </remarks>
/// <example>
/// <code>
/// var tracker = new StringSignalTracker("abc");
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
public sealed class StringSignalTracker
{
    /// <summary>
    /// Create a StringSignalTracker for Signal.
    /// </summary>
    public StringSignalTracker(string signal)
    {
        Signal = signal.NullToEmpty();
        _signalIsEmpty = string.IsNullOrEmpty(Signal);
        _signalIsMultiChar = Signal.Length > 1;

        if (_signalIsMultiChar)
            _activeCounters = new List<int>(Signal.Length);
    }

    public string Signal { get; }

    // These values could easily be computed; however, their precomputation is a PERF optimization noticed
    // (in DEBUG mode albeit... might have been compiled out) when profiling, and they do make the code slightly
    // more readable.
    private readonly bool _signalIsEmpty;
    private readonly bool _signalIsMultiChar;

    private bool _triggered;
    private readonly List<int> _activeCounters;

    /// <summary>
    /// IsCounting will be true when the Tracker's state represents a partially matched Signal.
    /// </summary>
    public bool IsCounting => _signalIsMultiChar && _activeCounters.Count > 0;

    /// <summary>
    /// IsTriggered will be true when the Signal has been matched. No further processing can take place until
    /// the Tracker has been Reset.
    /// </summary>
    public bool IsTriggered => _triggered;

    /// <summary>
    /// Puts the Tracker in a newly created state.
    /// </summary>
    public void Reset()
    {
        _triggered = false;

        if (_signalIsMultiChar)
            _activeCounters.Clear();
    }

    /// <summary>
    /// Update all state based on the character to Process.
    /// </summary>
    public void ProcessChar(char value)
    {
        if (_signalIsEmpty)
            return;

        if (_triggered)
            throw new InvalidOperationException("Cannot process a char when a String Signal Tracker is triggered.");

        if (!_signalIsMultiChar)
        {
            if (Signal[0] == value)
                _triggered = true;

            return;
        }

        ProcessCharSignalIsMultiChar(value);
    }

    private void ProcessCharSignalIsMultiChar(char value)
    {
        // Iterate backwards because we will be removing from the list during the iteration.
        for (int i = _activeCounters.Count - 1; i >= 0; i--)
        {
            if (Signal[_activeCounters[i]] == value)
            {
                if (_activeCounters[i] == Signal.Length - 1)
                {
                    _triggered = true;
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