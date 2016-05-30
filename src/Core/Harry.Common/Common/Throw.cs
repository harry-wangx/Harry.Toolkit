using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harry.Common
{
    public static class Throw
    {
        public static string IfEmpty(string value, Func<string> getMsg)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value),getMsg());
            }
            return value.Trim();
        }
        public static string IfEmpty(string value, string msg = "")
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value), msg);
            }
            return value.Trim();
        }
    }
}
