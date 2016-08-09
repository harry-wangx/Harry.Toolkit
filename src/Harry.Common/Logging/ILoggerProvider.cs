using System;

namespace Harry.Logging
{

    public interface ILoggerProvider : IDisposable
    {

        ILogger CreateLogger(string categoryName);
    }
}
