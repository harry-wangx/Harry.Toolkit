using Harry.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harry.Common
{
    public static class Utils
    {

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            return DateTime.UtcNow.ToTimeStamp();
        }
    }
}
