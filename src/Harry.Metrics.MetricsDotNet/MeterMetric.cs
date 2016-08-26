using Metrics_Net = Metrics;

namespace Harry.Metrics.MetricsDotNet
{
    public class MeterMetric : IMeter
    {
        Metrics_Net.Meter meter;
        public MeterMetric(string contextName, string name, string unit, params string[] tags)
        {
            meter = Metrics_Net.Metric.Context(contextName).Meter(name, unit, tags: tags);
        }

        public void Mark()
        {
            meter.Mark();
        }

        public void Mark(long count)
        {
            meter.Mark(count);
        }

        public void Mark(string item)
        {
            meter.Mark(item);
        }

        public void Mark(string item, long count)
        {
            meter.Mark(item, count);
        }

        public void Reset()
        {
            meter.Reset();
        }
    }
}
