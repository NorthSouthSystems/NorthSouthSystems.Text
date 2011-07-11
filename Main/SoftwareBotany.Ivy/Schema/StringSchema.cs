using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    public sealed class StringSchema
    {
        public StringSchema() { }

        public StringSchema(IEnumerable<StringSchemaEntry> entries)
        {
            if (entries == null)
                throw new ArgumentNullException("entries");

            foreach (StringSchemaEntry entry in entries)
                AddEntry(entry);
        }

        private readonly Dictionary<string, StringSchemaEntry> _entries = new Dictionary<string, StringSchemaEntry>();

        public void AddEntry(StringSchemaEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");

            VerifyEntry(entry);
            _entries.Add(entry.Header, entry);
        }

        internal StringSchemaEntry this[string header] { get { return _entries[header]; } }

        internal StringSchemaEntry GetEntryForValue(string value)
        {
            foreach (StringSchemaEntry entry in _entries.Values)
                if (value.StartsWith(entry.Header, StringComparison.Ordinal))
                    return entry;

            throw new ArgumentOutOfRangeException("value", value, "No matching schema definition.");
        }

        private void VerifyEntry(StringSchemaEntry entry)
        {
            foreach (StringSchemaEntry existingEntry in _entries.Values)
                if (existingEntry.Header.StartsWith(entry.Header, StringComparison.Ordinal) || entry.Header.StartsWith(existingEntry.Header, StringComparison.Ordinal))
                    throw new ArgumentOutOfRangeException("entry", entry.Header, "No entry.Header may StartWith any other existing entry.Header.");
        }
    }
}