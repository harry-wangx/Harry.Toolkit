using System;
using Metrics_Net = Metrics;

namespace Harry.Metrics.MetricsDotNet
{
    public class GaugeMetric : IGauge
    {
        private double currentValue = 0;

        public GaugeMetric(string contextName, string name, string unit, params string[] tags)
        {
            Metrics_Net.Metric.Context(contextName).Gauge(name, () => this.currentValue, unit, tags);
        }

        public void Update(double value)
        {
            this.currentValue = value;
        }
    }
}
