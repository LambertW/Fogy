using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Logging
{
    public class NullLogger<T> : ILogger<T>
    {
        public static readonly NullLogger<T> Instance;

        static NullLogger()
        {
            Instance = new NullLogger<T>();
        }

        public void Debug(string msg)
        {
        }

        public void Error(string msg)
        {
        }

        public void Error(string msg, Exception ex)
        {
        }

        public void Fatal(string msg)
        {
        }

        public void Info(string msg)
        {
        }

        public void Warn(string msg)
        {
        }
    }
}
