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
	public class FactoryController : BaseAdminController {
		public FactoryController(ILogger<FactoryController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Area_id, [FromQuery] uint[] Franchising_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Factory.Select
				.Where(!string.IsNullOrEmpty(key), "a.capacity like {0} or a.main_business like {0} or a.min_order like {0} or a.process_cost like {0} or a.sampling_period like {0} or a.sampling_price like {0} or a.telphone like {0} or a.title like {0} or a.turn_single_time like {0}", string.Concat("%", key, "%"));
			if (Area_id.Length > 0) select.WhereArea_id(Area_id);
			if (Franchising_id.Length > 0) select.WhereFranchising_id(Franchising_id);
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
			FactoryInfo item = Factory.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Area_id, [FromForm] string Capacity, [FromForm] string Main_business, [FromForm] string Min_order, [FromForm] string Process_cost, [FromForm] string Sampling_period, [FromForm] string Sampling_price, [FromForm] string Telphone, [FromForm] string Title, [FromForm] string Turn_single_time, [FromForm] uint[] mn_Franchising) {
			FactoryInfo item = new FactoryInfo();
			item.Area_id = Area_id;
			item.Capacity = Capacity;
			item.Create_time = DateTime.Now;
			item.Main_business = Main_business;
			item.Min_order = Min_order;
			item.Process_cost = Process_cost;
			item.Sampling_period = Sampling_period;
			item.Sampling_price = Sampling_price;
			item.Telphone = Telphone;
			item.Title = Title;
			item.Turn_single_time = Turn_single_time;
			item = Factory.Insert(item);
			//关联 Franchising
			foreach (uint mn_Franchising_in in mn_Franchising)
				item.FlagFranchising(mn_Franchising_in);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] uint? Area_id, [FromForm] string Capacity, [FromForm] string Main_business, [FromForm] string Min_order, [FromForm] string Process_cost, [FromForm] string Sampling_period, [FromForm] string Sampling_price, [FromForm] string Telphone, [FromForm] string Title, [FromForm] string Turn_single_time, [FromForm] uint[] mn_Franchising) {
			FactoryInfo item = Factory.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Area_id = Area_id;
			item.Capacity = Capacity;
			item.Create_time = DateTime.Now;
			item.Main_business = Main_business;
			item.Min_order = Min_order;
			item.Process_cost = Process_cost;
			item.Sampling_period = Sampling_period;
			item.Sampling_price = Sampling_price;
			item.Telphone = Telphone;
			item.Title = Title;
			item.Turn_single_time = Turn_single_time;
			int affrows = Factory.Update(item);
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
				affrows += Factory.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
