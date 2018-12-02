using Fogy.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Domain.Repositories
{
    public interface IConnectionStringProvider : ISingletonDependency
    {
        string GetConnectionString();
    }
}
