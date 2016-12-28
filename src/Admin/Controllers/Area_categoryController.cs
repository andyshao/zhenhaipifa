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
	public class Area_categoryController : BaseAdminController {
		public Area_categoryController(ILogger<Area_categoryController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Area_id, [FromQuery] uint?[] Category_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Area_category.Select;
			if (Area_id.Length > 0) select.WhereArea_id(Area_id);
			if (Category_id.Length > 0) select.WhereCategory_id(Category_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Area>("b", "b.id = a.area_id")
				.InnerJoin<Category>("c", "c.id = a.category_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_area", items.Select<Area_categoryInfo, AreaInfo>(a => a.Obj_area).ToBson(), 
				"items_category", items.Select<Area_categoryInfo, CategoryInfo>(a => a.Obj_category).ToBson());
		}

		[HttpGet(@"{Area_id}/{Category_id}/")]
		public APIReturn Get_item(uint? Area_id, uint? Category_id) {
			Area_categoryInfo item = Area_category.GetItem(Area_id, Category_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Area_id, [FromForm] uint? Category_id) {
			Area_categoryInfo item = new Area_categoryInfo();
			item.Area_id = Area_id;
			item.Category_id = Category_id;
			item = Area_category.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Area_id}/{Category_id}/")]
		public APIReturn Put_update(uint? Area_id, uint? Category_id) {
			Area_categoryInfo item = new Area_categoryInfo();
			item.Area_id = Area_id;
			item.Category_id = Category_id;
			int affrows = Area_category.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Area_id}/{Category_id}/")]
		public APIReturn Delete_delete(uint? Area_id, uint? Category_id) {
			int affrows = Area_category.Delete(Area_id, Category_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
