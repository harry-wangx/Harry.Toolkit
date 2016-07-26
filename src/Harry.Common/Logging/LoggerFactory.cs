using System;
using System.Collections.Generic;

namespace Harry.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public partial class LoggerFactory : ILoggerFactory
    {
        private readonly Dictionary<string, Logger> _loggers = new Dictionary<string, Logger>(StringComparer.Ordinal);
        private List<ILoggerProvider> _providers = new List<ILoggerProvider>();
        private readonly object _sync = new object();
        private bool _disposed = false;

#if NET20
        private static LoggerFactory _instance = null;
#else
        private static ILoggerFactory _instance = null; 
#endif
        private static object locker = new object();

        /// <summary>
        /// 创建Logger
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
#if NET20
        public Logger CreateLogger(string categoryName)
#else
        public ILogger CreateLogger(string categoryName) 
#endif
        {
            Logger logger;
            lock (_sync)
            {
                if (!_loggers.TryGetValue(categoryName, out logger))
                {
                    logger = new Logger(this, categoryName);
                    _loggers[categoryName] = logger;
                }
            }
            return logger;
        }

        /// <summary>
        /// 添加Provider
        /// </summary>
        /// <param name="provider"></param>
        public void AddProvider(ILoggerProvider provider)
        {
            lock (_sync)
            {
                //_providers = _providers.Concat(new[] { provider }).ToArray();
                _providers.Add(provider);
                foreach (var logger in _loggers)
                {
                    logger.Value.AddProvider(provider);
                }
            }
        }

        internal ILoggerProvider[] GetProviders()
        {
            return _providers.ToArray();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                foreach (var provider in _providers)
                {
                    try
                    {
                        provider.Dispose();
                    }
                    catch
                    {
                        // Swallow exceptions on dispose
                    }
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// logger工厂实例
        /// </summary>
#if NET20
        public static LoggerFactory Instance
#else
        public static ILoggerFactory Instance 
#endif
        {
            get
            {
                if (_instance != null)
                    return _instance;
                lock (locker)
                {
                    if (_instance != null)
                        return _instance;
                    _instance = new LoggerFactory();
                    return _instance;
                }
            }
        }


    }
}
