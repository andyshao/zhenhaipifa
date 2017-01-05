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
	public class Product_attrController : BaseAdminController {
		public Product_attrController(ILogger<Product_attrController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Pattr_id, [FromQuery] uint?[] Product_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Product_attr.Select
				.Where(!string.IsNullOrEmpty(key), "a.value like {0}", string.Concat("%", key, "%"));
			if (Pattr_id.Length > 0) select.WherePattr_id(Pattr_id);
			if (Product_id.Length > 0) select.WhereProduct_id(Product_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Pattr>("b", "b.id = a.pattr_id")
				.LeftJoin<Product>("c", "c.id = a.product_id").Skip((page - 1) * limit).Limit(limit).ToList();
			ViewBag.items = items;
			ViewBag.count = count;
			return View();
		}

		[HttpGet(@"add")]
		public ActionResult Edit() {
			return View();
		}
		[HttpGet(@"edit")]
		public ActionResult Edit([FromQuery] uint Pattr_id, [FromQuery] uint Product_id) {
			Product_attrInfo item = Product_attr.GetItem(Pattr_id, Product_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Pattr_id, [FromForm] uint? Product_id, [FromForm] string Value) {
			Product_attrInfo item = new Product_attrInfo();
			item.Pattr_id = Pattr_id;
			item.Product_id = Product_id;
			item.Value = Value;
			item = Product_attr.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Pattr_id, [FromQuery] uint Product_id, [FromForm] string Value) {
			Product_attrInfo item = Product_attr.GetItem(Pattr_id, Product_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Value = Value;
			int affrows = Product_attr.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] string[] ids) {
			int affrows = 0;
			foreach (string id in ids) {
				string[] vs = id.Split(',');
				affrows += Product_attr.Delete(uint.Parse(vs[0]), uint.Parse(vs[1]));
			}
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
