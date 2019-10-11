/*****************************************************************
 * 此段代码来源于<NET CLR via C# 第4版>29章[互锁构造]
 * ****************************************************************/
using System;
using System.Threading;

namespace Harry.Common
{
    //todo:整个类待确定
    public enum CoordinationStatus { AllDone, Timeout, Cancel };
    public sealed class AsyncCoordinator
    {
        private Int32 m_opCount = 1;//AllBegun内部调用JustEnded来递减它
        private Int32 m_statusReported = 0; //0=false,1=true
        private Action<CoordinationStatus> m_callback;
        private Timer m_timer;

        /// <summary>
        /// 该方法必须在发起一个操作之前调用
        /// </summary>
        /// <param name="opsToAdd"></param>
        public void AboutToBegin(Int32 opsToAdd = 1)
        {
            Interlocked.Add(ref m_opCount, opsToAdd);
        }

        /// <summary>
        /// 该方法必须在处理好一个操作的结果之后调用
        /// </summary>
        public void JustEnded()
        {
            if (Interlocked.Decrement(ref m_opCount) == 0)
            {
                ReportStatus(CoordinationStatus.AllDone);
            }
        }

        /// <summary>
        /// 该方法必须在发起所有操作之后调用
        /// </summary>
        /// <param name="callback">任务完成/取消时的回调函数</param>
        /// <param name="timeout">单位毫秒</param>
        public void AllBegun(Action<CoordinationStatus> callback,
            Int32 timeout = Timeout.Infinite)
        {
            m_callback = callback;
            if (timeout != Timeout.Infinite)
            {
                m_timer = new Timer(TimeExpired, null, timeout, Timeout.Infinite);
            }
            JustEnded();
        }

        private void TimeExpired(object o)
        {
            ReportStatus(CoordinationStatus.Timeout);
        }

        public void Cancel()
        {
            ReportStatus(CoordinationStatus.Cancel);
        }

        private void ReportStatus(CoordinationStatus status)
        {
            //如果状态从未报告过,就报告它;否则忽略它
            if (Interlocked.Exchange(ref m_statusReported, 1) == 0)
            {
                m_callback(status);
            }
        }
    }
}
