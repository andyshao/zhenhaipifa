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
	public class Markettype_categoryController : BaseAdminController {
		public Markettype_categoryController(ILogger<Markettype_categoryController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Category_id, [FromQuery] uint?[] Markettype_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Markettype_category.Select;
			if (Category_id.Length > 0) select.WhereCategory_id(Category_id);
			if (Markettype_id.Length > 0) select.WhereMarkettype_id(Markettype_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Category>("b", "b.id = a.category_id")
				.InnerJoin<Markettype>("c", "c.id = a.markettype_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_category", items.Select<Markettype_categoryInfo, CategoryInfo>(a => a.Obj_category).ToBson(), 
				"items_markettype", items.Select<Markettype_categoryInfo, MarkettypeInfo>(a => a.Obj_markettype).ToBson());
		}

		[HttpGet(@"{Category_id}/{Markettype_id}/")]
		public APIReturn Get_item(uint? Category_id, uint? Markettype_id) {
			Markettype_categoryInfo item = Markettype_category.GetItem(Category_id, Markettype_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Category_id, [FromForm] uint? Markettype_id) {
			Markettype_categoryInfo item = new Markettype_categoryInfo();
			item.Category_id = Category_id;
			item.Markettype_id = Markettype_id;
			item = Markettype_category.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Category_id}/{Markettype_id}/")]
		public APIReturn Put_update(uint? Category_id, uint? Markettype_id) {
			Markettype_categoryInfo item = new Markettype_categoryInfo();
			item.Category_id = Category_id;
			item.Markettype_id = Markettype_id;
			int affrows = Markettype_category.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Category_id}/{Markettype_id}/")]
		public APIReturn Delete_delete(uint? Category_id, uint? Markettype_id) {
			int affrows = Markettype_category.Delete(Category_id, Markettype_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
