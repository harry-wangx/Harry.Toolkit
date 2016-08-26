using System;
using Metrics_Net = Metrics;

namespace Harry.Metrics.MetricsDotNet
{
    public class GaugeMetric : IGauge
    {
        public void Update(string contextName, string name, double value, string unit, params string[] tags)
        {
            Metrics_Net.Metric.Context(contextName).Gauge(name, () => value, unit, tags);
        }

#if !NET20
        public void Update(string contextName, string name, Func<double> valueProvider, string unit, params string[] tags)
        {
            Metrics_Net.Metric.Context(contextName).Gauge(name, valueProvider, unit, tags);
        }
#endif
    }
}
