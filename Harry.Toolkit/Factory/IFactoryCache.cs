using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harry.Factory
{
    public interface IFactoryCache<T>
        where T : class
    {
        /// <summary>
        /// 尝试获取对象
        /// </summary>
        T Get(string name);

        /// <summary>
        /// 尝试清空指定名称的对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool TryRemove(string name);

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        void Clear();
    }
}
