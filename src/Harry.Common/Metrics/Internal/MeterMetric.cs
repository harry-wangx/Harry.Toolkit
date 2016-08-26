using System;
using System.Collections.Generic;

namespace Harry.Metrics.Internal
{
    internal class MeterMetric : IMeter
    {
        List<IMeter> meters = new List<IMeter>();
        private readonly object _sync = new object();

        internal MeterMetric(IMetricProvider[] providers, string contextName, string name, string unit, params string[] tags)
        {
            if (providers != null && providers.Length > 0)
            {
                foreach (var item in providers)
                {
                    meters.Add(item.CreateMeter(contextName, name, unit, tags));
                }
            }
        }

        public void Mark()
        {
            lock (_sync)
            {
                foreach (var c in meters)
                {
                    c.Mark();
                }
            }
        }

        public void Mark(long count)
        {
            lock (_sync)
            {
                foreach (var c in meters)
                {
                    c.Mark(count);
                }
            }
        }

        public void Mark(string item)
        {
            lock (_sync)
            {
                foreach (var c in meters)
                {
                    c.Mark(item);
                }
            }
        }

        public void Mark(string item, long count)
        {
            lock (_sync)
            {
                foreach (var c in meters)
                {
                    c.Mark(item, count);
                }
            }
        }

        public void Reset()
        {
            lock (_sync)
            {
                foreach (var c in meters)
                {
                    c.Reset();
                }
            }
        }
    }
}
