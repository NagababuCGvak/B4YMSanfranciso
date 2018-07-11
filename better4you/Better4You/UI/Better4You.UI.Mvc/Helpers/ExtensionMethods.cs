using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Better4You.UI.Mvc.Helpers
{
    public static class ExtensionMethods
    {
        public static DateTime AddBusinessDays(this DateTime date, int days)
        {
            if (days == 0)
                return date;

            if(date.DayOfWeek==DayOfWeek.Saturday)
            {
                date = date.AddDays(2);
                days -= 1;
            }
            else if(date.DayOfWeek ==DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
                days -= 1;
            }

            date = date.AddDays(days / 5 * 7);
            int extraDays = days % 5;

            if((int)date.DayOfWeek+extraDays>5)
            {
                extraDays += 2;
            }

            return date.AddDays(extraDays);

        }
    }
}