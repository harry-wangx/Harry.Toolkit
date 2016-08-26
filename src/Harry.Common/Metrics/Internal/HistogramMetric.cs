using System;
using System.Collections.Generic;

namespace Harry.Metrics.Internal
{
    internal class HistogramMetric: IHistogram
    {
        List<IHistogram> histograms = new List<IHistogram>();
        private readonly object _sync = new object();

        internal HistogramMetric(IMetricProvider[] providers, string contextName, string name, string unit, params string[] tags)
        {
            if (providers != null && providers.Length > 0)
            {
                foreach (var item in providers)
                {
                    histograms.Add(item.CreateHistogram(contextName, name, unit, tags));
                }
            }
        }

        public void Update(long value, string userValue = null)
        {
            lock (_sync)
            {
                foreach (var c in histograms)
                {
                    c.Update(value, userValue);
                }
            }
        }

        public void Reset()
        {
            lock (_sync)
            {
                foreach (var c in histograms)
                {
                    c.Reset();
                }
            }
        }
    }

}
