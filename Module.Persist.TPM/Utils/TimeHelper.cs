﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Utils
{
    public static class TimeHelper
    {
        public static DateTimeOffset ThisStartYear()
        {
            DateTimeOffset today = ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.Now);
            DateTimeOffset thisStartYear = ChangeTimeZoneUtil.ResetTimeZone(new DateTimeOffset(today.Year, 1, 1, 0, 0, 0, TimeSpan.Zero));
            return thisStartYear;
        }
        public static DateTimeOffset ThisEndYear()
        {
            DateTimeOffset today = ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.Now);
            DateTimeOffset thisStartYear = ChangeTimeZoneUtil.ResetTimeZone(new DateTimeOffset(today.Year, 12, 31, 23, 59, 59, 0, TimeSpan.Zero));
            return thisStartYear;
        }
        public static DateTimeOffset TodayStartDay()
        {
            DateTimeOffset today = ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.Now);
            DateTimeOffset thisStartYear = ChangeTimeZoneUtil.ResetTimeZone(new DateTimeOffset(today.Year, today.Month, today.Day, 0, 0, 0, TimeSpan.Zero));
            return thisStartYear;
        }
        public static DateTimeOffset TodayEndDay()
        {
            DateTimeOffset today = ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.Now);
            DateTimeOffset thisStartYear = ChangeTimeZoneUtil.ResetTimeZone(new DateTimeOffset(today.Year, today.Month, today.Day, 23, 59, 59, 999, TimeSpan.Zero));
            return thisStartYear;
        }
        public static DateTimeOffset StartDay(DateTimeOffset day)
        {
            DateTimeOffset inday = ChangeTimeZoneUtil.ResetTimeZone(day);
            DateTimeOffset thisStartYear = ChangeTimeZoneUtil.ResetTimeZone(new DateTimeOffset(inday.Year, inday.Month, inday.Day, 0, 0, 0, TimeSpan.Zero));
            return thisStartYear;
        }
        public static DateTimeOffset EndDay(DateTimeOffset day)
        {
            DateTimeOffset inday = ChangeTimeZoneUtil.ResetTimeZone(day);
            DateTimeOffset thisStartYear = ChangeTimeZoneUtil.ResetTimeZone(new DateTimeOffset(inday.Year, inday.Month, inday.Day, 23, 59, 999, TimeSpan.Zero));
            return thisStartYear;
        }
        public static DateTimeOffset GetYear(DateTimeOffset day)
        {
            DateTimeOffset today = ChangeTimeZoneUtil.ResetTimeZone(day);
            DateTimeOffset thisStartYear = ChangeTimeZoneUtil.ResetTimeZone(new DateTimeOffset(today.Year, 1, 1, 0, 0, 0, TimeSpan.Zero));
            return thisStartYear;
        }
        public static List<int> GetBudgetYears(DateTimeOffset startdispatch)
        {
            DateTimeOffset dispatchstart = ChangeTimeZoneUtil.ResetTimeZone(startdispatch);
            int month = dispatchstart.Month;
            List<int> buggetYears = new List<int>();
            buggetYears.Add(dispatchstart.Year);
            if (month == 12)
            {
                buggetYears.Add(dispatchstart.Year + 1);
            }
            if (month == 1)
            {
                buggetYears.Add(dispatchstart.Year - 1);
            }
            return buggetYears;
        }
        public static DateTimeOffset Now()
        {
            return ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.Now);
        }
        public static DateTimeOffset ThisBuggetYearStart()
        {
            return ThisStartYear().AddMonths(-1);
        }
        public static DateTimeOffset ThisBuggetYearEnd()
        {
            return ThisEndYear().AddMonths(+1);
        }
        public static DateTimeOffset NextBuggetYearStart()
        {
            return ThisStartYear().AddYears(1).AddMonths(-1);
        }
        public static DateTimeOffset NextBuggetYearEnd()
        {
            return ThisEndYear().AddYears(1).AddMonths(+1);
        }
        public static int ThisBuggetYear()
        {
            return ThisEndYear().Year;
        }
        public static int NextBuggetYear()
        {
            return ThisEndYear().AddYears(1).Year;
        }
    }
}
