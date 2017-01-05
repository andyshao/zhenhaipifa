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
	public class News_newstagController : BaseAdminController {
		public News_newstagController(ILogger<News_newstagController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] News_id, [FromQuery] uint?[] Newstag_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = News_newstag.Select;
			if (News_id.Length > 0) select.WhereNews_id(News_id);
			if (Newstag_id.Length > 0) select.WhereNewstag_id(Newstag_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<News>("b", "b.id = a.news_id")
				.LeftJoin<Newstag>("c", "c.id = a.newstag_id").Skip((page - 1) * limit).Limit(limit).ToList();
			ViewBag.items = items;
			ViewBag.count = count;
			return View();
		}

		[HttpGet(@"add")]
		public ActionResult Edit() {
			return View();
		}
		[HttpGet(@"edit")]
		public ActionResult Edit([FromQuery] uint News_id, [FromQuery] uint Newstag_id) {
			News_newstagInfo item = News_newstag.GetItem(News_id, Newstag_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? News_id, [FromForm] uint? Newstag_id) {
			News_newstagInfo item = new News_newstagInfo();
			item.News_id = News_id;
			item.Newstag_id = Newstag_id;
			item = News_newstag.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint News_id, [FromQuery] uint Newstag_id) {
			News_newstagInfo item = News_newstag.GetItem(News_id, Newstag_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			int affrows = News_newstag.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] string[] ids) {
			int affrows = 0;
			foreach (string id in ids) {
				string[] vs = id.Split(',');
				affrows += News_newstag.Delete(uint.Parse(vs[0]), uint.Parse(vs[1]));
			}
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
