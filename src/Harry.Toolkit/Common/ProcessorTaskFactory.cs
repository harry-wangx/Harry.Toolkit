//using System;
//using System.Collections.Generic;
//using System.Threading;
//#if  !NET35
//using System.Threading.Tasks;
//#endif


//namespace Harry.Common
//{
//    public class ProcessorTaskFactory
//    {
//#if NET35
//        private volatile bool isCancelled = false;
//#else
//        private Task spanProcessorTaskInstance;
//        private CancellationTokenSource cancellationTokenSource;
//#endif


//        private Harry.Logging.ILogger logger;
//        private readonly int defaultDelayTime ;
//        private readonly int encounteredAnErrorDelayTime;

//        private readonly object sync = new object();

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="logger"></param>
//        /// <param name="defaultDelayTime">每次任务执行间隔时间(单位:毫秒)</param>
//        /// <param name="encounteredAnErrorDelayTime">任务出错后间隔时间(单位:毫秒)</param>
//        public ProcessorTaskFactory(Harry.Logging.ILogger logger
//            , int defaultDelayTime = 500
//            , int encounteredAnErrorDelayTime = 30000
//#if  NET35

//#else
//                 , CancellationTokenSource cancellationTokenSource = null
//#endif
//                    )
//        {
//            this.logger = logger;
//            this.defaultDelayTime = defaultDelayTime;
//            this.encounteredAnErrorDelayTime = encounteredAnErrorDelayTime;
//#if  NET35

//#else
//            this.cancellationTokenSource = cancellationTokenSource ?? new CancellationTokenSource();
//#endif

//        }

//        //public ProcessorTaskFactory() : this(null)
//        //{ }


//        public virtual void CreateAndStart(Action action)
//        {
////#if !COREFX
////            //取消执行上下文在异步线程之间的流动
////            System.Threading.ExecutionContext.SuppressFlow();
////#endif

//#if  NET35
//            SyncHelper.ExecuteSafely(sync, () => isCancelled, () =>
//            {
//                isCancelled = false;
//                ThreadPool.QueueUserWorkItem(state =>
//                {

//                    ActionWrapper(action);
//                });
//            });
//#else
//            SyncHelper.ExecuteSafely(sync, () => spanProcessorTaskInstance == null || spanProcessorTaskInstance.Status == TaskStatus.Faulted,
//                () =>
//                {
//                    spanProcessorTaskInstance = Task.Factory.StartNew(() => ActionWrapper(action), cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
//                });
//#endif
////#if !COREFX
////            //恢复执行上下文在异步线程之间的流动
////            System.Threading.ExecutionContext.RestoreFlow();
////#endif

//        }

//        public virtual void StopTask()
//        {
//#if  NET35
//            SyncHelper.ExecuteSafely(sync, () => !isCancelled, () =>
//            {
//                isCancelled = true;
//            });
//#else
//            SyncHelper.ExecuteSafely(sync, () => cancellationTokenSource.Token.CanBeCanceled, () => cancellationTokenSource.Cancel());
//#endif

//        }

//        internal
//#if ASYNC
//            async
//#endif
//            void ActionWrapper(Action action)
//        {
//            while (!IsTaskCancelled())
//            {
//                int delayTime = defaultDelayTime;
//                try
//                {
//                    action();
//                }
//                catch (Exception ex)
//                {
//                    if (logger != null)
//                    {
//                        logger.Log(Logging.LogLevel.Error, 0, ex, "Error in ProcessorTaskFactory");
//                    }
//                    delayTime = encounteredAnErrorDelayTime;
//                }

//                // stop loop if task is cancelled while delay is in process

//#if ASYNC
//                try
//                {
//                    await Task.Delay(delayTime, cancellationTokenSource.Token);
//                }
//                catch (TaskCanceledException)
//                {
//                    break;
//                }
//#else
//                System.Threading.Thread.Sleep(delayTime);
//#endif



//            }
//        }

//        public virtual bool IsTaskCancelled()
//        {
//#if  NET35
//            return isCancelled;
//#else
//            return cancellationTokenSource.IsCancellationRequested;
//#endif

//        }
//    }
//}
