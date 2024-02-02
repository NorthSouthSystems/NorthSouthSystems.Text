namespace NorthSouthSystems.Text;

using System.Globalization;
using System.Xml;

/// <summary>
/// Wraps an individual field of a string row and allows for XElement style explicit type conversion
/// of the contained field.
/// </summary>
public sealed class StringFieldWrapper
{
    internal StringFieldWrapper(string columnName, string value)
    {
        ColumnName = columnName;
        Value = value;
    }

    public string ColumnName { get; }
    public string Value { get; }

    public override string ToString() => Value ?? string.Empty;

    public static explicit operator string(StringFieldWrapper field)
    {
        if (field == null)
            return null;

        return field.Value;
    }

    public static explicit operator bool(StringFieldWrapper field)
    {
        if (field == null || field.Value == null)
            throw new ArgumentNullException(nameof(field));

        return XmlConvert.ToBoolean(field.Value.ToLower(CultureInfo.InvariantCulture));
    }

    public static explicit operator bool?(StringFieldWrapper field)
    {
        if (field == null || string.IsNullOrWhiteSpace(field.Value))
            return (bool?)null;

        return XmlConvert.ToBoolean(field.Value.ToLower(CultureInfo.InvariantCulture));
    }

    public static explicit operator int(StringFieldWrapper field)
    {
        if (field == null || field.Value == null)
            throw new ArgumentNullException(nameof(field));

        return XmlConvert.ToInt32(field.Value);
    }

    public static explicit operator int?(StringFieldWrapper field)
    {
        if (field == null || string.IsNullOrWhiteSpace(field.Value))
            return (int?)null;

        return XmlConvert.ToInt32(field.Value);
    }

    [CLSCompliant(false)]
    public static explicit operator uint(StringFieldWrapper field)
    {
        if (field == null || field.Value == null)
            throw new ArgumentNullException(nameof(field));

        return XmlConvert.ToUInt32(field.Value);
    }

    [CLSCompliant(false)]
    public static explicit operator uint?(StringFieldWrapper field)
    {
        if (field == null || string.IsNullOrWhiteSpace(field.Value))
            return (uint?)null;

        return XmlConvert.ToUInt32(field.Value);
    }

    public static explicit operator long(StringFieldWrapper field)
    {
        if (field == null || field.Value == null)
            throw new ArgumentNullException(nameof(field));

        return XmlConvert.ToInt64(field.Value);
    }

    public static explicit operator long?(StringFieldWrapper field)
    {
        if (field == null || string.IsNullOrWhiteSpace(field.Value))
            return (long?)null;

        return XmlConvert.ToInt64(field.Value);
    }

    [CLSCompliant(false)]
    public static explicit operator ulong(StringFieldWrapper field)
    {
        if (field == null || field.Value == null)
            throw new ArgumentNullException(nameof(field));

        return XmlConvert.ToUInt64(field.Value);
    }

    [CLSCompliant(false)]
    public static explicit operator ulong?(StringFieldWrapper field)
    {
        if (field == null || string.IsNullOrWhiteSpace(field.Value))
            return (ulong?)null;

        return XmlConvert.ToUInt64(field.Value);
    }

    public static explicit operator float(StringFieldWrapper field)
    {
        if (field == null || field.Value == null)
            throw new ArgumentNullException(nameof(field));

        return XmlConvert.ToSingle(field.Value);
    }

    public static explicit operator float?(StringFieldWrapper field)
    {
        if (field == null || string.IsNullOrWhiteSpace(field.Value))
            return (float?)null;

        return XmlConvert.ToSingle(field.Value);
    }

    public static explicit operator double(StringFieldWrapper field)
    {
        if (field == null || field.Value == null)
            throw new ArgumentNullException(nameof(field));

        return XmlConvert.ToDouble(field.Value);
    }

    public static explicit operator double?(StringFieldWrapper field)
    {
        if (field == null || string.IsNullOrWhiteSpace(field.Value))
            return (double?)null;

        return XmlConvert.ToDouble(field.Value);
    }

    public static explicit operator decimal(StringFieldWrapper field)
    {
        if (field == null || field.Value == null)
            throw new ArgumentNullException(nameof(field));

        return XmlConvert.ToDecimal(field.Value);
    }

    public static explicit operator decimal?(StringFieldWrapper field)
    {
        if (field == null || string.IsNullOrWhiteSpace(field.Value))
            return (decimal?)null;

        return XmlConvert.ToDecimal(field.Value);
    }

    public static explicit operator DateTime(StringFieldWrapper field)
    {
        if (field == null || field.Value == null)
            throw new ArgumentNullException(nameof(field));

        return DateTime.Parse(field.Value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    }

    public static explicit operator DateTime?(StringFieldWrapper field)
    {
        if (field == null || string.IsNullOrWhiteSpace(field.Value))
            return (DateTime?)null;

        return DateTime.Parse(field.Value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    }

    public static explicit operator DateTimeOffset(StringFieldWrapper field)
    {
        if (field == null || field.Value == null)
            throw new ArgumentNullException(nameof(field));

        return XmlConvert.ToDateTimeOffset(field.Value);
    }

    public static explicit operator DateTimeOffset?(StringFieldWrapper field)
    {
        if (field == null || string.IsNullOrWhiteSpace(field.Value))
            return (DateTimeOffset?)null;

        return XmlConvert.ToDateTimeOffset(field.Value);
    }

    public static explicit operator TimeSpan(StringFieldWrapper field)
    {
        if (field == null || field.Value == null)
            throw new ArgumentNullException(nameof(field));

        return XmlConvert.ToTimeSpan(field.Value);
    }

    public static explicit operator TimeSpan?(StringFieldWrapper field)
    {
        if (field == null || string.IsNullOrWhiteSpace(field.Value))
            return (TimeSpan?)null;

        return XmlConvert.ToTimeSpan(field.Value);
    }

    public static explicit operator Guid(StringFieldWrapper field)
    {
        if (field == null || field.Value == null)
            throw new ArgumentNullException(nameof(field));

        return XmlConvert.ToGuid(field.Value);
    }

    public static explicit operator Guid?(StringFieldWrapper field)
    {
        if (field == null || string.IsNullOrWhiteSpace(field.Value))
            return (Guid?)null;

        return XmlConvert.ToGuid(field.Value);
    }
}