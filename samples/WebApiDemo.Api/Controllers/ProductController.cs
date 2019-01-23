using Fogy.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiDemo.Application.Application.Dtos;
using WebApiDemo.Dapper.Repositories;

namespace WebApiDemo.Api.Controllers
{
    public class ProductController : ApiController
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> GetProduct(int id)
        {
            var product = await _productRepository.GetAsync(id);

            return product.MapTo<ProductDto>();
        }
    }
}
