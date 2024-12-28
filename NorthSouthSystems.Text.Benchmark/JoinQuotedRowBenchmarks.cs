namespace NorthSouthSystems.Text;

[MemoryDiagnoser]
public class JoinQuotedRowBenchmarks
{
    [GlobalSetup]
    public void GlobalSetup() =>
        _rowsFields = RealEstateSalesCsvs.LinuxNewLines
            .SplitQuotedRows(Program.LinuxNewLineCsvSignals)
            .ToArray();

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