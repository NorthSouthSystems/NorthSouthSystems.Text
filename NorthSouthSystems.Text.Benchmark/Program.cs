namespace NorthSouthSystems.Text;

using BenchmarkDotNet.Running;

internal class Program
{
    internal const string CatalogDataGovDirectory = @"C:\CatalogDataGov";

    internal static readonly StringQuotedSignals LinuxNewLineCsvSignals =
        new StringQuotedSignalsBuilder()
            .Delimiter(",")
            .NewRow("\n")
            .Quote("\"")
            .ToSignals();

    internal static readonly StringQuotedSignals WindowsNewLineCsvSignals =
        new StringQuotedSignalsBuilder()
            .Delimiter(",")
            .NewRow("\r\n")
            .Quote("\"")
            .ToSignals();

    private static void Main(string[] args) =>
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
}