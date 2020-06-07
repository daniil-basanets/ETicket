using System;

namespace ETicket.ApplicationServices.Extensions
{
    public static class StringExtensions 
    {
        public static DateTime? ParseToDate(this string str)
        {
            return DateTime.TryParse(str, out var value) ? value.Date : (DateTime?)null;
        }

        public static bool? ParseToBoolean(this string str)
        {
            return Boolean.TryParse(str, out var value) ? value : (bool?)null;
        }
    }
}
