using System;
using System.Globalization;

namespace Better4You.Core
{
    public static class DateTimeExtensions
    {
        static readonly GregorianCalendar Gc = new GregorianCalendar();
        public static int GetWeekOfMonth(this DateTime time)
        {
            var first = new DateTime(time.Year, time.Month, 1);
            return time.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }

        static int GetWeekOfYear(this DateTime time)
        {
            return Gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }
    }
}
