﻿[MemoryDiagnoser]
public class B_JoinQuotedRow
{
    [GlobalSetup]
    public void GlobalSetup() =>
        _rowsFields = B_RealEstateSalesCsvs.LinuxNewLines
            .SplitQuotedRows(StringQuotedSignals.CsvNewRowLinux)
            .ToArray();

    private string[][] _rowsFields;

    [Benchmark]
    public void LinuxNewLines()
    {
        foreach (var fields in _rowsFields)
            fields.JoinQuotedRow(StringQuotedSignals.CsvNewRowLinux);
    }

    [Benchmark]
    public void WindowsNewLines()
    {
        foreach (var fields in _rowsFields)
            fields.JoinQuotedRow(StringQuotedSignals.CsvNewRowWindows);
    }
}