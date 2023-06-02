
namespace BuisnessLogic.Loggers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NLog;
    using NLog.Fluent;

    public class Logger : ILogger
    {
        private static Logger instance = new Logger();
        private static readonly object lockObject = new object();
        private static NLog.Logger _logger = LogManager.GetLogger("logfileRule");
        public static Logger Instance => instance;

        private Logger()
        {
            LogManager.LoadConfiguration("/mnt/c/Development/BuisnessLogic/nlog.config"); 
        }

        public void Info(string message)
        {
            lock (lockObject)
            {
                _logger.Info(message);
            }
        }

        public void Warn(string message)
        {
            lock (lockObject)
            {
                _logger.Warn(message);
            }
        }

        public void Error(string message)
        {
            lock (lockObject)
            {
                _logger.Error(message);
            }
        }

        public void Fatal(string message)
        {
            lock (lockObject)
            {
                _logger.Fatal(message);
            }
        }

        public void Debug(string message)
        {
            lock (lockObject)
            {
                _logger.Debug(message);
            }
        }
    }
}
