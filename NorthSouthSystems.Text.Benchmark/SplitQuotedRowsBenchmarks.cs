namespace NorthSouthSystems.Text;

using BenchmarkDotNet.Engines;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
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

    // NorthSouthSystems.Text No Mapping

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

    [Benchmark]
    public void WindowsNewLinesRFC4180() =>
        Split(_csvWindowsNewLines, StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary);

    [Benchmark]
    public void WindowsNewLinesRFC4180ForcedQuotes() =>
        Split(_csvWindowsNewLinesForcedQuotes, StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary);

    private void Split(string value, StringQuotedSignals signals) =>
        value.SplitQuotedRows(signals).Consume(_consumer);

    // NorthSouthSystems.Text With Mapping

    [Benchmark]
    public void WindowsNewLinesRFC4180WithMapping() =>
        SplitWithMapping(_csvWindowsNewLines, StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary)
            .Consume(_consumer);

    [Benchmark]
    public void WindowsNewLinesRFC4180ForcedQuotesWithMapping() =>
        SplitWithMapping(_csvWindowsNewLinesForcedQuotes, StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary)
            .Consume(_consumer);

    private IEnumerable<RealEstateSalesRecord> SplitWithMapping(string value, StringQuotedSignals signals)
    {
        // We could easily avoid using yield return and instead return the enumeration itself; however, we cannot
        // easily avoid using yield return for CsvHelper, so we are trying to keep things apples-to-apples.
        foreach (var row in value.SplitQuotedRows(signals).WithColumnHeaders())
        {
            yield return new RealEstateSalesRecord
            {
                SerialNumber = (int)row["Serial Number"],
                ListYear = (int)row["List Year"],
                DateRecorded = (DateTime?)row["Date Recorded"],
                Town = (string)row["Town"],
                Address = (string)row["Address"],
                AssessedValue = (decimal)row["Assessed Value"],
                SaleAmount = (decimal)row["Sale Amount"],
                SalesRatio = (string)row["Sales Ratio"],
                PropertyType = (string)row["Property Type"],
                ResidentialType = (string)row["Residential Type"],
                NonUseCode = (string)row["Non Use Code"],
                AssessorRemarks = (string)row["Assessor Remarks"],
                OPMRemarks = (string)row["OPM remarks"],
                Location = (string)row["Location"]
            };
        }
    }

    [Benchmark]
    public void WindowsNewLinesRFC4180ForcedQuotesWithMappingRaw() =>
        SplitWithMappingRaw(_csvWindowsNewLinesForcedQuotes, StringQuotedSignals.CsvRFC4180NewRowTolerantWindowsPrimary)
            .Consume(_consumer);

    private IEnumerable<RealEstateSalesRecordRaw> SplitWithMappingRaw(string value, StringQuotedSignals signals)
    {
        // We could easily avoid using yield return and instead return the enumeration itself; however, we cannot
        // easily avoid using yield return for CsvHelper, so we are trying to keep things apples-to-apples.
        foreach (var row in value.SplitQuotedRows(signals))
        {
            yield return new RealEstateSalesRecordRaw
            {
                SerialNumber = row[0],
                ListYear = row[1],
                DateRecorded = row[2],
                Town = row[3],
                Address = row[4],
                AssessedValue = row[5],
                SaleAmount = row[6],
                SalesRatio = row[7],
                PropertyType = row[8],
                ResidentialType = row[9],
                NonUseCode = row[10],
                AssessorRemarks = row[11],
                OPMRemarks = row[12],
                Location = row[13]
            };
        }
    }

    // CsvHelper Always Requires Mapping

    [Benchmark]
    public void CsvHelper_WindowsNewLinesRFC4180WithMapping() =>
        CsvHelperSplitWithMapping(_csvWindowsNewLines)
            .Consume(_consumer);

    [Benchmark]
    public void CsvHelper_WindowsNewLinesRFC4180ForcedQuotesWithMapping() =>
        CsvHelperSplitWithMapping(_csvWindowsNewLinesForcedQuotes)
            .Consume(_consumer);

    [Benchmark]
    public void CsvHelper_WindowsNewLinesRFC4180ForcedQuotesWithMappingAuto() =>
        CsvHelperSplitWithMappingAuto(_csvWindowsNewLinesForcedQuotes)
            .Consume(_consumer);

    private IEnumerable<RealEstateSalesRecord> CsvHelperSplitWithMapping(string value)
    {
        using var reader = new StringReader(value);
        using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

        csvReader.Read();
        csvReader.ReadHeader();

        while (csvReader.Read())
        {
            yield return new RealEstateSalesRecord
            {
                SerialNumber = csvReader.GetField<int>("Serial Number"),
                ListYear = csvReader.GetField<int>("List Year"),
                DateRecorded = csvReader.GetField<DateTime?>("Date Recorded"),
                Town = csvReader.GetField<string>("Town"),
                Address = csvReader.GetField<string>("Address"),
                AssessedValue = csvReader.GetField<decimal>("Assessed Value"),
                SaleAmount = csvReader.GetField<decimal>("Sale Amount"),
                SalesRatio = csvReader.GetField<string>("Sales Ratio"),
                PropertyType = csvReader.GetField<string>("Property Type"),
                ResidentialType = csvReader.GetField<string>("Residential Type"),
                NonUseCode = csvReader.GetField<string>("Non Use Code"),
                AssessorRemarks = csvReader.GetField<string>("Assessor Remarks"),
                OPMRemarks = csvReader.GetField<string>("OPM remarks"),
                Location = csvReader.GetField<string>("Location")
            };
        }
    }

    private IEnumerable<RealEstateSalesRecord> CsvHelperSplitWithMappingAuto(string value)
    {
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture);
        configuration.PrepareHeaderForMatch = args => args.Header.WhereIsInAnyCategory(CharCategories.Letter).ToUpperInvariant();

        using var reader = new StringReader(value);
        using var csvReader = new CsvReader(reader, configuration);

        // Must enumerate the records while inside of the using, and this also creates apples-to-apples with yield.
        foreach (var record in csvReader.GetRecords<RealEstateSalesRecord>())
            yield return record;
    }

    [Benchmark]
    public void CsvHelper_WindowsNewLinesRFC4180ForcedQuotesWithMappingRaw() =>
        CsvHelperSplitWithMappingRaw(_csvWindowsNewLinesForcedQuotes)
            .Consume(_consumer);

    [Benchmark]
    public void CsvHelper_WindowsNewLinesRFC4180ForcedQuotesWithMappingAutoRaw() =>
        CsvHelperSplitWithMappingAutoRaw(_csvWindowsNewLinesForcedQuotes)
            .Consume(_consumer);

    private IEnumerable<RealEstateSalesRecordRaw> CsvHelperSplitWithMappingRaw(string value)
    {
        using var reader = new StringReader(value);
        using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

        csvReader.Read();
        csvReader.ReadHeader();

        while (csvReader.Read())
        {
            yield return new RealEstateSalesRecordRaw
            {
                SerialNumber = csvReader.GetField(0),
                ListYear = csvReader.GetField(1),
                DateRecorded = csvReader.GetField(2),
                Town = csvReader.GetField(3),
                Address = csvReader.GetField(4),
                AssessedValue = csvReader.GetField(5),
                SaleAmount = csvReader.GetField(6),
                SalesRatio = csvReader.GetField(7),
                PropertyType = csvReader.GetField(8),
                ResidentialType = csvReader.GetField(9),
                NonUseCode = csvReader.GetField(10),
                AssessorRemarks = csvReader.GetField(11),
                OPMRemarks = csvReader.GetField(12),
                Location = csvReader.GetField(13)
            };
        }
    }

    private IEnumerable<RealEstateSalesRecordRaw> CsvHelperSplitWithMappingAutoRaw(string value)
    {
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture);
        configuration.PrepareHeaderForMatch = args => args.Header.WhereIsInAnyCategory(CharCategories.Letter).ToUpperInvariant();

        using var reader = new StringReader(value);
        using var csvReader = new CsvReader(reader, configuration);

        // Must enumerate the records while inside of the using, and this also creates apples-to-apples with yield.
        foreach (var record in csvReader.GetRecords<RealEstateSalesRecordRaw>())
            yield return record;
    }

    // Serial Number,List Year,Date Recorded,Town,Address,Assessed Value,Sale Amount,Sales Ratio,Property Type,Residential Type,Non Use Code,Assessor Remarks,OPM remarks,Location
    private class RealEstateSalesRecord
    {
        public int SerialNumber { get; set; }
        public int ListYear { get; set; }
        public DateTime? DateRecorded { get; set; }
        public string Town { get; set; }
        public string Address { get; set; }
        public decimal AssessedValue { get; set; }
        public decimal SaleAmount { get; set; }
        public string SalesRatio { get; set; }
        public string PropertyType { get; set; }
        public string ResidentialType { get; set; }
        public string NonUseCode { get; set; }
        public string AssessorRemarks { get; set; }
        public string OPMRemarks { get; set; }
        public string Location { get; set; }
    }

    private class RealEstateSalesRecordRaw
    {
        public string SerialNumber { get; set; }
        public string ListYear { get; set; }
        public string DateRecorded { get; set; }
        public string Town { get; set; }
        public string Address { get; set; }
        public string AssessedValue { get; set; }
        public string SaleAmount { get; set; }
        public string SalesRatio { get; set; }
        public string PropertyType { get; set; }
        public string ResidentialType { get; set; }
        public string NonUseCode { get; set; }
        public string AssessorRemarks { get; set; }
        public string OPMRemarks { get; set; }
        public string Location { get; set; }
    }
}