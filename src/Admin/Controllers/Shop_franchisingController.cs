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
	public class Shop_franchisingController : BaseAdminController {
		public Shop_franchisingController(ILogger<Shop_franchisingController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Franchising_id, [FromQuery] uint?[] Shop_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Shop_franchising.Select;
			if (Franchising_id.Length > 0) select.WhereFranchising_id(Franchising_id);
			if (Shop_id.Length > 0) select.WhereShop_id(Shop_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Franchising>("b", "b.id = a.franchising_id")
				.InnerJoin<Shop>("c", "c.id = a.shop_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_franchising", items.Select<Shop_franchisingInfo, FranchisingInfo>(a => a.Obj_franchising).ToBson(), 
				"items_shop", items.Select<Shop_franchisingInfo, ShopInfo>(a => a.Obj_shop).ToBson());
		}

		[HttpGet(@"{Franchising_id}/{Shop_id}/")]
		public APIReturn Get_item(uint? Franchising_id, uint? Shop_id) {
			Shop_franchisingInfo item = Shop_franchising.GetItem(Franchising_id, Shop_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Franchising_id, [FromForm] uint? Shop_id) {
			Shop_franchisingInfo item = new Shop_franchisingInfo();
			item.Franchising_id = Franchising_id;
			item.Shop_id = Shop_id;
			item = Shop_franchising.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Franchising_id}/{Shop_id}/")]
		public APIReturn Put_update(uint? Franchising_id, uint? Shop_id) {
			Shop_franchisingInfo item = new Shop_franchisingInfo();
			item.Franchising_id = Franchising_id;
			item.Shop_id = Shop_id;
			int affrows = Shop_franchising.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Franchising_id}/{Shop_id}/")]
		public APIReturn Delete_delete(uint? Franchising_id, uint? Shop_id) {
			int affrows = Shop_franchising.Delete(Franchising_id, Shop_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
