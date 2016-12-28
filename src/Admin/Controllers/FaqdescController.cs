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
	public class FaqdescController : BaseAdminController {
		public FaqdescController(ILogger<FaqdescController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Faq_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Faqdesc.Select
				.Where(!string.IsNullOrEmpty(key), "a.content like {0}", string.Concat("%", key, "%"));
			if (Faq_id.Length > 0) select.WhereFaq_id(Faq_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Faq>("b", "b.id = a.faq_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_faq", items.Select<FaqdescInfo, FaqInfo>(a => a.Obj_faq).ToBson());
		}

		[HttpGet(@"{Faq_id}/")]
		public APIReturn Get_item(uint? Faq_id) {
			FaqdescInfo item = Faqdesc.GetItem(Faq_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Faq_id, [FromForm] string Content) {
			FaqdescInfo item = new FaqdescInfo();
			item.Faq_id = Faq_id;
			item.Content = Content;
			item = Faqdesc.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Faq_id}/")]
		public APIReturn Put_update(uint? Faq_id, [FromForm] string Content) {
			FaqdescInfo item = new FaqdescInfo();
			item.Faq_id = Faq_id;
			item.Content = Content;
			int affrows = Faqdesc.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Faq_id}/")]
		public APIReturn Delete_delete(uint? Faq_id) {
			int affrows = Faqdesc.Delete(Faq_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
