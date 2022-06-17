using System;
using System.Collections.Generic;
using System.Linq;

namespace Harry.Factory
{
    public class DefaultFactory<T> : IFactory<T>
        where T : class
    {
        private readonly object _sync = new object();
        private volatile bool _disposed;
        private readonly List<IProvider<T>> _providers;

        /// <summary>
        /// 创建一个 <see cref="DefaultFactory"/> 实例.
        /// </summary>
        public DefaultFactory() : this(Array.Empty<IProvider<T>>())
        {
        }

        /// <summary>
        /// 创建一个 <see cref="DefaultFactory"/> 实例.
        /// </summary>
        /// <param name="providers">提供者[<see cref="IProvider<T>"/>]集合</param>
        public DefaultFactory(IEnumerable<IProvider<T>> providers)
        {
            _providers = (providers ?? throw new ArgumentNullException(nameof(providers))).ToList();
        }

        public T Create(string name)
        {
            if (CheckDisposed())
            {
                throw new ObjectDisposedException(nameof(DefaultFactory<T>));
            }
            lock (_sync)
            {
                T result = null;
                foreach (var provider in _providers)
                {
                    result = provider.Create(name);
                    if (result != null)
                        return result;
                }
                return null;
            }
        }

        public void AddProvider(IProvider<T> provider)
        {
            if (CheckDisposed())
            {
                throw new ObjectDisposedException(nameof(DefaultFactory<T>));
            }
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            lock (_sync)
            {
                _providers.Add(provider);
            }
        }

        protected virtual bool CheckDisposed() => _disposed;
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                foreach (var provider in _providers)
                {
                    try
                    {
                        provider.Dispose();
                    }
                    catch
                    {
                        // 屏蔽异常
                    }
                }
            }
        }
    }
}
