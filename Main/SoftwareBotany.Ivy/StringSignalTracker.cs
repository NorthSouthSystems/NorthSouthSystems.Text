using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareBotany.Ivy
{
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
    /// Console.WriteLine(tracker.CharsProcessed);
    /// Console.WriteLine(tracker.IsCounting);
    /// Console.WriteLine(tracker.IsTriggered);
    /// 
    /// tracker.Process('a');
    /// Console.WriteLine(tracker.CharsProcessed);
    /// Console.WriteLine(tracker.IsCounting);
    /// Console.WriteLine(tracker.IsTriggered);
    /// 
    /// tracker.Process('d');
    /// Console.WriteLine(tracker.CharsProcessed);
    /// Console.WriteLine(tracker.IsCounting);
    /// Console.WriteLine(tracker.IsTriggered);
    /// 
    /// tracker.Process('a');
    /// Console.WriteLine(tracker.CharsProcessed);
    /// Console.WriteLine(tracker.IsCounting);
    /// Console.WriteLine(tracker.IsTriggered);
    /// 
    /// tracker.Process('b');
    /// Console.WriteLine(tracker.CharsProcessed);
    /// Console.WriteLine(tracker.IsCounting);
    /// Console.WriteLine(tracker.IsTriggered);
    /// 
    /// tracker.Process('c');
    /// Console.WriteLine(tracker.CharsProcessed);
    /// Console.WriteLine(tracker.IsCounting);
    /// Console.WriteLine(tracker.IsTriggered);
    /// </code>
    /// Console Output:
    /// <code>
    /// 1
    /// False
    /// False
    /// 2
    /// True
    /// False
    /// 3
    /// False
    /// False
    /// 4
    /// True
    /// False
    /// 5
    /// True
    /// False
    /// 6
    /// False
    /// True
    /// </code>
    /// </example>
    public sealed class StringSignalTracker
    {
        /// <summary>
        /// Create a StringSignalTracker for Signal.
        /// </summary>
        public StringSignalTracker(string signal)
        {
            _signal = signal.NullToEmpty();
            _activeCounters = new List<int>(_signal.Length);
        }

        public string Signal { get { return _signal; } }
        private readonly string _signal;

        private bool _triggered;
        private readonly List<int> _activeCounters;

        /// <summary>
        /// IsCounting will be true when the Tracker's state represents a partially matched Signal.
        /// </summary>
        public bool IsCounting { get { return _signal.Length > 0 && _activeCounters.Count > 0; } }

        /// <summary>
        /// IsTriggered will be true when the Signal has been matched. No further processing can take place until
        /// the Tracker has been Reset.
        /// </summary>
        public bool IsTriggered { get { return _signal.Length > 0 && _triggered; } }

        /// <summary>
        /// The number of characters processed since construction of the last call to Reset.
        /// </summary>
        public int CharsProcessed { get; private set; }

        /// <summary>
        /// Puts the Tracker in a newly created state.
        /// </summary>
        public void Reset()
        {
            _triggered = false;
            _activeCounters.Clear();

            CharsProcessed = 0;
        }

        /// <summary>
        /// Update all state based on the character to Process.
        /// </summary>
        public void ProcessChar(char value)
        {
            CharsProcessed++;

            if (_signal.Length == 0)
                return;

            if (_triggered)
                throw new InvalidOperationException("Cannot process a char when a String Signal Tracker is triggered.");

            // Iterate backwards because we will be removing from the list during the iteration.
            for (int i = _activeCounters.Count - 1; i >= 0; i--)
            {
                if (_signal[_activeCounters[i]] == value)
                    _activeCounters[i]++;
                else
                    _activeCounters.RemoveAt(i);
            }

            if (_signal[0] == value)
                _activeCounters.Add(1);

            if (_activeCounters.Any(counter => counter == _signal.Length))
            {
                _triggered = true;
                _activeCounters.Clear();
            }
        }
    }
}