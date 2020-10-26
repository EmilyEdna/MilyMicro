using System;

namespace Mily.Utils
{
    public static class UtilsHelper
    {
        /// <summary>
        /// 时间格式化
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="FmtType">
        /// 0：yyyy-MM-dd HH:mm:ss
        /// 1：yyyy-MM-dd HH:mm:ss:ffff
        /// 2：yyyy年MM月dd日
        /// </param>
        /// <returns></returns>
        public static string ToFmtDate(this DateTime Date, int FmtType = 0)
        {
            return FmtType switch
            {
                0 => Date.ToString("yyyy-MM-dd HH:mm:ss"),
                1 => Date.ToString("yyyy-MM-dd HH:mm:ss:ffff"),
                2 => Date.ToString("yyyy年MM月dd日"),
                _ => Date.ToString(),
            };
        }
    }
}
