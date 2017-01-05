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
	public class ExpressdescController : BaseAdminController {
		public ExpressdescController(ILogger<ExpressdescController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Express_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Expressdesc.Select
				.Where(!string.IsNullOrEmpty(key), "a.content like {0}", string.Concat("%", key, "%"));
			if (Express_id.Length > 0) select.WhereExpress_id(Express_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Express>("b", "b.id = a.express_id").Skip((page - 1) * limit).Limit(limit).ToList();
			ViewBag.items = items;
			ViewBag.count = count;
			return View();
		}

		[HttpGet(@"add")]
		public ActionResult Edit() {
			return View();
		}
		[HttpGet(@"edit")]
		public ActionResult Edit([FromQuery] uint Express_id) {
			ExpressdescInfo item = Expressdesc.GetItem(Express_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Express_id, [FromForm] string Content) {
			ExpressdescInfo item = new ExpressdescInfo();
			item.Express_id = Express_id;
			item.Content = Content;
			item = Expressdesc.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Express_id, [FromForm] string Content) {
			ExpressdescInfo item = Expressdesc.GetItem(Express_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Content = Content;
			int affrows = Expressdesc.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Expressdesc.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
