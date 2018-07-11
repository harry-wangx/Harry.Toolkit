using Harry.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Harry
{
    [DebuggerStepThrough]
    public static class Throws
    {
#if COREFX || NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string IfNotHasValue(string value, Func<string> getMsg)
        {
            if (!value.HasValue())
            {
                throw new ArgumentNullException(nameof(value), getMsg());
            }
            return value.Trim();
        }

#if COREFX || NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string IfNotHasValue(string value, string msg = "")
        {
            if (!value.HasValue())
            {
                throw new ArgumentNullException(nameof(value), msg);
            }
            return value.Trim();
        }

#if COREFX || NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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
