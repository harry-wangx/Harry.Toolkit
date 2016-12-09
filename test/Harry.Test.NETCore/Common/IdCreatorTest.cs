using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Harry.Test.NETCore.Common
{
    public class IdCreatorTest
    {
        private object locker = new object();
        [Fact]
        public void TestConcurrent()
        {
            //测试并发性
            Dictionary<long, long> dicIds = new Dictionary<long, long>();
            for (int tt = 0; tt < 1000; tt++)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(m =>
                {
                    int value = (int)m;
                    Harry.Common.IdCreator ic = new Harry.Common.IdCreator(value);
                    for (int i = 0; i < 100; i++)
                    {
                        var id = ic.Create();
                    }
                }, tt);
            }
        }
    }
}
