#if !NET40

using System;
using System.Globalization;
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

            var cultureUi = CultureInfo.CurrentUICulture;
            var culture = CultureInfo.CurrentCulture;

            return _myTaskFactory.StartNew(() =>
            {

                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;
                return func();
            }).Unwrap().GetAwaiter().GetResult();
        }

        /// <summary>
        /// 同步调用异步方法
        /// </summary>
        /// <param name="func"></param>
        public static void RunSync(Func<Task> func)
        {

            var cultureUi = CultureInfo.CurrentUICulture;
            var culture = CultureInfo.CurrentCulture;

            _myTaskFactory.StartNew(() =>
            {

                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;

                return func();
            }).Unwrap().GetAwaiter().GetResult();
        }

        public static void ExecuteSafely(object sync, Func<bool> canExecute, Action actiontoExecuteSafely)
        {
            if (canExecute())
            {
                lock (sync)
                {
                    if (canExecute())
                    {
                        actiontoExecuteSafely();
                    }
                }
            }
        }

    }
}

#endif