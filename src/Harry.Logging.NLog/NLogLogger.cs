using System;

namespace Harry.Logging
{
    /// <summary>
    /// Wrap NLog's Logger in a Logging's interface <see cref="Logging.ILogger"/>.
    /// </summary>
    internal class NLogLogger : Logging.ILogger
    {
        private static readonly NullScope _nullScope = new NullScope();

        private readonly NLog.Logger _logger;

        public NLogLogger(NLog.Logger logger)
        {
            _logger = logger;
        }

        //todo  callsite showing the framework logging classes/methods
        public void Log<TState>(Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var nLogLogLevel = ConvertLogLevel(logLevel);
            if (IsEnabled(nLogLogLevel))
            {
                if (formatter == null)
                {
                    throw new ArgumentNullException(nameof(formatter));
                }
                var message = formatter(state, exception);

                if (!string.IsNullOrEmpty(message))
                {
#if NET20
                    _logger.Log(message, exception, nLogLogLevel);
#else
                    //message arguments are not needed as it is already checked that the loglevel is enabled.
                    var eventInfo = NLog.LogEventInfo.Create(nLogLogLevel, _logger.Name, message);
                    eventInfo.Exception = exception;
                    eventInfo.Properties["EventId.Id"] = eventId.Id;
                    eventInfo.Properties["EventId.Name"] = eventId.Name;
                    eventInfo.Properties["EventId"] = eventId;
                    _logger.Log(eventInfo); 
#endif
                }
            }
        }

        /// <summary>
        /// Is logging enabled for this logger at this <paramref name="logLevel"/>?
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(Logging.LogLevel logLevel)
        {
            var convertLogLevel = ConvertLogLevel(logLevel);
            return IsEnabled(convertLogLevel);
        }

        /// <summary>
        /// Is logging enabled for this logger at this <paramref name="logLevel"/>?
        /// </summary>
        private bool IsEnabled(NLog.LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        /// <summary>
        /// Convert loglevel to NLog variant.
        /// </summary>
        /// <param name="logLevel">level to be converted.</param>
        /// <returns></returns>
        private static NLog.LogLevel ConvertLogLevel(Logging.LogLevel logLevel)
        {
            switch (logLevel)
            {                
                case Logging.LogLevel.Trace:
                    return NLog.LogLevel.Trace;
                case Logging.LogLevel.Debug:
                    return NLog.LogLevel.Debug;
                case Logging.LogLevel.Information:
                    return NLog.LogLevel.Info;
                case Logging.LogLevel.Warning:
                    return NLog.LogLevel.Warn;
                case Logging.LogLevel.Error:
                    return NLog.LogLevel.Error;
                case Logging.LogLevel.Critical:
                    return NLog.LogLevel.Fatal;
                case Logging.LogLevel.None:
                    return NLog.LogLevel.Off;
                default:
                    return NLog.LogLevel.Debug;
            }
        }


        public IDisposable BeginScope<TState>(TState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }
#if NET20
            return _nullScope;
#else
            //TODO not working with async
            return NLog.NestedDiagnosticsContext.Push(state); 
#endif
        }

        private class NullScope : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}