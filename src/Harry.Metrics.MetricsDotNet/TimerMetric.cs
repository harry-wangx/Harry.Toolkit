using Metrics_Net = Metrics;

namespace Harry.Metrics.MetricsDotNet
{
    public class TimerMetric : ITimer
    {
        Metrics_Net.Timer timer;

        public TimerMetric(string contextName, string name, string unit, params string[] tags)
        {
            timer = Metrics_Net.Metric.Context(contextName).Timer(name, unit, tags: tags);
        }

        public void Record(long time, string userValue = null)
        {
            timer.Record(time, Metrics_Net.TimeUnit.Nanoseconds, userValue);
        }

        public void Reset()
        {
            timer.Reset();
        }
    }
}
