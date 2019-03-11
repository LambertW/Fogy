using MvcDemo.Core.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcDemo.SqlSugar.Repositories.System
{
	public interface IItemDemoRepository : ISqlSugarRepository<ItemDemo, Guid>
	{
	}
}
