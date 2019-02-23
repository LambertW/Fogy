using DapperExtensions;
using Fogy.Core;
using Fogy.Core.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace Fogy.Dapper.Expressions
{
    internal static class DapperExpressionExtensions
    {
        public static IPredicate ToPredicateGroup<TEntity, TPrimaryKey>(this Expression<Func<TEntity, bool>> expression) where TEntity : class, IEntity<TPrimaryKey>
        {
            Check.NotNull(expression, nameof(expression));

            var dev = new DapperExpressionVisitor<TEntity, TPrimaryKey>();
            IPredicate pg = dev.Process(expression);

            return pg;
        }
    }
}
