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
        T Create(string name);
    }
}
