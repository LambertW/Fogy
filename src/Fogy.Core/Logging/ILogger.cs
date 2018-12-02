using Fogy.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Logging
{
    public interface ILogger<T> : ITransientDependency
    {
        void Info(string msg);

        void Warn(string msg);

        void Error(string msg);

        void Error(string msg, Exception ex);

        void Fatal(string msg);

        void Debug(string msg);
    }
}
