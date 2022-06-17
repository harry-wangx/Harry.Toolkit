using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harry.Factory
{
    public class DefaultFactoryCache<T> : IFactoryCache<T>
        where T : class
    {
        private readonly ConcurrentDictionary<string, T> _cache = new ConcurrentDictionary<string, T>(concurrencyLevel: 1, capacity: 31, StringComparer.Ordinal); // 31 == default capacity
        private readonly IFactory<T> _factory;
        private readonly object _sync = new object();
        public DefaultFactoryCache(IFactory<T> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public T Get(string name)
        {
            name ??= string.Empty;

            if (_cache.TryGetValue(name, out T obj))
            {
                return obj;
            }

            lock (_sync)
            {
                if (_cache.TryGetValue(name, out obj))
                {
                    return obj;
                }

                if (_factory.TryCreate(name, out obj))
                {
                    _cache.TryAdd(name, obj);
                    return obj;
                }
                else
                {
                    _cache.TryAdd(name, null);
                    return obj;
                }
            }
        }

        public bool TryRemove(string name) =>
            _cache.TryRemove(name ?? String.Empty, out _);

        public void Clear() => _cache.Clear();
    }
}
