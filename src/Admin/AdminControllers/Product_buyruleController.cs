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
	public class Product_buyruleController : BaseAdminController {
		public Product_buyruleController(ILogger<Product_buyruleController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Product_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Product_buyrule.Select;
			if (Product_id.Length > 0) select.WhereProduct_id(Product_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Product>("b", "b.id = a.product_id").Skip((page - 1) * limit).Limit(limit).ToList();
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
			Product_buyruleInfo item = Product_buyrule.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Product_id, [FromForm] uint? Discount, [FromForm] uint? Ordering_end, [FromForm] uint? Ordering_start) {
			Product_buyruleInfo item = new Product_buyruleInfo();
			item.Product_id = Product_id;
			item.Discount = Discount;
			item.Ordering_end = Ordering_end;
			item.Ordering_start = Ordering_start;
			item = Product_buyrule.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] uint? Product_id, [FromForm] uint? Discount, [FromForm] uint? Ordering_end, [FromForm] uint? Ordering_start) {
			Product_buyruleInfo item = Product_buyrule.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Product_id = Product_id;
			item.Discount = Discount;
			item.Ordering_end = Ordering_end;
			item.Ordering_start = Ordering_start;
			int affrows = Product_buyrule.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Product_buyrule.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
