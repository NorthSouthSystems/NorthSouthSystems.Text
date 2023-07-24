namespace FOSStrich.Text;

/// <summary>
/// Represents the Signals that determine how fields of a row (sequence of chars) are delimited
/// (separated by Delimiter), quoted (surrounded by Quote) or escaped (contain signals preceeded by Escape)
/// to allow themselves to contain an instance of Delimiter, and put onto new rows (rows separated by NewRow) in the
/// case of serialization.
/// </summary>
/// <remarks>
/// Although the functionality is referred to as StringQuoted, a Quote signal is not required; a Delimiter is the
/// only required signal. Escaping is not a feature that is mutally exclusive to Quoting; however, it can entirely
/// obsolete Quoting if the Escape and Delimiter are both single character signals. Combining Escaping and Quoting
/// can result in unexpected behavior; however, there are several rules of thumb.
/// <list type="bullet">
/// <item>
/// <description>When Splitting, Escape behaves identically both outside of and inside of Quotes.</description>   
/// </item>
/// <item>
/// <description>
/// When Joining, a Quote signal will be Escaped while inside of Quotes. If the Escape signal
/// is specified, it will be used. If not, a preceeding Quote will be used as Escape.
/// </description>
/// </item>
/// <item>
/// <description>
/// When Joining, an Escape within a field is always Escaped by a second Escape.
/// </description>
/// </item>
/// </list>
/// One other important thing to note is that Escaping multiple character signals can have unexpected results.
/// </remarks>
public sealed class StringQuotedSignals
{
    public static StringQuotedSignals Csv { get; } = new(",", "\"", Environment.NewLine, string.Empty);
    public static StringQuotedSignals Pipe { get; } = new("|", "\"", Environment.NewLine, string.Empty);
    public static StringQuotedSignals Tab { get; } = new("\t", "\"", Environment.NewLine, string.Empty);

    /// <summary>
    /// Constructor with params for all signal values.
    /// </summary>
    /// <param name="delimiter">String used to separate fields of a row. It is the only param that cannot be null or empty.</param>
    /// <param name="quote">String used to surround (or quote) a field and allow it to contain an instance of Delimiter.</param>
    /// <param name="newRow">String used to separate rows during serialization.</param>
    /// <param name="escape">String used to escape the meaning of the immediately following character.</param>
    public StringQuotedSignals(string delimiter, string quote, string newRow, string escape)
    {
        Delimiter = delimiter.NullToEmpty();
        Quote = quote.NullToEmpty();
        NewRow = newRow.NullToEmpty();
        Escape = escape.NullToEmpty();

        if (!DelimiterIsSpecified)
            throw new ArgumentException("Delimiter must be non-null and non-empty.");

        if (ContainsAny(Delimiter, Quote, NewRow, Escape) || ContainsAny(Quote, NewRow, Escape) || ContainsAny(NewRow, Escape))
            throw new ArgumentException("No parameter may be containable within any other.");
    }

    private static bool ContainsAny(string source, params string[] compares) =>
        source.Length > 0
            && compares.Where(compare => compare.Length > 0).Any(compare => source.Contains(compare) || compare.Contains(source));

    public bool DelimiterIsSpecified => !string.IsNullOrEmpty(Delimiter);
    public string Delimiter { get; }

    public bool QuoteIsSpecified => !string.IsNullOrEmpty(Quote);
    public string Quote { get; }

    public bool NewRowIsSpecified => !string.IsNullOrEmpty(NewRow);
    public string NewRow { get; }

    public bool EscapeIsSpecified => !string.IsNullOrEmpty(Escape);
    public string Escape { get; }
}