using System;

namespace HrtzCraft.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static string[] Split(this string inputString, string separator, StringSplitOptions options)
        {
            return inputString.Split(new[] { separator }, options);
        }
    }
}
