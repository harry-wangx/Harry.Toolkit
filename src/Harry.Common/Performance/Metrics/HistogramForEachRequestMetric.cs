using Harry.Metrics;
using System;
using System.Collections.Generic;


namespace Harry.Performance.Metrics
{
    /// <summary>
    /// 统计请求用时情况(单位:毫秒)
    /// </summary>
    public class HistogramForEachRequestMetric: PerformanceMetricBase
    {
        public const string METRIC_NAME = "Call Elapsed Time";
        private IHistogram metric;

        public HistogramForEachRequestMetric(TrackInfo info):base(info)
        {
            this.metric = Metric.Histogram(info.ContextName, info.Name + " " + METRIC_NAME, "ms"); 

        }

        public override void OnComplete(long elapsedTicks, bool exceptionThrown)
        {
            long milliseconds = this.ConvertTicksToMilliseconds(elapsedTicks);

            metric.Update(milliseconds);
        }
    }
}
