using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace NorthSouthSystems.Text;

public static partial class StringExtensions
{
#pragma warning disable CA1034 // False positive.
    extension(string)
    {
        public static string Current(ref DefaultInterpolatedStringHandler handler)
            => handler.ToStringAndClear();

        public static string Invariant(ref InvariantInterpolatedStringHandler handler)
            => handler.ToStringAndClear();
    }
#pragma warning restore
}

// Adapted from a conversation with ChatGPT on 2025-08-25.
[InterpolatedStringHandler]
public ref struct InvariantInterpolatedStringHandler
{
    // The compiler calls this constructor for $"..." at the callsite.
    public InvariantInterpolatedStringHandler(int literalLength, int formattedCount) =>
        _inner = new(literalLength, formattedCount, CultureInfo.InvariantCulture);

    // This MUST be a mutable struct in order for the methods to behave properly!
    private DefaultInterpolatedStringHandler _inner;

    // Declared in the same order as the documentation's Methods table:
    // https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.defaultinterpolatedstringhandler?view=net-9.0

    [ExcludeFromCodeCoverage]
    public void AppendFormatted(object? value, int alignment = 0, string? format = null) =>
        _inner.AppendFormatted(value, alignment, format);

    [ExcludeFromCodeCoverage]
    public void AppendFormatted(scoped ReadOnlySpan<char> value, int alignment = 0, string? format = null) => //NOSONAR
        _inner.AppendFormatted(value, alignment, format);

    [ExcludeFromCodeCoverage]
    public void AppendFormatted(scoped ReadOnlySpan<char> value) =>
        _inner.AppendFormatted(value);

    [ExcludeFromCodeCoverage]
    public void AppendFormatted(string? value, int alignment = 0, string? format = null) => //NOSONAR
        _inner.AppendFormatted(value, alignment, format);

    [ExcludeFromCodeCoverage]
    public void AppendFormatted(string? value) =>
        _inner.AppendFormatted(value);

    [ExcludeFromCodeCoverage]
    public void AppendFormatted<T>(T value, int alignment, string? format) =>
        _inner.AppendFormatted(value, alignment, format);

    [ExcludeFromCodeCoverage]
    public void AppendFormatted<T>(T value, int alignment) =>
        _inner.AppendFormatted(value, alignment);

    [ExcludeFromCodeCoverage]
    public void AppendFormatted<T>(T value, string? format) =>
        _inner.AppendFormatted(value, format);

    [ExcludeFromCodeCoverage]
    public void AppendFormatted<T>(T value) =>
        _inner.AppendFormatted(value);

    [ExcludeFromCodeCoverage]
    public void AppendLiteral(string value) =>
        _inner.AppendLiteral(value);

    [ExcludeFromCodeCoverage]
    public override string ToString() =>
        _inner.ToString();

    // Leaving this in code coverage as a sanity check.
    public string ToStringAndClear() =>
        _inner.ToStringAndClear();
}