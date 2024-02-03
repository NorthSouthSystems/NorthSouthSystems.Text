namespace NorthSouthSystems.Text;

using BenchmarkDotNet.Running;

internal class Program
{
    internal const string CatalogDataGovDirectory = @"C:\CatalogDataGov";

    private static void Main(string[] args) =>
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
}