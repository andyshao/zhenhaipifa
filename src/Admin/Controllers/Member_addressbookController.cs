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
	public class Member_addressbookController : BaseAdminController {
		public Member_addressbookController(ILogger<Member_addressbookController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Member_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Member_addressbook.Select
				.Where(!string.IsNullOrEmpty(key), "a.address like {0} or a.name like {0} or a.tel like {0} or a.telphone like {0} or a.zip like {0}", string.Concat("%", key, "%"));
			if (Member_id.Length > 0) select.WhereMember_id(Member_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Member>("b", "b.id = a.member_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_member", items.Select<Member_addressbookInfo, MemberInfo>(a => a.Obj_member).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			Member_addressbookInfo item = Member_addressbook.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Member_id, [FromForm] string Address, [FromForm] bool? Is_default, [FromForm] string Name, [FromForm] string Tel, [FromForm] string Telphone, [FromForm] string Zip) {
			Member_addressbookInfo item = new Member_addressbookInfo();
			item.Member_id = Member_id;
			item.Address = Address;
			item.Create_time = DateTime.Now;
			item.Is_default = Is_default;
			item.Name = Name;
			item.Tel = Tel;
			item.Telphone = Telphone;
			item.Zip = Zip;
			item = Member_addressbook.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] uint? Member_id, [FromForm] string Address, [FromForm] bool? Is_default, [FromForm] string Name, [FromForm] string Tel, [FromForm] string Telphone, [FromForm] string Zip) {
			Member_addressbookInfo item = new Member_addressbookInfo();
			item.Id = Id;
			item.Member_id = Member_id;
			item.Address = Address;
			item.Create_time = DateTime.Now;
			item.Is_default = Is_default;
			item.Name = Name;
			item.Tel = Tel;
			item.Telphone = Telphone;
			item.Zip = Zip;
			int affrows = Member_addressbook.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Member_addressbook.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
