using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Application.Services.Dto
{
    public interface IPagedResult<T>
    {
        int CurrentPage { get; set; }

        long TotalPages { get; }

        long TotalItems { get; set; }

        int ItemsPerPage { get; set; }

        IReadOnlyList<T> Items { get; set; }
    }
}
