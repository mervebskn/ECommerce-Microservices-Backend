using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public static class DateTimeHelper
    {
        public static string FormatToIso8601(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        public static DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }

        public static bool IsDateInFuture(DateTime dateTime)
        {
            return dateTime > DateTime.UtcNow;
        }
    }

}
