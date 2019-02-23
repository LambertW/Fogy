using Fogy.Core.Dependency;
using Fogy.Core.Domain.Repositories;
using Fogy.Dapper.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.Core.Domain;

namespace WebApiDemo.Dapper.Repositories
{
    public interface IProductRepository : IDapperRepository<Product, int>
    {

    }
}
