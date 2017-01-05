using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using pifa.BLL;
using pifa.Model;

namespace pifa.AdminControllers {
	[Route("[controller]")]
	public class NewsController : BaseAdminController {
		public NewsController(ILogger<NewsController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint[] Newstag_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = News.Select
				.Where(!string.IsNullOrEmpty(key), "a.intro like {0} or a.source like {0} or a.title like {0}", string.Concat("%", key, "%"));
			if (Newstag_id.Length > 0) select.WhereNewstag_id(Newstag_id);
			int count;
			var items = select.Count(out count).Skip((page - 1) * limit).Limit(limit).ToList();
			ViewBag.items = items;
			ViewBag.count = count;
			return View();
		}

		[HttpGet(@"add")]
		public ActionResult Edit() {
			return View();
		}
		[HttpGet(@"edit")]
		public ActionResult Edit([FromQuery] uint Id) {
			NewsInfo item = News.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] string Intro, [FromForm] uint? Pv, [FromForm] string Source, [FromForm] NewsSTATE? State, [FromForm] string Title, [FromForm] uint[] mn_Newstag) {
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
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] string Intro, [FromForm] uint? Pv, [FromForm] string Source, [FromForm] NewsSTATE? State, [FromForm] string Title, [FromForm] uint[] mn_Newstag) {
			NewsInfo item = News.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
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
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += News.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
