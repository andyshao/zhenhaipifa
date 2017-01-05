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
	public class ShopstatController : BaseAdminController {
		public ShopstatController(ILogger<ShopstatController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Shop_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Shopstat.Select;
			if (Shop_id.Length > 0) select.WhereShop_id(Shop_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Shop>("b", "b.id = a.shop_id").Skip((page - 1) * limit).Limit(limit).ToList();
			ViewBag.items = items;
			ViewBag.count = count;
			return View();
		}

		[HttpGet(@"add")]
		public ActionResult Edit() {
			return View();
		}
		[HttpGet(@"edit")]
		public ActionResult Edit([FromQuery] uint Shop_id) {
			ShopstatInfo item = Shopstat.GetItem(Shop_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Shop_id, [FromForm] uint? Today_fav, [FromForm] uint? Today_session, [FromForm] uint? Today_share, [FromForm] uint? Total_fav, [FromForm] uint? Total_session, [FromForm] uint? Total_share) {
			ShopstatInfo item = new ShopstatInfo();
			item.Shop_id = Shop_id;
			item.Today_fav = Today_fav;
			item.Today_session = Today_session;
			item.Today_share = Today_share;
			item.Total_fav = Total_fav;
			item.Total_session = Total_session;
			item.Total_share = Total_share;
			item = Shopstat.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Shop_id, [FromForm] uint? Today_fav, [FromForm] uint? Today_session, [FromForm] uint? Today_share, [FromForm] uint? Total_fav, [FromForm] uint? Total_session, [FromForm] uint? Total_share) {
			ShopstatInfo item = Shopstat.GetItem(Shop_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Today_fav = Today_fav;
			item.Today_session = Today_session;
			item.Today_share = Today_share;
			item.Total_fav = Total_fav;
			item.Total_session = Total_session;
			item.Total_share = Total_share;
			int affrows = Shopstat.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Shopstat.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
