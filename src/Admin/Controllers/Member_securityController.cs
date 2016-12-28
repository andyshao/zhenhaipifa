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
	public class Member_securityController : BaseAdminController {
		public Member_securityController(ILogger<Member_securityController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Member_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Member_security.Select
				.Where(!string.IsNullOrEmpty(key), "a.password like {0}", string.Concat("%", key, "%"));
			if (Member_id.Length > 0) select.WhereMember_id(Member_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Member>("b", "b.id = a.member_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_member", items.Select<Member_securityInfo, MemberInfo>(a => a.Obj_member).ToBson());
		}

		[HttpGet(@"{Member_id}/")]
		public APIReturn Get_item(uint? Member_id) {
			Member_securityInfo item = Member_security.GetItem(Member_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Member_id, [FromForm] string Password) {
			Member_securityInfo item = new Member_securityInfo();
			item.Member_id = Member_id;
			item.Password = Password;
			item = Member_security.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Member_id}/")]
		public APIReturn Put_update(uint? Member_id, [FromForm] string Password) {
			Member_securityInfo item = new Member_securityInfo();
			item.Member_id = Member_id;
			item.Password = Password;
			int affrows = Member_security.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Member_id}/")]
		public APIReturn Delete_delete(uint? Member_id) {
			int affrows = Member_security.Delete(Member_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
