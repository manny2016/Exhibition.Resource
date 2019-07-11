
namespace Exhibition.Core
{
    using System;
    public static class DateTimeExtension
    {
        public static readonly DateTime UNIX_START_DATE = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static readonly long STICK_HOURLY = 60 * 60;
        public static readonly string PSTZoneId = "Pacific Standard Time";
        public static long ToUnixStampDateTime(this DateTime dateTime)
        {
            return ((dateTime.Ticks - UNIX_START_DATE.Ticks) / 10000000L);
        }
        public static long? ToUnixStampDateTime(this string text)
        {
            if (text == null) return null;
            return text.ToDateTime().ToUnixStampDateTime();
        }
        public static DateTime ToDateTimeFromUnixStamp(this long timestamp)
        {
            return UNIX_START_DATE.Add(TimeSpan.FromTicks(timestamp * 10000000L));
        }
        public static DateTime ToDateTimeFromUnixStamp(this long timestamp, string zoneId)
        {
            long lTime = long.Parse(timestamp.ToString() + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtUTC = UNIX_START_DATE.Add(toNow);

            DateTime dtPST = dtUTC.ToDateTime(zoneId);  //T imeZoneInfo.ConvertTime(dtUTC,TimeZoneInfo.ConvertTimeBySystemTimeZoneId();
            return dtPST;
        }
        public static DateTime ToDateTime(this string datetime)
        {
            return DateTime.Parse(datetime);
        }
        public static DateTime ToDateTimePST(this DateTime utc)
        {
            DateTime dtPST = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utc, PSTZoneId);  //T imeZoneInfo.ConvertTime(dtUTC,TimeZoneInfo.ConvertTimeBySystemTimeZoneId();
            return dtPST;
        }
        public static DateTime ToDateTime(this DateTime dtUTC, string zoneId)
        {
            DateTime dtPST = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dtUTC, zoneId);  //T imeZoneInfo.ConvertTime(dtUTC,TimeZoneInfo.ConvertTimeBySystemTimeZoneId();
            return dtPST;
        }
    }
}
