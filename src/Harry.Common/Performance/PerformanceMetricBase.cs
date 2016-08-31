using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Harry.Performance
{

    public abstract class PerformanceMetricBase : IDisposable
    {
        public PerformanceMetricBase(string contextName,string name):this(new TrackInfo(contextName, name))
        {

        }

        public PerformanceMetricBase(TrackInfo info)
        {
            this.TrackInfo = info;
        }



        protected TrackInfo TrackInfo;



        public virtual void OnStart()
        {

        }


        public virtual void OnComplete(long elapsedTicks, bool exceptionThrown)
        {

        }



        protected long ConvertTicksToMilliseconds(long elapsedTicks)
        {
            decimal d = Math.Round(1000 * (decimal)elapsedTicks / Stopwatch.Frequency);
            return Convert.ToInt64(d);
        }

        public virtual void Dispose()
        {
        }

    }
}
