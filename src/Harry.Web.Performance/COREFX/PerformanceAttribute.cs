#if COREFX
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harry.Web.Performance
{
    public class PerformanceAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //如果监控功能未打开,直接退出
            if (ConfigInfo.Value.PerformanceEnabled == false)
            {
                return;
            }

            //获取action或controller上面的属性,如果标记为DoNotTrackPerformanceAttribute,直接退出
            ActionDescriptor actionDescriptor = context.ActionDescriptor;


        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }


    }
}

#endif