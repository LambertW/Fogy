using MvcDemo.Core.System;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcDemo.SqlSugar.Repositories.System
{
	public class ItemDemoRepository : SqlSugarRepositoryBase<ItemDemo, Guid>, IItemDemoRepository
	{
		public ItemDemoRepository(SqlSugarClient dbContext) : base(dbContext)
		{
		}

	}
}
