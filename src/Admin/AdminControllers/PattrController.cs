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
	public class PattrController : BaseAdminController {
		public PattrController(ILogger<PattrController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Category_id, [FromQuery] uint?[] Parent_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Pattr.Select
				.Where(!string.IsNullOrEmpty(key), "a.name like {0}", string.Concat("%", key, "%"));
			if (Category_id.Length > 0) select.WhereCategory_id(Category_id);
			if (Parent_id.Length > 0) select.WhereParent_id(Parent_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Category>("b", "b.id = a.category_id").Skip((page - 1) * limit).Limit(limit).ToList();
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
			PattrInfo item = Pattr.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Category_id, [FromForm] uint? Parent_id, [FromForm] bool? Is_filter, [FromForm] string Name) {
			PattrInfo item = new PattrInfo();
			item.Category_id = Category_id;
			item.Parent_id = Parent_id;
			item.Is_filter = Is_filter;
			item.Name = Name;
			item = Pattr.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] uint? Category_id, [FromForm] uint? Parent_id, [FromForm] bool? Is_filter, [FromForm] string Name) {
			PattrInfo item = Pattr.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Category_id = Category_id;
			item.Parent_id = Parent_id;
			item.Is_filter = Is_filter;
			item.Name = Name;
			int affrows = Pattr.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Pattr.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
