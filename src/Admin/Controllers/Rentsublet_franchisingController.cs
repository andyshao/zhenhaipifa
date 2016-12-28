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
	public class Rentsublet_franchisingController : BaseAdminController {
		public Rentsublet_franchisingController(ILogger<Rentsublet_franchisingController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Franchising_id, [FromQuery] uint?[] Rentsublet_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Rentsublet_franchising.Select;
			if (Franchising_id.Length > 0) select.WhereFranchising_id(Franchising_id);
			if (Rentsublet_id.Length > 0) select.WhereRentsublet_id(Rentsublet_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Franchising>("b", "b.id = a.franchising_id")
				.InnerJoin<Rentsublet>("c", "c.id = a.rentsublet_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_franchising", items.Select<Rentsublet_franchisingInfo, FranchisingInfo>(a => a.Obj_franchising).ToBson(), 
				"items_rentsublet", items.Select<Rentsublet_franchisingInfo, RentsubletInfo>(a => a.Obj_rentsublet).ToBson());
		}

		[HttpGet(@"{Franchising_id}/{Rentsublet_id}/")]
		public APIReturn Get_item(uint? Franchising_id, uint? Rentsublet_id) {
			Rentsublet_franchisingInfo item = Rentsublet_franchising.GetItem(Franchising_id, Rentsublet_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Franchising_id, [FromForm] uint? Rentsublet_id) {
			Rentsublet_franchisingInfo item = new Rentsublet_franchisingInfo();
			item.Franchising_id = Franchising_id;
			item.Rentsublet_id = Rentsublet_id;
			item = Rentsublet_franchising.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Franchising_id}/{Rentsublet_id}/")]
		public APIReturn Put_update(uint? Franchising_id, uint? Rentsublet_id) {
			Rentsublet_franchisingInfo item = new Rentsublet_franchisingInfo();
			item.Franchising_id = Franchising_id;
			item.Rentsublet_id = Rentsublet_id;
			int affrows = Rentsublet_franchising.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Franchising_id}/{Rentsublet_id}/")]
		public APIReturn Delete_delete(uint? Franchising_id, uint? Rentsublet_id) {
			int affrows = Rentsublet_franchising.Delete(Franchising_id, Rentsublet_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
