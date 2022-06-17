using System.Collections.Generic;

namespace Harry.Component
{
    /// <summary>
    /// 组件管理器
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public interface IComponentManager<TObject>
        where TObject : class, IObject
    {
        /// <summary>
        /// 尝试获取对像
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet(string name, out TObject value);

        /// <summary>
        /// 获取所有对像
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TObject> GetAll();
    }
}
