using System;

namespace Harry.Factory
{
    public interface IFactory<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// 创建对像实例
        /// </summary>
        /// <param name="name">对像名称</param>
        bool TryCreate(string name, out T value);

        /// <summary>
        /// 添加提供者 <see cref="IProvider<T>"/> 到工厂.
        /// </summary>
        /// <param name="provider">提供者 <see cref="IProvider<T>"/>.</param>
        void AddProvider(IProvider<T> provider);
    }
}
