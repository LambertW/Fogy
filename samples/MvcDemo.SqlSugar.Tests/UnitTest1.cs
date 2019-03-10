using System;
using System.Collections.Generic;
using System.Linq;
using MvcDemo.Core.System;
using MvcDemo.SqlSugar.Repositories.System;
using SqlSugar;
using Xunit;

namespace MvcDemo.SqlSugar.Tests
{
	public class UnitTest1 : IDisposable
	{
		SqlSugarClient db;
		ItemDemoRepository _itemDemoRepository;

		public UnitTest1()
		{
			db = new SqlSugarClient(new ConnectionConfig
			{
				ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=Elight_v1;Integrated Security=True",
				DbType = DbType.SqlServer,
				IsAutoCloseConnection = true,
				ConfigureExternalServices = new ConfigureExternalServices
				{

				}
			});
			_itemDemoRepository = new ItemDemoRepository(db);
		}

		#region Query

		[Fact]
		public void Guid_InsertAndGetId()
		{
			var item = GetItemDemos(1).First();
			_itemDemoRepository.InsertAndGetId(item);

			Assert.True(item.Id != Guid.Empty);
		}

		[Fact]
		public void AsUpdateable_BatchUpdate()
		{
			var items = GetItemDemos(1000);
			_itemDemoRepository.InsertRange(items);

			var total = _itemDemoRepository.Count();

			Assert.True(total >= 1000);
		}

		[Fact]
		public void Count_ISoftDelete()
		{
			var items = GetItemDemos(2);
			items[0].IsDeleted = true;
			items[1].IsDeleted = false;
			_itemDemoRepository.InsertRange(items);

			var count = _itemDemoRepository.Count();
			Assert.True(count == 1);
			using (_itemDemoRepository.DisableFilter("ISoftDelete"))
			{
				count = _itemDemoRepository.Count();
				Assert.True(count == 2);

				using ( _itemDemoRepository.EnableFilter("ISoftDelete"))
				{
					count = _itemDemoRepository.Count();
					Assert.True(count == 1);
				}
			}

			count = _itemDemoRepository.Count();
			Assert.True(count == 1);
		}
		#endregion


		private List<ItemDemo> GetItemDemos(int num)
		{
			var demos = new List<ItemDemo>();
			for (int i = 0; i < num; i++)
			{
				demos.Add(new ItemDemo
				{
					CreationTime = DateTime.Now,
					Name = $"Name{i}"
				});
			}
			return demos;
		}

		public void Dispose()
		{
			//_itemDemoRepository.AsDeleteable().ExecuteCommand();
			db.Ado.ExecuteCommand("TRUNCATE TABLE dbo.Sys_Item_Demo ");
		}
	}
}
