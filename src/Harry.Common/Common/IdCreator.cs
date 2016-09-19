using System;
using System.Collections.Generic;


namespace Harry.Common
{
    /// <summary>
    /// 64位ID生成器,最高位为符号位,始终为0,可用位数63.
    /// 实例编号占10位,范围为0-1023
    /// 时间戳和索引共占53位
    /// </summary>
    public sealed class IdCreator
    {
        long timestamp = 0;//当前时间戳
        long index = 0;//索引/计数器
        int instanceID;//实例编号
        DateTime beginTime;//起始时间
        TimeStampType tsType;//时间戳类型
        int indexBitLength;//索引可用位数
        long tsMax = 0;//时间戳最大值
        long indexMax = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instanceID">实例编号(0-1023)</param>
        /// <param name="beginTime">起始时间</param>
        /// <param name="tsType">时间戳类型</param>
        /// <param name="indexBitLength">索引可用位数(1-32)</param>
        /// <param name="initTimestamp">当之前同一实例生成ID的timestamp值大于当前时间的时间戳时,
        /// 有可能会产生重复ID(如持续一段时间的大并发请求).设置initTimestamp比最后的时间戳大一些,可避免这种问题</param>
        public IdCreator(int instanceID, DateTime beginTime, TimeStampType tsType, int indexBitLength, long? initTimestamp = null)
        {
            if (instanceID < 0)
            {
                Random r = new Random();
                this.instanceID = r.Next(0, 1024);
            }
            else
            {
                this.instanceID = instanceID % 1024;
            }
            
            this.beginTime = beginTime;
            this.tsType = tsType;
            if (indexBitLength >= 1 && indexBitLength <= 32)
            {
                this.indexBitLength = indexBitLength;
            }
            else
            {
                throw new Exception("indexBitLength的值需在1-32之间");
            }
            tsMax = Convert.ToInt64(new string('1', 53 - indexBitLength), 2);
            indexMax = Convert.ToInt64(new string('1', indexBitLength), 2);

            if (initTimestamp != null)
            {
                this.timestamp = initTimestamp.Value;
            }
        }

        /// <summary>
        /// 默认每秒生成65536个ID,从1970年1月1日起,累计可使用4358年
        /// </summary>
        public IdCreator():this(-1)
        {

        }

        /// <summary>
        /// 默认每实例每秒生成65536个ID,从1970年1月1日起,累计可使用4358年
        /// </summary>
        /// <param name="instanceID">实例编号(0-1023)</param>
        public IdCreator(int instanceID) : this(instanceID, new DateTime(1970, 1, 1), TimeStampType.Second, 16)
        {

        }

        /// <summary>
        /// 生成64位ID
        /// </summary>
        /// <returns></returns>
        public long Create()
        {
            long id = 0;

            lock (this)
            {
                //增加时间戳部分
                long ts = -1;
                switch (tsType)
                {
                    case TimeStampType.Millisecond:
                        ts = (long)DateTime.Now.Subtract(beginTime).TotalMilliseconds;
                        break;
                    case TimeStampType.Second:
                        ts = (long)DateTime.Now.Subtract(beginTime).TotalSeconds;
                        break;
                }

                if (ts < 0)
                {
                    //本来想把这个挪到构造函数里面,以提高性能.但是突然想到,如果在系统运行期间,系统时间被修改,有可能会产生问题
                    throw new Exception("beginTime不能大于当前时间");
                }
                ts = ts % tsMax; //2199023255551; //如果超过41位,从0开始
                id = ts << (10 + indexBitLength);//腾出后面22位,给其它部分使用

                //增加实例部分
                id = id | ((long)instanceID << indexBitLength);

                //获取计数
                if (timestamp < ts)
                {
                    timestamp = ts;
                    index = 0;
                }
                else
                {
                    if (index > indexMax)
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
        /// 获取当前实例的时间戳
        /// </summary>
        public long CurrentTimestamp
        {
            get
            {
                return this.timestamp;
            }
        }
    }

    public enum TimeStampType
    {
        Millisecond,
        Second
    }
}
