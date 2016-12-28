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
	public class MarketController : BaseAdminController {
		public MarketController(ILogger<MarketController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Area_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Market.Select
				.Where(!string.IsNullOrEmpty(key), "a.title like {0}", string.Concat("%", key, "%"));
			if (Area_id.Length > 0) select.WhereArea_id(Area_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Area>("b", "b.id = a.area_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_area", items.Select<MarketInfo, AreaInfo>(a => a.Obj_area).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			MarketInfo item = Market.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Area_id, [FromForm] string Title) {
			MarketInfo item = new MarketInfo();
			item.Area_id = Area_id;
			item.Create_time = DateTime.Now;
			item.Title = Title;
			item = Market.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] uint? Area_id, [FromForm] string Title) {
			MarketInfo item = new MarketInfo();
			item.Id = Id;
			item.Area_id = Area_id;
			item.Create_time = DateTime.Now;
			item.Title = Title;
			int affrows = Market.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Market.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
