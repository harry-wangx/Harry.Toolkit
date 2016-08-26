using Harry.Metrics;
using System;
using System.Collections.Generic;


namespace Harry.Performance.Metrics
{
    /// <summary>
    /// 最后一次请求所耗时间
    /// </remarks>
    public class LastCallElapsedTimeMetric : PerformanceMetricBase
    {
        public const String COUNTER_NAME = "Last Call Elapsed Time";

        public LastCallElapsedTimeMetric(TrackInfo info)
            : base(info)
        {
        }

        public override void OnComplete(long elapsedTicks, bool exceptionThrown)
        {
            // Need to convert the elapsed ticks into milliseconds for this counter
            long milliseconds = this.ConvertTicksToMilliseconds(elapsedTicks);
;
            string name = string.Format("{0} {1} ", this.TrackInfo.Name, COUNTER_NAME);
            Metric.Gauge(this.TrackInfo.ContextName, name, milliseconds, "Milliseconds");
        }

        public override void Dispose()
        {
        }
    }
}
