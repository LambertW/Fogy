using Fogy.Core.Application.Services;
using Fogy.Core.Application.Services.Dto;
using Fogy.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Dapper.Application.Services
{
    public interface IAsyncCrudDapperAppService<TEntity, TEntityDto, TPrimaryKey, in TInsertInput, in TUpdateInput> : IAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TInsertInput, TUpdateInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
    }
}
