using Fogy.Core.Application.Services.Dto;
using Fogy.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Application.Services
{
    public interface IAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey> : IApplicationService
        where TEntity : class, IEntity<TPrimaryKey>
    {
        Task<TEntityDto> Get(TPrimaryKey id);

        Task<TPrimaryKey> Insert(TEntityDto input);

        Task<bool> Update(TEntityDto input);

        Task<bool> Delete(TPrimaryKey id);

        Task<List<TEntityDto>> GetList();

        Task<List<TEntityDto>> GetList(object predicate);

        Task<IPagedResult<TEntityDto>> GetList(string keyword, int pageIndex, int pageSize);
    }
}
