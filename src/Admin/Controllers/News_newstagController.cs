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
	public class News_newstagController : BaseAdminController {
		public News_newstagController(ILogger<News_newstagController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] News_id, [FromQuery] uint?[] Newstag_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = News_newstag.Select;
			if (News_id.Length > 0) select.WhereNews_id(News_id);
			if (Newstag_id.Length > 0) select.WhereNewstag_id(Newstag_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<News>("b", "b.id = a.news_id")
				.InnerJoin<Newstag>("c", "c.id = a.newstag_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_news", items.Select<News_newstagInfo, NewsInfo>(a => a.Obj_news).ToBson(), 
				"items_newstag", items.Select<News_newstagInfo, NewstagInfo>(a => a.Obj_newstag).ToBson());
		}

		[HttpGet(@"{News_id}/{Newstag_id}/")]
		public APIReturn Get_item(uint? News_id, uint? Newstag_id) {
			News_newstagInfo item = News_newstag.GetItem(News_id, Newstag_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? News_id, [FromForm] uint? Newstag_id) {
			News_newstagInfo item = new News_newstagInfo();
			item.News_id = News_id;
			item.Newstag_id = Newstag_id;
			item = News_newstag.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{News_id}/{Newstag_id}/")]
		public APIReturn Put_update(uint? News_id, uint? Newstag_id) {
			News_newstagInfo item = new News_newstagInfo();
			item.News_id = News_id;
			item.Newstag_id = Newstag_id;
			int affrows = News_newstag.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{News_id}/{Newstag_id}/")]
		public APIReturn Delete_delete(uint? News_id, uint? Newstag_id) {
			int affrows = News_newstag.Delete(News_id, Newstag_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
