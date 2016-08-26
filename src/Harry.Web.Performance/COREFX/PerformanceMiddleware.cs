//#if COREFX
////性能监控中间件
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Harry.Web.Performance
//{
//    public class PerformanceMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private Stopwatch sw;

//        public PerformanceMiddleware(RequestDelegate next)
//        {
//            _next = next;
//            sw = new Stopwatch();
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            //todo:记录请求次数
            

//            sw.Start();
//            try
//            {
//                await _next.Invoke(context);
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            sw.Stop();

//            //todo:记录耗时
//        }
//    }
//}

//#endif