using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareBotany.Ivy
{
    public static partial class StringSchemaExtensions
    {
        public static StringSchemaEntryAndStrings SplitSchemaLine(this string value, StringSchema schema)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (schema == null)
                throw new ArgumentNullException("schema");

            StringSchemaEntry entry = schema.GetEntryForValue(value);

            using (var charEnumerator = value.Substring(entry.Header.Length).GetEnumerator())
            {
                return new StringSchemaEntryAndStrings(entry, StringFixedExtensions.SplitFixedImplementation(charEnumerator, entry.FillCharacter, entry.Widths));
            }
        }

        public static IEnumerable<StringSchemaEntryAndStrings> SplitSchemaLines(this IEnumerable<string> strings, StringSchema schema)
        {
            return strings.Select(s => SplitSchemaLine(s, schema));
        }
    }
}