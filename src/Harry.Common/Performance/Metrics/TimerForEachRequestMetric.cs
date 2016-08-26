using System;
using System.Collections.Generic;
using Harry.Metrics;

namespace Harry.Performance.Metrics
{
    /// <summary>
    /// 跟踪统计耗时情况
    /// </summary>
    public class TimerForEachRequestMetric : PerformanceMetricBase
    {
        private ITimer averageTimeCounter;


        public TimerForEachRequestMetric(TrackInfo info)
            : base(info)
        {
            this.averageTimeCounter = Metric.Timer(info.ContextName, info.Name, "Requests");
        }


        /// <summary>
        /// 跟踪结束时进行统计
        /// </summary>
        /// <param name="elapsedTicks"></param>
        /// <param name="exceptionThrown"></param>
        public override void OnComplete(long elapsedTicks, bool exceptionThrown)
        {
            averageTimeCounter.Record(elapsedTicks);
        }


        public override void Dispose()
        {
        }
    }
}
