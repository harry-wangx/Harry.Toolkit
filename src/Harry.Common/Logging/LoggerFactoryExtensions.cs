#if !NET20
using Harry.Common;
using System;

namespace Harry.Logging
{
    /// <summary>
    /// ILoggerFactory
    /// </summary>
    public static class LoggerFactoryExtensions
    {

        public static ILogger CreateLogger(this ILoggerFactory factory, Type type)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return factory.CreateLogger(TypeNameHelper.GetTypeDisplayName(type));
        }
    }

}

#endif