using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using pifa.BLL;
using pifa.Model;

namespace pifa.AdminControllers {
	[Route("[controller]")]
	public class Order_productitemController : BaseAdminController {
		public Order_productitemController(ILogger<Order_productitemController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Order_id, [FromQuery] uint?[] Productitem_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Order_productitem.Select
				.Where(!string.IsNullOrEmpty(key), "a.title like {0}", string.Concat("%", key, "%"));
			if (Order_id.Length > 0) select.WhereOrder_id(Order_id);
			if (Productitem_id.Length > 0) select.WhereProductitem_id(Productitem_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Order>("b", "b.id = a.order_id")
				.LeftJoin<Productitem>("c", "c.id = a.productitem_id").Skip((page - 1) * limit).Limit(limit).ToList();
			ViewBag.items = items;
			ViewBag.count = count;
			return View();
		}

		[HttpGet(@"add")]
		public ActionResult Edit() {
			return View();
		}
		[HttpGet(@"edit")]
		public ActionResult Edit([FromQuery] uint Order_id, [FromQuery] uint Productitem_id) {
			Order_productitemInfo item = Order_productitem.GetItem(Order_id, Productitem_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Order_id, [FromForm] uint? Productitem_id, [FromForm] uint? Number, [FromForm] decimal? Price, [FromForm] Order_productitemSTATE? State, [FromForm] string Title) {
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
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Order_id, [FromQuery] uint Productitem_id, [FromForm] uint? Number, [FromForm] decimal? Price, [FromForm] Order_productitemSTATE? State, [FromForm] string Title) {
			Order_productitemInfo item = Order_productitem.GetItem(Order_id, Productitem_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Number = Number;
			item.Price = Price;
			item.State = State;
			item.Title = Title;
			int affrows = Order_productitem.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] string[] ids) {
			int affrows = 0;
			foreach (string id in ids) {
				string[] vs = id.Split(',');
				affrows += Order_productitem.Delete(uint.Parse(vs[0]), uint.Parse(vs[1]));
			}
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
