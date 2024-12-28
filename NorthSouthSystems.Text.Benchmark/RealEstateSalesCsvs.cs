namespace NorthSouthSystems.Text;

using System.IO;

internal static class RealEstateSalesCsvs
{
    static RealEstateSalesCsvs()
    {
        string filepath = Path.Combine(Program.CatalogDataGovDirectory, "Real_Estate_Sales_2001-2020_GL.csv");

        LinuxNewLines = File.ReadAllText(filepath).TrimEnd();

        if (LinuxNewLines.Contains("\r\n"))
            throw new FormatException();

        LinuxNewLinesForcedQuotes = ReJoin(Program.LinuxNewLineCsvSignals, true);

        WindowsNewLines = ReJoin(Program.WindowsNewLineCsvSignals, false);
        WindowsNewLinesForcedQuotes = ReJoin(Program.WindowsNewLineCsvSignals, true);

        string ReJoin(StringQuotedSignals signals, bool forceQuotes) =>
            string.Join(signals.NewRow,
                LinuxNewLines.SplitQuotedRows(Program.LinuxNewLineCsvSignals)
                    .Select(row => row.JoinQuotedRow(signals, forceQuotes)));
    }

    internal static bool IsInitialized =>
        new[] { LinuxNewLines, LinuxNewLinesForcedQuotes, WindowsNewLines, WindowsNewLinesForcedQuotes }
            .All(StringExtensions.IsNotNullAndNotEmpty);

    internal static string LinuxNewLines { get; }
    internal static string LinuxNewLinesForcedQuotes { get; }
    internal static string WindowsNewLines { get; }
    internal static string WindowsNewLinesForcedQuotes { get; }
}