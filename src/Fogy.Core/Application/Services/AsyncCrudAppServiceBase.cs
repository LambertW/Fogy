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
    public abstract class AsyncCrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey> : IAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        public IObjectMapper ObjectMapper { get; set; }
        protected readonly IRepository<TEntity, TPrimaryKey> Repository;

        protected AsyncCrudAppServiceBase(IRepository<TEntity, TPrimaryKey> repository)
        {
            Repository = repository;
        }

        public async virtual Task<TEntityDto> Get(TPrimaryKey id)
        {
            var entity = await Repository.GetAsync(id);
            return ObjectMapper.Map<TEntityDto>(entity);
        }

        public async virtual Task<TPrimaryKey> Insert(TEntityDto input)
        {
            var entity = ObjectMapper.Map<TEntity>(input);
            var primaryKey = await Repository.InsertAsync(entity);

            return primaryKey;
        }

        public async virtual Task<bool> Update(TEntityDto input)
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

        protected virtual void MapToEntity(TEntityDto updateInput, TEntity entity)
        {
            ObjectMapper.Map(updateInput, entity);
        }
    }
}
