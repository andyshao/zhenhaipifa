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
	public class MemberController : BaseAdminController {
		public MemberController(ILogger<MemberController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Member.Select
				.Where(!string.IsNullOrEmpty(key), "a.email like {0} or a.telphone like {0} or a.username like {0}", string.Concat("%", key, "%"));
			int count;
			var items = select.Count(out count).Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count);
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			MemberInfo item = Member.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Id, [FromForm] string Email, [FromForm] DateTime? Lastlogin_time, [FromForm] string Telphone, [FromForm] string Username) {
			MemberInfo item = new MemberInfo();
			item.Id = Id;
			item.Create_time = DateTime.Now;
			item.Email = Email;
			item.Lastlogin_time = Lastlogin_time;
			item.Telphone = Telphone;
			item.Username = Username;
			item = Member.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] string Email, [FromForm] DateTime? Lastlogin_time, [FromForm] string Telphone, [FromForm] string Username) {
			MemberInfo item = new MemberInfo();
			item.Id = Id;
			item.Create_time = DateTime.Now;
			item.Email = Email;
			item.Lastlogin_time = Lastlogin_time;
			item.Telphone = Telphone;
			item.Username = Username;
			int affrows = Member.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Member.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
