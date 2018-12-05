using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Application.Application.Dtos
{
    public class ProductDto
    {
        public float Price { get; internal set; }
        public string Name { get; internal set; }
    }
}
