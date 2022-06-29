using Harry.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Harry
{
    [DebuggerStepThrough]
    public static class Check
    {
        #region NotNull
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NotNull<T>(T value, string paramName) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(paramName);

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NotNull<T>(T? value, string paramName) where T : struct
        {
            if (value == null)
                throw new ArgumentNullException(paramName);

            return value.Value;
        }
        #endregion

        #region NotEmpty
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NotNullOrEmpty(string value, string paramName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(paramName);

            return value;
        }
        #endregion

        /// <summary>
        /// 合并多个异常
        /// </summary>
        /// <param name="exceptions"></param>
        public static Exception MergeExceptions(IEnumerable<Exception> exceptions)
        {
            StringBuilder sb = new StringBuilder(2048);
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
