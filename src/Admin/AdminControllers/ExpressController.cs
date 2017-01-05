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
	public class ExpressController : BaseAdminController {
		public ExpressController(ILogger<ExpressController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Area_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Express.Select
				.Where(!string.IsNullOrEmpty(key), "a.address like {0} or a.service_features like {0} or a.telphone like {0} or a.title like {0}", string.Concat("%", key, "%"));
			if (Area_id.Length > 0) select.WhereArea_id(Area_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Area>("b", "b.id = a.area_id").Skip((page - 1) * limit).Limit(limit).ToList();
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
			ExpressInfo item = Express.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Area_id, [FromForm] string Address, [FromForm] string Service_features, [FromForm] string Telphone, [FromForm] string Title) {
			ExpressInfo item = new ExpressInfo();
			item.Area_id = Area_id;
			item.Address = Address;
			item.Create_time = DateTime.Now;
			item.Service_features = Service_features;
			item.Telphone = Telphone;
			item.Title = Title;
			item = Express.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] uint? Area_id, [FromForm] string Address, [FromForm] string Service_features, [FromForm] string Telphone, [FromForm] string Title) {
			ExpressInfo item = Express.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Area_id = Area_id;
			item.Address = Address;
			item.Create_time = DateTime.Now;
			item.Service_features = Service_features;
			item.Telphone = Telphone;
			item.Title = Title;
			int affrows = Express.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Express.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
