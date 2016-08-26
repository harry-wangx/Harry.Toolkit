using Harry.Common;
using System;
using System.Collections.Generic;


namespace Harry.Metrics
{
    public interface ITimer : ResetableMetric
    {
        /// <summary>
        /// 记录时长
        /// </summary>
        /// <param name="time">纳秒 Nanoseconds</param>
        /// <param name="userValue"></param>
        void Record(long time, string userValue = null);

    }

}
