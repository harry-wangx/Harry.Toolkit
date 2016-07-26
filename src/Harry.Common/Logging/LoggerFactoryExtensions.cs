using System;
using Harry.Logging.Abstractions.Internal;

namespace Harry.Logging
{
#if NET20
    public partial class LoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// Creates a new ILogger instance using the full name of the given type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        public  Logger<T> CreateLogger<T>()
        {
            return new Logger<T>(this);
        }
        /// <summary>
        /// Creates a new ILogger instance using the full name of the given type.
        /// </summary>
        /// <param name="type">The type.</param>
        public  Logger CreateLogger(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return this.CreateLogger(TypeNameHelper.GetTypeDisplayName(type));
        }
    }
#else
    /// <summary>
    /// ILoggerFactory �չ
    /// </summary>
    public static class LoggerFactoryExtensions
    {
        /// <summary>
        /// Creates a new ILogger instance using the full name of the given type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="factory">The factory.</param>
        public static ILogger<T> CreateLogger<T>(this ILoggerFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            return new Logger<T>(factory);
        }
        /// <summary>
        /// Creates a new ILogger instance using the full name of the given type.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="type">The type.</param>
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
#endif
} 
