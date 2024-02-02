# What?
This repository hosts a .NET library containing various string utilities. Included in the library are fluent APIs for parsing and creating quoted delimited and fixed-width positional text.  For example, quoted CSV such as that used by Microsoft Excel is supported.

# How?
## Quoted Delimited Text (e.g. CSV) Splitting and Joining
**Simple Split**
{{
string row = "a,b,c";
string[]() fields = row.SplitQuotedRow(StringQuotedSignals.Csv);

foreach(string field in fields);
    Console.WriteLine(field);
}}
Console Output:
a
b
c

**Simple Join**
{{
string[]()() fields = new string[]()() { "a", "b", "c" };
string result = fields.JoinQuotedRow(StringQuotedSignals.Csv);
Console.WriteLine(result);
}}
Console Output:
a,b,c

**Quoted Split**
{{
StringQuotedSignals signals = new StringQuotedSignals(",", "'", Environment.NewLine, null);
string row = "'a,a',b,c";
string[]() fields = row.SplitQuotedRow(signals);

foreach(string field in fields);
    Console.WriteLine(field);
}}
Console Output:
a,a
b
c

**Quoted Join**
{{
string[]()() fields = new string[]()() { "a,a", "b", "c" };
string result = fields.JoinQuotedRow(StringQuotedSignals.Csv);
Console.WriteLine(result);
}}
Console Output:
"a,a",b,c
## Fixed-Width (a.k.a. Positional) Text Splitting and Joining
**Split**
{{
string row = "A-B-C";
string[]()() fields = row.SplitFixedRow(new []()() { 2, 2, 1 });

foreach(string field in fields)
    Console.WriteLine(field);

fields = row.SplitFixedRow(new []() { 2, 2, 1 }, '-');

foreach(string field in fields)
    Console.WriteLine(field);
}}
Console Output:
A-
B-
C
A
B
C

**Join**
{{
string fields = new []() { "A", "B", "C" };
string row = fields.JoinFixedRow(new []() { 2, 2, 1 });
Console.WriteLine(row);

row = fields.JoinFixedRow(new []() { 2, 2, 1 }, '-', false);
Console.WriteLine(row);
}}
Console Output:
A B C
A-B-C
## Schema Fixed-Width (a.k.a. Positional) Text Splitting and Joining
**Split**
{{
var schema = new StringSchema();
schema.AddEntry(new StringSchemaEntry("A", new[]() { 1, 1, 1 }));
schema.AddEntry(new StringSchemaEntry("B", new[]() { 2, 2, 2 }));
schema.AddEntry(new StringSchemaEntry("CD", new[]() { 3, 3, 3 }));

var split = "A123".SplitSchemaRow(schema);
Console.WriteLine(split.Entry.Header);

foreach(StringFieldWrapper field in split.Result.Fields)
    Console.WriteLine(field);

split = "B123456".SplitSchemaRow(schema);
Console.WriteLine(split.Entry.Header);

foreach(StringFieldWrapper field in split.Result.Fields)
    Console.WriteLine(field);

split = "CD123456789".SplitSchemaRow(schema);
Console.WriteLine(split.Entry.Header);

foreach(StringFieldWrapper field in split.Result.Fields)
    Console.WriteLine(field);
}}
Console Output:
A
1
2
3
B
12
34
56
CD
123
456
789

**Join**
{{
var a = new StringSchemaEntry("A", new[]() { 1, 1, 1 });
var b = new StringSchemaEntry("B", new[]() { 2, 2, 2 });
var c = new StringSchemaEntry("C", new[]() { 2, 2, 2 }, '-');

string[]()() fields = new[]()() { "1", "2", "3" };
string join = fields.JoinSchemaRow(a);
Console.WriteLine(join);

fields = new[]() { "12", "34", "56" };
join = fields.JoinSchemaRow(b);
Console.WriteLine(join);

fields = new[]() { "1", "2", "3" };
join = fields.JoinSchemaRow(c);
Console.WriteLine(join);
}}
Console Output:
A123
B123456
C1-2-3-
## Other String Utilities
**Filter**
{{
Console.WriteLine("a1b2c3d".Filter(CharFilters.None));
Console.WriteLine("a1b2c3d".Filter(CharFilters.RemoveLetters));
Console.WriteLine("a1b2c3d".Filter(CharFilters.RemoveLetters | CharFilters.RemoveDigits));
Console.WriteLine("a1b2-c3d".Filter(CharFilters.RemovePunctuation));
Console.WriteLine("a1b2-c3d".Filter(CharFilters.RemoveLetters | CharFilters.RemoveDigits));
}}
Console Output:
a1b2c3d
123

a1b2c3d
-

**NormalizeWhiteSpace**
{{
Console.WriteLine(" A  B C   D   ".NormalizeWhiteSpace());
Console.WriteLine(("Lots\tOf" + Environment.NewLine + "Changes").NormalizeWhiteSpace());
}}
Console Output:
A B C D
Lots Of
Changes

**SpaceCamelCase**
{{
Console.WriteLine("FooBarFoo FooBarFoo".SpaceCamelCase());
Console.WriteLine("123A".SpaceCamelCase());
Console.WriteLine("123a".SpaceCamelCase());
Console.WriteLine("A123".SpaceCamelCase());
Console.WriteLine("A123A".SpaceCamelCase());
}}
Console Output:
Foo Bar Foo Foo Bar Foo
123 A
123 a
A 123
A 123 A

**ToLowerCamelCase**
{{
Console.WriteLine("FooBar".ToLowerCamelCase());
}}
Console Output:
fooBar

**ToUpperCamelCase**
{{
Console.WriteLine("fooBar".ToUpperCamelCase());
}}
Console Output:
FooBar