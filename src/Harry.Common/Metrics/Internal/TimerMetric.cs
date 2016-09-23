using System;
using System.Collections.Generic;

namespace Harry.Metrics.Internal
{
    internal sealed class TimerMetric : ITimer
    {

        List<ITimer> timers = new List<ITimer>();
        private readonly object _sync = new object();

        internal TimerMetric(IMetricProvider[] providers, string contextName, string name, string unit, params string[] tags)
        {
            if (providers != null && providers.Length > 0)
            {
                foreach (var item in providers)
                {
                    timers.Add(item.CreateTimer(contextName, name, unit,tags));
                }
            }
        }

        public void Record(long time, string userValue = null)
        {
            lock (_sync)
            {
                foreach (var c in timers)
                {
                    c.Record(time, userValue);
                }
            }
        }


        public void Reset()
        {
            lock (_sync)
            {
                foreach (var c in timers)
                {
                    c.Reset();
                }
            }
        }


    }
}
