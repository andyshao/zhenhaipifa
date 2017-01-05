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
	public class Shop_friendly_linksController : BaseAdminController {
		public Shop_friendly_linksController(ILogger<Shop_friendly_linksController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Shop_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Shop_friendly_links.Select
				.Where(!string.IsNullOrEmpty(key), "a.title like {0} or a.url like {0}", string.Concat("%", key, "%"));
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
		public ActionResult Edit([FromQuery] uint Id) {
			Shop_friendly_linksInfo item = Shop_friendly_links.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Shop_id, [FromForm] byte? Sort, [FromForm] string Title, [FromForm] string Url) {
			Shop_friendly_linksInfo item = new Shop_friendly_linksInfo();
			item.Shop_id = Shop_id;
			item.Create_time = DateTime.Now;
			item.Sort = Sort;
			item.Title = Title;
			item.Url = Url;
			item = Shop_friendly_links.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] uint? Shop_id, [FromForm] byte? Sort, [FromForm] string Title, [FromForm] string Url) {
			Shop_friendly_linksInfo item = Shop_friendly_links.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Shop_id = Shop_id;
			item.Create_time = DateTime.Now;
			item.Sort = Sort;
			item.Title = Title;
			item.Url = Url;
			int affrows = Shop_friendly_links.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Shop_friendly_links.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
