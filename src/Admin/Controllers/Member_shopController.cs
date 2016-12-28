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
	public class Member_shopController : BaseAdminController {
		public Member_shopController(ILogger<Member_shopController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Member_id, [FromQuery] uint?[] Shop_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Member_shop.Select;
			if (Member_id.Length > 0) select.WhereMember_id(Member_id);
			if (Shop_id.Length > 0) select.WhereShop_id(Shop_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Member>("b", "b.id = a.member_id")
				.InnerJoin<Shop>("c", "c.id = a.shop_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_member", items.Select<Member_shopInfo, MemberInfo>(a => a.Obj_member).ToBson(), 
				"items_shop", items.Select<Member_shopInfo, ShopInfo>(a => a.Obj_shop).ToBson());
		}

		[HttpGet(@"{Member_id}/{Shop_id}/")]
		public APIReturn Get_item(uint? Member_id, uint? Shop_id) {
			Member_shopInfo item = Member_shop.GetItem(Member_id, Shop_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Member_id, [FromForm] uint? Shop_id) {
			Member_shopInfo item = new Member_shopInfo();
			item.Member_id = Member_id;
			item.Shop_id = Shop_id;
			item.Create_time = DateTime.Now;
			item = Member_shop.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Member_id}/{Shop_id}/")]
		public APIReturn Put_update(uint? Member_id, uint? Shop_id) {
			Member_shopInfo item = new Member_shopInfo();
			item.Member_id = Member_id;
			item.Shop_id = Shop_id;
			item.Create_time = DateTime.Now;
			int affrows = Member_shop.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Member_id}/{Shop_id}/")]
		public APIReturn Delete_delete(uint? Member_id, uint? Shop_id) {
			int affrows = Member_shop.Delete(Member_id, Shop_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
