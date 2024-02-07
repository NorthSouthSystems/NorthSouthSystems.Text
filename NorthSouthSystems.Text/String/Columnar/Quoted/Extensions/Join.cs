namespace NorthSouthSystems.Text;

/// <summary>
/// Extensions for Splitting and Joining delimited sequences of characters (rows) that may possess quoted (surrounded by Quote)
/// fields in order for them to contain instances of Delimiter.
/// </summary>
public static partial class StringQuotedExtensions
{
    /// <summary>
    /// Joins a sequence of fields, separates them with Delimiter, and allows for instances of Delimiter (or the NewRow signal)
    /// to occur within individual fields.  Such fields will be quoted (surrounded by Quote) to allow for this behavior. Instances
    /// of the Quote signal within fields will be escaped by doubling (Quote + Quote).
    /// </summary>
    /// <param name="forceQuotes">
    /// Dictates whether to force every field to be quoted regardless of whether or not the field contains an instance
    /// of Delimiter or NewRow. (default = false)
    /// </param>
    /// <example>
    /// <code>
    /// string[] fields = new string[] { "a", "b", "c" };
    /// string result = fields.JoinQuotedRow(StringQuotedSignals.Csv, false);
    /// Console.WriteLine(result);
    /// </code>
    /// Console Output:<br/>
    /// a,b,c<br/>
    /// <code>
    /// string[] fields = new string[] { "a,a", "b", "c" };
    /// string result = fields.JoinQuotedRow(StringQuotedSignals.Csv, false);
    /// Console.WriteLine(result);
    /// </code>
    /// Console Output:<br/>
    /// "a,a",b,c<br/>
    /// <code>
    /// string[] fields = new string[] { "a", "b" + Environment.NewLine + "b", "c" };
    /// string result = fields.JoinQuotedRow(StringQuotedSignals.Csv, true);
    /// Console.WriteLine(result);
    /// </code>
    /// Console Output:<br/>
    /// "a","b<br/>
    /// b","c"<br/>
    /// <code>
    /// string[] fields = new string[] { "a\"a", "b", "c" };
    /// string result = fields.JoinQuotedRow(StringQuotedSignals.Csv, false);
    /// Console.WriteLine(result);
    /// </code>
    /// Console Output:<br/>
    /// a""a,b,c<br/>
    /// </example>
    public static string JoinQuotedRow(this IEnumerable<string> fields, StringQuotedSignals signals, bool forceQuotes = false)
    {
        if (fields == null)
            throw new ArgumentNullException(nameof(fields));

        if (signals == null)
            throw new ArgumentNullException(nameof(signals));

        if (forceQuotes && !signals.QuoteIsSpecified)
            throw new ArgumentException("Quote'ing forced; therefore, signals.Quote must not be null or empty.");

        return string.Join(signals.Delimiter, fields.Select(field => QuoteAndEscapeField(field ?? string.Empty, signals, forceQuotes)));
    }

    private static string QuoteAndEscapeField(string field, StringQuotedSignals signals, bool forceQuotes)
    {
        if (ContainsEscape())
            field = field.Replace(signals.Escape, signals.Escape + signals.Escape);

        if (forceQuotes || signals.QuoteIsSpecified)
        {
            if (ContainsQuote())
                field = signals.Quote + field.Replace(signals.Quote, (signals.EscapeIsSpecified ? signals.Escape : signals.Quote) + signals.Quote) + signals.Quote;
            else if (forceQuotes || ContainsDelimiter() || ContainsNewRow())
                field = signals.Quote + field + signals.Quote;
        }
        else if (signals.EscapeIsSpecified)
        {
            if (ContainsDelimiter())
                field = field.Replace(signals.Delimiter, signals.Escape + signals.Delimiter);

            if (ContainsNewRow())
                field = field.Replace(signals.NewRow, signals.Escape + signals.NewRow);
        }
        else if (ContainsDelimiter() || ContainsNewRow())
            throw new ArgumentException("Quoting or Escaping is required; therefore, either signals.Quote or signals.Escape must not be null or empty.");

        return field;

        bool ContainsDelimiter() => field.Contains(signals.Delimiter);
        bool ContainsQuote() => signals.QuoteIsSpecified && field.Contains(signals.Quote);
        bool ContainsNewRow() => signals.NewRowIsSpecified && field.Contains(signals.NewRow);
        bool ContainsEscape() => signals.EscapeIsSpecified && field.Contains(signals.Escape);
    }
}