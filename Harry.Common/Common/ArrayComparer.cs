using Harry.Extensions;
using System.Collections.Generic;

namespace Harry.Common
{
    /// <summary>
    /// 数组比较器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArrayComparer<T> : IEqualityComparer<T[]>
    {
        #region 静态
        static ArrayComparer()
        {
            Default = new ArrayComparer<T>();
        }

        /// <summary>
        /// 默认比较器
        /// </summary>
        public static ArrayComparer<T> Default { get; private set; }
        #endregion


        private readonly IEqualityComparer<T> comparer;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="comparer"></param>
        public ArrayComparer(IEqualityComparer<T> comparer = null)
        {
            if (comparer != null)
                this.comparer = comparer;
            else
                this.comparer = EqualityComparer<T>.Default;
        }

        public bool Equals(T[] x, T[] y)
        {
            return x.Equals<T>(y);
        }

        public int GetHashCode(T[] obj)
        {
            var hashCodeCombiner = HashCodeCombiner.Start();
            hashCodeCombiner.Add(obj, this.comparer);
            return hashCodeCombiner.CombinedHash;
        }
    }
}
