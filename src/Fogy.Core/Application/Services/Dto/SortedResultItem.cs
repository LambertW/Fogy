using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Application.Services.Dto
{
    public class SortedResultItem : ISortedResultItem
    {
        public bool Ascending { get; set; }

        public string PropertyName { get; set; }
    }
}
