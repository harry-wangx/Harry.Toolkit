﻿
using System;
using System.Collections.Generic;
using System.Diagnostics;

#if COREFX
using Microsoft.Extensions.Configuration;
#endif

namespace Harry.Web.Performance
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class ConfigInfo
    {

#if COREFX
        static ConfigInfo()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

        }
        private static IConfigurationRoot Configuration { get; }


#endif

        /// <summary>
        /// Constant for the appSettings key that stores the name of the Performance Counter Category
        /// </summary>
        public const String EnablePerformanceMonitoring = "AspNetPerformance:PerformanceCategoryName";


        #region Properties

        /// <summary>
        /// Property indicating if performance is enabled for the app
        /// </summary>
        /// <remarks>
        /// Performance is considered enabled if there is a Performance Counter Category name
        /// in the app config and that category exists on the machine
        /// </remarks>
        public bool PerformanceEnabled { get; set; }


        /// <summary>
        /// Gets the process id of the ASP.NET worker process the application is running inside
        /// </summary>
        public int ProcessId { get; private set; }

        /// <summary>
        /// Gets a String of the name of the Performance Counter Category to use
        /// </summary>
        /// <remarks>
        /// This performance counter category needs to exist on the machine for performance to
        /// be tracked.  Nominally, it is a good idea to make this name the same as the application
        /// name so it is easy to tell the performance for one app versus another
        /// </remarks>
        public String PerformanceCategoryName { get; private set; }

        #endregion


        #region Static Members

        /// <summary>
        /// Static variable (defined as a System.Lazy) to hold the single instance of the ConfigInfo object
        /// </summary>
        private static Lazy<ConfigInfo> configInfo = new Lazy<ConfigInfo>(() => InitializeConfigInfo());


        /// <summary>
        /// Property to get the instance of the ConfigValue class
        /// </summary>
        public static ConfigInfo Value
        {
            get
            {
                return configInfo.Value;
            }
        }


        /// <summary>
        /// Helper method to initialize the ConfigInfo object
        /// </summary>
        /// <remarks>
        /// This method gets the process id of the current process.  Then it looks in the appSettings for
        /// the name of the performance counter category.  Finally, it makes sure the category exists.  If
        /// there is no value in the appSettings or the category does not exist, then the PerformanceEnabled
        /// flag will be set to false and a message written to the trace log.  Otherwise PerformanceEnabled
        /// will be marked as true
        /// </remarks>
        /// <returns>A ConfigInfo object</returns>
        private static ConfigInfo InitializeConfigInfo()
        {
            ConfigInfo info = new ConfigInfo();
            info.ProcessId = Process.GetCurrentProcess().Id;

            RefreshConfig(info);

#if COREFX
            var token = Configuration.GetReloadToken();
            token.RegisterChangeCallback(OnConfigChanged, null);
#endif

            return info;
        }

        private static void RefreshConfig(ConfigInfo info)
        {
#if COREFX
            string enableperformancemonitoring = Configuration[EnablePerformanceMonitoring];

#else
            String enableperformancemonitoring = System.Configuration.ConfigurationManager.AppSettings[EnablePerformanceMonitoring];
#endif

            if (String.IsNullOrWhiteSpace(enableperformancemonitoring))
            {
                //Trace.WriteLine("No appSettings value was found to enable performance monitoring");
                info.PerformanceEnabled = false;
            }
            else
            {
                // There is a category name, so make sure it exists
                info.PerformanceCategoryName = enableperformancemonitoring;
                info.PerformanceEnabled = true;
            }
        }

#if COREFX
        private static void OnConfigChanged(object state)
        {
            ConfigInfo info = ConfigInfo.Value;
            if (info != null)
            {
                RefreshConfig(info);
            }
            var token = Configuration.GetReloadToken();
            token.RegisterChangeCallback(OnConfigChanged, null);
        }
#endif

#endregion

    }
}
