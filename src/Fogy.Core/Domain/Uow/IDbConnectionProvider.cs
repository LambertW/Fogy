using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Domain.Uow
{
    public interface IDbConnectionProvider
    {
        IDbConnection GetDbConnection();
    }
}
