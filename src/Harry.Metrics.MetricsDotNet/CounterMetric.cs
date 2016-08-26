using Metrics_Net = Metrics;

namespace Harry.Metrics.MetricsDotNet
{
    public class CounterMetric : ICounter
    {
        Metrics_Net.Counter counter;
        public CounterMetric(string contextName, string name, string unit, params string[] tags)
        {
            counter = Metrics_Net.Metric.Context(contextName).Counter(name, unit, tags);
        }

        public void Decrement()
        {
            counter.Decrement();
        }

        public void Decrement(long amount)
        {
            counter.Decrement(amount);
        }

        public void Decrement(string item)
        {
            counter.Decrement(item);
        }

        public void Decrement(string item, long amount)
        {
            counter.Decrement(item, amount);
        }

        public void Increment()
        {
            counter.Increment();
        }

        public void Increment(long amount)
        {
            counter.Increment(amount);
        }

        public void Increment(string item)
        {
            counter.Increment(item);
        }

        public void Increment(string item, long amount)
        {
            counter.Increment(item, amount);
        }

        public void Reset()
        {
            counter.Reset();
        }
    }
}
