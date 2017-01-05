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
	public class ProductController : BaseAdminController {
		public ProductController(ILogger<ProductController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Category_id, [FromQuery] uint?[] Shop_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Product.Select
				.Where(!string.IsNullOrEmpty(key), "a.title like {0} or a.unit like {0}", string.Concat("%", key, "%"));
			if (Category_id.Length > 0) select.WhereCategory_id(Category_id);
			if (Shop_id.Length > 0) select.WhereShop_id(Shop_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Category>("b", "b.id = a.category_id")
				.LeftJoin<Shop>("c", "c.id = a.shop_id").Skip((page - 1) * limit).Limit(limit).ToList();
			ViewBag.items = items;
			ViewBag.count = count;
			return View();
		}

		[HttpGet(@"add")]
		public ActionResult Edit() {
			return View();
		}
		[HttpGet(@"edit")]
		public ActionResult Edit([FromQuery] uint Id) {
			ProductInfo item = Product.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Category_id, [FromForm] uint? Shop_id, [FromForm] ProductICON[] Icon, [FromForm] decimal? Price, [FromForm] uint? Stock, [FromForm] string Title, [FromForm] string Unit) {
			ProductInfo item = new ProductInfo();
			item.Category_id = Category_id;
			item.Shop_id = Shop_id;
			item.Create_time = DateTime.Now;
			item.Icon = null;
			Icon?.ToList().ForEach(a => item.Icon = (item.Icon ?? 0) | a);
			item.Price = Price;
			item.Stock = Stock;
			item.Title = Title;
			item.Unit = Unit;
			item = Product.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] uint? Category_id, [FromForm] uint? Shop_id, [FromForm] ProductICON[] Icon, [FromForm] decimal? Price, [FromForm] uint? Stock, [FromForm] string Title, [FromForm] string Unit) {
			ProductInfo item = Product.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Category_id = Category_id;
			item.Shop_id = Shop_id;
			item.Create_time = DateTime.Now;
			item.Icon = null;
			Icon?.ToList().ForEach(a => item.Icon = (item.Icon ?? 0) | a);
			item.Price = Price;
			item.Stock = Stock;
			item.Title = Title;
			item.Unit = Unit;
			int affrows = Product.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Product.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
