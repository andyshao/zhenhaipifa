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
	public class FaqController : BaseAdminController {
		public FaqController(ILogger<FaqController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Faqtype_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Faq.Select
				.Where(!string.IsNullOrEmpty(key), "a.title like {0}", string.Concat("%", key, "%"));
			if (Faqtype_id.Length > 0) select.WhereFaqtype_id(Faqtype_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Faqtype>("b", "b.id = a.faqtype_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_faqtype", items.Select<FaqInfo, FaqtypeInfo>(a => a.Obj_faqtype).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			FaqInfo item = Faq.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Faqtype_id, [FromForm] string Title) {
			FaqInfo item = new FaqInfo();
			item.Faqtype_id = Faqtype_id;
			item.Create_time = DateTime.Now;
			item.Title = Title;
			item = Faq.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] uint? Faqtype_id, [FromForm] string Title) {
			FaqInfo item = new FaqInfo();
			item.Id = Id;
			item.Faqtype_id = Faqtype_id;
			item.Create_time = DateTime.Now;
			item.Title = Title;
			int affrows = Faq.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Faq.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
