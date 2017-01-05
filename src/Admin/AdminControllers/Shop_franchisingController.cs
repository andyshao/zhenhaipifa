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
	public class Shop_franchisingController : BaseAdminController {
		public Shop_franchisingController(ILogger<Shop_franchisingController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Franchising_id, [FromQuery] uint?[] Shop_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Shop_franchising.Select;
			if (Franchising_id.Length > 0) select.WhereFranchising_id(Franchising_id);
			if (Shop_id.Length > 0) select.WhereShop_id(Shop_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Franchising>("b", "b.id = a.franchising_id")
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
		public ActionResult Edit([FromQuery] uint Franchising_id, [FromQuery] uint Shop_id) {
			Shop_franchisingInfo item = Shop_franchising.GetItem(Franchising_id, Shop_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Franchising_id, [FromForm] uint? Shop_id) {
			Shop_franchisingInfo item = new Shop_franchisingInfo();
			item.Franchising_id = Franchising_id;
			item.Shop_id = Shop_id;
			item = Shop_franchising.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Franchising_id, [FromQuery] uint Shop_id) {
			Shop_franchisingInfo item = Shop_franchising.GetItem(Franchising_id, Shop_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			int affrows = Shop_franchising.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] string[] ids) {
			int affrows = 0;
			foreach (string id in ids) {
				string[] vs = id.Split(',');
				affrows += Shop_franchising.Delete(uint.Parse(vs[0]), uint.Parse(vs[1]));
			}
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
