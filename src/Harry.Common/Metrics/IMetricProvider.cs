using System;

namespace Harry.Metrics
{
    public interface IMetricProvider : IDisposable
    {
        IGauge CreateGauge(string contextName, string name, string unit, params string[] tags);
        ICounter CreateCounter(string contextName, string name, string unit, params string[] tags);
        IHistogram CreateHistogram(string contextName, string name, string unit, params string[] tags);
        IMeter CreateMeter(string contextName, string name, string unit,  params string[] tags);
        ITimer CreateTimer(string contextName, string name, string unit,  params string[] tags);
    }
}
