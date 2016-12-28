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
	public class PattrController : BaseAdminController {
		public PattrController(ILogger<PattrController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Category_id, [FromQuery] uint?[] Parent_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Pattr.Select
				.Where(!string.IsNullOrEmpty(key), "a.name like {0}", string.Concat("%", key, "%"));
			if (Category_id.Length > 0) select.WhereCategory_id(Category_id);
			if (Parent_id.Length > 0) select.WhereParent_id(Parent_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Category>("b", "b.id = a.category_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_category", items.Select<PattrInfo, CategoryInfo>(a => a.Obj_category).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			PattrInfo item = Pattr.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Category_id, [FromForm] uint? Parent_id, [FromForm] bool? Is_filter, [FromForm] string Name) {
			PattrInfo item = new PattrInfo();
			item.Category_id = Category_id;
			item.Parent_id = Parent_id;
			item.Is_filter = Is_filter;
			item.Name = Name;
			item = Pattr.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] uint? Category_id, [FromForm] uint? Parent_id, [FromForm] bool? Is_filter, [FromForm] string Name) {
			PattrInfo item = new PattrInfo();
			item.Id = Id;
			item.Category_id = Category_id;
			item.Parent_id = Parent_id;
			item.Is_filter = Is_filter;
			item.Name = Name;
			int affrows = Pattr.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Pattr.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
