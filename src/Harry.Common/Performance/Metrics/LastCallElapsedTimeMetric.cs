using Harry.Metrics;
using System;
using System.Collections.Generic;


namespace Harry.Performance.Metrics
{
    /// <summary>
    /// 最后一次请求所耗时间
    /// </remarks>
    public sealed class LastCallElapsedTimeMetric : PerformanceMetricBase
    {
        public const String COUNTER_NAME = "Last Call Elapsed Time";
        private IGauge callsInProgressCounter;

        public LastCallElapsedTimeMetric(TrackInfo info)
            : base(info)
        {
            this.callsInProgressCounter = Metric.Gauge(info.ContextName, info.Name + " " + COUNTER_NAME, "ms");
        }

        public override void OnComplete(long elapsedTicks, bool exceptionThrown)
        {
            long milliseconds = this.ConvertTicksToMilliseconds(elapsedTicks);
            callsInProgressCounter.Update(milliseconds);
        }

        public override void Dispose()
        {
        }
    }
}
