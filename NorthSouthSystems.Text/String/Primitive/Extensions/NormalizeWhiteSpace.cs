namespace NorthSouthSystems.Text;

public static partial class StringExtensions
{
    /// <inheritdoc cref="NormalizeWhiteSpace(IEnumerable{char}, string)"/>
    public static string NormalizeWhiteSpace(this string value) => NormalizeWhiteSpace((IEnumerable<char>)value, Environment.NewLine).ToNewString();

    /// <inheritdoc cref="NormalizeWhiteSpace(IEnumerable{char}, string)"/>
    public static string NormalizeWhiteSpace(this string value, string respectNewLine) => NormalizeWhiteSpace((IEnumerable<char>)value, respectNewLine).ToNewString();

    /// <inheritdoc cref="NormalizeWhiteSpace(IEnumerable{char}, string)"/>
    public static IEnumerable<char> NormalizeWhiteSpace(this IEnumerable<char> chars) => NormalizeWhiteSpace(chars, Environment.NewLine);

    /// <summary>
    /// Trims the front and back sides of a sequence of chars while replacing any repeating instances of whitespace with a
    /// single instance of whitespace.  Newlines consisting of more than one whitespace character (Windows Newlines = \r\n) are
    /// treated as a single whitespace instance.
    /// </summary>
    /// <param name="respectNewLine">The Newline string to treat as a single whitespace instance. (default = Environment.NewLine)</param>
    /// <example>
    /// <code>
    /// Console.WriteLine(" A  B C   D   ".NormalizeWhiteSpace());
    /// Console.WriteLine(("Lots\tOf" + Environment.NewLine + "Changes").NormalizeWhiteSpace());
    /// </code>
    /// Console Output:<br/>
    /// A B C D<br/>
    /// Lots Of<br/>
    /// Changes<br/>
    /// </example>
    public static IEnumerable<char> NormalizeWhiteSpace(this IEnumerable<char> chars, string respectNewLine)
    {
        if (chars == null)
            throw new ArgumentNullException(nameof(chars));

        if (!string.IsNullOrWhiteSpace(respectNewLine))
            throw new ArgumentException("Must only contain WhiteSpace chars.", nameof(respectNewLine));

        bool bufferedSpace = false;
        bool bufferedNewLine = false;
        var newLineTracker = new StringSignalTracker(respectNewLine);

        foreach (char c in chars.SkipWhile(char.IsWhiteSpace))
        {
            if (char.IsWhiteSpace(c))
            {
                bufferedSpace = true;
            }
            else
            {
                if (bufferedNewLine)
                {
                    foreach (char newLineChar in respectNewLine)
                        yield return newLineChar;

                    bufferedNewLine = false;
                    bufferedSpace = false;
                }

                if (bufferedSpace)
                {
                    yield return ' ';

                    bufferedSpace = false;
                }

                yield return c;
            }

            newLineTracker.ProcessChar(c);

            if (newLineTracker.IsTriggered)
            {
                newLineTracker.Reset();

                bufferedNewLine = true;
            }
        }
    }
}