using System;

namespace Orbit.Application.Extensions
{
    public static class DoubleExtension
    {
        public static double ToDouble(this double? value) => value == null ? 0 : Convert.ToDouble(value);
        public static double ConvertToBOE(this double value) => Math.Round(value / 5658.53, 2);
        public static double? ToNullableDouble(this object obj)
        {
            if (obj==null || string.IsNullOrWhiteSpace(obj.ToString())) return null;
            return Convert.ToDouble(obj);
        }
        public static double? ToNullable(this double value)
        {
            if (value > 0) return Math.Round(value, 2);
            return null;
        }
    }
}
