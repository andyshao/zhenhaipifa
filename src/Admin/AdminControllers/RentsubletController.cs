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
	public class RentsubletController : BaseAdminController {
		public RentsubletController(ILogger<RentsubletController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Market_id, [FromQuery] uint[] Franchising_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Rentsublet.Select;
			if (Market_id.Length > 0) select.WhereMarket_id(Market_id);
			if (Franchising_id.Length > 0) select.WhereFranchising_id(Franchising_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Market>("b", "b.id = a.market_id").Skip((page - 1) * limit).Limit(limit).ToList();
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
			RentsubletInfo item = Rentsublet.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Market_id, [FromForm] decimal? Price, [FromForm] RentsubletTYPE? Type, [FromForm] uint[] mn_Franchising) {
			RentsubletInfo item = new RentsubletInfo();
			item.Market_id = Market_id;
			item.Create_time = DateTime.Now;
			item.Price = Price;
			item.Type = Type;
			item = Rentsublet.Insert(item);
			//关联 Franchising
			foreach (uint mn_Franchising_in in mn_Franchising)
				item.FlagFranchising(mn_Franchising_in);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] uint? Market_id, [FromForm] decimal? Price, [FromForm] RentsubletTYPE? Type, [FromForm] uint[] mn_Franchising) {
			RentsubletInfo item = Rentsublet.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Market_id = Market_id;
			item.Create_time = DateTime.Now;
			item.Price = Price;
			item.Type = Type;
			int affrows = Rentsublet.Update(item);
			//关联 Franchising
			if (mn_Franchising.Length == 0) {
				item.UnflagFranchisingALL();
			} else {
				List<uint> mn_Franchising_list = mn_Franchising.ToList();
				foreach (var Obj_franchising in item.Obj_franchisings) {
					int idx = mn_Franchising_list.FindIndex(a => a == Obj_franchising.Id);
					if (idx == -1) item.UnflagFranchising(Obj_franchising.Id);
					else mn_Franchising_list.RemoveAt(idx);
				}
				mn_Franchising_list.ForEach(a => item.FlagFranchising(a));
			}
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Rentsublet.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
