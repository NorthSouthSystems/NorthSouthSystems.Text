using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    public class StringPositionalSchema
    {
        public StringPositionalSchema() { }

        public StringPositionalSchema(IEnumerable<KeyValuePair<string, int[]>> entries)
        {
            foreach (KeyValuePair<string, int[]> entry in entries)
                AddEntry(entry);
        }

        private readonly Dictionary<string, int[]> _entries = new Dictionary<string, int[]>();

        public void AddEntry(KeyValuePair<string, int[]> entry)
        {
            VerifyEntry(entry);
            _entries.Add(entry.Key, entry.Value);
        }

        internal int[] this[string key] { get { return _entries[key]; } }

        internal KeyValuePair<string, int[]> GetEntryForValue(string value)
        {
            foreach (KeyValuePair<string, int[]> entry in _entries)
                if (value.StartsWith(entry.Key))
                    return entry;

            throw new ArgumentException("No matching schema definition found for value: " + value);
        }

        private void VerifyEntry(KeyValuePair<string, int[]> entry)
        {
            if (string.IsNullOrEmpty(entry.Key))
                throw new ArgumentException("Each schema.Key must be non-null and non-empty.");

            StringPositionalHelpers.SplitOrJoinPositionalVerifyLengths(entry.Value);

            foreach(KeyValuePair<string, int[]> existingEntry in _entries)
                if (existingEntry.Key.StartsWith(entry.Key) || entry.Key.StartsWith(existingEntry.Key))
                    throw new ArgumentException("No schema.Key may StartWith any other schema.Key.");
        }
    }
}