using System;
using System.Collections.Generic;
using Harry.Extensions;

namespace Harry.Common
{
    /// <summary>
    /// 64位ID生成器,最高位为符号位,始终为0,可用位数63.
    /// 实例编号占10位,范围为0-1023
    /// 时间戳和索引共占53位
    /// </summary>
    public sealed class IdCreator
    {
        private static readonly Random r = new Random();
        private static readonly object type_lock = new object();
        private static IdCreator _default = new IdCreator();

        private readonly long instanceID;//实例编号
        private readonly int indexBitLength;//索引可用位数
        private readonly long tsMax = 0;//时间戳最大值
        private readonly long indexMax = 0;
        private readonly object m_lock = new object();

        private long timestamp = 0;//当前时间戳
        private long index = 0;//索引/计数器

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instanceID">实例编号(0-1023)</param>
        /// <param name="indexBitLength">索引可用位数(1-32).每秒可生成ID数等于2的indexBitLength次方.大并发情况下,当前秒内ID数达到最大值时,将使用下一秒的时间戳,不影响获取ID.</param>
        /// <param name="initTimestamp">初始化时间戳,精确到秒.当之前同一实例生成ID的timestamp值大于当前时间的时间戳时,
        /// 有可能会产生重复ID(如持续一段时间的大并发请求).设置initTimestamp比最后的时间戳大一些,可避免这种问题</param>
        public IdCreator(int instanceID, int indexBitLength, long? initTimestamp = null)
        {
            if (instanceID < 0)
            {
                //这里给每个实例随机生成个实例编号
                this.instanceID = r.Next(0, 1024);
            }
            else
            {
                this.instanceID = instanceID % 1024;
            }

            if (indexBitLength < 1)
            {
                this.indexBitLength = 1;
            }
            else if (indexBitLength > 32)
            {
                this.indexBitLength = 32;
            }
            else
            {
                this.indexBitLength = indexBitLength;
            }
            tsMax = Convert.ToInt64(new string('1', 53 - indexBitLength), 2);
            indexMax = Convert.ToInt64(new string('1', indexBitLength), 2);

            if (initTimestamp != null)
            {
                this.timestamp = initTimestamp.Value;
            }
        }

        /// <summary>
        /// 默认每实例每秒生成65536个ID,从1970年1月1日起,累计可使用4358年
        /// </summary>
        /// <param name="instanceID">实例编号(0-1023)</param>
        public IdCreator(int instanceID) : this(instanceID, 16)
        {

        }

        /// <summary>
        /// 默认每秒生成65536个ID,从1970年1月1日起,累计可使用4358年
        /// </summary>
        public IdCreator() : this(-1)
        {

        }

        /// <summary>
        /// 生成64位ID
        /// </summary>
        /// <returns></returns>
        public long Create()
        {
            long id = 0;

            lock (m_lock)
            {
                //增加时间戳部分
                long ts = DateTime.Now.ToTimeStamp();

                ts = ts % tsMax;  //如果超过时间戳允许的最大值,从0开始
                id = ts << (10 + indexBitLength);//腾出后面部分,给实例编号和索引编号使用

                //增加实例部分
                id = id | (instanceID << indexBitLength);

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

        /// <summary>
        /// 默认每实例每秒生成65536个ID,从1970年1月1日起,累计可使用4358年
        /// </summary>
        public static IdCreator Default
        {
            get
            {
                if (_default != null) return _default;

                #region 方法一:通过双检索创建
                //                //这里没有用lock关键字,
                //                //据<clr via c#>里面说,把Monitor.Exit放在finally里面并不好,
                //                //因为这时候,还有可能会有其它线程进入到该方法,
                //                //这个时候应该挂起线程
                //                System.Threading.Monitor.Enter(type_lock);
                //                //Java这里有个BUG,JVM在读到第一个_default的时候,会把_default的NULL值放到CPU寄存器,
                //                //第二次在这里判断时,多个线程的结果都会为True,会创建多次IdCreator.
                //                //.net不会这样,CLR任何锁方法的调用,都构成了一个完整的内存栅栏,在栅栏之前写入的任何
                //                //变量都必须在栅栏之前完成;在栅栏之后的任何变量读取都必须在栅栏之后开始.
                //                //也就是说下面获取的_default的值,总是最新的.
                //                if (_default == null)
                //                {
                //                    //这里使用Volatile.Write方法,是因为编译器有可能这样做:为_default分配完内存,
                //                    //将引用发布(赋给)_default,再调用构造器.当多线程并发执行的时候,有可能会造成
                //                    //在调用构造器之前(这个时候_default不为NULL),就开始使用IdCreator.
                //                    //Volatile.Write()能解决这个问题,但是只有.net4.5及以上的版本才支持.
                //                    //使用volatile关键字也能解决这个问题,但同时会使所有读取操作具有"易变性",
                //                    //会使性能受到损害.
                //#if ASYNC
                //                    var temp = new IdCreator();
                //                    System.Threading.Volatile.Write(ref _default,temp);
                //#else
                //                    _default = new IdCreator();
                //#endif
                //                }
                //                System.Threading.Monitor.Exit(type_lock); 
                #endregion

                #region 方法二:使用Interlocked.CompareExchange创建,优势:速度快,不阻塞线程.劣势:可能会创建多个实例
                ////创建一个新的单实例对象,并把它固定下来(如果另一个线程还没有固定它的话)
                //IdCreator temp = new IdCreator();
                //System.Threading.Interlocked.CompareExchange(ref _default, temp, null);

                ////如果这个线程竞争失败,新建的第二个单实例对象会被垃圾回收
                #endregion

                #region 方法三:使用System.Lazy
                //懒了,不写例子了
                #endregion

                #region 方法四:其实像这个例子中,直接给静态变量_default new一个对象也耗费不了多少资源.既省事,访问速度又快.

                #endregion
                return _default;
            }
        }
    }

}
