using System;

namespace Harry.Metrics
{
    public interface IGauge
    {
        void Update(double value);
    }
}
