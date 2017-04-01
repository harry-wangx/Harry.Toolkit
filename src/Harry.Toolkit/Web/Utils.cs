using System;
using System.Collections.Generic;
using Harry.Extensions;

#if COREFX
using Microsoft.AspNetCore.Http;
#else
using System.Web;
#endif

namespace Harry.Web
{
    public static class Utils
    {
        static char[] separator = new char[] { ',' };

        public readonly static string UnknownIP = "0.0.0.0";



#if COREFX
        public static string GetClientIP(HttpContext context = null)
        {
            return context?.Connection?.RemoteIpAddress?.ToString();
        }
#else
        public static string GetClientIP(HttpRequest request = null)
        {
            if (request == null)
            {
                request = System.Web.HttpContext.Current?.Request;
            }
            return request?.UserHostAddress;
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ipForwarded">真实IP(只有使用的是透明代理时才能拿到真实IP,
        /// 如果使用的是匿名代理或欺骗性代理,拿到的只能是空值或假数据)
        /// 注:HTTP_VIA返回的并不是代理服务器地址,而服务器使用了CDN加速时会有值
        /// </param>
        /// <returns></returns>
        public static string GetClientIP(HttpRequest request, out string ipForwarded)
        {
            if (request == null)
            {
                request = System.Web.HttpContext.Current?.Request;
            }
            var ipArray = request.ServerVariables["HTTP_X_FORWARDED_FOR"].HasValue() ? request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(separator, StringSplitOptions.RemoveEmptyEntries) : null;
            if (ipArray != null && ipArray.Length > 0)
            {
                ipForwarded = ipArray[0].Trim();
                if (!ipForwarded.IsIPv4())
                {
                    ipForwarded = null;
                }
            }
            else
            {
                ipForwarded = null;
            }
            return request?.UserHostAddress;
        } 
#endif

        /// <summary>
        /// 是否为内网IP 
        /// http://en.wikipedia.org/wiki/Private_network
        /// </summary>
        public static bool IsPrivateIP(string s)
        {
            return (s.StartsWith("192.168.") || s.StartsWith("10.") || s.StartsWith("127.0.0."));
        }
    }
}
