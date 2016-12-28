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
	public class MarketdescController : BaseAdminController {
		public MarketdescController(ILogger<MarketdescController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Market_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Marketdesc.Select
				.Where(!string.IsNullOrEmpty(key), "a.content like {0} or a.url like {0}", string.Concat("%", key, "%"));
			if (Market_id.Length > 0) select.WhereMarket_id(Market_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Market>("b", "b.id = a.market_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_market", items.Select<MarketdescInfo, MarketInfo>(a => a.Obj_market).ToBson());
		}

		[HttpGet(@"{Market_id}/")]
		public APIReturn Get_item(uint? Market_id) {
			MarketdescInfo item = Marketdesc.GetItem(Market_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Market_id, [FromForm] string Content, [FromForm] string Url) {
			MarketdescInfo item = new MarketdescInfo();
			item.Market_id = Market_id;
			item.Content = Content;
			item.Url = Url;
			item = Marketdesc.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Market_id}/")]
		public APIReturn Put_update(uint? Market_id, [FromForm] string Content, [FromForm] string Url) {
			MarketdescInfo item = new MarketdescInfo();
			item.Market_id = Market_id;
			item.Content = Content;
			item.Url = Url;
			int affrows = Marketdesc.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Market_id}/")]
		public APIReturn Delete_delete(uint? Market_id) {
			int affrows = Marketdesc.Delete(Market_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
