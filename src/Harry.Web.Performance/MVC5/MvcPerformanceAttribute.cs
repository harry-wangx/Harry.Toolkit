#if NET40 || NET45
using Harry.Logging;
using Harry.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Harry.Web.Performance
{
    public class MvcPerformanceAttribute : ActionFilterAttribute
    {
        ILogger logger = LoggerFactory.Instance.CreateLogger("Harry.Web.Performance.MvcPerformanceAttribute");
        public MvcPerformanceAttribute()
        {

        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //如果监控功能未打开,直接退出
            if (ConfigInfo.Value.PerformanceEnabled == false)
            {
                return;
            }


            //获取action或controller上面的属性,如果标记为DoNotTrackPerformanceAttribute,直接退出
            ActionDescriptor actionDescriptor = filterContext.ActionDescriptor;

            if (actionDescriptor.GetCustomAttributes(typeof(DoNotTrackPerformanceAttribute), true).Length > 0
                || actionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(DoNotTrackPerformanceAttribute), true).Length > 0)
            {
                return;
            }

            //获取action相关信息
            ActionInfo info = this.CreateActionInfo(filterContext);

            //加载监控器
            PerformanceTracker tracker = new PerformanceTracker(logger, PerformanceMetricFactory.GetPerformanceMetrics(info).ToArray());

            //保存到当前上下文中
            String contextKey = this.GetUniqueContextKey(filterContext.ActionDescriptor.UniqueId);
            HttpContext.Current.Items.Add(contextKey, tracker);

            //启动监控
            tracker.ProcessStart();
        }



        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //获取唯一key
            String contextKey = this.GetUniqueContextKey(filterContext.ActionDescriptor.UniqueId);

            //如果未曾启动监控统计,直接退出
            if (HttpContext.Current.Items.Contains(contextKey) == false)
            {
                return;
            }

            PerformanceTracker tracker = HttpContext.Current.Items[contextKey] as PerformanceTracker;

            if (tracker != null)
            {
                bool exceptionThrown = (filterContext.Exception != null);
                tracker.ProcessComplete(exceptionThrown);
            }
        }


        #region Helper Methdos

        /// <summary>
        /// 获取当前Action相关信息
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private ActionInfo CreateActionInfo(ActionExecutingContext actionContext)
        {
            var parameters = actionContext.ActionDescriptor.GetParameters().Select(p => p.ParameterName);
            String parameterString = String.Join(",", parameters);

            int processId = ConfigInfo.Value.ProcessId;
            String controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            String actionName = actionContext.ActionDescriptor.ActionName;
            String httpMethod = HttpContext.Current.Request.HttpMethod;
            int contentLength = HttpContext.Current.Request.ContentLength;

            ActionInfo info = new ActionInfo(processId, 
                controllerName, actionName, httpMethod, parameterString, contentLength);

            return info;
        }


        /// <summary>
        /// 获取唯一ID
        /// </summary>
        /// <param name="actionUniqueId"></param>
        /// <returns></returns>
        private String GetUniqueContextKey(String actionUniqueId)
        {
            return this.GetType().FullName + ":" + actionUniqueId;
        }

        #endregion
    }
}

#endif