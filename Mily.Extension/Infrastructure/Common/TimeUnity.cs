using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Extension.Infrastructure.Common
{
    public class TimeUnity
    {
        /// <summary>
        /// 时间戳转时间
        /// </summary>
        /// <param name="TimeStamp"></param>
        /// <returns></returns>
        public static DateTime ConvertStamptime(string TimeStamp)
        {
            DateTime StartTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Utc, TimeZoneInfo.Local);
            TimeSpan Span = new TimeSpan(long.Parse(TimeStamp + "0000000"));
            return StartTime.Add(Span);
        }
        /// <summary>
        /// 时间转时间戳
        /// </summary>
        /// <param name="TimeStamp"></param>
        /// <returns></returns>
        public static string ConvertDateTime(DateTime TimeStamp) {
            DateTime StartTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Utc, TimeZoneInfo.Local);
            return (((TimeStamp - StartTime).TotalMilliseconds) / 1000).ToString();
        }
    }
}
