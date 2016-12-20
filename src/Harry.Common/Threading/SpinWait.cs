/*******************************************************************************************
 *这个类是从https://referencesource.microsoft.com/#mscorlib/system/threading/SpinWait.cs扒下来的
 *因为NET20下面缺少一些API,这里根据我的理解,进行了部分修改
 * 
 *******************************************************************************************/

#if NET20 || NET35
using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Threading;

namespace Harry.Threading
{
    [HostProtection(Synchronization = true, ExternalThreading = true)]
    public struct SpinWait
    {
        internal const int YIELD_THRESHOLD = 10; // 真正放弃时间片前自旋次数
        internal const int SLEEP_0_EVERY_HOW_MANY_TIMES = 5; // After how many yields should we Sleep(0)?
        internal const int SLEEP_1_EVERY_HOW_MANY_TIMES = 20; // After how many yields should we Sleep(1)?

        // 调用SpinOnce方法的次数
        private int m_count;

        /// <summary>
        /// 调用 <see cref="SpinOnce"/> 的次数
        /// </summary>
        public int Count
        {
            get { return m_count; }
        }

        /// <summary>
        /// 如果调用SpinOnce方法已超10次,或系统为单核CPU,则返回true,下一次调用<see cref="SpinOnce"/>时,将放弃当前线程的时间片
        /// </summary>
        public bool NextSpinWillYield
        {
            get { return m_count > YIELD_THRESHOLD || PlatformHelper.IsSingleProcessor; }
        }

        /// <summary>
        /// 执行一次等待
        /// </summary>
        public void SpinOnce()
        {
            if (NextSpinWillYield)
            {
                int yieldsSoFar = (m_count >= YIELD_THRESHOLD ? m_count - YIELD_THRESHOLD : m_count);

                if ((yieldsSoFar % SLEEP_1_EVERY_HOW_MANY_TIMES) == (SLEEP_1_EVERY_HOW_MANY_TIMES - 1))
                {
                    Thread.Sleep(1);
                }
                //else if ((yieldsSoFar % SLEEP_0_EVERY_HOW_MANY_TIMES) == (SLEEP_0_EVERY_HOW_MANY_TIMES - 1))
                //{
                //    Thread.Sleep(0);
                //}
                else
                {
                    //#if PFX_LEGACY_3_5
                    //                    Platform.Yield();
                    //#else
                    //                    Thread.Yield();
                    //#endif


                    //因为.net 2.0下面没有Thread.Yield()方法,所以只能是在这里直接调用Sleep(0),
                    //告诉系统放弃当前线程时间片的剩余部分.
                    //不好的地方就是,Sleep(0)不允许较低优先级的线程运行,而Sleep(1)总是强迫进行上下文切换
                    Thread.Sleep(0);
                }
            }
            else
            {
                //优先使用Thread.SpinWait自旋
                Thread.SpinWait(4 << m_count);
            }

            // Finally, increment our spin counter.
            m_count = (m_count == int.MaxValue ? YIELD_THRESHOLD : m_count + 1);
        }

        /// <summary>
        /// 重设置<see cref="SpinOnce"/>计数
        /// </summary>
        public void Reset()
        {
            m_count = 0;
        }

        #region Static Methods
        /// <summary>
        /// 自旋至满足条件为止
        /// </summary>
        /// <param name="condition"></param>
        public static void SpinUntil(Func<bool> condition)
        {
#if DEBUG
            bool result = 
#endif
            SpinUntil(condition, Timeout.Infinite);
        }

        /// <summary>
        /// 自旋至满足条件或超时为止
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static bool SpinUntil(Func<bool> condition, TimeSpan timeout)
        {
            Int64 totalMilliseconds = (Int64)timeout.TotalMilliseconds;
            if (totalMilliseconds < -1 || totalMilliseconds > Int32.MaxValue)
            {
                throw new System.ArgumentOutOfRangeException(
                    "timeout", timeout, GetResourceString("SpinWait_SpinUntil_TimeoutWrong"));
            }

            // Call wait with the timeout milliseconds
            return SpinUntil(condition, (int)timeout.TotalMilliseconds);
        }

        private static string GetResourceString(string str)
        {
            return str;
        }

