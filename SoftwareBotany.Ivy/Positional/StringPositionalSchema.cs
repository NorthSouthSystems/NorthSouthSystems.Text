using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    public sealed class StringPositionalSchema
    {
        public StringPositionalSchema() { }

        public StringPositionalSchema(IEnumerable<StringPositionalSchemaEntry> entries)
        {
            if (entries == null)
                throw new ArgumentNullException("entries");

            foreach (StringPositionalSchemaEntry entry in entries)
                AddEntry(entry);
        }

        private readonly Dictionary<string, StringPositionalSchemaEntry> _entries = new Dictionary<string, StringPositionalSchemaEntry>();

        public void AddEntry(StringPositionalSchemaEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");

            VerifyEntry(entry);
            _entries.Add(entry.Header, entry);
        }

        internal StringPositionalSchemaEntry this[string header] { get { return _entries[header]; } }

        internal StringPositionalSchemaEntry GetEntryForValue(string value)
        {
            foreach (StringPositionalSchemaEntry entry in _entries.Values)
                if (value.StartsWith(entry.Header, StringComparison.Ordinal))
                    return entry;

            throw new ArgumentOutOfRangeException("value", value, "No matching schema definition.");
        }

        private void VerifyEntry(StringPositionalSchemaEntry entry)
        {
            foreach (StringPositionalSchemaEntry existingEntry in _entries.Values)
                if (existingEntry.Header.StartsWith(entry.Header, StringComparison.Ordinal) || entry.Header.StartsWith(existingEntry.Header, StringComparison.Ordinal))
                    throw new ArgumentOutOfRangeException("entry", entry.Header, "No entry.Header may StartWith any other existing entry.Header.");
        }
    }
}