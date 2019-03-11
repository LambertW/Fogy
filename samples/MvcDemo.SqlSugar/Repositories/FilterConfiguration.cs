using Fogy.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcDemo.SqlSugar.Repositories
{
	public class FilterConfiguration
	{
		public string FilterName { get; }

		public bool IsEnabled { get; }

		public FilterConfiguration(string filterName, bool isEnabled)
		{
			FilterName = filterName;
			IsEnabled = isEnabled;
		}
	}
}
