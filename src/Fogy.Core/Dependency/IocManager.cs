using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Fogy.Core.Dependency
{
    public class IocManager : IIocManager
    {
        public IContainer IocContainer => throw new NotImplementedException();
    }
}
