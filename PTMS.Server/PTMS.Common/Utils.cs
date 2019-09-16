using System;
using System.Linq;
using System.Security.Claims;

namespace PTMS.Common
{
    public static class Utils
    {
        public static string NaString = "-";
        public static string DateFormat = "dd.MM.yyyy";
        public static string DateTimeFormat = "dd.MM.yyyy HH:mm";

        public static string ToDateTimeString(this DateTime? dateTime)
        {
            return dateTime.HasValue ?
                dateTime.Value.ToDateTimeString()
                : NaString;
        }

        public static string ToDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString(DateTimeFormat);
        }

        public static string GetRoleName(this ClaimsPrincipal principal)
        {
            return principal.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .First();
        }
    }
}

