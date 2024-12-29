namespace NorthSouthSystems.Text;

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
    public static StringQuotedSignals CsvRFC4180NewRowTolerantWindowsPrimary { get; } =
        new StringQuotedSignalsBuilder()
            .Delimiter(",")
            .NewRowTolerantWindowsPrimary()
            .Quote("\"")
            .ToSignals();

    private static readonly IEnumerable<string> _newRowsTolerant = ["\n", "\r", "\r\n"];

    internal StringQuotedSignals(string[] delimiters, string[] newRows, string quote, string escape)
    {
        _delimiters = (delimiters ?? []).Where(StringExtensions.IsNotNullAndNotEmpty).ToArray();
        Delimiter = _delimiters.FirstOrDefault().NullToEmpty();

        if (!DelimiterIsSpecified)
            throw new ArgumentException("Delimiter must be non-null and non-empty.");

        _newRows = (newRows ?? []).Where(StringExtensions.IsNotNullAndNotEmpty).ToArray();
        NewRow = _newRows.FirstOrDefault().NullToEmpty();

        Quote = quote.NullToEmpty();
        Escape = escape.NullToEmpty();

        EscapedDelimiter = Escape + Delimiter;
        EscapedNewRow = Escape + NewRow;
        EscapedQuote = (EscapeIsSpecified ? Escape : Quote) + Quote;
        EscapedEscape = Escape + Escape;
    }

    public bool DelimiterIsSpecified => !string.IsNullOrEmpty(Delimiter);
    public string Delimiter { get; }
    public IReadOnlyList<string> Delimiters => _delimiters;
    private readonly string[] _delimiters;

    public bool NewRowIsSpecified => !string.IsNullOrEmpty(NewRow);
    public string NewRow { get; }
    public IReadOnlyList<string> NewRows => _newRows;
    private readonly string[] _newRows;

    internal bool IsNewRowTolerant =>
        _newRows?.Length == 3 && _newRows.All(_newRowsTolerant.Contains);

    public bool QuoteIsSpecified => !string.IsNullOrEmpty(Quote);
    public string Quote { get; }

    public bool EscapeIsSpecified => !string.IsNullOrEmpty(Escape);
    public string Escape { get; }

    internal bool IsSimple =>
        (Delimiters.Count == 1 && Delimiter.Length == 1)
        && (!NewRowIsSpecified || (NewRows.Count == 1 && NewRow.Length == 1))
        && (!QuoteIsSpecified || Quote.Length == 1)
        && (!EscapeIsSpecified || Escape.Length == 1);

    internal bool IsNewRowTolerantSimple =>
        (Delimiters.Count == 1 && Delimiter.Length == 1)
        && IsNewRowTolerant
        && (!QuoteIsSpecified || Quote.Length == 1)
        && (!EscapeIsSpecified || Escape.Length == 1);

    internal string EscapedDelimiter { get; }
    internal string EscapedNewRow { get; }
    internal string EscapedQuote { get; }
    internal string EscapedEscape { get; }
}