using Fogy.AutoMapper;
using Fogy.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiDemo.Application.Application.Dtos;
using WebApiDemo.Core.Domain;
using WebApiDemo.Dapper.Repositories;

namespace WebApiDemo.Api.Controllers
{
    public class ProductController : ApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductRepository productRepository,
            ILogger<ProductController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> Get()
        {
            var list = await _productRepository.GetAllAsync();
            return list.MapTo<List<ProductDto>>();
        }

        public async Task<ProductDto> GetProduct(int id)
        {
            var product = await _productRepository.GetAsync(id);

            return product.MapTo<ProductDto>();
        }
        
        [HttpPost]
        public async Task Post(ProductDto dto)
        {
            await _productRepository.InsertAsync(dto.MapTo<Product>());
        }

        [HttpPut]
        public async Task Put(int id, ProductDto dto)
        {
            await _productRepository.UpdateAsync(dto.MapTo<Product>());
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var deleted = await _productRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return Ok();
        }
    }
}
