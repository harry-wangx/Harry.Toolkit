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

        public Logger(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public void Log(string msg, Exception ex, LogLevel level)
        {
            string fileName = Name + "_" + ".txt";
            string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, level.ToString());
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
            sb.AppendLine("==>");
            sb.AppendLine(ex?.StackTrace);
            sb.AppendLine(new string('-', 30));
            System.IO.File.AppendAllText(System.IO.Path.Combine(path, fileName), sb.ToString());
        }

        internal bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }
    }
}

#endif