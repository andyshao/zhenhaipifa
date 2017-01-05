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
	public class Markettype_categoryController : BaseAdminController {
		public Markettype_categoryController(ILogger<Markettype_categoryController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Category_id, [FromQuery] uint?[] Markettype_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Markettype_category.Select;
			if (Category_id.Length > 0) select.WhereCategory_id(Category_id);
			if (Markettype_id.Length > 0) select.WhereMarkettype_id(Markettype_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Category>("b", "b.id = a.category_id")
				.LeftJoin<Markettype>("c", "c.id = a.markettype_id").Skip((page - 1) * limit).Limit(limit).ToList();
			ViewBag.items = items;
			ViewBag.count = count;
			return View();
		}

		[HttpGet(@"add")]
		public ActionResult Edit() {
			return View();
		}
		[HttpGet(@"edit")]
		public ActionResult Edit([FromQuery] uint Category_id, [FromQuery] uint Markettype_id) {
			Markettype_categoryInfo item = Markettype_category.GetItem(Category_id, Markettype_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Category_id, [FromForm] uint? Markettype_id) {
			Markettype_categoryInfo item = new Markettype_categoryInfo();
			item.Category_id = Category_id;
			item.Markettype_id = Markettype_id;
			item = Markettype_category.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Category_id, [FromQuery] uint Markettype_id) {
			Markettype_categoryInfo item = Markettype_category.GetItem(Category_id, Markettype_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			int affrows = Markettype_category.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] string[] ids) {
			int affrows = 0;
			foreach (string id in ids) {
				string[] vs = id.Split(',');
				affrows += Markettype_category.Delete(uint.Parse(vs[0]), uint.Parse(vs[1]));
			}
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
