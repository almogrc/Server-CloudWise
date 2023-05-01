using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Extentions
{
    public static class DatetimeExtention
    {
        public static string ToRfc3339String(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", DateTimeFormatInfo.InvariantInfo);
        }
    }
}
