using System;
using _NLog = NLog;
namespace Harry.Logging.NLog
{

    internal class NLogLogger : ILogger
    {
        private readonly _NLog.Logger _logger;

        public NLogLogger(_NLog.Logger logger)
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
                    var eventInfo = _NLog.LogEventInfo.Create(nLogLogLevel, _logger.Name, message);
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
        private bool IsEnabled(_NLog.LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }


        private static _NLog.LogLevel ConvertLogLevel(Logging.LogLevel logLevel)
        {
            switch (logLevel)
            {                
                case Logging.LogLevel.Trace:
                    return _NLog.LogLevel.Trace;
                case Logging.LogLevel.Debug:
                    return _NLog.LogLevel.Debug;
                case Logging.LogLevel.Info:
                    return _NLog.LogLevel.Info;
                case Logging.LogLevel.Warn:
                    return _NLog.LogLevel.Warn;
                case Logging.LogLevel.Error:
                    return _NLog.LogLevel.Error;
                case Logging.LogLevel.Fatal:
                    return _NLog.LogLevel.Fatal;
                case Logging.LogLevel.Off:
                    return _NLog.LogLevel.Off;
                default:
                    return _NLog.LogLevel.Debug;
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