        /// <summary>
        /// 自旋至满足条件或超时为止
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public static bool SpinUntil(Func<bool> condition, int millisecondsTimeout)
        {
            if (millisecondsTimeout < Timeout.Infinite)
            {
                throw new ArgumentOutOfRangeException(
                   "millisecondsTimeout", millisecondsTimeout, GetResourceString("SpinWait_SpinUntil_TimeoutWrong"));
            }
            if (condition == null)
            {
                throw new ArgumentNullException("condition", GetResourceString("SpinWait_SpinUntil_ArgumentNull"));
            }
            uint startTime = 0;
            if (millisecondsTimeout != 0 && millisecondsTimeout != Timeout.Infinite)
            {
                startTime = TimeoutHelper.GetTime();
            }
            SpinWait spinner = new SpinWait();
            while (!condition())
            {
                if (millisecondsTimeout == 0)
                {
                    return false;
                }

                spinner.SpinOnce();

                if (millisecondsTimeout != Timeout.Infinite && spinner.NextSpinWillYield)
                {
                    if (millisecondsTimeout <= (TimeoutHelper.GetTime() - startTime))
                    {
                        return false;
                    }
                }
            }
            return true;

        }
        #endregion

    }


    /// <summary>
    /// A helper class to get the number of processors, it updates the numbers of processors every sampling interval.
    /// </summary>
    internal static class PlatformHelper
    {
        private const int PROCESSOR_COUNT_REFRESH_INTERVAL_MS = 30000; // How often to refresh the count, in milliseconds.
        private static volatile int s_processorCount; // The last count seen.
        private static volatile int s_lastProcessorCountRefreshTicks; // The last time we refreshed.

        /// <summary>
        /// Gets the number of available processors
        /// </summary>
        internal static int ProcessorCount
        {
            get
            {
                int now = Environment.TickCount;
                int procCount = s_processorCount;
                if (procCount == 0 || (now - s_lastProcessorCountRefreshTicks) >= PROCESSOR_COUNT_REFRESH_INTERVAL_MS)
                {
                    s_processorCount = procCount = Environment.ProcessorCount;
                    s_lastProcessorCountRefreshTicks = now;
                }

                if (procCount > 0 && procCount <= 64)
                {

                }
                else
                {
                    throw new Exception("Processor count not within the expected range (1 - 64).");
                }

                return procCount;
            }
        }

        /// <summary>
        /// Gets whether the current machine has only a single processor.
        /// </summary>
        internal static bool IsSingleProcessor
        {
            get { return ProcessorCount == 1; }
        }
    }

    /// <summary>
    /// A helper class to capture a start time using Environment.TickCout as a time in milliseconds, also updates a given timeout bu subtracting the current time from
    /// the start time
    /// </summary>
    internal static class TimeoutHelper
    {
        /// <summary>
        /// Returns the Environment.TickCount as a start time in milliseconds as a uint, TickCount tools over from postive to negative every ~ 25 days
        /// then ~25 days to back to positive again, uint is sued to ignore the sign and double the range to 50 days
        /// </summary>
        /// <returns></returns>
        public static uint GetTime()
        {
            return (uint)Environment.TickCount;
        }

        /// <summary>
        /// Helper function to measure and update the elapsed time
        /// </summary>
        /// <param name="startTime"> The first time (in milliseconds) observed when the wait started</param>
        /// <param name="originalWaitMillisecondsTimeout">The orginal wait timeoutout in milliseconds</param>
        /// <returns>The new wait time in milliseconds, -1 if the time expired</returns>
        public static int UpdateTimeOut(uint startTime, int originalWaitMillisecondsTimeout)
        {
            // The function must be called in case the time out is not infinite
            if (originalWaitMillisecondsTimeout == Timeout.Infinite)
            {
                throw new Exception("originalWaitMillisecondsTimeout不能为Timeout.Infinite");
            }

            uint elapsedMilliseconds = (GetTime() - startTime);

            // Check the elapsed milliseconds is greater than max int because this property is uint
            if (elapsedMilliseconds > int.MaxValue)
            {
                return 0;
            }

            // Subtract the elapsed time from the current wait time
            int currentWaitTimeout = originalWaitMillisecondsTimeout - (int)elapsedMilliseconds; ;
            if (currentWaitTimeout <= 0)
            {
                return 0;
            }

            return currentWaitTimeout;
        }
    }
}

#endif