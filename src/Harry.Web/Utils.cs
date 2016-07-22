using System;
using System.Collections.Generic;
using System.Web;

namespace Harry.Web
{
    public static class Utils
    {
        static char[] separator = new char[] { ',' };

        public static string GetClientIP(HttpRequest request = null)
        {
            if (request == null)
            {
                request = System.Web.HttpContext.Current.Request;
            }
            return request?.UserHostAddress;
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="request"></param>
        /// <param name="realIP">真实IP(只有使用的是透明代理时才能拿到真实IP,
        /// 如果使用的是匿名代理或欺骗性代理,拿到的只能是空值或假数据)
        /// 注:HTTP_VIA返回的并不是代理服务器地址,而服务器使用了CDN加速时会有值
        /// </param>
        /// <returns></returns>
        public static string GetClientIP(HttpRequest request, out string realIP)
        {
            if (request == null)
            {
                request = System.Web.HttpContext.Current.Request;
            }
            var ipArray = Common.Utils.HasValue(request.ServerVariables["HTTP_X_FORWARDED_FOR"]) ? request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(separator, StringSplitOptions.RemoveEmptyEntries) : null;
            if (ipArray != null && ipArray.Length > 0)
            {
                realIP = ipArray[0];
            }
            else
            {
                realIP = null;
            }
            return request?.UserHostAddress;
        }
    }
}
