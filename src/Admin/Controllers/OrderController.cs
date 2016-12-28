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
	public class OrderController : BaseAdminController {
		public OrderController(ILogger<OrderController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Member_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Order.Select
				.Where(!string.IsNullOrEmpty(key), "a.code like {0} or a.express_code like {0} or a.express_name like {0} or a.paymethod like {0} or a.remark like {0}", string.Concat("%", key, "%"));
			if (Member_id.Length > 0) select.WhereMember_id(Member_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Member>("b", "b.id = a.member_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_member", items.Select<OrderInfo, MemberInfo>(a => a.Obj_member).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			OrderInfo item = Order.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Member_id, [FromForm] string Code, [FromForm] string Express_code, [FromForm] string Express_name, [FromForm] string Paymethod, [FromForm] string Remark, [FromForm] OrderSTATE? State, [FromForm] decimal? Total_express_price, [FromForm] decimal? Total_original_price, [FromForm] decimal? Total_price) {
			OrderInfo item = new OrderInfo();
			item.Member_id = Member_id;
			item.Code = Code;
			item.Create_time = DateTime.Now;
			item.Express_code = Express_code;
			item.Express_name = Express_name;
			item.Paymethod = Paymethod;
			item.Remark = Remark;
			item.State = State;
			item.Total_express_price = Total_express_price;
			item.Total_original_price = Total_original_price;
			item.Total_price = Total_price;
			item.Update_time = DateTime.Now;
			item = Order.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] uint? Member_id, [FromForm] string Code, [FromForm] string Express_code, [FromForm] string Express_name, [FromForm] string Paymethod, [FromForm] string Remark, [FromForm] OrderSTATE? State, [FromForm] decimal? Total_express_price, [FromForm] decimal? Total_original_price, [FromForm] decimal? Total_price) {
			OrderInfo item = new OrderInfo();
			item.Id = Id;
			item.Member_id = Member_id;
			item.Code = Code;
			item.Create_time = DateTime.Now;
			item.Express_code = Express_code;
			item.Express_name = Express_name;
			item.Paymethod = Paymethod;
			item.Remark = Remark;
			item.State = State;
			item.Total_express_price = Total_express_price;
			item.Total_original_price = Total_original_price;
			item.Total_price = Total_price;
			item.Update_time = DateTime.Now;
			int affrows = Order.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Order.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
