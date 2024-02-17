namespace NorthSouthSystems.Text;

using BenchmarkDotNet.Engines;
using System.IO;

[MemoryDiagnoser]
public class SplitQuotedRowsBenchmarks
{
    [GlobalSetup]
    public void GlobalSetup()
    {
        string realEstateSalesCsvFilepath = Path.Combine(Program.CatalogDataGovDirectory, "Real_Estate_Sales_2001-2020_GL.csv");

        _csvLinuxNewLines = File.ReadAllText(realEstateSalesCsvFilepath);

        if (_csvLinuxNewLines.Contains("\r\n"))
            throw new FormatException();

        _csvWindowsNewLines = _csvLinuxNewLines.Replace("\n", "\r\n");
    }

    private string _csvLinuxNewLines;
    private string _csvWindowsNewLines;

    private readonly Consumer _consumer = new();

    [Benchmark]
    public void LinuxNewLines() => _csvLinuxNewLines
        .SplitQuotedRows(Program.LinuxNewLineCsvSignals)
        .Consume(_consumer);

    [Benchmark]
    public void WindowsNewLines() => _csvWindowsNewLines
        .SplitQuotedRows(Program.WindowsNewLineCsvSignals)
        .Consume(_consumer);
}