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
	public class CategoryController : BaseAdminController {
		public CategoryController(ILogger<CategoryController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Parent_id, [FromQuery] uint[] Area_id, [FromQuery] uint[] Markettype_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Category.Select
				.Where(!string.IsNullOrEmpty(key), "a.title like {0}", string.Concat("%", key, "%"));
			if (Parent_id.Length > 0) select.WhereParent_id(Parent_id);
			if (Area_id.Length > 0) select.WhereArea_id(Area_id);
			if (Markettype_id.Length > 0) select.WhereMarkettype_id(Markettype_id);
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
			CategoryInfo item = Category.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Parent_id, [FromForm] string Title, [FromForm] uint[] mn_Area, [FromForm] uint[] mn_Markettype) {
			CategoryInfo item = new CategoryInfo();
			item.Parent_id = Parent_id;
			item.Title = Title;
			item = Category.Insert(item);
			//关联 Area
			foreach (uint mn_Area_in in mn_Area)
				item.FlagArea(mn_Area_in);
			//关联 Markettype
			foreach (uint mn_Markettype_in in mn_Markettype)
				item.FlagMarkettype(mn_Markettype_in);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] uint? Parent_id, [FromForm] string Title, [FromForm] uint[] mn_Area, [FromForm] uint[] mn_Markettype) {
			CategoryInfo item = Category.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Parent_id = Parent_id;
			item.Title = Title;
			int affrows = Category.Update(item);
			//关联 Area
			if (mn_Area.Length == 0) {
				item.UnflagAreaALL();
			} else {
				List<uint> mn_Area_list = mn_Area.ToList();
				foreach (var Obj_area in item.Obj_areas) {
					int idx = mn_Area_list.FindIndex(a => a == Obj_area.Id);
					if (idx == -1) item.UnflagArea(Obj_area.Id);
					else mn_Area_list.RemoveAt(idx);
				}
				mn_Area_list.ForEach(a => item.FlagArea(a));
			}
			//关联 Markettype
			if (mn_Markettype.Length == 0) {
				item.UnflagMarkettypeALL();
			} else {
				List<uint> mn_Markettype_list = mn_Markettype.ToList();
				foreach (var Obj_markettype in item.Obj_markettypes) {
					int idx = mn_Markettype_list.FindIndex(a => a == Obj_markettype.Id);
					if (idx == -1) item.UnflagMarkettype(Obj_markettype.Id);
					else mn_Markettype_list.RemoveAt(idx);
				}
				mn_Markettype_list.ForEach(a => item.FlagMarkettype(a));
			}
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Category.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
