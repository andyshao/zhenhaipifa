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
	public class Factory_franchisingController : BaseAdminController {
		public Factory_franchisingController(ILogger<Factory_franchisingController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Factory_id, [FromQuery] uint?[] Franchising_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Factory_franchising.Select;
			if (Factory_id.Length > 0) select.WhereFactory_id(Factory_id);
			if (Franchising_id.Length > 0) select.WhereFranchising_id(Franchising_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Factory>("b", "b.id = a.factory_id")
				.InnerJoin<Franchising>("c", "c.id = a.franchising_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_factory", items.Select<Factory_franchisingInfo, FactoryInfo>(a => a.Obj_factory).ToBson(), 
				"items_franchising", items.Select<Factory_franchisingInfo, FranchisingInfo>(a => a.Obj_franchising).ToBson());
		}

		[HttpGet(@"{Factory_id}/{Franchising_id}/")]
		public APIReturn Get_item(uint? Factory_id, uint? Franchising_id) {
			Factory_franchisingInfo item = Factory_franchising.GetItem(Factory_id, Franchising_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Factory_id, [FromForm] uint? Franchising_id) {
			Factory_franchisingInfo item = new Factory_franchisingInfo();
			item.Factory_id = Factory_id;
			item.Franchising_id = Franchising_id;
			item = Factory_franchising.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Factory_id}/{Franchising_id}/")]
		public APIReturn Put_update(uint? Factory_id, uint? Franchising_id) {
			Factory_franchisingInfo item = new Factory_franchisingInfo();
			item.Factory_id = Factory_id;
			item.Franchising_id = Franchising_id;
			int affrows = Factory_franchising.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Factory_id}/{Franchising_id}/")]
		public APIReturn Delete_delete(uint? Factory_id, uint? Franchising_id) {
			int affrows = Factory_franchising.Delete(Factory_id, Franchising_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
