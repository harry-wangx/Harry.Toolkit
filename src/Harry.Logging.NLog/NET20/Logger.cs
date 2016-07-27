#if NET20
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NLog
{
    public class Logger
    {
        static object locker = new object();
        LogLevel _level = LogLevel.Trace;

        public Logger(string name)
        {
            this.Name = name;
            string strLevel = System.Configuration.ConfigurationManager.AppSettings["log:level"];
            //Enum.Parse(typeof(LogLevel), strLevel);
            if (Harry.Common.Utils.HasValue(strLevel))
            {
                try
                {
                    var level = Enum.Parse(typeof(LogLevel), strLevel,true);
                    if (level != null)
                    {
                        this._level = (LogLevel)level;
                    }
                } catch {
                }
            }
        }

        public string Name { get; private set; }

        public void Log(string msg, Exception ex, LogLevel level)
        {
            if (IsEnabled(level))
            {
                string fileName = Name + ".txt";
                string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Log");
                path = System.IO.Path.Combine(path, level.ToString());
                path = System.IO.Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd"));

                if (!Directory.Exists(path))
                {
                    lock (locker)
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {ex?.Message} msg:{msg}");
                if (ex != null)
                {
                    sb.Append("==>");
                    sb.AppendLine(ex.StackTrace);
                }
                sb.AppendLine(new string('-', 30));
                System.IO.File.AppendAllText(System.IO.Path.Combine(path, fileName), sb.ToString()); 
            }
        }

        internal bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _level;
        }
    }
}

#endif