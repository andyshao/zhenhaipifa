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
	public class CategoryController : BaseAdminController {
		public CategoryController(ILogger<CategoryController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Parent_id, [FromQuery] uint[] Area_id, [FromQuery] uint[] Markettype_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Category.Select
				.Where(!string.IsNullOrEmpty(key), "a.title like {0}", string.Concat("%", key, "%"));
			if (Parent_id.Length > 0) select.WhereParent_id(Parent_id);
			if (Area_id.Length > 0) select.WhereArea_id(Area_id);
			if (Markettype_id.Length > 0) select.WhereMarkettype_id(Markettype_id);
			int count;
			var items = select.Count(out count).Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count);
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			CategoryInfo item = Category.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Parent_id, [FromForm] string Title, [FromForm] uint[] mn_Area, [FromForm] uint[] mn_Markettype) {
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

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] uint? Parent_id, [FromForm] string Title, [FromForm] uint[] mn_Area, [FromForm] uint[] mn_Markettype) {
			CategoryInfo item = new CategoryInfo();
			item.Id = Id;
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
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Category.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
