using Fogy.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.Application.Application.Dtos;
using WebApiDemo.Dapper.Repositories;

namespace WebApiDemo.Application.Application
{
    public class ProductAppService : IProductAppService
    {
        private readonly IProductRepository _productRepository;

        public ProductAppService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            return await _productRepository.DeleteAsync(id);    
        }

        public async Task<ProductDto> GetProduct(int id)
        {
            var product = await _productRepository.GetAsync(id);
            return product.MapTo<ProductDto>();
        }

        public async Task<List<ProductDto>> GetProducts()
        {
            var list = await  _productRepository.GetAllAsync();
            return list.MapTo<List<ProductDto>>();
        }

        public async Task<ProductDto> InsertAsync(ProductDto dto)
        {
            var product = await _productRepository.InsertAndGetIdAsync(new Core.Domain.Product
            {
                Name = dto.Name,
                Price = dto.Price
            });

            return dto;
        }

        public async Task<bool> UpdateProduct(int id, ProductDto dto)
        {
            var product = await _productRepository.GetAsync(id);
            product.Name = dto.Name;
            product.Price = dto.Price;

            return await _productRepository.UpdateAsync(product);
        }
    }
}
