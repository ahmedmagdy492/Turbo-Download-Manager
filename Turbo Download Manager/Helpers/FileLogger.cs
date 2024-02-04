using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo_Download_Manager.Helpers
{
    public enum LogLevel
    {
        Info,
        Warning, 
        Error
    };

    public class FileLogger : ILogger
    {
        private readonly string _fileName;

        public FileLogger(string fileName)
        {
            _fileName = fileName;
        }

        private void WriteToFile(string content)
        {
            using(StreamWriter sw = new StreamWriter(_fileName, true))
            {
                foreach(var item in content) 
                { 
                    sw.Write(item);
                }
            }
        }

        private string FormatMessage(LogLevel logLevel, string message)
        {
            return $"[{logLevel.ToString().ToUpper()}]: {message}";
        }

        public void LogInfo(string message)
        {
            WriteToFile(FormatMessage(LogLevel.Info, message));
        }

        public void LogWarning(string message)
        {
            WriteToFile(FormatMessage(LogLevel.Warning, message));
        }

        public void LogError(string message)
        {
            WriteToFile(FormatMessage(LogLevel.Error, message));
        }
    }
}
