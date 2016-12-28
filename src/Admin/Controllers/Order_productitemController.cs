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
	public class Order_productitemController : BaseAdminController {
		public Order_productitemController(ILogger<Order_productitemController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Order_id, [FromQuery] uint?[] Productitem_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Order_productitem.Select
				.Where(!string.IsNullOrEmpty(key), "a.title like {0}", string.Concat("%", key, "%"));
			if (Order_id.Length > 0) select.WhereOrder_id(Order_id);
			if (Productitem_id.Length > 0) select.WhereProductitem_id(Productitem_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Order>("b", "b.id = a.order_id")
				.InnerJoin<Productitem>("c", "c.id = a.productitem_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_order", items.Select<Order_productitemInfo, OrderInfo>(a => a.Obj_order).ToBson(), 
				"items_productitem", items.Select<Order_productitemInfo, ProductitemInfo>(a => a.Obj_productitem).ToBson());
		}

		[HttpGet(@"{Order_id}/{Productitem_id}/")]
		public APIReturn Get_item(uint? Order_id, uint? Productitem_id) {
			Order_productitemInfo item = Order_productitem.GetItem(Order_id, Productitem_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Order_id, [FromForm] uint? Productitem_id, [FromForm] uint? Number, [FromForm] decimal? Price, [FromForm] Order_productitemSTATE? State, [FromForm] string Title) {
			Order_productitemInfo item = new Order_productitemInfo();
			item.Order_id = Order_id;
			item.Productitem_id = Productitem_id;
			item.Number = Number;
			item.Price = Price;
			item.State = State;
			item.Title = Title;
			item = Order_productitem.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Order_id}/{Productitem_id}/")]
		public APIReturn Put_update(uint? Order_id, uint? Productitem_id, [FromForm] uint? Number, [FromForm] decimal? Price, [FromForm] Order_productitemSTATE? State, [FromForm] string Title) {
			Order_productitemInfo item = new Order_productitemInfo();
			item.Order_id = Order_id;
			item.Productitem_id = Productitem_id;
			item.Number = Number;
			item.Price = Price;
			item.State = State;
			item.Title = Title;
			int affrows = Order_productitem.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Order_id}/{Productitem_id}/")]
		public APIReturn Delete_delete(uint? Order_id, uint? Productitem_id) {
			int affrows = Order_productitem.Delete(Order_id, Productitem_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
