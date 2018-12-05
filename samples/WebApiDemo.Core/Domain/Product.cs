using Fogy.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Core.Domain
{
    public class Product : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }
    }
}
