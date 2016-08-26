using System;

namespace Harry.Metrics
{
    public interface IGauge
    {
        void Update(string contextName, string name, double value, string unit, params string[] tags);
#if !NET20
        void Update(string contextName, string name, Func<double> valueProvider, string unit, params string[] tags);
#endif
    }
}
