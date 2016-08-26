using System;
using System.Collections.Generic;

namespace Harry.Metrics.Internal
{
    internal class GaugeMetric : IGauge,Harry.Common.IHideObjectMembers
    {
        List<IGauge> gauges = new List<IGauge>();
        private readonly object _sync = new object();

        internal GaugeMetric(IMetricProvider[] providers)
        {
            if (providers != null && providers.Length > 0)
            {
                foreach (var item in providers)
                {
                    gauges.Add(item.CreateGauge());
                }
            }
        }

        public void Update(string contextName, string name, double value, string unit, params string[] tags)
        {
            lock (_sync)
            {
                foreach (var item in gauges)
                {
                    item.Update(contextName,name,value,unit,tags);
                }
            }
        }

#if !NET20
        public void Update(string contextName, string name, Func<double> valueProvider, string unit, params string[] tags)
        {
            lock (_sync)
            {
                foreach (var item in gauges)
                {
                    item.Update(contextName, name, valueProvider, unit, tags);
                }
            }
        }
#endif

        internal void AddProvider(IMetricProvider provider)
        {
            if (provider == null)
                return;

            lock (_sync)
            {
                gauges.Add(provider.CreateGauge());
            }
        }

    }
}
