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

        _csvLinuxNewLines = File.ReadAllText(realEstateSalesCsvFilepath).TrimEnd();

        if (_csvLinuxNewLines.Contains("\r\n"))
            throw new FormatException();

        _csvLinuxNewLinesForcedQuotes = ReJoin(Program.LinuxNewLineCsvSignals, true);

        _csvWindowsNewLines = ReJoin(Program.WindowsNewLineCsvSignals, false);
        _csvWindowsNewLinesForcedQuotes = ReJoin(Program.WindowsNewLineCsvSignals, true);

        string ReJoin(StringQuotedSignals signals, bool forceQuotes) =>
            string.Join(signals.NewRow,
                _csvLinuxNewLines.SplitQuotedRows(Program.LinuxNewLineCsvSignals)
                    .Select(row => row.JoinQuotedRow(signals, forceQuotes)));
    }

    private string _csvLinuxNewLines;
    private string _csvLinuxNewLinesForcedQuotes;

    private string _csvWindowsNewLines;
    private string _csvWindowsNewLinesForcedQuotes;

    private readonly Consumer _consumer = new();

    [Benchmark]
    public void LinuxNewLines() =>
        Split(_csvLinuxNewLines, Program.LinuxNewLineCsvSignals);

    [Benchmark]
    public void LinuxNewLinesForcedQuotes() =>
        Split(_csvLinuxNewLinesForcedQuotes, Program.LinuxNewLineCsvSignals);

    [Benchmark]
    public void WindowsNewLines() =>
        Split(_csvWindowsNewLines, Program.WindowsNewLineCsvSignals);

    [Benchmark]
    public void WindowsNewLinesForcedQuotes() =>
        Split(_csvWindowsNewLinesForcedQuotes, Program.WindowsNewLineCsvSignals);

    private void Split(string value, StringQuotedSignals signals) =>
        value.SplitQuotedRows(signals).Consume(_consumer);
}