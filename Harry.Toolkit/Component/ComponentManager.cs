using System;
using System.Collections.Generic;

namespace Harry.Component
{
    /// <summary>
    /// 组件管理器
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    internal class ComponentManager<TObject>
        where TObject : class, IObject
    {
        private Dictionary<string, TObject> _dicObjects = new Dictionary<string, TObject>(StringComparer.Ordinal);
        
        public ComponentManager(IEnumerable<TObject> objects)
        {
            if (objects == null) throw new ArgumentNullException(nameof(objects));

            foreach (var item in objects)
            {
                if (!_dicObjects.TryAdd(item.Name, item))
                {
                    throw new ArgumentException($"类型{typeof(TObject)}中,名称[{item.Name}]有重复");
                }
            }
        }

        /// <summary>
        /// 尝试获取对像
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet(string name, out TObject value)
        {
            return _dicObjects.TryGetValue(name, out value);
        }

        /// <summary>
        /// 获取所有对象
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TObject> GetAll()
        {
            return _dicObjects.Values;
        }
    }
}
