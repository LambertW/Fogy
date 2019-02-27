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

        public int PageSize { get; set; } = 10;
    }
}
