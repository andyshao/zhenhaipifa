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
	public class ShopstatController : BaseAdminController {
		public ShopstatController(ILogger<ShopstatController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Shop_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Shopstat.Select;
			if (Shop_id.Length > 0) select.WhereShop_id(Shop_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Shop>("b", "b.id = a.shop_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_shop", items.Select<ShopstatInfo, ShopInfo>(a => a.Obj_shop).ToBson());
		}

		[HttpGet(@"{Shop_id}/")]
		public APIReturn Get_item(uint? Shop_id) {
			ShopstatInfo item = Shopstat.GetItem(Shop_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Shop_id, [FromForm] uint? Today_fav, [FromForm] uint? Today_session, [FromForm] uint? Today_share, [FromForm] uint? Total_fav, [FromForm] uint? Total_session, [FromForm] uint? Total_share) {
			ShopstatInfo item = new ShopstatInfo();
			item.Shop_id = Shop_id;
			item.Today_fav = Today_fav;
			item.Today_session = Today_session;
			item.Today_share = Today_share;
			item.Total_fav = Total_fav;
			item.Total_session = Total_session;
			item.Total_share = Total_share;
			item = Shopstat.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Shop_id}/")]
		public APIReturn Put_update(uint? Shop_id, [FromForm] uint? Today_fav, [FromForm] uint? Today_session, [FromForm] uint? Today_share, [FromForm] uint? Total_fav, [FromForm] uint? Total_session, [FromForm] uint? Total_share) {
			ShopstatInfo item = new ShopstatInfo();
			item.Shop_id = Shop_id;
			item.Today_fav = Today_fav;
			item.Today_session = Today_session;
			item.Today_share = Today_share;
			item.Total_fav = Total_fav;
			item.Total_session = Total_session;
			item.Total_share = Total_share;
			int affrows = Shopstat.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Shop_id}/")]
		public APIReturn Delete_delete(uint? Shop_id) {
			int affrows = Shopstat.Delete(Shop_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
