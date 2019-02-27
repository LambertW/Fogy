using Dapper;
using DapperExtensions;
using Fogy.Core.Domain.Entities;
using Fogy.Core.Domain.Entities.Auditing;
using Fogy.Core.Domain.Repositories;
using Fogy.Core.Domain.Uow;
using Fogy.Dapper.Expressions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Dapper.Repositories
{
    public abstract class DapperRepositoryBase<TEntity, TPrimaryKey> : IDapperRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        public IDbConnection DbConnection { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }
        public IDbTransaction DbTransaction { get; set; }

        public DapperRepositoryBase(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            DbConnection = unitOfWork.Connection;
            DbTransaction = unitOfWork.Transaction;
        }

        protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> executeFunc)
        {
            try
            {
                if (DbConnection.State != ConnectionState.Open)
                    DbConnection.Open();

                return await executeFunc(DbConnection);
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

                return await c.DeleteAsync(entity, DbTransaction);
            });
        }

        public virtual async Task<bool> DeleteAsync(TPrimaryKey id)
        {
            return await WithConnection(async c =>
            {
                var entity = await c.GetAsync<TEntity>(id, DbTransaction);

                if (entity is IHasDeletionTime)
                    ((IHasDeletionTime)entity).DeletionTime = DateTime.Now;

                return await c.DeleteAsync(entity, DbTransaction);
            });
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await WithConnection(async c =>
            {
                return await c.GetListAsync<TEntity>(DbTransaction);
            });
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            return await WithConnection(async c =>
            {
                if (entity is IHasModificationTime)
                    ((IHasModificationTime)entity).LastModificationTime = DateTime.Now;

                return await c.UpdateAsync(entity, DbTransaction);
            });
        }

        public virtual async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await WithConnection(async c =>
            {
                return await c.GetAsync<TEntity>(predicate, DbTransaction);
            });
        }

        public virtual async Task<TEntity> SingleAsync(TPrimaryKey id)
        {
            return await SingleAsync(CreateEqualityExpressionForId(id));
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await WithConnection(async c =>
            {
                var result = await c.GetListAsync<TEntity>(predicate.ToPredicateGroup<TEntity, TPrimaryKey>(), transaction: DbTransaction);

                return result.FirstOrDefault();
            });
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return await FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, bool ascending = true, params Expression<Func<TEntity, object>>[] sortingExpression)
        {
            return await WithConnection(async c =>
            {
                var pg = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                var result = await c.GetListAsync<TEntity>(pg, sortingExpression.ToSortable<TEntity, TPrimaryKey>(ascending), DbTransaction);

                return result;
            });
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllPagedAsync(Expression<Func<TEntity, bool>> predicate, int pageNumber, int itemsPerPage, string sortingProperty, bool ascending = true)
        {
            return await WithConnection(async c =>
            {
                var pg = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                var result = await c.GetPageAsync<TEntity>(pg, new List<ISort> { new Sort { Ascending = ascending, PropertyName = sortingProperty } }, pageNumber, itemsPerPage, DbTransaction);

                return result;
            });
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllPagedAsync(Expression<Func<TEntity, bool>> predicate, int pageNumber, int itemsPerPage, bool ascending = true, params Expression<Func<TEntity, object>>[] sortingExpression)
        {
            return await WithConnection(async c =>
            {
                var pg = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                var result = await c.GetPageAsync<TEntity>(pg, sortingExpression.ToSortable<TEntity, TPrimaryKey>(ascending), pageNumber, itemsPerPage, DbTransaction);

                return result;
            });
        }

        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            return await WithConnection(async c =>
            {
                return await c.GetAsync<TEntity>(id, DbTransaction);
            });
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await WithConnection(async c =>
            {
                return await c.CountAsync<TEntity>(predicate.ToPredicateGroup<TEntity, TPrimaryKey>(), DbTransaction);
            });
        }

        public virtual async Task<IEnumerable<TEntity>> QueryAsync(string query, object parameters = null)
        {
            return await WithConnection(async c =>
            {
                return await c.QueryAsync<TEntity>(query, parameters, DbTransaction);
            });
        }

        public virtual async Task<IEnumerable<TAny>> QueryAsync<TAny>(string query, object parameters = null)
        {
            return await WithConnection(async c =>
            {
                return await c.QueryAsync<TAny>(query, parameters, DbTransaction);
            });
        }

        public virtual async Task<int> ExecuteAsync(string query, object parameters = null)
        {
            return await WithConnection(async c =>
            {
                return await c.ExecuteAsync(query, parameters, DbTransaction);
            });
        }

        public virtual async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            return await WithConnection(async c =>
            {
                if (entity is IHasCreationTime)
                    ((IHasCreationTime)entity).CreationTime = DateTime.Now;

                return await c.InsertAsync(entity, DbTransaction);
            });
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await WithConnection(async c =>
            {
                if (entity is IHasCreationTime)
                    ((IHasCreationTime)entity).CreationTime = DateTime.Now;

                return await c.InsertAsync(entity, DbTransaction);
            });
        }

        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            ParameterExpression lambdaParam = Expression.Parameter(typeof(TEntity));

            BinaryExpression lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
            );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }

    public abstract class DapperRepositoryBase<TEntity> : DapperRepositoryBase<TEntity, int> where TEntity : class, IEntity
    {
        public DapperRepositoryBase(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
