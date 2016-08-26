using Metrics_Net = Metrics;

namespace Harry.Metrics.MetricsDotNet
{
    public class HistogramMetric : IHistogram
    {
        Metrics_Net.Histogram histogram;

        public HistogramMetric(string contextName, string name, string unit, params string[] tags)
        {
            histogram = Metrics_Net.Metric.Context(contextName).Histogram(name, unit, tags: tags);
        }

        public void Update(long value, string userValue = null)
        {
            histogram.Update(value, userValue);
        }

        public void Reset()
        {
            histogram.Reset();
        }


    }
}
