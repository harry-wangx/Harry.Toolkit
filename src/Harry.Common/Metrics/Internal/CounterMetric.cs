using System;
using System.Collections.Generic;

namespace Harry.Metrics.Internal
{
    internal sealed class CounterMetric : ICounter
    {
        List<ICounter> counters = new List<ICounter>();
        private readonly object _sync = new object();

        internal CounterMetric(IMetricProvider[] providers, string contextName, string name, string unit, params string[] tags)
        {
            if (providers != null && providers.Length > 0)
            {
                foreach (var item in providers)
                {
                    counters.Add(item.CreateCounter(contextName, name, unit, tags));
                }
            }
        }

        public void Decrement()
        {
            lock (_sync)
            {
                foreach (var item in counters)
                {
                    item.Decrement();
                }
            }
        }

        public void Decrement(long amount)
        {
            lock (_sync)
            {
                foreach (var item in counters)
                {
                    item.Decrement(amount);
                }
            }
        }

        public void Decrement(string item)
        {
            lock (_sync)
            {
                foreach (var c in counters)
                {
                    c.Decrement(item);
                }
            }
        }

        public void Decrement(string item, long amount)
        {
            lock (_sync)
            {
                foreach (var c in counters)
                {
                    c.Decrement(item, amount);
                }
            }
        }

        public void Increment()
        {
            lock (_sync)
            {
                foreach (var c in counters)
                {
                    c.Increment();
                }
            }
        }

        public void Increment(long amount)
        {
            lock (_sync)
            {
                foreach (var c in counters)
                {
                    c.Increment(amount);
                }
            }
        }

        public void Increment(string item)
        {
            lock (_sync)
            {
                foreach (var c in counters)
                {
                    c.Increment(item);
                }
            }
        }

        public void Increment(string item, long amount)
        {
            lock (_sync)
            {
                foreach (var c in counters)
                {
                    c.Increment(item, amount);
                }
            }
        }

        public void Reset()
        {
            lock (_sync)
            {
                foreach (var c in counters)
                {
                    c.Reset();
                }
            }
        }
    }
}
