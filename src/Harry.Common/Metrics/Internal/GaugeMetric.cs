using System;
using System.Collections.Generic;

namespace Harry.Metrics.Internal
{
    internal sealed class GaugeMetric : IGauge,Harry.Common.IHideObjectMembers
    {
        List<IGauge> gauges = new List<IGauge>();
        private readonly object _sync = new object();

        internal GaugeMetric(IMetricProvider[] providers, string contextName, string name, string unit, params string[] tags)
        {
            if (providers != null && providers.Length > 0)
            {
                foreach (var item in providers)
                {
                    gauges.Add(item.CreateGauge(contextName, name, unit, tags));
                }
            }
        }

        public void Update(double value)
        {
            lock (_sync)
            {
                foreach (var item in gauges)
                {
                    item.Update(value);
                }
            }
        }

    }
}
