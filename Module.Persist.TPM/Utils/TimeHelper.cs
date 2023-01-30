using System;
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
    }
}
