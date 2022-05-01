using System;

namespace Orbit.Application.Extensions
{
    public static class DateExtension
    {
        public static DateTime EndOfMonth(this DateTime date) => new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        public static DateTime StartOfMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1);
        public static DateTime? DateOnly(this DateTime? date) => date?.Date;
        public static DateTime? DateOnly(this DateTime date) => date.Date;
        public static DateTime ToDateTime(this object date) => Convert.ToDateTime(date);
        public static string DisplayDate(this object date) => Convert.ToDateTime(date).ToString("dd MMM yyyy");
        public static string DisplayDate(this DateTime date) => date.ToString("dd MMM yyyy");

       
    }
}
