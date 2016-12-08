//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//#if NET20 || NET35

//#else
//using System.Threading.Tasks;
//#endif


//namespace Harry.Common
//{
//    public class ProcessorTaskFactory
//    {
//#if NET20 || NET35
//        volatile bool isRunning = false;
//        volatile bool isCancelled = false;
//#else
//        private Task spanProcessorTaskInstance;
//        private CancellationTokenSource cancellationTokenSource;
//#endif


//        private Harry.Logging.ILogger logger;
//        private const int defaultDelayTime = 500;
//        private const int encounteredAnErrorDelayTime = 30000;

//        readonly object sync = new object();

//        public ProcessorTaskFactory(Harry.Logging.ILogger logger
//#if NET20 || NET35

//#else
//         , CancellationTokenSource cancellationTokenSource=null
//#endif
//            )
//        {
//            this.logger = logger;
//#if NET20 || NET35

//#else
//            this.cancellationTokenSource = cancellationTokenSource ?? new CancellationTokenSource();
//#endif

//        }


//        public virtual void CreateAndStart(Action action)
//        {

//#if NET20 || NET35
//            AsyncHelper.ExecuteSafely(sync, () => isCancelled, ()=> {
//                isCancelled = false;
//                ThreadPool.QueueUserWorkItem(state=> {
                    
//                    ActionWrapper(action);
//                });
//            });
//#else
//            AsyncHelper.ExecuteSafely(sync, () => spanProcessorTaskInstance == null || spanProcessorTaskInstance.Status == TaskStatus.Faulted,
//                () =>
//                {
//                    spanProcessorTaskInstance = Task.Factory.StartNew(() => ActionWrapper(action), cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
//                });
//#endif

//        }

//        public virtual void StopTask()
//        {
//#if NET20 || NET35
//            AsyncHelper.ExecuteSafely(sync, () => !isCancelled, () => {
//                    isCancelled = true;
//            });
//#else
//            AsyncHelper.ExecuteSafely(sync, () => cancellationTokenSource.Token.CanBeCanceled, () => cancellationTokenSource.Cancel());
//#endif

//        }

//        internal async void ActionWrapper(Action action)
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
//                        logger.Log(Logging.LogLevel.Error, 0, ex, "Error in SpanProcessorTask");
//                    }
//                    delayTime = encounteredAnErrorDelayTime;
//                }

//                // stop loop if task is cancelled while delay is in process
//                try
//                {
//#if NET20 || NET35
            
//#else
//                    await Task.Delay(delayTime, cancellationTokenSource.Token);
//#endif

//                }
//                catch (TaskCanceledException)
//                {
//                    break;
//                }

//            }
//        }

//        public virtual bool IsTaskCancelled()
//        {
//#if NET20 || NET35
            
//#else
//            return cancellationTokenSource.IsCancellationRequested;
//#endif

//        }
//    }
//}
