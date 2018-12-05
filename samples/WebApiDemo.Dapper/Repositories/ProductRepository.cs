using Fogy.Core.Domain.Repositories;
using Fogy.Dapper;
using WebApiDemo.Core.Domain;

namespace WebApiDemo.Dapper.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IConnectionStringProvider provider) 
            : base(provider)
        {
        }
    }
}
