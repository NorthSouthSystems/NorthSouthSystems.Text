using System.Globalization;

public class T_StringExtensions_InterpolatedStringHandler
{
    [Fact]
    public void CurrentAndInvariant()
    {
        decimal currency = 1_234.56m;

        WithCulture("en-US", () =>
        {
            string.Current($"{currency:C2}").Should().Be("$1,234.56");
            string.Invariant($"{currency:C2}").Should().Be("¤1,234.56");
        });

        WithCulture("de-DE", () =>
        {
            string.Current($"{currency:C2}").Should().Be("1.234,56 €");
            string.Invariant($"{currency:C2}").Should().Be("¤1,234.56");
        });
    }

    // Copied from NorthSouthSystems.BCL.Opinions/Globalization/CultureInfoX.cs
    internal static void WithCulture(string name, Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        var currentCulture = CultureInfo.CurrentCulture;

        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(name);
            action();
        }
        finally
        {
            CultureInfo.CurrentCulture = currentCulture;
        }
    }
}