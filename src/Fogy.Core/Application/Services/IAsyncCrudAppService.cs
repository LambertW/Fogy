using Fogy.Core.Application.Services.Dto;
using Fogy.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fogy.Core.Application.Services
{
    public interface IAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TInsertInput, in TUpdateInput, in TGetInput, in TDeleteInput> : IApplicationService
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        Task<TEntityDto> Get(TGetInput input);

        Task<TEntityDto> Insert(TInsertInput input);

        Task<TEntityDto> Update(TUpdateInput input);

        Task<bool> Delete(TDeleteInput input);

        Task<PagedResultDto<TEntityDto>> GetAll(TGetAllInput input);
    }
}
