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

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            return await WithConnection(async c =>
            {
                return await c.DeleteAsync(entity);
            });
        }

        public async Task<bool> DeleteAsync(TPrimaryKey id)
        {
            return await WithConnection(async c =>
            {
                var entity = await c.GetAsync<TEntity>(id);

                return await c.DeleteAsync(entity);
            });
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await WithConnection(async c =>
            {
                return await c.GetListAsync<TEntity>();
            });
        }

        public async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            return await WithConnection(async c =>
            {
                return await c.GetAsync<TEntity>(id);
            });
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            return await WithConnection(async c =>
            {
                return await c.InsertAsync(entity);
            });
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            return await WithConnection(async c =>
            {
                return await c.UpdateAsync(entity);
            });
        }
    }
}
