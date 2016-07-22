using System;
using System.Collections.Generic;

namespace Harry.Common
{
    public static class Utils
    {
        private static DateTime Jan1st1970Utc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp(DateTime? dt = null)
        {
            if (dt == null)
            {
                dt = DateTime.UtcNow;
            }
            return (long)((dt.Value - Jan1st1970Utc).TotalMilliseconds);
        }

        /// <summary>
        /// 判断输入字符串是否不为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasValue(string value)
        {
#if NET35 || NET20
            return !string.IsNullOrEmpty(value);
#else
            return !string.IsNullOrWhiteSpace(value);
#endif
        }
    }
}
