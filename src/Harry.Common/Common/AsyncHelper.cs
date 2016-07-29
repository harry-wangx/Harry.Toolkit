#if ASYNC
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Harry.Common
{
    public static class AsyncHelper
    {
        private static readonly TaskFactory _myTaskFactory = new TaskFactory(CancellationToken.None,
            TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        /// <summary>
        /// 同步调用异步方法
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
#if NET45 
            var cultureUi = CultureInfo.CurrentUICulture;
            var culture = CultureInfo.CurrentCulture;
#endif
            return _myTaskFactory.StartNew(() =>
            {
#if NET45 
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;
#endif
                return func();
            }).Unwrap().GetAwaiter().GetResult();
        }

        /// <summary>
        /// 同步调用异步方法
        /// </summary>
        /// <param name="func"></param>
        public static void RunSync(Func<Task> func)
        {
#if NET45 
            var cultureUi = CultureInfo.CurrentUICulture;
            var culture = CultureInfo.CurrentCulture;
#endif
            _myTaskFactory.StartNew(() =>
            {
#if NET45 
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;
#endif
                return func();
            }).Unwrap().GetAwaiter().GetResult();
        }
    }
}
#endif
