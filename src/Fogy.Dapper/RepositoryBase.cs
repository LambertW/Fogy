using Fogy.Core.Domain.Entities;
using Fogy.Core.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Fogy.Core.Application.Services.Dto;
using Fogy.Core.Domain.Entities.Auditing;

namespace Fogy.Dapper
{
    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        public string ConnectionString { get; }

        public RepositoryBase(IConnectionStringProvider provider)
        {
            ConnectionString = provider.GetConnectionString();
        }

        protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> executeFunc)
        {
            try
            {
                using(var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    return await executeFunc(connection);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception($"{GetType().FullName}.WithConnection() executed with a SQL timeout exception", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception($"{GetType().FullName}.WithConnection() executed with a SQL exception (not timeout)", ex);
            }
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            return await WithConnection(async c =>
            {
                if (entity is IHasDeletionTime)
                    ((IHasDeletionTime)entity).DeletionTime = DateTime.Now;
                //TODO SoftDelete

                return await c.DeleteAsync(entity);
            });
        }

        public virtual async Task<bool> DeleteAsync(TPrimaryKey id)
        {
            return await WithConnection(async c =>
            {
                var entity = await c.GetAsync<TEntity>(id);

                if (entity is IHasDeletionTime)
                    ((IHasDeletionTime)entity).DeletionTime = DateTime.Now;

                return await c.DeleteAsync(entity);
            });
        }

        public virtual async Task<IEnumerable<TEntity>> GetListAsync()
        {
            return await WithConnection(async c =>
            {
                return await c.GetListAsync<TEntity>();
            });
        }

        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            return await WithConnection(async c =>
            {
                return await c.GetAsync<TEntity>(id);
            });
        }

        public virtual async Task<TPrimaryKey> InsertAsync(TEntity entity)
        {
            return await WithConnection(async c =>
            {
                if (entity is IHasCreationTime)
                    ((IHasCreationTime)entity).CreationTime = DateTime.Now;

                return await c.InsertAsync(entity);
            });
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            return await WithConnection(async c =>
            {
                if (entity is IHasModificationTime)
                    ((IHasModificationTime)entity).LastModificationTime = DateTime.Now;

                return await c.UpdateAsync(entity);
            });
        }

        public virtual async Task<PagedResultDto<TEntity>> GetPagedAsync(IPagedResultRequest request)
        {
            return await WithConnection(async c =>
            {
                var queryCount = await c.CountAsync<TEntity>(request.Predicate);

                var sortList = new List<ISort>();
                if (!request.Sorts.Any())
                {
                    sortList.Add(Predicates.Sort<TEntity>(t => t.Id));
                }
                else
                {
                    sortList.AddRange(request.Sorts.Select(t => new Sort { Ascending = t.Ascending, PropertyName = t.PropertyName }));
                }

                var queryResult = await c.GetPageAsync<TEntity>(request.Predicate, sortList, request.PageIndex - 1, request.ItemsPerPage);

                var result = new PagedResultDto<TEntity>(request, queryCount, queryResult.ToList());

                return result;
            });
        }

        public virtual async Task<IEnumerable<TEntity>> GetListAsync(object predicate)
        {
            return await WithConnection(async c =>
            {
                return await c.GetListAsync<TEntity>(predicate);
            });
        }

        public virtual async Task<PagedResultDto<TEntity>> GetPagedAsync(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            keyword = $"%{keyword}%";

            return await GetPagedAsync(null, pageIndex, pageSize);
        }

        public virtual async Task<PagedResultDto<TEntity>> GetPagedAsync(object predicate, int pageIndex = 1, int pageSize = 10)
        {
            var request = new PagedResultRequestDto
            {
                ItemsPerPage = pageSize,
                PageIndex = pageIndex,
                Predicate = predicate
            };

            return await GetPagedAsync(request);
        }

        public virtual async Task<int> CountAsync(object predicate)
        {
            return await WithConnection(async c =>
            {
                return await c.CountAsync<TEntity>(predicate);
            });
        }

        public virtual async Task<bool> DeleteAsync(object predicate)
        {
            return await WithConnection(async c =>
            {
                return await c.DeleteAsync(predicate);
            });
        }
    }

    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, int> where TEntity : class, IEntity
    {
        public RepositoryBase(IConnectionStringProvider provider)
            : base(provider)
        {
        }
    }
}
