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
	public class Order_addressController : BaseAdminController {
		public Order_addressController(ILogger<Order_addressController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Order_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Order_address.Select
				.Where(!string.IsNullOrEmpty(key), "a.address like {0} or a.name like {0} or a.tel like {0} or a.telphone like {0} or a.zip like {0}", string.Concat("%", key, "%"));
			if (Order_id.Length > 0) select.WhereOrder_id(Order_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Order>("b", "b.id = a.order_id").Skip((page - 1) * limit).Limit(limit).ToList();
			ViewBag.items = items;
			ViewBag.count = count;
			return View();
		}

		[HttpGet(@"add")]
		public ActionResult Edit() {
			return View();
		}
		[HttpGet(@"edit")]
		public ActionResult Edit([FromQuery] uint Order_id) {
			Order_addressInfo item = Order_address.GetItem(Order_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Order_id, [FromForm] string Address, [FromForm] string Name, [FromForm] string Tel, [FromForm] string Telphone, [FromForm] string Zip) {
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
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Order_id, [FromForm] string Address, [FromForm] string Name, [FromForm] string Tel, [FromForm] string Telphone, [FromForm] string Zip) {
			Order_addressInfo item = Order_address.GetItem(Order_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Address = Address;
			item.Name = Name;
			item.Tel = Tel;
			item.Telphone = Telphone;
			item.Zip = Zip;
			int affrows = Order_address.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Order_address.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
