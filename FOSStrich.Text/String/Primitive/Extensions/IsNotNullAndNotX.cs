namespace FOSStrich.Text;

public static partial class StringExtensions
{
    /// <summary>
    /// Despite its location in the StringExtensions class, this is not an extension method.
    /// Instead, it is a simple static method that is the inverse of string.IsNullOrEmpty.
    /// </summary>
    /// <remarks>
    /// The primary use case for this method is as a Func argument to LINQ operators
    /// such as Where, First, Last, etc. For more concise usage, a global using such as
    /// "global using StringX = StringExtensions;" is recommended.
    /// </remarks>
    /// <example>
    /// <code>
    /// string[] values = new[] { null, string.Empty, "a", "bc" };
    /// 
    /// foreach (string value in values.Where(StringExtensions.IsNotNullAndNotEmpty))
    ///     Console.WriteLine(value);
    /// </code>
    /// Console Output:
    /// a<br/>
    /// bc<br/>
    /// </example>
    public static bool IsNotNullAndNotEmpty(string value) => !string.IsNullOrEmpty(value);

    /// <summary>
    /// Despite its location in the StringExtensions class, this is not an extension method.
    /// Instead, it is a simple static method that is the inverse of string.IsNullOrWhiteSpace.
    /// </summary>
    /// <remarks><see cref="IsNotNullAndNotEmpty(string)"/></remarks>
    public static bool IsNotNullAndNotWhiteSpace(string value) => !string.IsNullOrWhiteSpace(value);
}