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
	public class Rentsublet_franchisingController : BaseAdminController {
		public Rentsublet_franchisingController(ILogger<Rentsublet_franchisingController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Franchising_id, [FromQuery] uint?[] Rentsublet_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Rentsublet_franchising.Select;
			if (Franchising_id.Length > 0) select.WhereFranchising_id(Franchising_id);
			if (Rentsublet_id.Length > 0) select.WhereRentsublet_id(Rentsublet_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Franchising>("b", "b.id = a.franchising_id")
				.LeftJoin<Rentsublet>("c", "c.id = a.rentsublet_id").Skip((page - 1) * limit).Limit(limit).ToList();
			ViewBag.items = items;
			ViewBag.count = count;
			return View();
		}

		[HttpGet(@"add")]
		public ActionResult Edit() {
			return View();
		}
		[HttpGet(@"edit")]
		public ActionResult Edit([FromQuery] uint Franchising_id, [FromQuery] uint Rentsublet_id) {
			Rentsublet_franchisingInfo item = Rentsublet_franchising.GetItem(Franchising_id, Rentsublet_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Franchising_id, [FromForm] uint? Rentsublet_id) {
			Rentsublet_franchisingInfo item = new Rentsublet_franchisingInfo();
			item.Franchising_id = Franchising_id;
			item.Rentsublet_id = Rentsublet_id;
			item = Rentsublet_franchising.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Franchising_id, [FromQuery] uint Rentsublet_id) {
			Rentsublet_franchisingInfo item = Rentsublet_franchising.GetItem(Franchising_id, Rentsublet_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			int affrows = Rentsublet_franchising.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] string[] ids) {
			int affrows = 0;
			foreach (string id in ids) {
				string[] vs = id.Split(',');
				affrows += Rentsublet_franchising.Delete(uint.Parse(vs[0]), uint.Parse(vs[1]));
			}
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
