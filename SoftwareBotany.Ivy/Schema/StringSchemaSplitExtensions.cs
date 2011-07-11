using System;
using System.Collections.Generic;

namespace SoftwareBotany.Ivy
{
    public static partial class StringPositionalSplitExtensions
    {
        public static StringSchemaEntryAndStrings SplitSchema(this string value, StringSchema schema)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (schema == null)
                throw new ArgumentNullException("schema");

            StringSchemaEntry entry = schema.GetEntryForValue(value);
            return new StringSchemaEntryAndStrings(entry, SplitPositionalImplementation(value.Substring(entry.Header.Length), entry.Lengths));
        }

        public static IEnumerable<StringSchemaEntryAndStrings> SplitSchema(this IEnumerable<string> strings, StringSchema schema)
        {
            foreach (string s in strings)
                yield return SplitSchema(s, schema);
        }
    }
}