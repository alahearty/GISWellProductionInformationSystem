using System;

namespace Orbit.Application.Extensions
{
    public static class StringExtension
    {
        public static bool IsNotNullOrWhiteSpace(this string text) => !string.IsNullOrWhiteSpace(text);
        public static bool IsMatch(this string source, string toCheck)=> source?.IndexOf(toCheck, StringComparison.CurrentCulture) >= 0;
    }
}
