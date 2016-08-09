#if !NET20
using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long ToTimeStamp(this DateTime dt)
        {
            return Common.Utils.GetTimeStamp(dt);
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long ToTimeStamp(this DateTimeOffset dt)
        {
            return Common.Utils.GetTimeStamp(dt);
        }
    }
}

#endif