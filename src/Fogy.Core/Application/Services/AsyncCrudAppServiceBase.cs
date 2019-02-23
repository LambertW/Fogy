using Fogy.Core.Application.Services.Dto;
using Fogy.Core.Domain.Entities;
using Fogy.Core.Domain.Repositories;
using Fogy.Core.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Application.Services
{
    public abstract class AsyncCrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TInsertInput, TUpdateInput, TGetInput, TDeleteInput> 
        : IAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TInsertInput, TUpdateInput, TGetInput, TDeleteInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        public virtual IObjectMapper ObjectMapper { get; set; }
        protected readonly IRepository<TEntity, TPrimaryKey> Repository;

        protected AsyncCrudAppServiceBase(IRepository<TEntity, TPrimaryKey> repository)
        {
            Repository = repository;
        }

        public async virtual Task<TEntityDto> Get(TGetInput input)
        {
            var entity = await Repository.GetAsync(input.Id);
            return MapToEntityDto(entity);
        }

        public async virtual Task<TEntityDto> Insert(TInsertInput input)
        {
            var entity = MapToEntity(input);
            var primaryKey = await Repository.InsertAndGetIdAsync(entity);

            return MapToEntityDto(entity);
        }

        public async virtual Task<TEntityDto> Update(TUpdateInput input)
        {
            var entity = await Repository.GetAsync(input.Id);
            MapToEntity(input, entity);

            var result = await Repository.UpdateAsync(entity);

            return MapToEntityDto(entity);
        }

        public async virtual Task<bool> Delete(TDeleteInput input)
        {
            return await Repository.DeleteAsync(input.Id);
        }

        public async virtual Task<PagedResultDto<TEntityDto>> GetAll(TGetAllInput input)
        {
            var list = await Repository.GetAllAsync();

            var totalCount = await Repository.CountAsync(null);
            // TODO
            return new PagedResultDto<TEntityDto>(0, 0, totalCount, list.Select(t => MapToEntityDto(t)).ToList());
        }

        protected virtual void MapToEntity(TUpdateInput updateInput, TEntity entity)
        {
            ObjectMapper.Map(updateInput, entity);
        }

        protected virtual TEntity MapToEntity(TInsertInput entity)
        {
            return ObjectMapper.Map<TEntity>(entity);
        }

        protected virtual TEntityDto MapToEntityDto(TEntity entity)
        {
            return ObjectMapper.Map<TEntityDto>(entity);
        }
    }
}
