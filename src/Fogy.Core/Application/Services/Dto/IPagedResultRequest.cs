using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Application.Services.Dto
{
    public interface IPagedResultRequest
    {
        int PageIndex { get; set; }

        int ItemsPerPage { get; set; }
    }
}
