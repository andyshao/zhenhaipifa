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
	public class Member_productController : BaseAdminController {
		public Member_productController(ILogger<Member_productController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Member_id, [FromQuery] uint?[] Product_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Member_product.Select;
			if (Member_id.Length > 0) select.WhereMember_id(Member_id);
			if (Product_id.Length > 0) select.WhereProduct_id(Product_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Member>("b", "b.id = a.member_id")
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
		public ActionResult Edit([FromQuery] uint Member_id, [FromQuery] uint Product_id) {
			Member_productInfo item = Member_product.GetItem(Member_id, Product_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Member_id, [FromForm] uint? Product_id) {
			Member_productInfo item = new Member_productInfo();
			item.Member_id = Member_id;
			item.Product_id = Product_id;
			item.Create_time = DateTime.Now;
			item = Member_product.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Member_id, [FromQuery] uint Product_id) {
			Member_productInfo item = Member_product.GetItem(Member_id, Product_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Create_time = DateTime.Now;
			int affrows = Member_product.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] string[] ids) {
			int affrows = 0;
			foreach (string id in ids) {
				string[] vs = id.Split(',');
				affrows += Member_product.Delete(uint.Parse(vs[0]), uint.Parse(vs[1]));
			}
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
