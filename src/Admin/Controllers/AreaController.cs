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
	public class AreaController : BaseAdminController {
		public AreaController(ILogger<AreaController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Parent_id, [FromQuery] uint[] Category_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Area.Select
				.Where(!string.IsNullOrEmpty(key), "a.name like {0}", string.Concat("%", key, "%"));
			if (Parent_id.Length > 0) select.WhereParent_id(Parent_id);
			if (Category_id.Length > 0) select.WhereCategory_id(Category_id);
			int count;
			var items = select.Count(out count).Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count);
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			AreaInfo item = Area.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Parent_id, [FromForm] string Name, [FromForm] uint[] mn_Category) {
			AreaInfo item = new AreaInfo();
			item.Parent_id = Parent_id;
			item.Name = Name;
			item = Area.Insert(item);
			//关联 Category
			foreach (uint mn_Category_in in mn_Category)
				item.FlagCategory(mn_Category_in);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] uint? Parent_id, [FromForm] string Name, [FromForm] uint[] mn_Category) {
			AreaInfo item = new AreaInfo();
			item.Id = Id;
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
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Area.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
