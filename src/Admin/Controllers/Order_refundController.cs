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
	public class Order_refundController : BaseAdminController {
		public Order_refundController(ILogger<Order_refundController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Order_id, [FromQuery] uint?[] Productitem_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Order_refund.Select
				.Where(!string.IsNullOrEmpty(key), "a.descript like {0} or a.email like {0} or a.img_url like {0} or a.tel like {0} or a.telphone like {0}", string.Concat("%", key, "%"));
			if (Order_id.Length > 0) select.WhereOrder_id(Order_id);
			if (Productitem_id.Length > 0) select.WhereProductitem_id(Productitem_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Order>("b", "b.id = a.order_id")
				.InnerJoin<Productitem>("c", "c.id = a.productitem_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_order", items.Select<Order_refundInfo, OrderInfo>(a => a.Obj_order).ToBson(), 
				"items_productitem", items.Select<Order_refundInfo, ProductitemInfo>(a => a.Obj_productitem).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			Order_refundInfo item = Order_refund.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Order_id, [FromForm] uint? Productitem_id, [FromForm] string Descript, [FromForm] string Email, [FromForm] string Img_url, [FromForm] Order_refundSTATE? State, [FromForm] string Tel, [FromForm] string Telphone, [FromForm] decimal? Wealth) {
			Order_refundInfo item = new Order_refundInfo();
			item.Order_id = Order_id;
			item.Productitem_id = Productitem_id;
			item.Create_time = DateTime.Now;
			item.Descript = Descript;
			item.Email = Email;
			item.Img_url = Img_url;
			item.State = State;
			item.Tel = Tel;
			item.Telphone = Telphone;
			item.Wealth = Wealth;
			item = Order_refund.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] uint? Order_id, [FromForm] uint? Productitem_id, [FromForm] string Descript, [FromForm] string Email, [FromForm] string Img_url, [FromForm] Order_refundSTATE? State, [FromForm] string Tel, [FromForm] string Telphone, [FromForm] decimal? Wealth) {
			Order_refundInfo item = new Order_refundInfo();
			item.Id = Id;
			item.Order_id = Order_id;
			item.Productitem_id = Productitem_id;
			item.Create_time = DateTime.Now;
			item.Descript = Descript;
			item.Email = Email;
			item.Img_url = Img_url;
			item.State = State;
			item.Tel = Tel;
			item.Telphone = Telphone;
			item.Wealth = Wealth;
			int affrows = Order_refund.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Order_refund.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
