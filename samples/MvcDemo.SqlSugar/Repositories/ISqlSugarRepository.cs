using Fogy.Core.Domain.Entities;
using Fogy.Core.Domain.Repositories;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MvcDemo.SqlSugar.Repositories
{
	public interface ISqlSugarRepository<TEntity, TPrimaryKey> : IRepository where TEntity : class, IEntity<TPrimaryKey>, new()
	{
		IDeleteable<TEntity> AsDeleteable();
		IInsertable<TEntity> AsInsertable(TEntity insertObj);
		IInsertable<TEntity> AsInsertable(TEntity[] insertObjs);
		IInsertable<TEntity> AsInsertable(List<TEntity> insertObjs);
		ISugarQueryable<TEntity> AsQueryable();
		IUpdateable<TEntity> AsUpdateable(TEntity updateObj);
		IUpdateable<TEntity> AsUpdateable(TEntity[] updateObjs);
		IUpdateable<TEntity> AsUpdateable(List<TEntity> updateObjs);
		int Count(Expression<Func<TEntity, bool>> whereExpression);
		bool Delete(Expression<Func<TEntity, bool>> whereExpression);
		bool Delete(TEntity deleteObj);
		bool DeleteById(TPrimaryKey id);
		bool DeleteByIds(TPrimaryKey[] ids);
		TEntity GetById(TPrimaryKey id);
		List<TEntity> GetList(Expression<Func<TEntity, bool>> whereExpression);
		List<TEntity> GetList();
		List<TEntity> GetPageList(List<IConditionalModel> conditionalList, PageModel page, Expression<Func<TEntity, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);
		List<TEntity> GetPageList(List<IConditionalModel> conditionalList, PageModel page);
		List<TEntity> GetPageList(Expression<Func<TEntity, bool>> whereExpression, PageModel page, Expression<Func<TEntity, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);
		List<TEntity> GetPageList(Expression<Func<TEntity, bool>> whereExpression, PageModel page);
		TEntity GetSingle(Expression<Func<TEntity, bool>> whereExpression);
		bool Insert(TEntity insertObj);
		bool InsertRange(TEntity[] insertObjs);
		bool InsertRange(List<TEntity> insertObjs);
		TPrimaryKey InsertAndGetId(TEntity insertObj);
		bool Any(Expression<Func<TEntity, bool>> whereExpression);
		bool Update(TEntity updateObj);
		bool Update(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> whereExpression);
		bool UpdateRange(TEntity[] updateObjs);
		bool UpdateRange(List<TEntity> updateObjs);
	}
}
