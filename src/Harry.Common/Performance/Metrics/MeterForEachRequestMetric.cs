using Harry.Metrics;
using System;
using System.Collections.Generic;

namespace Harry.Performance.Metrics
{
    /// <summary>
    /// 统计请求的分布情况
    /// </summary>
    public sealed class MeterForEachRequestMetric : PerformanceMetricBase
    {
        private IMeter metric;
        public MeterForEachRequestMetric(TrackInfo info)
            : base(info)
        {
            this.metric = Metric.Meter(info.ContextName, info.Name, "Requests");
        }

        public override void OnStart()
        {
            this.metric.Mark();
        }
    }
}
