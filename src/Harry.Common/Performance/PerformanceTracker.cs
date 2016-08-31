using Harry.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#if NET20 || NET35
using System.Threading;
#else
using System.Threading.Tasks;
#endif

namespace Harry.Performance
{
    /// <summary>
    /// 性能跟踪类
    /// </summary>
    public class PerformanceTracker:IDisposable
    {
        private Stopwatch stopwatch;
        private List<PerformanceMetricBase> performanceMetrics;
        ILogger logger;

        public PerformanceTracker(ILogger logger, params PerformanceMetricBase[] performanceMetrics)
        {
            this.logger = logger;

            if (performanceMetrics != null && performanceMetrics.Length > 0)
            {
                this.performanceMetrics = new List<PerformanceMetricBase>();
                this.performanceMetrics.AddRange(performanceMetrics);
            }

        }


        /// <summary>
        /// 启动监控
        /// </summary>
        public void ProcessStart()
        {
            try
            {
                if (performanceMetrics != null && performanceMetrics.Count > 0)
                {
#if NET20 || NET35
                    ThreadPool.QueueUserWorkItem(new WaitCallback(OnProcessStart), null);
#else
                Task t = Task.Factory.StartNew(() =>
                {
                    OnProcessStart(null);
                });
#endif
                }


                this.stopwatch = Stopwatch.StartNew();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, 0, ex, "启动性能监控出错");
            }
        }

        private void OnProcessStart(object state)
        {
            foreach (PerformanceMetricBase m in this.performanceMetrics)
            {
                m.OnStart();
            }
        }


        /// <summary>
        /// 监控结束
        /// </summary>
        /// <param name="unhandledExceptionFlag"></param>
        public void ProcessComplete(bool unhandledExceptionFlag=false)
        {
            try
            {
                this.stopwatch.Stop();

                if (performanceMetrics != null && performanceMetrics.Count > 0)
                {
#if NET20 || NET35
                    ThreadPool.QueueUserWorkItem(new WaitCallback(OnProcessComplete), unhandledExceptionFlag);
#else
                // Iterate through each metric and call the OnComplete() method
                // Start off a task to do this so it can it does not block and minimized impact to the user
                Task t = Task.Factory.StartNew(() =>
                {
                    foreach (PerformanceMetricBase m in this.performanceMetrics)
                    {
                        m.OnComplete(this.stopwatch.ElapsedTicks, unhandledExceptionFlag);
                    }
                });
#endif
                }

            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, 0, ex, "性能监控结束时出错");
            }
        }

        private void OnProcessComplete(object state)
        {
            bool unhandledExceptionFlag = Convert.ToBoolean(state);
            foreach (PerformanceMetricBase m in this.performanceMetrics)
            {
                m.OnComplete(this.stopwatch.ElapsedTicks, unhandledExceptionFlag);
            }
        }

        public void Dispose()
        {
            if (stopwatch.IsRunning)
            {
                ProcessComplete();
            }
        }
    }
}
