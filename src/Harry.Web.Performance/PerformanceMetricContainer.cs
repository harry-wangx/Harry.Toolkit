/*
 *
 */
using Harry.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harry.Web.Performance
{
    public class PerformanceMetricContainer
    {
        private List<PerformanceMetricBase> performanceMetrics;

        public PerformanceMetricContainer(ActionInfo actionInfo, List<PerformanceMetricBase> metrics)
        {
            this.ActionInfo = actionInfo;
            this.performanceMetrics = metrics;
        }

        #region Properties and Methods


        public ActionInfo ActionInfo { get; private set; }



        public List<PerformanceMetricBase> GetPerformanceMetrics()
        {
            return this.performanceMetrics.ToList();
        }


        internal void DisposePerformanceMetrics()
        {
            foreach (PerformanceMetricBase metric in this.performanceMetrics)
            {
                metric.Dispose();
            }
        }

        #endregion

    }
}
