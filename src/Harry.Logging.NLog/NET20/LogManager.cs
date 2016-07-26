#if NET20
using System;
using System.Collections.Generic;

namespace NLog
{
    public static class LogManager
    {
        static object locker = new object();
        static Dictionary<string, Logger> _loggers = new Dictionary<string, Logger>();
        internal static Logger GetLogger(string name)
        {
            if (_loggers.ContainsKey(name))
            {
                return _loggers[name];
            }
            lock (locker)
            {
                if (_loggers.ContainsKey(name))
                {
                    return _loggers[name];
                }
                _loggers[name] = new Logger(name);
                return _loggers[name];
            }
        }
    }
}

#endif