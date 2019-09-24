using System;
using TimeZoneConverter;

namespace Jmwoo.Common.Date
{
    public static class DateTimeExtensions
    {
        // timezone
        public static DateTime UtcToTimeZone(this DateTime dateTimeUtc, TimeZone toTimeZone)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, TZConvert.GetTimeZoneInfo(toTimeZone.Value));
        }

        public static DateTime TimeZoneToUtc(this DateTime dateTime, TimeZone fromTimeZone)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, TZConvert.GetTimeZoneInfo(fromTimeZone.Value));
        }

        public static DateTime UtcToPst(this DateTime dateTimeUtc) => UtcToTimeZone(dateTimeUtc, TimeZone.PacificStandardTime);
        public static DateTime PstToUtc(this DateTime dateTimePst) => TimeZoneToUtc(dateTimePst, TimeZone.PacificStandardTime);

        // epoch
        public static DateTime Epoch => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static DateTime FromUnixTime(this long unixTime) => Epoch.AddSeconds(unixTime);
        public static long ToUnixTime(this DateTime dateTime) => Convert.ToInt64((dateTime - Epoch).TotalSeconds);
    }

    public class TimeZone
    {
        public string Value { get; }

        private TimeZone(string value)
        {
            Value = value;
        }

        public static TimeZone PacificStandardTime => new TimeZone("Pacific Standard Time");
    }
}
