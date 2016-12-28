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
	public class Order_addressController : BaseAdminController {
		public Order_addressController(ILogger<Order_addressController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Order_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Order_address.Select
				.Where(!string.IsNullOrEmpty(key), "a.address like {0} or a.name like {0} or a.tel like {0} or a.telphone like {0} or a.zip like {0}", string.Concat("%", key, "%"));
			if (Order_id.Length > 0) select.WhereOrder_id(Order_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Order>("b", "b.id = a.order_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_order", items.Select<Order_addressInfo, OrderInfo>(a => a.Obj_order).ToBson());
		}

		[HttpGet(@"{Order_id}/")]
		public APIReturn Get_item(uint? Order_id) {
			Order_addressInfo item = Order_address.GetItem(Order_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Order_id, [FromForm] string Address, [FromForm] string Name, [FromForm] string Tel, [FromForm] string Telphone, [FromForm] string Zip) {
			Order_addressInfo item = new Order_addressInfo();
			item.Order_id = Order_id;
			item.Address = Address;
			item.Name = Name;
			item.Tel = Tel;
			item.Telphone = Telphone;
			item.Zip = Zip;
			item = Order_address.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Order_id}/")]
		public APIReturn Put_update(uint? Order_id, [FromForm] string Address, [FromForm] string Name, [FromForm] string Tel, [FromForm] string Telphone, [FromForm] string Zip) {
			Order_addressInfo item = new Order_addressInfo();
			item.Order_id = Order_id;
			item.Address = Address;
			item.Name = Name;
			item.Tel = Tel;
			item.Telphone = Telphone;
			item.Zip = Zip;
			int affrows = Order_address.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Order_id}/")]
		public APIReturn Delete_delete(uint? Order_id) {
			int affrows = Order_address.Delete(Order_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
