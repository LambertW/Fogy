using Fogy.Core.Application.Services.Dto;
using Fogy.Core.Domain.Entities;
using Fogy.Core.ObjectMapper;
using Fogy.Dapper.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Dapper.Application.Services
{
    public abstract class AsyncCrudDapperAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TInsertInput, TUpdateInput> : IAsyncCrudDapperAppService<TEntity, TEntityDto, TPrimaryKey, TInsertInput, TUpdateInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        public virtual IObjectMapper ObjectMapper { get; set; }
        protected readonly IDapperRepository<TEntity, TPrimaryKey> Repository;

        protected AsyncCrudDapperAppServiceBase(IDapperRepository<TEntity, TPrimaryKey> repository)
        {
            Repository = repository;
        }

        public async virtual Task<TEntityDto> Get(TPrimaryKey id)
        {
            var entity = await Repository.GetAsync(id);
            return ObjectMapper.Map<TEntityDto>(entity);
        }

        public async virtual Task<TPrimaryKey> Insert(TInsertInput input)
        {
            var entity = ObjectMapper.Map<TEntity>(input);
            var primaryKey = await Repository.InsertAsync(entity);

            return primaryKey;
        }

        public async virtual Task<bool> Update(TUpdateInput input)
        {
            var entity = await Repository.GetAsync(input.Id);
            MapToEntity(input, entity);

            return await Repository.UpdateAsync(entity);
        }

        public async virtual Task<bool> Delete(TPrimaryKey id)
        {
            return await Repository.DeleteAsync(id);
        }

        public async virtual Task<List<TEntityDto>> GetList()
        {
            var list = await Repository.GetListAsync();
            return ObjectMapper.Map<List<TEntityDto>>(list);
        }

        public async virtual Task<List<TEntityDto>> GetList(object predicate)
        {
            var list = await Repository.GetListAsync(predicate);
            return ObjectMapper.Map<List<TEntityDto>>(list);
        }

        public async virtual Task<IPagedResult<TEntityDto>> GetList(string keyword, int pageIndex, int pageSize)
        {
            var page = await Repository.GetPagedAsync(keyword, pageIndex, pageSize);
            return ObjectMapper.Map<IPagedResult<TEntityDto>>(page);
        }

        protected virtual void MapToEntity(TUpdateInput updateInput, TEntity entity)
        {
            ObjectMapper.Map(updateInput, entity);
        }
    }
}
