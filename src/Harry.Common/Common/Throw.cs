using System;

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
    }
}
