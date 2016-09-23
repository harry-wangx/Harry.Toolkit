using System;
using System.Collections.Generic;
using Harry.Metrics;

namespace Harry.Performance.Metrics
{
    /// <summary>
    /// 跟踪统计耗时情况
    /// </summary>
    public sealed class TimerForEachRequestMetric : PerformanceMetricBase
    {
        private ITimer metric;


        public TimerForEachRequestMetric(TrackInfo info)
            : base(info)
        {
            this.metric = Metric.Timer(info.ContextName, info.Name, "Requests");
        }


        /// <summary>
        /// 跟踪结束时进行统计
        /// </summary>
        /// <param name="elapsedTicks"></param>
        /// <param name="exceptionThrown"></param>
        public override void OnComplete(long elapsedTicks, bool exceptionThrown)
        {
            long milliseconds = this.ConvertTicksToMilliseconds(elapsedTicks);
            metric.Record(milliseconds); 
        }


        public override void Dispose()
        {
        }
    }
}
