using System;
using System.Collections.Generic;
using System.Linq;

namespace Orbit.Application.Extensions
{
    public static class CollectionsExtension
    {
        public static double Aggregate(this IEnumerable<double?> data) => Math.Round(Convert.ToDouble(data.Sum()), 2);
        public static double Aggregate(this IEnumerable<double> data) => Math.Round(data.Sum(), 2);
        public static double Aggregate(this IEnumerable<object> data) => Math.Round(data.ToListOfDouble().Sum(), 2);
        public static double AggregateByMillion(this IEnumerable<double?> data) => Math.Round(Convert.ToDouble(data.Sum() / 1000000), 2);
        public static double AggregateByThousand(this IEnumerable<double?> data) => Math.Round(Convert.ToDouble(data.Sum() / 1000), 2);
        public static IEnumerable<DateTime> ConvertToDateTime(this IEnumerable<object> dates) => dates.Select(x => Convert.ToDateTime(x));
        public static IEnumerable<DateTime> ToEndOfMonthDateTime(this IEnumerable<object> dates) => dates.Select(x => Convert.ToDateTime(x).EndOfMonth());
        public static IEnumerable<DateTime> ToEndOfMonthDateTime(this IEnumerable<DateTime?> dates) => dates.Select(x => Convert.ToDateTime(x).EndOfMonth());
        public static IEnumerable<double?> ToNullableDouble(this IEnumerable<object> results) => results.Select(x => x.ToNullableDouble());
        public static IEnumerable<double> ToListOfDouble(this IEnumerable<object> results) => results.Select(x => Convert.ToDouble(x));
        public static IEnumerable<double> ConvertToBOE(this IEnumerable<object> results) => results.Select(x => Convert.ToDouble(x) / 5658.53);
        public static IEnumerable<double> ConvertToBOE(this IEnumerable<double?> results) => results.Select(x => Convert.ToDouble(x) / 5658.53);
        public static IEnumerable<double> ConvertToBOE(this IEnumerable<double> results) => results.Select(x => x / 5658.53);
        public static bool NotAny(this IEnumerable<object> data) => !data.Any();
        public static bool NotAny(this IEnumerable<double> data) => !data.Any();
        public static bool NotAny(this IEnumerable<double?> data) => !data.Any();
        public static bool NotAny(this IEnumerable<DateTime?> data) => !data.Any();
        public static bool NotAny(this IEnumerable<DateTime> data) => !data.Any();
        public static double ToAverage(this IEnumerable<double?> data)
        {
            if (data.NotAny()) return 0;
            var result = data.ToListOfDouble();
            if (result.NotAny()) return 0;
            return Math.Round(result.Average(), 0);
        }
        public static double ToAverageBOE(this IEnumerable<double?> data) 
        {
             if (data.NotAny()) return 0;
            var result = data.ToListOfDouble();
            if (result.NotAny()) return 0;
           return  Math.Round(result.Average() / 5658.53, 0);
        }
        public static double AverageByMillion(this IEnumerable<double?> data)
        {
            if (data.NotAny()) return 0;
            var result = data.ToListOfDouble();
            if (result.NotAny()) return 0;
            return Math.Round(result.Average() / 1000000, 0);
        }
        public static double AverageByThousand(this IEnumerable<double?> data)
        {
            if (data.NotAny()) return 0;
            var result = data.ToListOfDouble();
            if (result.NotAny()) return 0;
            return Math.Round(result.Average() / 1000, 0);
        }
        public static IEnumerable<double> ToListOfDouble(this IEnumerable<double?> values)
        {
            var list = new List<double>();
            foreach (var value in values)
            {
                if (value != null)
                    list.Add((double)value);
            }
            return list;
        }

    }
}
