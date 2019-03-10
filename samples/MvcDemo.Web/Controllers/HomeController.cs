using MvcDemo.Core.System;
using MvcDemo.SqlSugar.Repositories.System;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcDemo.Web.Controllers
{
	public class HomeController : Controller
	{
		SqlSugarClient db;
		ItemDemoRepository itemDemoRepository;
		public HomeController()
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
			itemDemoRepository = new ItemDemoRepository(db);
		}

		public ActionResult Index()
		{
			var count = itemDemoRepository.Count();

			var id = itemDemoRepository.InsertAndGetId(new ItemDemo { CreationTime = DateTime.Now });

			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}