using Fogy.Core.Application.Services.Dto;
using Fogy.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Domain.Repositories
{
    public interface IRepository
    {
    }

    public interface IRepository<TEntity, TPrimaryKey> : IRepository where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Select/Get/Query

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetAsync(TPrimaryKey id);

        Task<PagedResultDto<TEntity>> GetPagedAsync(IPagedResultRequest request);

        #endregion

        Task<TPrimaryKey> InsertAsync(TEntity entity);

        Task<bool> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(TEntity entity);

        Task<bool> DeleteAsync(TPrimaryKey id);
    }

    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : class, IEntity<int>
    {

    }
}
