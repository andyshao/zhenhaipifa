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
	public class ShopsecurityController : BaseAdminController {
		public ShopsecurityController(ILogger<ShopsecurityController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Shop_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Shopsecurity.Select
				.Where(!string.IsNullOrEmpty(key), "a.idcard like {0} or a.idcard_img1 like {0} or a.idcard_img2 like {0} or a.license_img like {0}", string.Concat("%", key, "%"));
			if (Shop_id.Length > 0) select.WhereShop_id(Shop_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Shop>("b", "b.id = a.shop_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_shop", items.Select<ShopsecurityInfo, ShopInfo>(a => a.Obj_shop).ToBson());
		}

		[HttpGet(@"{Shop_id}/")]
		public APIReturn Get_item(uint? Shop_id) {
			ShopsecurityInfo item = Shopsecurity.GetItem(Shop_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Shop_id, [FromForm] string Idcard, [FromForm] string Idcard_img1, [FromForm] string Idcard_img2, [FromForm] string License_img) {
			ShopsecurityInfo item = new ShopsecurityInfo();
			item.Shop_id = Shop_id;
			item.Idcard = Idcard;
			item.Idcard_img1 = Idcard_img1;
			item.Idcard_img2 = Idcard_img2;
			item.License_img = License_img;
			item = Shopsecurity.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Shop_id}/")]
		public APIReturn Put_update(uint? Shop_id, [FromForm] string Idcard, [FromForm] string Idcard_img1, [FromForm] string Idcard_img2, [FromForm] string License_img) {
			ShopsecurityInfo item = new ShopsecurityInfo();
			item.Shop_id = Shop_id;
			item.Idcard = Idcard;
			item.Idcard_img1 = Idcard_img1;
			item.Idcard_img2 = Idcard_img2;
			item.License_img = License_img;
			int affrows = Shopsecurity.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Shop_id}/")]
		public APIReturn Delete_delete(uint? Shop_id) {
			int affrows = Shopsecurity.Delete(Shop_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
