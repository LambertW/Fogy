using Fogy.Core.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.NLog
{
    public class Logger<T> : ILogger<T>
    {
        private readonly ILogger _logger;

        public Logger()
        {
            _logger = LogManager.GetLogger(typeof(T).FullName);
        }

        public void Debug(string msg)
        {
            _logger.Debug(msg);
        }

        public void Error(string msg)
        {
            _logger.Error(msg);
        }

        public void Error(string msg, Exception ex)
        {
            _logger.Error(ex, msg);
        }

        public void Fatal(string msg)
        {
            _logger.Fatal(msg);
        }

        public void Info(string msg)
        {
            _logger.Info(msg);
        }

        public void Warn(string msg)
        {
            _logger.Warn(msg);
        }
    }
}
