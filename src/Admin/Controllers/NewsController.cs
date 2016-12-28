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
	public class NewsController : BaseAdminController {
		public NewsController(ILogger<NewsController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint[] Newstag_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = News.Select
				.Where(!string.IsNullOrEmpty(key), "a.intro like {0} or a.source like {0} or a.title like {0}", string.Concat("%", key, "%"));
			if (Newstag_id.Length > 0) select.WhereNewstag_id(Newstag_id);
			int count;
			var items = select.Count(out count).Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count);
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			NewsInfo item = News.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] string Intro, [FromForm] uint? Pv, [FromForm] string Source, [FromForm] NewsSTATE? State, [FromForm] string Title, [FromForm] uint[] mn_Newstag) {
			NewsInfo item = new NewsInfo();
			item.Create_time = DateTime.Now;
			item.Intro = Intro;
			item.Pv = Pv;
			item.Source = Source;
			item.State = State;
			item.Title = Title;
			item.Update_time = DateTime.Now;
			item = News.Insert(item);
			//关联 Newstag
			foreach (uint mn_Newstag_in in mn_Newstag)
				item.FlagNewstag(mn_Newstag_in);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] string Intro, [FromForm] uint? Pv, [FromForm] string Source, [FromForm] NewsSTATE? State, [FromForm] string Title, [FromForm] uint[] mn_Newstag) {
			NewsInfo item = new NewsInfo();
			item.Id = Id;
			item.Create_time = DateTime.Now;
			item.Intro = Intro;
			item.Pv = Pv;
			item.Source = Source;
			item.State = State;
			item.Title = Title;
			item.Update_time = DateTime.Now;
			int affrows = News.Update(item);
			//关联 Newstag
			if (mn_Newstag.Length == 0) {
				item.UnflagNewstagALL();
			} else {
				List<uint> mn_Newstag_list = mn_Newstag.ToList();
				foreach (var Obj_newstag in item.Obj_newstags) {
					int idx = mn_Newstag_list.FindIndex(a => a == Obj_newstag.Id);
					if (idx == -1) item.UnflagNewstag(Obj_newstag.Id);
					else mn_Newstag_list.RemoveAt(idx);
				}
				mn_Newstag_list.ForEach(a => item.FlagNewstag(a));
			}
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = News.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
