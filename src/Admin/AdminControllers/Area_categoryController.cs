using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using pifa.BLL;
using pifa.Model;

namespace pifa.AdminControllers {
	[Route("[controller]")]
	public class Area_categoryController : BaseAdminController {
		public Area_categoryController(ILogger<Area_categoryController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Area_id, [FromQuery] uint?[] Category_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Area_category.Select;
			if (Area_id.Length > 0) select.WhereArea_id(Area_id);
			if (Category_id.Length > 0) select.WhereCategory_id(Category_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Area>("b", "b.id = a.area_id")
				.LeftJoin<Category>("c", "c.id = a.category_id").Skip((page - 1) * limit).Limit(limit).ToList();
			ViewBag.items = items;
			ViewBag.count = count;
			return View();
		}

		[HttpGet(@"add")]
		public ActionResult Edit() {
			return View();
		}
		[HttpGet(@"edit")]
		public ActionResult Edit([FromQuery] uint Area_id, [FromQuery] uint Category_id) {
			Area_categoryInfo item = Area_category.GetItem(Area_id, Category_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Area_id, [FromForm] uint? Category_id) {
			Area_categoryInfo item = new Area_categoryInfo();
			item.Area_id = Area_id;
			item.Category_id = Category_id;
			item = Area_category.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Area_id, [FromQuery] uint Category_id) {
			Area_categoryInfo item = Area_category.GetItem(Area_id, Category_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			int affrows = Area_category.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] string[] ids) {
			int affrows = 0;
			foreach (string id in ids) {
				string[] vs = id.Split(',');
				affrows += Area_category.Delete(uint.Parse(vs[0]), uint.Parse(vs[1]));
			}
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
