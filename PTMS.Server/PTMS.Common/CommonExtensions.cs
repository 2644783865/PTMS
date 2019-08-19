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

        public static string PrepareRouteName(this string routeName)
        {
            var result = routeName.ToUpper();
            result = result.Replace("ТР", "Тр.");
            result = result.Replace("..", ".");
            result = result.Replace(" ", string.Empty);

            return result;
        }
    }
}
