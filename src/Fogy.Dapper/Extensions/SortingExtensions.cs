using DapperExtensions;
using Fogy.Core;
using Fogy.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Dapper
{
    internal static class SortingExtensions
    {
        public static List<ISort> ToSortable<TEntity, TPrimaryKey>(this Expression<Func<TEntity, object>>[] sortingExpression, bool ascending = true) where TEntity : class, IEntity<TPrimaryKey>
        {
            var sortList = new List<ISort>();

            if(sortingExpression == null || !sortingExpression.Any())
            {
                sortList.Add(new Sort { Ascending = ascending, PropertyName = nameof(IEntity.Id) });
                return sortList;
            }

            sortingExpression.ToList().ForEach(sortExpression =>
            {
                MemberInfo sortProperty = ReflectionHelper.GetProperty(sortExpression);
                sortList.Add(new Sort { Ascending = ascending, PropertyName = sortProperty.Name });
            });

            return sortList;
        }
    }
}
