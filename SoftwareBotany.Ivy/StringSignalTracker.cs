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
            Signal = signal.NullToEmpty();
            _activeCounters = new List<int>(Signal.Length);
        }

        public readonly string Signal;

        private bool _triggered;
        private readonly List<int> _activeCounters;

        public bool IsCounting { get { return Signal.Length > 0 && _activeCounters.Count > 0; } }
        public bool IsTriggered { get { return Signal.Length > 0 && _triggered; } }
        public int CharsProcessed { get; private set; }

        public void Reset()
        {
            _triggered = false;
            _activeCounters.Clear();

            CharsProcessed = 0;
        }

        public void ProcessChar(char c)
        {
            CharsProcessed++;

            if (Signal.Length == 0)
                return;

            if (_triggered)
                throw new InvalidOperationException("Cannot ProcessChar when a StringSignalTracker IsTriggered.");

            // Iterate backwards because we will be removing from the list during the iteration.
            for (int i = _activeCounters.Count - 1; i >= 0; i--)
            {
                if (Signal[_activeCounters[i]] == c)
                    _activeCounters[i]++;
                else
                    _activeCounters.RemoveAt(i);
            }

            if (Signal[0] == c)
                _activeCounters.Add(1);

            if (_activeCounters.Any(counter => counter == Signal.Length))
            {
                _triggered = true;
                _activeCounters.Clear();
            }
        }
    }
}