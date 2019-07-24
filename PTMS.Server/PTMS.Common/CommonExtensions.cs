using System;
using System.Globalization;

namespace PTMS.Common
{
    public static class CommonExtensions
    {
        public static string ToDateString(this DateTime date)
        {
            return date.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("ru-Ru"));
        }
    }
}
