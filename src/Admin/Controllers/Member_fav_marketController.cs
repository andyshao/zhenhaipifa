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
	public class Member_fav_marketController : BaseAdminController {
		public Member_fav_marketController(ILogger<Member_fav_marketController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Market_id, [FromQuery] uint?[] Member_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Member_fav_market.Select;
			if (Market_id.Length > 0) select.WhereMarket_id(Market_id);
			if (Member_id.Length > 0) select.WhereMember_id(Member_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Market>("b", "b.id = a.market_id")
				.InnerJoin<Member>("c", "c.id = a.member_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_market", items.Select<Member_fav_marketInfo, MarketInfo>(a => a.Obj_market).ToBson(), 
				"items_member", items.Select<Member_fav_marketInfo, MemberInfo>(a => a.Obj_member).ToBson());
		}

		[HttpGet(@"{Market_id}/{Member_id}/")]
		public APIReturn Get_item(uint? Market_id, uint? Member_id) {
			Member_fav_marketInfo item = Member_fav_market.GetItem(Market_id, Member_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Market_id, [FromForm] uint? Member_id) {
			Member_fav_marketInfo item = new Member_fav_marketInfo();
			item.Market_id = Market_id;
			item.Member_id = Member_id;
			item.Create_time = DateTime.Now;
			item = Member_fav_market.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Market_id}/{Member_id}/")]
		public APIReturn Put_update(uint? Market_id, uint? Member_id) {
			Member_fav_marketInfo item = new Member_fav_marketInfo();
			item.Market_id = Market_id;
			item.Member_id = Member_id;
			item.Create_time = DateTime.Now;
			int affrows = Member_fav_market.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Market_id}/{Member_id}/")]
		public APIReturn Delete_delete(uint? Market_id, uint? Member_id) {
			int affrows = Member_fav_market.Delete(Market_id, Member_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
