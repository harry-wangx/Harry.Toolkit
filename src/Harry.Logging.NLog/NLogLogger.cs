using System;

namespace Harry.Logging
{

    internal class NLogLogger : ILogger
    {
        private readonly NLog.Logger _logger;

        public NLogLogger(NLog.Logger logger)
        {
            _logger = logger;
        }

        public void Log(LogLevel logLevel, EventId eventId, Exception exception, string message)
        {
            var nLogLogLevel = ConvertLogLevel(logLevel);
            if (IsEnabled(nLogLogLevel))
            {
                if (!string.IsNullOrEmpty(message))
                {
                    //message arguments are not needed as it is already checked that the loglevel is enabled.
                    var eventInfo = NLog.LogEventInfo.Create(nLogLogLevel, _logger.Name, message);
                    eventInfo.Exception = exception;
                    eventInfo.Properties["EventId.Id"] = eventId.Id;
                    eventInfo.Properties["EventId.Name"] = eventId.Name;
                    eventInfo.Properties["EventId"] = eventId;
                    _logger.Log(eventInfo); 
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


        private static NLog.LogLevel ConvertLogLevel(Logging.LogLevel logLevel)
        {
            switch (logLevel)
            {                
                case Logging.LogLevel.Trace:
                    return NLog.LogLevel.Trace;
                case Logging.LogLevel.Debug:
                    return NLog.LogLevel.Debug;
                case Logging.LogLevel.Info:
                    return NLog.LogLevel.Info;
                case Logging.LogLevel.Warn:
                    return NLog.LogLevel.Warn;
                case Logging.LogLevel.Error:
                    return NLog.LogLevel.Error;
                case Logging.LogLevel.Fatal:
                    return NLog.LogLevel.Fatal;
                case Logging.LogLevel.Off:
                    return NLog.LogLevel.Off;
                default:
                    return NLog.LogLevel.Debug;
            }
        }


//        public IDisposable BeginScope<TState>(TState state)
//        {
//            if (state == null)
//            {
//                throw new ArgumentNullException(nameof(state));
//            }
//#if NET20
//            return _nullScope;
//#else
//            //TODO not working with async
//            return NLog.NestedDiagnosticsContext.Push(state); 
//#endif
//        }

        public void Dispose()
        {

        }

    }
}