using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    public static partial class StringSchemaExtensions
    {
        public static StringSchemaEntryAndColumns SplitSchemaLine(this string line, StringSchema schema)
        {
            if (line == null)
                throw new ArgumentNullException("line");

            if (schema == null)
                throw new ArgumentNullException("schema");

            StringSchemaEntry entry = schema.GetEntryForLine(line);

            using (var charEnumerator = line.Substring(entry.Header.Length).GetEnumerator())
            {
                return new StringSchemaEntryAndColumns(entry, StringFixedExtensions.SplitFixedImplementation(charEnumerator, entry.FillCharacter, entry.Widths));
            }
        }

        public static IEnumerable<StringSchemaEntryAndColumns> SplitSchemaLines(this IEnumerable<string> lines, StringSchema schema)
        {
            return lines.Select(line => SplitSchemaLine(line, schema));
        }
    }
}