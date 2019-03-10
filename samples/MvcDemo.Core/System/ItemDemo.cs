using Fogy.Core.Domain.Entities;
using Fogy.Core.Domain.Entities.Auditing;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcDemo.Core.System
{
	[SugarTable("Sys_Item_Demo")]
	public class ItemDemo : IEntity<Guid>, IHasCreationTime, IHasModificationTime, IPassivable, ISoftDelete
	{
		[SugarColumn(IsPrimaryKey = true)]
		public Guid Id { get; set; }

		public string EnCode { get; set; }

		public Guid ParentId { get; set; }

		public string Name { get; set; }

		public int? Layer { get; set; }

		public int? SortCode { get; set; }

		public string IsTree { get; set; }

		public string Remark { get; set; }

		public string CreateUser { get; set; }

		public string ModifyUser { get; set; }

		public DateTime? LastModificationTime { get; set; }
		public DateTime CreationTime { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
	}
}
