using Fogy.Core;
using Fogy.Core.Domain.Entities;
using Fogy.Core.Extensions;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MvcDemo.SqlSugar.Repositories
{
	/// <summary>
	/// Implement functions like what SimpleClient done.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TPrimaryKey"></typeparam>
	public abstract class SqlSugarRepositoryBase<TEntity, TPrimaryKey> : ISqlSugarRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>, new()
	{
		protected SqlSugarClient Context { get; set; }

		private List<FilterConfiguration> _filters = new List<FilterConfiguration>();

		protected ConcurrentDictionary<Type, Type[]> InterfacesOfTEntity { get; set; } = new ConcurrentDictionary<Type, Type[]>();

		protected Type[] GetInterfacesOfTEntity()
		{
			//// GET TEntity type.
			//var entityType = queryable.GetType().GetInterfaces()
			//	.Where(i => i.IsGenericType)
			//	.Where(i => i.GetGenericTypeDefinition() == typeof(ISugarQueryable<>))
			//	.Select(i => i.GetGenericArguments().First())
			//	.First();

			//// Get interfaces from TEntity.
			//var interfaces = entityType.GetInterfaces();

			if (InterfacesOfTEntity.ContainsKey(typeof(TEntity)))
			{
				return InterfacesOfTEntity[typeof(TEntity)];
			}

			var interfaces = typeof(TEntity).GetInterfaces();
			InterfacesOfTEntity.TryAdd(typeof(TEntity), interfaces);

			return interfaces;
		}

		public SqlSugarRepositoryBase(SqlSugarClient context)
		{
			Context = context;
			_filters.Add(new FilterConfiguration(nameof(ISoftDelete), true));
		}

		#region Deleteable

		public IDeleteable<TEntity> AsDeleteable()
		{
			var deleteable = Context.Deleteable<TEntity>();

			ApplyDeleteFilterList(deleteable);

			return deleteable;
		}
		#endregion

		#region Queryable

		public ISugarQueryable<TEntity> AsQueryable()
		{
			var queryable = Context.Queryable<TEntity>();

			ApplyQueryFilterList(queryable);

			return queryable;
		}
		#endregion

		#region Updateable

		/// <summary>
		/// 未指定具体更新内容, 批量更新
		/// </summary>
		/// <returns></returns>
		public IUpdateable<TEntity> AsUpdateable()
		{
			return AsUpdateable(new TEntity[] { new TEntity() });
		}

		public IUpdateable<TEntity> AsUpdateable(TEntity updateObj)
		{
			return AsUpdateable(new TEntity[] { updateObj });
		}
		
		public IUpdateable<TEntity> AsUpdateable(List<TEntity> updateObjs)
		{
			return AsUpdateable(updateObjs.ToArray());
		}

		public IUpdateable<TEntity> AsUpdateable(TEntity[] updateObjs)
		{
			CheckValidIds(updateObjs);

			var updateable = Context.Updateable(updateObjs);
			//ApplyUpdateFilterList(updateable);

			return updateable;
		}

		private void CheckValidIds(TEntity[] updateObjs)
		{
			foreach (var item in updateObjs)
			{
				if (item.Id.Equals(default(TPrimaryKey)))
					throw new ArgumentException($"invalid parameter {updateObjs}");
			}
		}

		#endregion

		#region Insertable

		public IInsertable<TEntity> AsInsertable(TEntity insertObj)
		{
			return AsInsertable(new TEntity[] { insertObj });
		}

		public IInsertable<TEntity> AsInsertable(List<TEntity> insertObjs)
		{
			return AsInsertable(insertObjs.ToArray());
		}

		public IInsertable<TEntity> AsInsertable(TEntity[] insertObjs)
		{
			GenerateValueForEmptyPrimaryKey(insertObjs);

			return Context.Insertable(insertObjs);
		}

		private void GenerateValueForEmptyPrimaryKey(TEntity[] insertObjs)
		{
			if (typeof(TPrimaryKey) == typeof(Guid))
			{
				foreach (var item in insertObjs)
				{
					if (Guid.Parse(item.Id.ToString()) == Guid.Empty)
					{
						item.Id = (TPrimaryKey)TypeDescriptor.GetConverter(typeof(TPrimaryKey)).ConvertFromInvariantString(GetNextGuid().ToString());
					}
				}
			}
			else if (typeof(TPrimaryKey) == typeof(string))
			{
				foreach (var item in insertObjs)
				{
					if (item.Id.ToString().IsNullOrEmpty())
					{
						item.Id = (TPrimaryKey)TypeDescriptor.GetConverter(typeof(TPrimaryKey)).ConvertFromInvariantString(Guid.NewGuid().ToString());
					}
				}
			}
		}

		#endregion

		public Guid GetNextGuid()
		{
			byte[] b = Guid.NewGuid().ToByteArray();
			DateTime dateTime = new DateTime(1900, 1, 1);
			DateTime now = DateTime.Now;
			TimeSpan timeSpan = new TimeSpan(now.Ticks - dateTime.Ticks);
			TimeSpan timeOfDay = now.TimeOfDay;
			byte[] bytes1 = BitConverter.GetBytes(timeSpan.Days);
			byte[] bytes2 = BitConverter.GetBytes((long)(timeOfDay.TotalMilliseconds / 3.333333));
			Array.Reverse(bytes1);
			Array.Reverse(bytes2);
			Array.Copy(bytes1, bytes1.Length - 2, b, b.Length - 6, 2);
			Array.Copy(bytes2, bytes2.Length - 4, b, b.Length - 4, 4);
			return new Guid(b);
		}

		#region Saveable
		public ISaveable<TEntity> AsSaveable(TEntity insertObj)
		{
			return AsSaveable(new List<TEntity> { insertObj });
		}

		public ISaveable<TEntity> AsSaveable(List<TEntity> insertObjs)
		{
			GenerateValueForEmptyPrimaryKey(insertObjs.ToArray());

			return Context.Saveable(insertObjs);
		}
		#endregion

		#region Apply Filters

		public IDisposable DisableFilter(params string[] filterNames)
		{
			var disabledFilters = new List<string>();

			foreach (var filterName in filterNames)
			{
				var filterIndex = GetFilterIndex(filterName);

				if (_filters[filterIndex].IsEnabled) {
					disabledFilters.Add(filterName);
					_filters[filterIndex] = new FilterConfiguration(filterName, false);
				}
			}

			return new DisposeAction(() => EnableFilter(disabledFilters.ToArray()));
		}

		public IDisposable EnableFilter(params string[] filterNames)
		{
			var enabledFilters = new List<string>();

			foreach (var filterName in filterNames)
			{
				var filterIndex = GetFilterIndex(filterName);

				if (!_filters[filterIndex].IsEnabled)
				{
					enabledFilters.Add(filterName);
					_filters[filterIndex] = new FilterConfiguration(filterName, true);
				}
			}

			return new DisposeAction(() => DisableFilter(enabledFilters.ToArray()));
		}

		private int GetFilterIndex(string filterName)
		{
			var filterIndex = _filters.FindIndex(t => t.FilterName == filterName);
			if (filterIndex < 0)
				throw new FogyException($"filter name {filterName} not exists.");

			return filterIndex;
		}

		protected virtual void ApplyDeleteFilterList(IDeleteable<TEntity> deleteable)
		{
			if (_filters[GetFilterIndex(nameof(ISoftDelete))].IsEnabled && GetInterfacesOfTEntity().Contains(typeof(ISoftDelete)))
			{
				var dbColumnName = Context.EntityMaintenance.GetDbColumnName<TEntity>(nameof(ISoftDelete.IsDeleted));
				deleteable = deleteable.Where($"{dbColumnName} = 0");
			}
		}

		protected virtual void ApplyQueryFilterList(ISugarQueryable<TEntity> queryable)
		{
			if (_filters[GetFilterIndex(nameof(ISoftDelete))].IsEnabled && GetInterfacesOfTEntity().Contains(typeof(ISoftDelete)))
			{
				var dbColumnName = Context.EntityMaintenance.GetDbColumnName<TEntity>(nameof(ISoftDelete.IsDeleted));
				queryable = queryable.Where($"{dbColumnName} = 0");
			}
		}

		#endregion

		#region Query

		public int Count()
		{
			return Count(null);
		}

		public int Count(Expression<Func<TEntity, bool>> whereExpression)
		{
			return AsQueryable().WhereIF(whereExpression != null, whereExpression).Count();
		}

		public bool Any(Expression<Func<TEntity, bool>> whereExpression)
		{
			return AsQueryable().WhereIF(whereExpression != null, whereExpression).Any();
		}

		public List<TEntity> GetList(Expression<Func<TEntity, bool>> whereExpression)
		{
			return AsQueryable().WhereIF(whereExpression != null, whereExpression).ToList();
		}

		public List<TEntity> GetList()
		{
			return AsQueryable().ToList();
		}

		public List<TEntity> GetPageList(List<IConditionalModel> conditionalList, PageModel page, Expression<Func<TEntity, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
		{
			int count = 0;
			var result = AsQueryable().OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(conditionalList).ToPageList(page.PageIndex, page.PageSize, ref count);
			page.PageCount = count;

			return result;
		}

		public List<TEntity> GetPageList(List<IConditionalModel> conditionalList, PageModel page)
		{
			return GetPageList(conditionalList, page, null);
		}

		public List<TEntity> GetPageList(Expression<Func<TEntity, bool>> whereExpression, PageModel page, Expression<Func<TEntity, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
		{
			int count = 0;
			var result = AsQueryable().OrderByIF(orderByExpression != null, orderByExpression, orderByType).WhereIF(whereExpression != null, whereExpression).ToPageList(page.PageIndex, page.PageSize, ref count);
			page.PageCount = count;
			return result;
		}

		public List<TEntity> GetPageList(Expression<Func<TEntity, bool>> whereExpression, PageModel page)
		{
			return GetPageList(whereExpression, page, null);
		}

		public TEntity GetById(TPrimaryKey id)
		{
			return AsQueryable().InSingle(id);
		}

		public TEntity GetSingle(Expression<Func<TEntity, bool>> whereExpression)
		{
			return AsQueryable().WhereIF(whereExpression != null, whereExpression).Single();
		}

		#endregion

		#region Delete

		public bool Delete(Expression<Func<TEntity, bool>> whereExpression)
		{
			if (GetInterfacesOfTEntity().Contains(typeof(ISoftDelete)))
			{
				var list = GetList(whereExpression);
				list.ForEach(t => ((ISoftDelete)t).IsDeleted = true);
				return AsUpdateable(list).ExecuteCommand() > 0;
			}

			return AsDeleteable().Where(whereExpression).ExecuteCommand() > 0;
		}

		public bool Delete(TEntity deleteObj)
		{
			return DeleteByIds(new TPrimaryKey[] { deleteObj.Id });
		}

		public bool DeleteById(TPrimaryKey id)
		{
			return DeleteByIds(new TPrimaryKey[] { id });
		}

		public bool DeleteByIds(TPrimaryKey[] ids)
		{
			if(GetInterfacesOfTEntity().Contains(typeof(ISoftDelete)))
			{
				return Context.Ado.ExecuteCommand($"UPD {Context.EntityMaintenance.GetTableName<TEntity>()} WHERE Id = @Id AND {Context.EntityMaintenance.GetDbColumnName<TEntity>(nameof(ISoftDelete))} = ", ids) > 0;
			}

			return AsDeleteable().In(ids).ExecuteCommand() > 0;
		}
		#endregion

		#region Insert

		public bool Insert(TEntity insertObj)
		{
			return InsertRange(new TEntity[] { insertObj });
		}

		public bool InsertRange(List<TEntity> insertObjs)
		{
			return InsertRange(insertObjs.ToArray());
		}

		public bool InsertRange(TEntity[] insertObjs)
		{
			return AsInsertable(insertObjs).ExecuteCommand() > 0;
		}

		/// <summary>
		/// Only use for int/long type primary key.
		/// </summary>
		/// <param name="insertObj"></param>
		/// <returns></returns>
		public TPrimaryKey InsertAndGetId(TEntity insertObj)
		{
			AsInsertable(insertObj).ExecuteCommandIdentityIntoEntity();

			return insertObj.Id;
		}
		#endregion

		#region Update

		public bool Update(TEntity updateObj)
		{
			return UpdateRange(new TEntity[] { updateObj });
		}

		public bool Update(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> whereExpression)
		{
			return AsUpdateable().UpdateColumns(columns).Where(whereExpression).ExecuteCommand() > 0;
		}

		public bool UpdateRange(List<TEntity> updateObjs)
		{
			return UpdateRange(updateObjs.ToArray());
		}

		public bool UpdateRange(TEntity[] updateObjs)
		{
			return AsUpdateable(updateObjs).ExecuteCommand() > 0;
		}
		#endregion

		#region InsertOrUpdate
		public TEntity InsertOrUpdate(TEntity entity)
		{
			return AsSaveable(entity).ExecuteReturnEntity();
		}

		public int InsertOrUpdate(List<TEntity> entity)
		{


			return AsSaveable(entity).ExecuteReturnList().Count();
		}

		#endregion
	}
}