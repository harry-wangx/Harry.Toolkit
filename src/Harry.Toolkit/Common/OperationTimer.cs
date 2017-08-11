/*来自<NET CLR via c# 4 > 第12章 泛型*/
using System;
using System.Diagnostics;

namespace Harry.Common
{
    /// <summary>
    /// 测试代码执行性能用
    /// </summary>
    public sealed class OperationTimer : IDisposable
    {
        private Stopwatch m_stopwatch;
        private string m_text;
        private int m_collectionCount;

        public OperationTimer(string text)
        {
            PrepareForOperation();

            m_text = text;
            m_collectionCount = GC.CollectionCount(0);

            //这应该是方法的最后一个语句,从而最大程度保证计时的准确性
            m_stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            Console.WriteLine($"{m_stopwatch.Elapsed} (GCs={GC.CollectionCount(0) - m_collectionCount})  {m_text}");
        }

        private static void PrepareForOperation()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
