using System;
using System.Globalization;
using System.Xml;

namespace SoftwareBotany.Ivy
{
    /// <summary>
    /// Wraps an individual field of a string row and allows for XElement style explicit type conversion
    /// of the contained field.
    /// </summary>
    public sealed class StringFieldWrapper
    {
        internal StringFieldWrapper(string columnName, string value)
        {
            _columnName = columnName;
            _value = value;
        }

        public string ColumnName { get { return _columnName; } }
        private readonly string _columnName;

        public string Value { get { return _value; } }
        private readonly string _value;

        public override string ToString() { return _value ?? string.Empty; }

        public static explicit operator string(StringFieldWrapper field)
        {
            if (field == null)
                return null;

            return field._value;
        }

        public static explicit operator bool(StringFieldWrapper field)
        {
            if (field == null || field._value == null)
                throw new ArgumentNullException("field");

            return XmlConvert.ToBoolean(field._value.ToLower(CultureInfo.InvariantCulture));
        }

        public static explicit operator bool?(StringFieldWrapper field)
        {
            if (field == null || string.IsNullOrWhiteSpace(field._value))
                return (bool?)null;

            return XmlConvert.ToBoolean(field._value.ToLower(CultureInfo.InvariantCulture));
        }

        public static explicit operator int(StringFieldWrapper field)
        {
            if (field == null || field._value == null)
                throw new ArgumentNullException("field");

            return XmlConvert.ToInt32(field._value);
        }

        public static explicit operator int?(StringFieldWrapper field)
        {
            if (field == null || string.IsNullOrWhiteSpace(field._value))
                return (int?)null;

            return XmlConvert.ToInt32(field._value);
        }

        [CLSCompliant(false)]
        public static explicit operator uint(StringFieldWrapper field)
        {
            if (field == null || field._value == null)
                throw new ArgumentNullException("field");

            return XmlConvert.ToUInt32(field._value);
        }

        [CLSCompliant(false)]
        public static explicit operator uint?(StringFieldWrapper field)
        {
            if (field == null || string.IsNullOrWhiteSpace(field._value))
                return (uint?)null;

            return XmlConvert.ToUInt32(field._value);
        }

        public static explicit operator long(StringFieldWrapper field)
        {
            if (field == null || field._value == null)
                throw new ArgumentNullException("field");

            return XmlConvert.ToInt64(field._value);
        }

        public static explicit operator long?(StringFieldWrapper field)
        {
            if (field == null || string.IsNullOrWhiteSpace(field._value))
                return (long?)null;

            return XmlConvert.ToInt64(field._value);
        }

        [CLSCompliant(false)]
        public static explicit operator ulong(StringFieldWrapper field)
        {
            if (field == null || field._value == null)
                throw new ArgumentNullException("field");

            return XmlConvert.ToUInt64(field._value);
        }

        [CLSCompliant(false)]
        public static explicit operator ulong?(StringFieldWrapper field)
        {
            if (field == null || string.IsNullOrWhiteSpace(field._value))
                return (ulong?)null;

            return XmlConvert.ToUInt64(field._value);
        }

        public static explicit operator float(StringFieldWrapper field)
        {
            if (field == null || field._value == null)
                throw new ArgumentNullException("field");

            return XmlConvert.ToSingle(field._value);
        }

        public static explicit operator float?(StringFieldWrapper field)
        {
            if (field == null || string.IsNullOrWhiteSpace(field._value))
                return (float?)null;

            return XmlConvert.ToSingle(field._value);
        }

        public static explicit operator double(StringFieldWrapper field)
        {
            if (field == null || field._value == null)
                throw new ArgumentNullException("field");

            return XmlConvert.ToDouble(field._value);
        }

        public static explicit operator double?(StringFieldWrapper field)
        {
            if (field == null || string.IsNullOrWhiteSpace(field._value))
                return (double?)null;

            return XmlConvert.ToDouble(field._value);
        }

        public static explicit operator decimal(StringFieldWrapper field)
        {
            if (field == null || field._value == null)
                throw new ArgumentNullException("field");

            return XmlConvert.ToDecimal(field._value);
        }

        public static explicit operator decimal?(StringFieldWrapper field)
        {
            if (field == null || string.IsNullOrWhiteSpace(field._value))
                return (decimal?)null;

            return XmlConvert.ToDecimal(field._value);
        }

        public static explicit operator DateTime(StringFieldWrapper field)
        {
            if (field == null || field._value == null)
                throw new ArgumentNullException("field");

            return DateTime.Parse(field._value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }

        public static explicit operator DateTime?(StringFieldWrapper field)
        {
            if (field == null || string.IsNullOrWhiteSpace(field._value))
                return (DateTime?)null;

            return DateTime.Parse(field._value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }

        public static explicit operator DateTimeOffset(StringFieldWrapper field)
        {
            if (field == null || field._value == null)
                throw new ArgumentNullException("field");

            return XmlConvert.ToDateTimeOffset(field._value);
        }

        public static explicit operator DateTimeOffset?(StringFieldWrapper field)
        {
            if (field == null || string.IsNullOrWhiteSpace(field._value))
                return (DateTimeOffset?)null;

            return XmlConvert.ToDateTimeOffset(field._value);
        }

        public static explicit operator TimeSpan(StringFieldWrapper field)
        {
            if (field == null || field._value == null)
                throw new ArgumentNullException("field");

            return XmlConvert.ToTimeSpan(field._value);
        }

        public static explicit operator TimeSpan?(StringFieldWrapper field)
        {
            if (field == null || string.IsNullOrWhiteSpace(field._value))
                return (TimeSpan?)null;

            return XmlConvert.ToTimeSpan(field._value);
        }

        public static explicit operator Guid(StringFieldWrapper field)
        {
            if (field == null || field._value == null)
                throw new ArgumentNullException("field");

            return XmlConvert.ToGuid(field._value);
        }

        public static explicit operator Guid?(StringFieldWrapper field)
        {
            if (field == null || string.IsNullOrWhiteSpace(field._value))
                return (Guid?)null;

            return XmlConvert.ToGuid(field._value);
        }
    }
}