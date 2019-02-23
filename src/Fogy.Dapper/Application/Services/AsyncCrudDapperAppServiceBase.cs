using Fogy.Core.Application.Services.Dto;
using Fogy.Core.Domain.Entities;
using Fogy.Core.ObjectMapper;
using Fogy.Dapper.Application.Services.Dto;
using Fogy.Dapper.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fogy.Dapper.Application.Services
{
    public abstract class AsyncCrudDapperAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TInsertInput, TUpdateInput, TGetInput, TDeleteInput> 
        : IAsyncCrudDapperAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TInsertInput, TUpdateInput, TGetInput, TDeleteInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput : IEntityDto<TPrimaryKey>
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

            await Repository.UpdateAsync(entity);

            return MapToEntityDto(entity);
        }

        public async virtual Task<bool> Delete(TDeleteInput input)
        {
            return await Repository.DeleteAsync(input.Id);
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

        public async virtual Task<TEntityDto> Get(TGetInput input)
        {
            var entity = await Repository.GetAsync(input.Id);
            return MapToEntityDto(entity);
        }

        public async virtual Task<PagedResultDto<TEntityDto>> GetAll(TGetAllInput input)
        {
            Expression<Func<TEntity, bool>> expression = null;
            Tuple<Expression<Func<TEntity, object>>[], bool> sorting = null;
            int pageNumber = 0;
            int itemsPerPage = 0;

            int totalCount = 0;
            IEnumerable<TEntity> list = null;

            expression = ApplyFiltering(input);

            sorting = ApplySorting(input);

            var pageInput = input as IPagedResultRequest;
            if(pageInput != null)
            {
                pageNumber = pageInput.PageIndex - 1;
                if (pageNumber < 0) pageNumber = 0;

                itemsPerPage = pageInput.ItemsPerPage;
                list = await Repository.GetAllPagedAsync(expression, pageNumber, itemsPerPage, sorting.Item2, sorting.Item1);
            }
            else
            {
                list = await Repository.GetAllAsync(expression, sorting.Item2, sorting.Item1);
            }

            totalCount = await Repository.CountAsync(expression);

            return new PagedResultDto<TEntityDto>(pageNumber, itemsPerPage, totalCount, list.Select(MapToEntityDto).ToList());
        }

        protected virtual Tuple<Expression<Func<TEntity, object>>[], bool> ApplySorting(TGetAllInput input)
        {
            Expression<Func<TEntity, object>>[] sortingExpression;
            var asceding = true;
            var sortInput = input as IDapperSortedRequest<TEntity, TPrimaryKey>;
            if(sortInput != null)
            {
                asceding = sortInput.Ascending;
                sortingExpression = sortInput.SortingExpression?.ToArray();
            }
            else
            {
                sortingExpression = new Expression<Func<TEntity, object>>[] { item => item.Id };
            }

            return new Tuple<Expression<Func<TEntity, object>>[], bool>(sortingExpression, asceding);
        }

        protected virtual Expression<Func<TEntity, bool>> ApplyFiltering(TGetAllInput input)
        {
            var filterInput = input as IDapperFilterRequest<TEntity, TPrimaryKey>;
            if (filterInput != null)
                return filterInput.FiltersExpression;

            return null;
        }

        public async virtual Task<List<TEntityDto>> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            var list = await Repository.GetAllAsync(predicate);
            return ObjectMapper.Map<List<TEntityDto>>(list);
        }
    }
}
