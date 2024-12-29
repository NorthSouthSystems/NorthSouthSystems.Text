﻿namespace NorthSouthSystems.Text;

public static partial class StringQuotedExtensions
{
    /// <summary>
    /// Splits a row (sequence of chars) representing delimited fields (separated by Delimiter) while allowing for instances of the
    /// Delimiter to occur within individual fields.  Such fields must be quoted (surrounded by Quote) to allow for this behavior.
    /// A NewRow signal outside of Quotes will cause an exception because multiple rows are not allowed for this method.
    /// Escaping takes precedence over all other evaluation logic. Only individual characters can be Escaped.
    /// </summary>
    /// <example>
    /// <code>
    /// string row = "a,b,c";
    /// string[] fields = row.SplitQuotedRow(StringQuotedSignals.Csv);
    /// 
    /// foreach(string field in fields);
    ///     Console.WriteLine(field);
    /// </code>
    /// Console Output:<br/>
    /// a<br/>
    /// b<br/>
    /// c<br/>
    /// <code>
    /// StringQuotedSignals signals = new StringQuotedSignals(",", "'", Environment.NewLine, null);
    /// string row = "'a,a',b,c";
    /// string[] fields = row.SplitQuotedRow(signals);
    /// 
    /// foreach(string field in fields);
    ///     Console.WriteLine(field);
    /// </code>
    /// Console Output:<br/>
    /// a,a<br/>
    /// b<br/>
    /// c<br/>
    /// <code>
    /// StringQuotedSignals signals = new StringQuotedSignals(",", "'", Environment.NewLine, null);
    /// string row = "a''a,b,c";
    /// string[] fields = row.SplitQuotedRow(signals);
    /// 
    /// foreach(string field in fields);
    ///     Console.WriteLine(field);
    /// </code>
    /// Console Output:<br/>
    /// a'a<br/>
    /// b<br/>
    /// c<br/>
    /// </example>
    public static string[] SplitQuotedRow(this IEnumerable<char> row, StringQuotedSignals signals)
    {
        if (row == null)
            throw new ArgumentNullException(nameof(row));

        if (signals == null)
            throw new ArgumentNullException(nameof(signals));

        var processor = CreateSplitQuotedProcessor(signals);
        string[][] rowsFields = processor.Process(row).Take(2).ToArray();

        if (rowsFields.Length > 1)
            throw new ArgumentException("A NewRow signal is not allowed outside of Quotes.", nameof(row));

        return rowsFields.Length == 0 ? Array.Empty<string>() : rowsFields[0];
    }

    /// <summary>
    /// Splits a row (a sequence of chars) representing delimited fields (separated by Delimiter) while allowing for instances of the
    /// Delimiter to occur within individual fields.  Such fields must be quoted (surrounded by Quote) to allow for this behavior.
    /// A NewRow signal outside of Quotes is allowed and signals that a new row has begun.
    /// Escaping takes precedence over all other evaluation logic. Only individual characters can be Escaped.
    /// </summary>
    /// <returns>A set of fields for each row in the stream as it is identified.</returns>
    /// <example>
    /// <code>
    /// string rows = "a,b,c" + Environment.NewLine + "d,e,f";
    /// var rowsFields = rows.SplitQuotedRows(StringQuotedSignals.Csv);
    /// 
    /// foreach(string[] rowFields in rowsFields)
    /// {
    ///     Console.WriteLine("Row");
    ///     
    ///     foreach(string field in rowFields)
    ///         Console.WriteLine(field);
    /// }
    /// </code>
    /// Console Output:<br/>
    /// Row<br/>
    /// a<br/>
    /// b<br/>
    /// c<br/>
    /// Row<br/>
    /// d<br/>
    /// e<br/>
    /// f<br/>
    /// </example>
    public static IEnumerable<string[]> SplitQuotedRows(this IEnumerable<char> rows, StringQuotedSignals signals)
    {
        if (rows == null)
            throw new ArgumentNullException(nameof(rows));

        if (signals == null)
            throw new ArgumentNullException(nameof(signals));

        var processor = CreateSplitQuotedProcessor(signals);
        return processor.Process(rows);
    }

    private static ISplitQuotedProcessor CreateSplitQuotedProcessor(StringQuotedSignals signals) =>
        signals.IsNewRowTolerantSimple
            ? new NewRowTolerantSimpleSplitQuotedProcessor(signals)
            : (signals.IsSimple
                ? new SimpleSplitQuotedProcessor(signals)
                : new FullSplitQuotedProcessor(signals));

    private interface ISplitQuotedProcessor { IEnumerable<string[]> Process(IEnumerable<char> rows); }
}