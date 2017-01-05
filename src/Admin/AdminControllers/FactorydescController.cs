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
	public class FactorydescController : BaseAdminController {
		public FactorydescController(ILogger<FactorydescController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Factory_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Factorydesc.Select
				.Where(!string.IsNullOrEmpty(key), "a.address like {0} or a.content like {0} or a.url like {0} or a.username like {0}", string.Concat("%", key, "%"));
			if (Factory_id.Length > 0) select.WhereFactory_id(Factory_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Factory>("b", "b.id = a.factory_id").Skip((page - 1) * limit).Limit(limit).ToList();
			ViewBag.items = items;
			ViewBag.count = count;
			return View();
		}

		[HttpGet(@"add")]
		public ActionResult Edit() {
			return View();
		}
		[HttpGet(@"edit")]
		public ActionResult Edit([FromQuery] uint Factory_id) {
			FactorydescInfo item = Factorydesc.GetItem(Factory_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Factory_id, [FromForm] string Address, [FromForm] string Content, [FromForm] string Url, [FromForm] string Username) {
			FactorydescInfo item = new FactorydescInfo();
			item.Factory_id = Factory_id;
			item.Address = Address;
			item.Content = Content;
			item.Url = Url;
			item.Username = Username;
			item = Factorydesc.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Factory_id, [FromForm] string Address, [FromForm] string Content, [FromForm] string Url, [FromForm] string Username) {
			FactorydescInfo item = Factorydesc.GetItem(Factory_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Address = Address;
			item.Content = Content;
			item.Url = Url;
			item.Username = Username;
			int affrows = Factorydesc.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Factorydesc.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
