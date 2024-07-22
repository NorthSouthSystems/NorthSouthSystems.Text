namespace NorthSouthSystems.Text;

using System.Runtime.CompilerServices;

public sealed class StringQuotedSignalsBuilder
{
    /// <summary>
    /// String used to separate fields of a row. It is the only required Signal.
    /// </summary>
    /// <param name="primary">Used for Splitting and Joining.</param>
    /// <param name="alternates">Used only for Splitting.</param>
    public StringQuotedSignalsBuilder Delimiter(string primary, params string[] alternates) =>
        MultiHelper(() => _delimiters, x => _delimiters = x, primary, alternates);

    private string[] _delimiters;

    public StringQuotedSignalsBuilder NewRowTolerantEnvironmentPrimary() => Environment.NewLine switch
    {
        "\n" => NewRowTolerantLinuxPrimary(),
        "\r\n" => NewRowTolerantWindowsPrimary(),
        "\r" => NewRow("\r", "\n"),
        _ => NewRow(Environment.NewLine)
    };

    public StringQuotedSignalsBuilder NewRowTolerantLinuxPrimary() => NewRow("\n", "\r\n");

    public StringQuotedSignalsBuilder NewRowTolerantWindowsPrimary() => NewRow("\r\n", "\n");

    /// <summary>
    /// String used to separate rows.
    /// </summary>
    /// <param name="primary">Used for Splitting and Joining.</param>
    /// <param name="alternates">Used only for Splitting.</param>
    public StringQuotedSignalsBuilder NewRow(string primary, params string[] alternates) =>
        MultiHelper(() => _newRows, x => _newRows = x, primary, alternates);

    private string[] _newRows;

    /// <summary>
    /// String used to surround (i.e. quote) a field and allow it to contain an instance of Delimiter or NewRow.
    /// </summary>
    public StringQuotedSignalsBuilder Quote(string signal) =>
        Helper(() => _quote, x => _quote = x, signal);

    private string _quote;

    /// <summary>
    /// String used to escape the meaning of the immediately following character.
    /// </summary>
    public StringQuotedSignalsBuilder Escape(string signal) =>
        Helper(() => _escape, x => _escape = x, signal);

    private string _escape;

    private StringQuotedSignalsBuilder MultiHelper(Func<string[]> getter, Action<string[]> setter,
        string primary, string[] alternates, [CallerMemberName] string callerMemberName = null)
    {
        var multi = alternates.Prepend(primary).Where(StringExtensions.IsNotNullAndNotEmpty).Distinct().ToArray();

        // We know that all strings are NotNull, NotEmpty (Length >= 1), and Distinct by the LINQ above. We also know that
        // AnyPermutationPair sorts from longest to shortest string. We leverage both facts here when we ensure that no signals
        // are impossible to trigger by being "hidden" by other "nested" signals.
        if (StringExtensions.AnyPermutationPair(multi, (x, y) => x.Length > y.Length && x.Substring(0, x.Length - 1).Contains(y)))
            throw new ArgumentException($"{callerMemberName} may only EndsWith another {callerMemberName}. All other StartsWith or Contains are invalid.");

        return Helper(getter, setter, multi);
    }

    private StringQuotedSignalsBuilder Helper<T>(Func<T> getter, Action<T> setter, T multiOrSignal)
    {
        var rollback = getter();

        setter(multiOrSignal);

        if (AnySignalContainsAnyOther())
        {
            setter(rollback);
            throw new ArgumentException("No Signal may Contains any other Signal.");
        }

        return this;
    }

    private bool AnySignalContainsAnyOther()
    {
        foreach (var delimiter in (_delimiters ?? []).DefaultIfEmpty())
            foreach (var newRow in (_newRows ?? []).DefaultIfEmpty())
                if (StringExtensions.AnyPermutationPairContains(new[] { delimiter, newRow, _quote, _escape }.Where(StringExtensions.IsNotNullAndNotEmpty)))
                    return true;

        return false;
    }

    public StringQuotedSignals ToSignals()
    {
        if ((_delimiters?.Length ?? 0) == 0)
            throw new ArgumentException("Delimiter must be non-null and non-empty.");

        return new(_delimiters, _newRows, _quote, _escape);
    }
}