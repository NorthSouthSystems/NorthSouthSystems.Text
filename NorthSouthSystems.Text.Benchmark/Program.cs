namespace NorthSouthSystems.Text;

using BenchmarkDotNet.Running;

internal class Program
{
    internal const string CatalogDataGovDirectory = @"C:\CatalogDataGov";

    internal static readonly StringQuotedSignals LinuxNewLineCsvSignals = new(",", "\"", "\n", string.Empty);
    internal static readonly StringQuotedSignals WindowsNewLineCsvSignals = new(",", "\"", "\r\n", string.Empty);

    private static void Main(string[] args) =>
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
}