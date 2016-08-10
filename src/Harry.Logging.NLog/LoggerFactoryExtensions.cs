#if !NET20
using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Harry.Logging
{
    public static class LoggerFactoryExtensions
    {

        public static ILoggerFactory AddNLog(this ILoggerFactory factory, string configurationFilePath = null)
        {
            //ignore this
            LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging")));
            LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging.Abstractions")));
#if !NET20 && !NET35 && !NET40
            LogManager.AddHiddenAssembly(typeof(LoggerFactoryExtensions).GetTypeInfo().Assembly);
#endif

            using (var provider = new NLogLoggerProvider())
            {
                factory.AddProvider(provider);
            }
            return factory;
        }

        ///// <summary>
        ///// Apply NLog configuration from XML config.
        ///// </summary>
        ///// <param name="env"></param>
        ///// <param name="configFileRelativePath">relative path to NLog configuration file.</param>
        //public static void ConfigureNLog(this IHostingEnvironment env, string configFileRelativePath)
        //{
        //    var fileName = Path.Combine(env.ContentRootPath, configFileRelativePath);
        //    ConfigureNLog(fileName);
        //}

        /// <summary>
        /// Apply NLog configuration from XML config.
        /// </summary>
        /// <param name="fileName">absolute path  NLog configuration file.</param>
        private static void ConfigureNLog(string fileName)
        {
            LogManager.Configuration = new XmlLoggingConfiguration(fileName, true);
        }
    }
}

#endif