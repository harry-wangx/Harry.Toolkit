using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.Extensions
{
    public static class DateTimeExtensions
    {
        public readonly static DateTimeOffset Jan1st1970 = new DateTimeOffset(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long ToTimeStamp(this DateTime dt)
        {
            return (long)((dt - Jan1st1970).TotalSeconds);
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long ToTimeStamp(this DateTimeOffset dt)
        {
            return (long)((dt - Jan1st1970).TotalSeconds);
        }

    }
}
