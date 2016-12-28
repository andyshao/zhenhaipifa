using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using pifa.BLL;
using pifa.Model;

namespace pifa.Admin.Controllers {
	[Route("api/[controller]")]
	[Obsolete]
	public class FactoryController : BaseAdminController {
		public FactoryController(ILogger<FactoryController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Area_id, [FromQuery] uint[] Franchising_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Factory.Select
				.Where(!string.IsNullOrEmpty(key), "a.capacity like {0} or a.main_business like {0} or a.min_order like {0} or a.process_cost like {0} or a.sampling_period like {0} or a.sampling_price like {0} or a.telphone like {0} or a.title like {0} or a.turn_single_time like {0}", string.Concat("%", key, "%"));
			if (Area_id.Length > 0) select.WhereArea_id(Area_id);
			if (Franchising_id.Length > 0) select.WhereFranchising_id(Franchising_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Area>("b", "b.id = a.area_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_area", items.Select<FactoryInfo, AreaInfo>(a => a.Obj_area).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			FactoryInfo item = Factory.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Area_id, [FromForm] string Capacity, [FromForm] string Main_business, [FromForm] string Min_order, [FromForm] string Process_cost, [FromForm] string Sampling_period, [FromForm] string Sampling_price, [FromForm] string Telphone, [FromForm] string Title, [FromForm] string Turn_single_time, [FromForm] uint[] mn_Franchising) {
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

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] uint? Area_id, [FromForm] string Capacity, [FromForm] string Main_business, [FromForm] string Min_order, [FromForm] string Process_cost, [FromForm] string Sampling_period, [FromForm] string Sampling_price, [FromForm] string Telphone, [FromForm] string Title, [FromForm] string Turn_single_time, [FromForm] uint[] mn_Franchising) {
			FactoryInfo item = new FactoryInfo();
			item.Id = Id;
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
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Factory.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
