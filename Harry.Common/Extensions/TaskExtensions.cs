#if !NET35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Harry.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        /// 调用异步方法时,不提示未使用await错误
        /// </summary>
        /// <param name="task"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NoWarning(this Task task) { }
    }
}

#endif