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
	public class NewstagController : BaseAdminController {
		public NewstagController(ILogger<NewstagController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint[] News_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Newstag.Select
				.Where(!string.IsNullOrEmpty(key), "a.name like {0}", string.Concat("%", key, "%"));
			if (News_id.Length > 0) select.WhereNews_id(News_id);
			int count;
			var items = select.Count(out count).Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count);
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			NewstagInfo item = Newstag.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] string Name, [FromForm] uint? Total_news, [FromForm] uint[] mn_News) {
			NewstagInfo item = new NewstagInfo();
			item.Create_time = DateTime.Now;
			item.Name = Name;
			item.Total_news = Total_news;
			item = Newstag.Insert(item);
			//关联 News
			foreach (uint mn_News_in in mn_News)
				item.FlagNews(mn_News_in);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] string Name, [FromForm] uint? Total_news, [FromForm] uint[] mn_News) {
			NewstagInfo item = new NewstagInfo();
			item.Id = Id;
			item.Create_time = DateTime.Now;
			item.Name = Name;
			item.Total_news = Total_news;
			int affrows = Newstag.Update(item);
			//关联 News
			if (mn_News.Length == 0) {
				item.UnflagNewsALL();
			} else {
				List<uint> mn_News_list = mn_News.ToList();
				foreach (var Obj_news in item.Obj_newss) {
					int idx = mn_News_list.FindIndex(a => a == Obj_news.Id);
					if (idx == -1) item.UnflagNews(Obj_news.Id);
					else mn_News_list.RemoveAt(idx);
				}
				mn_News_list.ForEach(a => item.FlagNews(a));
			}
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Newstag.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
