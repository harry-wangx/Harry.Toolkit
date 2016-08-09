
using System;

namespace Harry.Logging
{

    public interface ILoggerFactory : IDisposable
    {

        ILogger CreateLogger(string categoryName); 


        ILoggerFactory AddProvider(ILoggerProvider provider);
    }
} 
