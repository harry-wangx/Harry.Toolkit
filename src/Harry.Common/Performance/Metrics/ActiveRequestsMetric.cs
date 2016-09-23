using Harry.Metrics;
using System;
using System.Diagnostics;


namespace Harry.Performance.Metrics
{
    /// <summary>
    /// 统计当前正在请求数量
    /// </summary>
    public sealed class ActiveRequestsMetric : PerformanceMetricBase
    {
        public const String COUNTER_NAME = "ActiveRequests";
        private ICounter callsInProgressCounter;

        public ActiveRequestsMetric(TrackInfo info)
            : base(info)
        {
            this.callsInProgressCounter = Metric.Counter(info.ContextName,info.Name+" "+ COUNTER_NAME, COUNTER_NAME);
        }


        /// <summary>
        /// 开始执行时加1
        /// </summary>
        public override void OnStart()
        {
            this.callsInProgressCounter.Increment();
        }


        /// <summary>
        /// 执行结束后减1
        /// </summary>
        /// <param name="elapsedTicks"></param>
        /// <param name="exceptionThrown"></param>
        public override void OnComplete(long elapsedTicks, bool exceptionThrown)
        {
            this.callsInProgressCounter.Decrement();
        }

        public override void Dispose()
        {
            //this.callsInProgressCounter.Dispose();
        }
    }
}
