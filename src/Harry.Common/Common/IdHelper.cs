using System;
using System.Text;

namespace Harry.Common
{
    public static class IdHelper
    {
        static long timestamp = 0;
        static long index = 0;
        static object locker = new object();

        /// <summary>
        /// 生成一个64位ID号
        /// </summary>
        /// <param name="beginTime">起始时间(自起始时间算起,可使用69年)</param>
        /// <param name="hostId">需要为每台机器或每个进程配置一个序列号,范围(0-1023)</param>
        /// <returns></returns>
        public static long CreateId(DateTime beginTime, int hostId = 0)
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
                hostId = hostId % 1024;
                id = id | ((long)hostId << 12);

                //获取计数
                if (timestamp < ms)
                {
                    timestamp = ms;
                    index = 0;
                }
                else
                {
                    if (index > 4095)
                    {
                        timestamp++;
                        index = 0;
                    }
                }

                id = id | index;

                index++;

            }
            return id;
        }

        /// <summary>
        /// 生成一个纯数字的字符型ID
        /// </summary>
        /// <param name="beginTime">起始时间(自起始时间算起,可使用69年)</param>
        /// <param name="hostId">需要为每台机器或每个进程配置一个序列号,范围(0-1023)</param>
        /// <param name="totalWidth">ID长度(如生成的整型ID长度小于totalWidth,则左侧用0补齐)</param>
        /// <returns></returns>
        public static string CreateId(DateTime beginTime, int hostId, int totalWidth)
        {
            return CreateId(beginTime, hostId).ToString().PadLeft(totalWidth, '0');
        }

        /// <summary>
        /// 生成一个包含字符的字符型ID,长度较短
        /// </summary>
        /// <param name="beginTime">起始时间(自起始时间算起,可使用69年)</param>
        /// <param name="hostId">需要为每台机器或每个进程配置一个序列号,范围(0-1023)</param>
        /// <param name="totalWidth">ID长度(如生成的整型ID长度小于totalWidth,则左侧用0补齐)</param>
        /// <returns></returns>
        public static string CreateIdWithChar(DateTime beginTime, int hostId = 0)
        {
            var id = CreateId(beginTime, hostId);
            StringBuilder sb = new StringBuilder();

            sb.Append(getByteString(id, 56));
            sb.Append(getByteString(id, 48));
            sb.Append(getByteString(id, 40));
            sb.Append(getByteString(id, 32));
            sb.Append(getByteString(id, 24));
            sb.Append(getByteString(id, 16));
            sb.Append(getByteString(id, 8));
            sb.Append(getByteString(id, 0));

            return sb.ToString();
        }

        private static string getByteString(long id, int offset)
        {
            long value = (255L << offset) & id;
            value = value >> offset;
            return ((byte)value).ToString("x").PadLeft(2, '0');
        }

    }
}
