using System;

namespace Harry.Factory
{
    /// <summary>
    /// 提供者接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProvider<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// 尝试创建对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryCreate(string name,out T value);
    }
}
