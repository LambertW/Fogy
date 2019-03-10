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
				InitKeyType = InitKeyType.Attribute,
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

		#region Saveable
		[Fact]
		public void Saveable()
		{
			var item1 = GetItemDemos(1).First();
			var item2 = GetItemDemos(1).First();
			var item3 = GetItemDemos(1).First();

			_itemDemoRepository.Insert(item1);

			var beforeItemNum = _itemDemoRepository.Count();

			_itemDemoRepository.InsertOrUpdate(new List<ItemDemo>
			{
				item1,
				item2,
				item3
			});

			var afterItemNum = _itemDemoRepository.Count();

			Assert.True(afterItemNum - beforeItemNum == 2);
			Assert.True(item2.Id != Guid.Empty);
			Assert.True(item3.Id != Guid.Empty);
		}
		#endregion

		#region Update
		
		[Fact]
		public void Update_And_ReturnList()
		{
			var items = GetItemDemos(3);

			_itemDemoRepository.InsertRange(items);
			items.ForEach(t => t.Name = "Updated");
			var result = _itemDemoRepository.UpdateRange(items);
			Assert.True(result);

			var list = _itemDemoRepository.AsQueryable().In(new[] { items[0].Id, items[1].Id, items[2].Id, }).ToList();
			Assert.True(list[0].Name == "Updated");
		}

		[Fact]
		public void UpdateEmptyPrimaryKey_ThrowsException()
		{
			var items = GetItemDemos(3);

			Assert.Throws<ArgumentException>(
				() => { var result = _itemDemoRepository.UpdateRange(items); });
		}

		[Fact]
		public void Update_SoftDelete()
		{
			var items = GetItemDemos(2);
			items[0].IsDeleted = false;
			items[1].IsDeleted = true;

			_itemDemoRepository.InsertRange(items);
			items[1].Name = "Test";
			var result = _itemDemoRepository.Update(items[1]);
			Assert.True(!result);

			var entity = _itemDemoRepository.GetById(items[1].Id);
			Assert.True(entity.Name != "Test");
		}

		#endregion

		#region Insert

		[Fact]
		public void InsertList_And_GeneratedId()
		{
			var items = GetItemDemos(3);
			var list = _itemDemoRepository.InsertRange(items);

			Assert.True(items[0].Id != Guid.Empty);
			Assert.True(items[1].Id != Guid.Empty);
			Assert.True(items[2].Id != Guid.Empty);
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
					Name = $"Name{i}",
					IsDeleted = false
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
