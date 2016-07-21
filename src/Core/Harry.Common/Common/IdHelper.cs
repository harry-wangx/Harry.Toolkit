using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harry.Common
{
    public static class IdHelper
    {
        static long timestamp = 0;
        static long index = 0;
        static object locker = new object();

        /// <summary>
        /// 生成一个ID号
        /// </summary>
        /// <param name="beginTime">起始时间(自起始时间算起,可使用69年)</param>
        /// <param name="hostID">需要为每台机器或每个进程配置一个序列号,范围(0-1023)</param>
        /// <returns></returns>
        public static long CreateID(DateTime beginTime, int hostID = 0)
        {
            long id = 0;

            lock (locker)
            {
                //获取最多41位时间戳
                long ms = (long)DateTime.Now.Subtract(beginTime).TotalMilliseconds;
                if (ms < 0)
                {
                    throw new Exception("beginTime不能大于当前时间");
                }
                ms = ms % 2199023255551; //如果超过41位,从0开始
                id = ms << 22;//腾出后面22位,给其它部分使用

                //获取机器编号
                hostID = hostID % 1024;
                id = id | ((long)hostID << 12);

                //获取计数
                if (timestamp != ms)
                {
                    timestamp = ms;
                    index = 0;
                }

                id = id | index;

                if (index < 4095)
                {
                    index++;
                }
                else
                {
                    //这句代码恐怕很难执行到
                    System.Threading.Thread.Sleep(1);
                }
            }
            return id;
        }
    }
}
