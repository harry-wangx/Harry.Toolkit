using System;
namespace Harry.Component
{
    public static class ComponentExtensions
    {
        #region IComponentManager 获取对象
        /// <summary>
        /// 尝试获取对象
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="manager"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGet<TObject>(this IComponentManager<TObject> manager, out TObject value)
            where TObject : class, IObject
        {
            return manager.TryGet(String.Empty, out value);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="manager"></param>
        /// <param name="name"></param>
        /// <returns>返回<see cref="TObject"/>对象或者null</returns>
        public static TObject Get<TObject>(this IComponentManager<TObject> manager, string name)
            where TObject : class, IObject
        {
            if (manager.TryGet(name, out TObject value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="manager"></param>
        /// <returns>返回<see cref="TObject"/>对象或者null</returns>
        public static TObject Get<TObject>(this IComponentManager<TObject> manager)
            where TObject : class, IObject
        {
            if (manager.TryGet(out TObject value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region IComponentManager 获取组组件
        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="manager"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetComponent<TObject, TComponent>(this IComponentManager<TObject> manager, string name, out TComponent value)
            where TObject : class, IObject
            where TComponent : class
        {
            if (manager.TryGet(name, out TObject obj))
            {
                if (obj is TComponent component)
                {
                    value = component;
                    return true;
                }

            }
            value = null;
            return false;
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="manager"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetComponent<TObject, TComponent>(this IComponentManager<TObject> manager, out TComponent value)
            where TObject : class, IObject
            where TComponent : class
        {
            return manager.TryGetComponent(string.Empty, out value);
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="manager"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TComponent GetComponent<TObject, TComponent>(this IComponentManager<TObject> manager, string name)
            where TObject : class, IObject
            where TComponent : class
        {
            if (manager.TryGetComponent(name, out TComponent value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="manager"></param>
        /// <returns></returns>
        public static TComponent GetComponent<TObject, TComponent>(this IComponentManager<TObject> manager)
            where TObject : class, IObject
            where TComponent : class
        {
            if (manager.TryGetComponent(out TComponent value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }
        #endregion


        #region IObject
        /// <summary>
        /// 尝试获取组件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TComponent GetComponent<TComponent>(this IObject obj)
            where TComponent : class
        {
            return obj as TComponent;
        }
        #endregion
    }
}
