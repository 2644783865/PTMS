using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

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
