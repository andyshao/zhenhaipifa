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
	public class FactorydescController : BaseAdminController {
		public FactorydescController(ILogger<FactorydescController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Factory_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Factorydesc.Select
				.Where(!string.IsNullOrEmpty(key), "a.address like {0} or a.content like {0} or a.url like {0} or a.username like {0}", string.Concat("%", key, "%"));
			if (Factory_id.Length > 0) select.WhereFactory_id(Factory_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Factory>("b", "b.id = a.factory_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_factory", items.Select<FactorydescInfo, FactoryInfo>(a => a.Obj_factory).ToBson());
		}

		[HttpGet(@"{Factory_id}/")]
		public APIReturn Get_item(uint? Factory_id) {
			FactorydescInfo item = Factorydesc.GetItem(Factory_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Factory_id, [FromForm] string Address, [FromForm] string Content, [FromForm] string Url, [FromForm] string Username) {
			FactorydescInfo item = new FactorydescInfo();
			item.Factory_id = Factory_id;
			item.Address = Address;
			item.Content = Content;
			item.Url = Url;
			item.Username = Username;
			item = Factorydesc.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Factory_id}/")]
		public APIReturn Put_update(uint? Factory_id, [FromForm] string Address, [FromForm] string Content, [FromForm] string Url, [FromForm] string Username) {
			FactorydescInfo item = new FactorydescInfo();
			item.Factory_id = Factory_id;
			item.Address = Address;
			item.Content = Content;
			item.Url = Url;
			item.Username = Username;
			int affrows = Factorydesc.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Factory_id}/")]
		public APIReturn Delete_delete(uint? Factory_id) {
			int affrows = Factorydesc.Delete(Factory_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
