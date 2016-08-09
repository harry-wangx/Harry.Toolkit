
using System;

namespace Harry.Logging
{
    public interface ILogger:IDisposable
    {
        void Log(LogLevel logLevel, EventId eventId, Exception exception, string message);

        bool IsEnabled(LogLevel logLevel);
    }
}
