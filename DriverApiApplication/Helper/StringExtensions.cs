using System;
using System.Linq;

namespace DriverApiApplication.Helper
{
    public static class StringExtensions
    {
        public static string SortCaseInsensitive(this string str)
        {
            return new string(str.OrderBy(c => char.ToLower(c)).ToArray());
        }
    }
}
