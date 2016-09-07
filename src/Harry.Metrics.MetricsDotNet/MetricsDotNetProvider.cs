
using System;

namespace Harry.Metrics.MetricsDotNet
{
    public class MetricsDotNetProvider : IMetricProvider
    {

        public IGauge CreateGauge(string contextName, string name, string unit, params string[] tags)
        {
            return new GaugeMetric(contextName, name, unit, tags);
        }

        public ICounter CreateCounter(string contextName, string name, string unit, params string[] tags)
        {
            return new CounterMetric(contextName, name, unit, tags);
        }

        public IMeter CreateMeter(string contextName, string name, string unit, params string[] tags)
        {
            return new MeterMetric(contextName, name, unit, tags);
        }

        public IHistogram CreateHistogram(string contextName, string name, string unit, params string[] tags)
        {
            return new HistogramMetric(contextName, name, unit, tags);
        }

        public ITimer CreateTimer(string contextName, string name, string unit, params string[] tags)
        {
            return new TimerMetric(contextName, name, unit, tags);
        }

        public void Dispose()
        {
            
        }


    }
}
