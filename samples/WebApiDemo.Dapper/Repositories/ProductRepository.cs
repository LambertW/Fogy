using Fogy.Core.Domain.Repositories;
using Fogy.Dapper;
using Fogy.Dapper.Repositories;
using WebApiDemo.Core.Domain;

namespace WebApiDemo.Dapper.Repositories
{
    public class ProductRepository : DapperRepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IConnectionStringProvider provider) 
            : base(provider)
        {
        }
    }
}
