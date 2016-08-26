#if NET40 || NET45
using Harry.Logging;
using Harry.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Harry.Web.Performance
{
    public class WebApiPerformanceAttribute : ActionFilterAttribute
    {
        ILogger logger = LoggerFactory.Instance.CreateLogger("Harry.Web.Performance.WebApiPerformanceAttribute");

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //如果监控功能未打开,直接退出
            if (ConfigInfo.Value.PerformanceEnabled == false)
            {
                return;
            }

            //获取action或controller上面的属性,如果标记为DoNotTrackPerformanceAttribute,直接退出
            HttpActionDescriptor actionDescriptor = actionContext.ActionDescriptor;
            if (actionDescriptor.GetCustomAttributes<DoNotTrackPerformanceAttribute>().Count > 0
                || actionDescriptor.ControllerDescriptor.GetCustomAttributes<DoNotTrackPerformanceAttribute>().Count > 0)
            {
                return;
            }

            //获取action相关信息
            ActionInfo info = this.CreateActionInfo(actionContext);

            //加载监控器
            PerformanceTracker tracker = new PerformanceTracker(logger, PerformanceMetricFactory.GetPerformanceMetrics(info).ToArray());

            //保存到当前上下文中
            actionContext.Request.Properties.Add(this.GetType().FullName, tracker);

            //启动监控
            tracker.ProcessStart();
        }


        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            String key = this.GetType().FullName;

            //如果未曾启动监控统计,直接退出
            if (actionExecutedContext.Request.Properties.ContainsKey(key) == false)
            {
                return;
            }

            PerformanceTracker tracker = actionExecutedContext.Request.Properties[key] as PerformanceTracker;


            if (tracker != null)
            {
                bool exceptionThrown = (actionExecutedContext.Exception != null);
                tracker.ProcessComplete(exceptionThrown);
            }
        }


        /// <summary>
        /// 获取当前Action相关信息
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private ActionInfo CreateActionInfo(HttpActionContext actionContext)
        {
            var parameters = actionContext.ActionDescriptor.GetParameters().Select(p => p.ParameterName);
            String parameterString = String.Join(",", parameters);

            int processId = ConfigInfo.Value.ProcessId;
            String controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            String actionName = actionContext.ActionDescriptor.ActionName;
            String httpMethod = HttpContext.Current.Request.HttpMethod;
            int contentLength = HttpContext.Current.Request.ContentLength;

            ActionInfo info = new ActionInfo(processId,
                controllerName, actionName, httpMethod, parameterString, contentLength);

            return info;
        }

    }
}

#endif