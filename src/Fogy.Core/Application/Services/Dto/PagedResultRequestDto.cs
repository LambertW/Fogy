using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Application.Services.Dto
{
    public class PagedResultRequestDto : IPagedResultRequest
    {
        public PagedResultRequestDto()
        {
        }

        public int PageIndex { get; set; } = 1;

        public int ItemsPerPage { get; set; } = 10;

        public object Predicate { get; set; }

        public IList<ISortedResultItem> Sorts { get; set; } = new List<ISortedResultItem>();
    }
}
