using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.Common
{
    public static class Throw
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

        /// <summary>
        /// 合并多个异常
        /// </summary>
        /// <param name="exceptions"></param>
        public static Exception  MergeExceptions(List<Exception> exceptions)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in exceptions)
            {
                sb.Append(item.Message
                    + Environment.NewLine
                    + item.StackTrace
                    + Environment.NewLine
                    + "----------------------------------------------------"
                    + Environment.NewLine);
            }
            return new Exception(sb.ToString());
        }
    }
}
