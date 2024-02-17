namespace NorthSouthSystems.Text;

using System.IO;

[MemoryDiagnoser]
public class JoinQuotedRowBenchmarks
{
    [GlobalSetup]
    public void GlobalSetup()
    {
        string realEstateSalesCsvFilepath = Path.Combine(Program.CatalogDataGovDirectory, "Real_Estate_Sales_2001-2020_GL.csv");

        string csvLinuxNewLines = File.ReadAllText(realEstateSalesCsvFilepath);

        if (csvLinuxNewLines.Contains("\r\n"))
            throw new FormatException();

        _rowsFields = csvLinuxNewLines
            .SplitQuotedRows(Program.LinuxNewLineCsvSignals)
            .ToArray();
    }

    private string[][] _rowsFields;

    [Benchmark]
    public void LinuxNewLines()
    {
        foreach (var fields in _rowsFields)
            fields.JoinQuotedRow(Program.LinuxNewLineCsvSignals);
    }

    [Benchmark]
    public void WindowsNewLines()
    {
        foreach (var fields in _rowsFields)
            fields.JoinQuotedRow(Program.WindowsNewLineCsvSignals);
    }
}