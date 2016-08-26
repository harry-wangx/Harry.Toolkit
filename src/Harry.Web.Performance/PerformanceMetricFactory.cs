using Harry.Performance;
using Harry.Performance.Metrics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Harry.Web.Performance
{
    public static class PerformanceMetricFactory
    {

        #region Static Variables

        private static Dictionary<ActionInfo, PerformanceMetricContainer> performanceMetrics;


        private static List<Func<PerformanceMetricBase>> customMetrics;


        private static Object lockObject;

        #endregion


        static PerformanceMetricFactory()
        {
            performanceMetrics = new Dictionary<ActionInfo, PerformanceMetricContainer>();
            customMetrics = new List<Func<PerformanceMetricBase>>();
            lockObject = new Object();
        }


        public static List<PerformanceMetricBase> GetPerformanceMetrics(ActionInfo info)
        {
            if (performanceMetrics.ContainsKey(info) == false)
            {
                lock (lockObject)
                {
                    // Check Again
                    if (performanceMetrics.ContainsKey(info) == false)
                    {
                        List<PerformanceMetricBase> metrics = CreateMetricsForAction(info);
                        PerformanceMetricContainer pmc = new PerformanceMetricContainer(info, metrics);
                        performanceMetrics.Add(info, pmc);
                    }
                }
            }

            return performanceMetrics[info].GetPerformanceMetrics();
        }



        private static List<PerformanceMetricBase> CreateMetricsForAction(ActionInfo actionInfo)
        {
            List<PerformanceMetricBase> metrics = new List<PerformanceMetricBase>();

            // Add the standard metrics
            metrics.Add(new TimerForEachRequestMetric(actionInfo.GetTrackInfo()));
            metrics.Add(new ActiveRequestsMetric(actionInfo.GetTrackInfo()));
            metrics.Add(new LastCallElapsedTimeMetric(actionInfo.GetTrackInfo()));


            // Now add any custom metrics the user may have added
            foreach (var x in customMetrics)
            {
                PerformanceMetricBase customMetric = x();
                metrics.Add(customMetric);
            }

            return metrics;
        }





        public static void AddCustomPerformanceMetric(Func<PerformanceMetricBase> customMetricCreator)
        {
            customMetrics.Add(customMetricCreator);
        }



        /// <summary>
        /// Method to clean up the performance counters on application exit
        /// </summary>
        /// <remarks>
        /// This method should only be called on application exit
        /// </remarks>
        public static void CleanupPerformanceMetrics()
        {
            // We'll make sure no one is trying to add while we are doing this, but should not
            // really be an issue
            lock (lockObject)
            {
                foreach (var pmc in performanceMetrics.Values)
                {
                    pmc.DisposePerformanceMetrics();
                }

                performanceMetrics.Clear();
#if !COREFX
                PerformanceCounter.CloseSharedResources();
#endif
            }
        }

    }
}
