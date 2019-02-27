using Fogy.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Domain.Uow
{
    public interface IConnectionStringProvider : ISingletonDependency
    {
        string GetConnectionString();

        string GetConnectionString(string connectionStringName);
    }
}
