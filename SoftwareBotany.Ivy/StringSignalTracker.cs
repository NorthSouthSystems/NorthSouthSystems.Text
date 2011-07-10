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
    public sealed class StringSignalTracker
    {
        public StringSignalTracker(string signal)
        {
            _signal = signal.NullToEmpty();
            _activeCounters = new List<int>(_signal.Length);
        }

        public string Signal { get { return _signal; } }
        private readonly string _signal;

        private bool _triggered;
        private readonly List<int> _activeCounters;

        public bool IsCounting { get { return _signal.Length > 0 && _activeCounters.Count > 0; } }
        public bool IsTriggered { get { return _signal.Length > 0 && _triggered; } }
        public int CharsProcessed { get; private set; }

        public void Reset()
        {
            _triggered = false;
            _activeCounters.Clear();

            CharsProcessed = 0;
        }

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