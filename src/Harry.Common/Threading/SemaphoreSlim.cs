/*
 * 原地址 https://referencesource.microsoft.com/#mscorlib/system/threading/SemaphoreSlim.cs
 */

#if NET20 || NET35
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace Harry.Threading
{
    [ComVisible(false)]
    [HostProtection(Synchronization = true, ExternalThreading = true)]
    [DebuggerDisplay("Current Count = {m_currentCount}")]
    public class SemaphoreSlim : IDisposable
    {
        #region Private Fields

        //当前数量
        private volatile int m_currentCount;

        //最大可用数量
        private readonly int m_maxCount;

        // 等待的线程数量
        private volatile int m_waitCount;

        //锁对象
        private object m_lockObj;

        //// Act as the semaphore wait handle, it's lazily initialized if needed, the first WaitHandle call initialize it
        //// and wait an release sets and resets it respectively as long as it is not null
        private volatile ManualResetEvent m_waitHandle;

        // No maximum constant
        private const int NO_MAXIMUM = Int32.MaxValue;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the current count of the <see cref="SemaphoreSlim"/>.
        /// </summary>
        /// <value>The current count of the <see cref="SemaphoreSlim"/>.</value>
        public int CurrentCount
        {
            get { return m_currentCount; }
        }

        /// <summary>
        /// 获取内核锁 <see cref="T:System.Threading.WaitHandle"/>
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The <see
        /// cref="SemaphoreSlim"/> has been disposed.</exception>
        public WaitHandle AvailableWaitHandle
        {
            get
            {
                CheckDispose();

                // Return it directly if it is not null
                if (m_waitHandle != null)
                    return m_waitHandle;

                //lock the count to avoid multiple threads initializing the handle if it is null
                lock (m_lockObj)
                {
                    if (m_waitHandle == null)
                    {
                        // The initial state for the wait handle is true if the count is greater than zero
                        // false otherwise
                        m_waitHandle = new ManualResetEvent(m_currentCount != 0);
                    }
                }
                return m_waitHandle;
            }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SemaphoreSlim"/> class, specifying
        /// the initial number of requests that can be granted concurrently.
        /// </summary>
        /// <param name="initialCount">The initial number of requests for the semaphore that can be granted
        /// concurrently.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="initialCount"/>
        /// is less than 0.</exception>
        public SemaphoreSlim(int initialCount)
            : this(initialCount, NO_MAXIMUM)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemaphoreSlim"/> class, specifying
        /// the initial and maximum number of requests that can be granted concurrently.
        /// </summary>
        /// <param name="initialCount">The initial number of requests for the semaphore that can be granted
        /// concurrently.</param>
        /// <param name="maxCount">The maximum number of requests for the semaphore that can be granted
        /// concurrently.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"> <paramref name="initialCount"/>
        /// is less than 0. -or-
        /// <paramref name="initialCount"/> is greater than <paramref name="maxCount"/>. -or-
        /// <paramref name="maxCount"/> is less than 0.</exception>
        public SemaphoreSlim(int initialCount, int maxCount)
        {
            if (initialCount < 0 || initialCount > maxCount)
            {
                throw new ArgumentOutOfRangeException(
                    "initialCount", initialCount, GetResourceString("SemaphoreSlim_ctor_InitialCountWrong"));
            }

            //validate input
            if (maxCount <= 0)
            {
                throw new ArgumentOutOfRangeException("maxCount", maxCount, GetResourceString("SemaphoreSlim_ctor_MaxCountWrong"));
            }

            m_maxCount = maxCount;
            m_lockObj = new object();
            m_currentCount = initialCount;
        }

        #endregion

        #region  Methods
        public void Wait()
        {
            Wait(Timeout.Infinite, null);
        }


        public void Wait(Func<bool> funUntil)
        {
            Wait(Timeout.Infinite, funUntil);
        }


        public bool Wait(TimeSpan timeout)
        {
            Int64 totalMilliseconds = (Int64)timeout.TotalMilliseconds;
            if (totalMilliseconds < -1 || totalMilliseconds > Int32.MaxValue)
            {
                throw new System.ArgumentOutOfRangeException(
                    "timeout", timeout, GetResourceString("SemaphoreSlim_Wait_TimeoutWrong"));
            }

            return Wait((int)timeout.TotalMilliseconds, null);
        }


        public bool Wait(TimeSpan timeout, Func<bool> funUntil)
        {
            Int64 totalMilliseconds = (Int64)timeout.TotalMilliseconds;
            if (totalMilliseconds < -1 || totalMilliseconds > Int32.MaxValue)
            {
                throw new System.ArgumentOutOfRangeException(
                    "timeout", timeout, GetResourceString("SemaphoreSlim_Wait_TimeoutWrong"));
            }

            return Wait((int)timeout.TotalMilliseconds, funUntil);
        }


        public bool Wait(int millisecondsTimeout)
        {
            return Wait(millisecondsTimeout, null);
        }


        public bool Wait(int millisecondsTimeout, Func<bool> funUntil)
        {
            
            CheckDispose();

            if (millisecondsTimeout < -1)
            {
                throw new ArgumentOutOfRangeException(
                    "totalMilliSeconds", millisecondsTimeout, GetResourceString("SemaphoreSlim_Wait_TimeoutWrong"));
            }

            //cancellationToken.ThrowIfCancellationRequested();
            if (funUntil != null && funUntil())
            {
                throw new OperationCanceledException();
            }
            

            uint startTime = 0;
            if (millisecondsTimeout != Timeout.Infinite && millisecondsTimeout > 0)
            {
                startTime = TimeoutHelper.GetTime();
            }

            bool waitSuccessful = false;
            OperationCanceledException oce = null;
            try
            {

                //首先尝试使用SpinWait自旋
                SpinWait spin = new SpinWait();
                while (m_currentCount == 0 && !spin.NextSpinWillYield)
                {
                    spin.SpinOnce();
                }
                //放在finally里面,可以防止线程的abort操作影响m_waitCount计数,导致不能正常释放锁,造成死锁
                try { }
                finally
                {
                    Monitor.Enter(m_lockObj);
                    m_waitCount++;
                }

                // 如果 count > 0 ,继续执行.
                // 否则,线程开始等待
                if (m_currentCount == 0)
                {
                    if (millisecondsTimeout == 0)
                    {
                        return false;
                    }

                    // 准备主等待...
                    // 直到超时或取消
                    try
                    {
                        waitSuccessful = WaitUntilCountOrTimeout(millisecondsTimeout, startTime, funUntil);
                    }
                    catch (OperationCanceledException e) {
                        oce = e;
                    }
                }

                //Contract.Assert(!waitSuccessful || m_currentCount > 0,
                //    "If the wait was successful, there should be count available.");
                if (m_currentCount > 0)
                {
                    //当前线程可以继续执行
                    waitSuccessful = true;
                    m_currentCount--;
                }
                else if (oce != null)
                {
                    throw oce;
                }

                // Exposing wait handle which is lazily initialized if needed
                if (m_waitHandle != null && m_currentCount == 0)
                {
                    m_waitHandle.Reset();
                }

            }
            finally
            {
                // Release the lock

                m_waitCount--;
                Monitor.Exit(m_lockObj);
                if (oce != null)
                {
                    CancellationTokenCanceledEventHandler(this);
                }
            }

            return  waitSuccessful;
        }

        /// <summary>
        /// 当前线程开始等待,直至超时或取消
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="startTime"></param>
        /// <param name="funUntil"></param>
        /// <returns></returns>
        private bool WaitUntilCountOrTimeout(int millisecondsTimeout, uint startTime, Func<bool> funUntil)
        {
            int remainingWaitMilliseconds = Timeout.Infinite;

            //如果count=0 ,通过Monitor.Wait()方法,长时间等待
            while (m_currentCount == 0)
            {
                if (funUntil != null && funUntil())
                {
                    throw new OperationCanceledException("cancelled");
                }

                if (millisecondsTimeout != Timeout.Infinite)
                {
                    remainingWaitMilliseconds = TimeoutHelper.UpdateTimeOut(startTime, millisecondsTimeout);
                    if (remainingWaitMilliseconds <= 0)
                    {
                        // 已经超时,直接返回
                        return false;
                    }
                }
                // ** the actual wait **
                //如果在指定的时间过期之前重新获取该锁，则为 true；
                //如果在指定的时间过期之后重新获取该锁，则为 false。 此方法只有在重新获取该锁后才会返回
                if (!Monitor.Wait(m_lockObj, remainingWaitMilliseconds))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Exits the <see cref="SemaphoreSlim"/> once.
        /// </summary>
        /// <returns>The previous count of the <see cref="SemaphoreSlim"/>.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The current instance has already been
        /// disposed.</exception>
        public int Release()
        {
            return Release(1);
        }

        /// <summary>
        /// Exits the <see cref="SemaphoreSlim"/> a specified number of times.
        /// </summary>
        /// <param name="releaseCount">The number of times to exit the semaphore.</param>
        /// <returns>The previous count of the <see cref="SemaphoreSlim"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="releaseCount"/> is less
        /// than 1.</exception>
        /// <exception cref="T:System.Threading.SemaphoreFullException">The <see cref="SemaphoreSlim"/> has
        /// already reached its maximum size.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The current instance has already been
        /// disposed.</exception>
        public int Release(int releaseCount)
        {
            CheckDispose();

            // Validate input
            if (releaseCount < 1)
            {
                throw new ArgumentOutOfRangeException(
                    "releaseCount", releaseCount, GetResourceString("SemaphoreSlim_Release_CountWrong"));
            }
            int returnCount;

            lock (m_lockObj)
            {
                // Read the m_currentCount into a local variable to avoid unnecessary volatile accesses inside the lock.
                int currentCount = m_currentCount;
                returnCount = currentCount;

                // If the release count would result exceeding the maximum count, throw SemaphoreFullException.
                if (m_maxCount - currentCount < releaseCount)
                {
                    throw new SemaphoreFullException();
                }

                // Increment the count by the actual release count
                currentCount += releaseCount;

                // Signal to any synchronous waiters
                int waitCount = m_waitCount;
                if (currentCount == 1 || waitCount == 1)
                {
                    Monitor.Pulse(m_lockObj);
                }
                else if (waitCount > 1)
                {
                    Monitor.PulseAll(m_lockObj);
                }

                m_currentCount = currentCount;

                //// Exposing wait handle if it is not null
                //if (m_waitHandle != null && returnCount == 0 && currentCount > 0)
                //{
                //    m_waitHandle.Set();
                //}
            }

            // And return the count
            return returnCount;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //if (m_waitHandle != null)
                //{
                //    m_waitHandle.Close();
                //    m_waitHandle = null;
                //}
                m_lockObj = null;
            }
        }


        /// <summary>
        /// 通知所有等待的线程对象状态的更改
        /// </summary>
        /// <param name="semaphore"></param>
        private static void CancellationTokenCanceledEventHandler(SemaphoreSlim semaphore)
        {
            lock (semaphore.m_lockObj)
            {
                Monitor.PulseAll(semaphore.m_lockObj); //wake up all waiters.
            }
        }

        private void CheckDispose()
        {
            if (m_lockObj == null)
            {
                throw new ObjectDisposedException(null, GetResourceString("SemaphoreSlim_Disposed"));
            }
        }

        /// <summary>
        /// 获取str相对应的本地语言内容,这里偷懒,就直接原样返回了
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string GetResourceString(string str)
        {
            return str;
        }
        #endregion
    }
}
#endif