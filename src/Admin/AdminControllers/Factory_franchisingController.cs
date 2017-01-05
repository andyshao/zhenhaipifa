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
	public class Factory_franchisingController : BaseAdminController {
		public Factory_franchisingController(ILogger<Factory_franchisingController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Factory_id, [FromQuery] uint?[] Franchising_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Factory_franchising.Select;
			if (Factory_id.Length > 0) select.WhereFactory_id(Factory_id);
			if (Franchising_id.Length > 0) select.WhereFranchising_id(Franchising_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Factory>("b", "b.id = a.factory_id")
				.LeftJoin<Franchising>("c", "c.id = a.franchising_id").Skip((page - 1) * limit).Limit(limit).ToList();
			ViewBag.items = items;
			ViewBag.count = count;
			return View();
		}

		[HttpGet(@"add")]
		public ActionResult Edit() {
			return View();
		}
		[HttpGet(@"edit")]
		public ActionResult Edit([FromQuery] uint Factory_id, [FromQuery] uint Franchising_id) {
			Factory_franchisingInfo item = Factory_franchising.GetItem(Factory_id, Franchising_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Factory_id, [FromForm] uint? Franchising_id) {
			Factory_franchisingInfo item = new Factory_franchisingInfo();
			item.Factory_id = Factory_id;
			item.Franchising_id = Franchising_id;
			item = Factory_franchising.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Factory_id, [FromQuery] uint Franchising_id) {
			Factory_franchisingInfo item = Factory_franchising.GetItem(Factory_id, Franchising_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			int affrows = Factory_franchising.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] string[] ids) {
			int affrows = 0;
			foreach (string id in ids) {
				string[] vs = id.Split(',');
				affrows += Factory_franchising.Delete(uint.Parse(vs[0]), uint.Parse(vs[1]));
			}
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
