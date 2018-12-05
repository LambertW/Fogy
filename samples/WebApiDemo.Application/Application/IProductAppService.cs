using Fogy.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.Application.Application.Dtos;

namespace WebApiDemo.Application.Application
{
    public interface IProductAppService : ITransientDependency
    {
        Task<List<ProductDto>> GetProducts();

        Task<ProductDto> GetProduct(int id);

        Task<bool> UpdateProduct(int id, ProductDto dto);

        Task<bool> DeleteProduct(int id);

        Task<ProductDto> InsertAsync(ProductDto dto);
    }
}
