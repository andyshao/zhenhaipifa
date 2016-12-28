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
	public class ExpressController : BaseAdminController {
		public ExpressController(ILogger<ExpressController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Area_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Express.Select
				.Where(!string.IsNullOrEmpty(key), "a.address like {0} or a.service_features like {0} or a.telphone like {0} or a.title like {0}", string.Concat("%", key, "%"));
			if (Area_id.Length > 0) select.WhereArea_id(Area_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Area>("b", "b.id = a.area_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_area", items.Select<ExpressInfo, AreaInfo>(a => a.Obj_area).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			ExpressInfo item = Express.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Area_id, [FromForm] string Address, [FromForm] string Service_features, [FromForm] string Telphone, [FromForm] string Title) {
			ExpressInfo item = new ExpressInfo();
			item.Area_id = Area_id;
			item.Address = Address;
			item.Create_time = DateTime.Now;
			item.Service_features = Service_features;
			item.Telphone = Telphone;
			item.Title = Title;
			item = Express.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] uint? Area_id, [FromForm] string Address, [FromForm] string Service_features, [FromForm] string Telphone, [FromForm] string Title) {
			ExpressInfo item = new ExpressInfo();
			item.Id = Id;
			item.Area_id = Area_id;
			item.Address = Address;
			item.Create_time = DateTime.Now;
			item.Service_features = Service_features;
			item.Telphone = Telphone;
			item.Title = Title;
			int affrows = Express.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Express.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
