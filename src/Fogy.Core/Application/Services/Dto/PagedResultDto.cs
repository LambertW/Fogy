using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Application.Services.Dto
{
    [Serializable]
    public class PagedResultDto<T> : IPagedResult<T>
    {
        public int CurrentPage { get; set; }

        public long TotalPages
        {
            get
            {
                long result = TotalItems / ItemsPerPage;

                if ((TotalItems % ItemsPerPage) != 0)
                    result++;

                return result;
            }
        }

        public long TotalItems { get; set; }

        public int ItemsPerPage { get; set; }

        public IReadOnlyList<T> Items { get; set; }

        public PagedResultDto(IPagedResultRequest request, long totalItems, IReadOnlyList<T> items)
        {
            CurrentPage = request.PageIndex;
            TotalItems = totalItems;
            ItemsPerPage = request.PageSize;
            Items = items;
        }

        public PagedResultDto(int pageIndex, int itemsPerPage, long totalItems, IReadOnlyList<T> items)
        {
            CurrentPage = pageIndex;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
            Items = items;
        }
    }
}
