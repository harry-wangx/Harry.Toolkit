using System;
using System.Collections.Generic;


namespace Harry.Common
{
    /// <summary>
    /// 64位ID生成器,最高位为符号位,始终为0,可用位数63.
    /// 实例编号占10位,范围为0-1023
    /// 时间戳和索引共占53位
    /// </summary>
    public class IdCreator
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
        /// <param name="instanceID">实例编号</param>
        /// <param name="beginTime">起始时间</param>
        /// <param name="tsType">时间戳类型</param>
        /// <param name="indexBitLength">索引可用位数(1-32)</param>
        public IdCreator(int instanceID, DateTime beginTime, TimeStampType tsType, int indexBitLength)
        {
            this.instanceID = Math.Abs(instanceID) % 1024;
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
    }

    public enum TimeStampType
    {
        Millisecond,
        Second
    }
}
