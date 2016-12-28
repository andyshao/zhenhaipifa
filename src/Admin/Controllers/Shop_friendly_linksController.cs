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
	public class Shop_friendly_linksController : BaseAdminController {
		public Shop_friendly_linksController(ILogger<Shop_friendly_linksController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Shop_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Shop_friendly_links.Select
				.Where(!string.IsNullOrEmpty(key), "a.title like {0} or a.url like {0}", string.Concat("%", key, "%"));
			if (Shop_id.Length > 0) select.WhereShop_id(Shop_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Shop>("b", "b.id = a.shop_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_shop", items.Select<Shop_friendly_linksInfo, ShopInfo>(a => a.Obj_shop).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			Shop_friendly_linksInfo item = Shop_friendly_links.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Shop_id, [FromForm] byte? Sort, [FromForm] string Title, [FromForm] string Url) {
			Shop_friendly_linksInfo item = new Shop_friendly_linksInfo();
			item.Shop_id = Shop_id;
			item.Create_time = DateTime.Now;
			item.Sort = Sort;
			item.Title = Title;
			item.Url = Url;
			item = Shop_friendly_links.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] uint? Shop_id, [FromForm] byte? Sort, [FromForm] string Title, [FromForm] string Url) {
			Shop_friendly_linksInfo item = new Shop_friendly_linksInfo();
			item.Id = Id;
			item.Shop_id = Shop_id;
			item.Create_time = DateTime.Now;
			item.Sort = Sort;
			item.Title = Title;
			item.Url = Url;
			int affrows = Shop_friendly_links.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Shop_friendly_links.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
