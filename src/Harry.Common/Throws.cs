using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Harry
{
    [DebuggerStepThrough]
    public static class Throws
    {
#if !NET20
        public static string IfEmpty(string value, Func<string> getMsg)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value), getMsg());
            }
            return value.Trim();
        }
#endif 

        public static string IfEmpty(string value, string msg = "")
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value), msg);
            }
            return value.Trim();
        }

        public static T IfNull<T>(T value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            return value;
        }

        /// <summary>
        /// 合并多个异常
        /// </summary>
        /// <param name="exceptions"></param>
        public static Exception MergeExceptions(IEnumerable<Exception> exceptions)
        {
            StringBuilder sb = new StringBuilder(1024);
            foreach (var item in exceptions)
            {
                sb.AppendLine(item.Message);
                sb.AppendLine(item.StackTrace);
                sb.AppendLine("----------------------------------------------------");
            }
            return new Exception(sb.ToString());
        }
    }
}
