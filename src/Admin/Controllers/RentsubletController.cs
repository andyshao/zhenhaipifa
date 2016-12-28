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
	public class RentsubletController : BaseAdminController {
		public RentsubletController(ILogger<RentsubletController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Market_id, [FromQuery] uint[] Franchising_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Rentsublet.Select;
			if (Market_id.Length > 0) select.WhereMarket_id(Market_id);
			if (Franchising_id.Length > 0) select.WhereFranchising_id(Franchising_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Market>("b", "b.id = a.market_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_market", items.Select<RentsubletInfo, MarketInfo>(a => a.Obj_market).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			RentsubletInfo item = Rentsublet.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Market_id, [FromForm] decimal? Price, [FromForm] RentsubletTYPE? Type, [FromForm] uint[] mn_Franchising) {
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

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] uint? Market_id, [FromForm] decimal? Price, [FromForm] RentsubletTYPE? Type, [FromForm] uint[] mn_Franchising) {
			RentsubletInfo item = new RentsubletInfo();
			item.Id = Id;
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
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Rentsublet.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
