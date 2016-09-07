﻿using System;
using System.Collections.Generic;
using Harry.Common;
using Harry.Metrics.Internal;

namespace Harry.Metrics
{
    public static class Metric
    {
        private static readonly List<IMetricProvider> _providers = new List<IMetricProvider>();


        public static IGauge Gauge(string contextName, string name,string unit, params string[] tags)
        {
            return  new GaugeMetric(_providers.ToArray(), contextName, name, unit, tags);
        }

        public static ICounter Counter(string contextName, string name, string unit, params string[] tags)
        {
            return new CounterMetric(_providers.ToArray(), contextName, name, unit, tags);
        }

        public static IMeter Meter(string contextName, string name, string unit, params string[] tags)
        {
            return new MeterMetric(_providers.ToArray(), contextName, name, unit, tags);
        }

        public static IHistogram Histogram(string contextName, string name, string unit, params string[] tags)
        {
            return new HistogramMetric(_providers.ToArray(), contextName, name, unit, tags);
        }

        public static ITimer Timer(string contextName, string name, string unit, params string[] tags)
        {
            return new TimerMetric(_providers.ToArray(), contextName, name,unit, tags);
        }

        /// <summary>
        /// 添加MetricProvider
        /// </summary>
        /// <param name="provider"></param>
        public static void AddProvider(IMetricProvider provider)
        {
            _providers.Add(provider);
        }


    }
}
