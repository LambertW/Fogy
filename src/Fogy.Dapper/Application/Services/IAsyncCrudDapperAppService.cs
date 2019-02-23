using Fogy.Core.Application.Services;
using Fogy.Core.Application.Services.Dto;
using Fogy.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Dapper.Application.Services
{
    public interface IAsyncCrudDapperAppService<TEntity, TEntityDto, TPrimaryKey, in TGetAllInput, in TInsertInput, in TUpdateInput, in TGetInput, in TDeleteInput> 
        : IApplicationService
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput: IEntityDto<TPrimaryKey>
    {
        Task<TEntityDto> Get(TGetInput input);

        Task<TEntityDto> Insert(TInsertInput input);

        Task<TEntityDto> Update(TUpdateInput input);

        Task<bool> Delete(TDeleteInput input);

        Task<PagedResultDto<TEntityDto>> GetAll(TGetAllInput input);

        Task<List<TEntityDto>> GetList(Expression<Func<TEntity, bool>> predicate);
    }
}
