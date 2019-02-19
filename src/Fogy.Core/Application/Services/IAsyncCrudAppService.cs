using Fogy.Core.Application.Services.Dto;
using Fogy.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fogy.Core.Application.Services
{
    public interface IAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, in TInsertInput, in TUpdateInput> : IApplicationService
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        Task<TEntityDto> Get(TPrimaryKey id);

        Task<TPrimaryKey> Insert(TInsertInput input);

        Task<bool> Update(TUpdateInput input);

        Task<bool> Delete(TPrimaryKey id);

        Task<List<TEntityDto>> GetList();
    }
}
