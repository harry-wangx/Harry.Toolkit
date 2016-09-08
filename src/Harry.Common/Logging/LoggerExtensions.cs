

using System;
using System.Collections.Generic;
using System.Globalization;
namespace Harry.Logging
{
    public static class LoggerExtensions
    {
        //------------------------------------------TRACE------------------------------------------//

        public static void Trace(
#if !NET20
            this
#endif
            ILogger logger, EventId eventId, Exception exception, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Trace, eventId, exception, message);
        }

        public static void Trace(
#if !NET20
            this
#endif
            ILogger logger, EventId eventId, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Trace, eventId, null, message);
        }

        public static void Trace(
#if !NET20
            this
#endif
            ILogger logger, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Trace, 0, null, message);
        }

        public static void Trace(
#if !NET20
            this
#endif
            ILogger logger, Func<string> func)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.Log(LogLevel.Trace, 0, null, func());
            }
        }


        //------------------------------------------DEBUG------------------------------------------//

        public static void Debug(
#if !NET20
            this
#endif
            ILogger logger, EventId eventId, Exception exception, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Debug, eventId, exception, message);
        }

        public static void Debug(
#if !NET20
            this
#endif
            ILogger logger, EventId eventId, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Debug, eventId, null, message);
        }


        public static void Debug(
#if !NET20
            this
#endif
            ILogger logger, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Debug, 0, null, message);
        }

        public static void Debug(
#if !NET20
            this
#endif
            ILogger logger, Func<string> func)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.Log(LogLevel.Debug, 0, null, func());
            }
        }

        //------------------------------------------Info------------------------------------------//


        public static void Info(
#if !NET20
            this
#endif
            ILogger logger, EventId eventId, Exception exception, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Info, eventId, exception, message);
        }

        public static void Info(
#if !NET20
            this
#endif
            ILogger logger, EventId eventId, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Info, eventId, null, message);
        }

        public static void Info(
#if !NET20
            this
#endif
            ILogger logger, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Info, 0, null, message);
        }

        public static void Info(
#if !NET20
            this
#endif
            ILogger logger, Func<string> func)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (logger.IsEnabled(LogLevel.Info))
            {
                logger.Log(LogLevel.Info, 0, null, func()); 
            }
        }

        //------------------------------------------Warn------------------------------------------//

        public static void Warn(
#if !NET20
            this
#endif
            ILogger logger, EventId eventId, Exception exception, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Warn, eventId, exception, message);
        }

        public static void Warn(
#if !NET20
            this
#endif
            ILogger logger, EventId eventId, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Warn, eventId, null, message);
        }

        public static void Warn(
#if !NET20
            this
#endif
            ILogger logger, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Warn, 0, null, message);
        }

        public static void Warn(
#if !NET20
            this
#endif
            ILogger logger, Func<string> func)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (logger.IsEnabled(LogLevel.Warn))
            {
                logger.Log(LogLevel.Warn, 0, null, func()); 
            }
        }

        //------------------------------------------ERROR------------------------------------------//

        public static void Error(
#if !NET20
            this
#endif
            ILogger logger, EventId eventId, Exception exception, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Error, eventId, exception, message);
        }

        public static void Error(
#if !NET20
            this
#endif
            ILogger logger, EventId eventId, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Error, eventId, null, message);
        }

        public static void Error(
#if !NET20
            this
#endif
            ILogger logger, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Error, 0, null, message);
        }

        public static void Error(
#if !NET20
            this
#endif
            ILogger logger, Func<string> func)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.Log(LogLevel.Error, 0, null, func()); 
            }
        }

        //------------------------------------------Fatal------------------------------------------//

        public static void Fatal(
#if !NET20
            this
#endif
            ILogger logger, EventId eventId, Exception exception, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Fatal, eventId, exception, message);
        }

        public static void Fatal(
#if !NET20
            this
#endif
            ILogger logger, EventId eventId, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Fatal, eventId, null, message);
        }

        public static void Fatal(
#if !NET20
            this
#endif
            ILogger logger, string message)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Fatal, 0, null, message);
        }

        public static void Fatal(
#if !NET20
            this
#endif
            ILogger logger, Func<string> func)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (logger.IsEnabled(LogLevel.Fatal))
            {
                logger.Log(LogLevel.Fatal, 0, null, func()); 
            }
        }
    }

}

