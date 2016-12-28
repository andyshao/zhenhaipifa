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
	public class NewsdescController : BaseAdminController {
		public NewsdescController(ILogger<NewsdescController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] News_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Newsdesc.Select
				.Where(!string.IsNullOrEmpty(key), "a.content like {0}", string.Concat("%", key, "%"));
			if (News_id.Length > 0) select.WhereNews_id(News_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<News>("b", "b.id = a.news_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_news", items.Select<NewsdescInfo, NewsInfo>(a => a.Obj_news).ToBson());
		}

		[HttpGet(@"{News_id}/")]
		public APIReturn Get_item(uint? News_id) {
			NewsdescInfo item = Newsdesc.GetItem(News_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? News_id, [FromForm] string Content) {
			NewsdescInfo item = new NewsdescInfo();
			item.News_id = News_id;
			item.Content = Content;
			item = Newsdesc.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{News_id}/")]
		public APIReturn Put_update(uint? News_id, [FromForm] string Content) {
			NewsdescInfo item = new NewsdescInfo();
			item.News_id = News_id;
			item.Content = Content;
			int affrows = Newsdesc.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{News_id}/")]
		public APIReturn Delete_delete(uint? News_id) {
			int affrows = Newsdesc.Delete(News_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
