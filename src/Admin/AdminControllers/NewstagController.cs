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
	public class NewstagController : BaseAdminController {
		public NewstagController(ILogger<NewstagController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint[] News_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Newstag.Select
				.Where(!string.IsNullOrEmpty(key), "a.name like {0}", string.Concat("%", key, "%"));
			if (News_id.Length > 0) select.WhereNews_id(News_id);
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
			NewstagInfo item = Newstag.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] string Name, [FromForm] uint? Total_news, [FromForm] uint[] mn_News) {
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
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] string Name, [FromForm] uint? Total_news, [FromForm] uint[] mn_News) {
			NewstagInfo item = Newstag.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
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
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Newstag.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
