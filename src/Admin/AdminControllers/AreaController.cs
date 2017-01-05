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
	public class AreaController : BaseAdminController {
		public AreaController(ILogger<AreaController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Parent_id, [FromQuery] uint[] Category_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Area.Select
				.Where(!string.IsNullOrEmpty(key), "a.name like {0}", string.Concat("%", key, "%"));
			if (Parent_id.Length > 0) select.WhereParent_id(Parent_id);
			if (Category_id.Length > 0) select.WhereCategory_id(Category_id);
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
			AreaInfo item = Area.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Parent_id, [FromForm] string Name, [FromForm] uint[] mn_Category) {
			AreaInfo item = new AreaInfo();
			item.Parent_id = Parent_id;
			item.Name = Name;
			item = Area.Insert(item);
			//关联 Category
			foreach (uint mn_Category_in in mn_Category)
				item.FlagCategory(mn_Category_in);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] uint? Parent_id, [FromForm] string Name, [FromForm] uint[] mn_Category) {
			AreaInfo item = Area.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Parent_id = Parent_id;
			item.Name = Name;
			int affrows = Area.Update(item);
			//关联 Category
			if (mn_Category.Length == 0) {
				item.UnflagCategoryALL();
			} else {
				List<uint> mn_Category_list = mn_Category.ToList();
				foreach (var Obj_category in item.Obj_categorys) {
					int idx = mn_Category_list.FindIndex(a => a == Obj_category.Id);
					if (idx == -1) item.UnflagCategory(Obj_category.Id);
					else mn_Category_list.RemoveAt(idx);
				}
				mn_Category_list.ForEach(a => item.FlagCategory(a));
			}
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Area.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
