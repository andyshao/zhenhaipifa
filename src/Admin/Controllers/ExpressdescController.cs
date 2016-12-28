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
	public class ExpressdescController : BaseAdminController {
		public ExpressdescController(ILogger<ExpressdescController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Express_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Expressdesc.Select
				.Where(!string.IsNullOrEmpty(key), "a.content like {0}", string.Concat("%", key, "%"));
			if (Express_id.Length > 0) select.WhereExpress_id(Express_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Express>("b", "b.id = a.express_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_express", items.Select<ExpressdescInfo, ExpressInfo>(a => a.Obj_express).ToBson());
		}

		[HttpGet(@"{Express_id}/")]
		public APIReturn Get_item(uint? Express_id) {
			ExpressdescInfo item = Expressdesc.GetItem(Express_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Express_id, [FromForm] string Content) {
			ExpressdescInfo item = new ExpressdescInfo();
			item.Express_id = Express_id;
			item.Content = Content;
			item = Expressdesc.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Express_id}/")]
		public APIReturn Put_update(uint? Express_id, [FromForm] string Content) {
			ExpressdescInfo item = new ExpressdescInfo();
			item.Express_id = Express_id;
			item.Content = Content;
			int affrows = Expressdesc.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Express_id}/")]
		public APIReturn Delete_delete(uint? Express_id) {
			int affrows = Expressdesc.Delete(Express_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
