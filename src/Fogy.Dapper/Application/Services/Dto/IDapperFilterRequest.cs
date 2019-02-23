using Fogy.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Dapper.Application.Services.Dto
{
    public interface IDapperFilterRequest<TEntity, TPrimaryKey> where TEntity : IEntity<TPrimaryKey>
    {
        Expression<Func<TEntity, bool>> FiltersExpression { get; set; }
    }
}
