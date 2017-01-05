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
	public class Order_refundController : BaseAdminController {
		public Order_refundController(ILogger<Order_refundController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Order_id, [FromQuery] uint?[] Productitem_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Order_refund.Select
				.Where(!string.IsNullOrEmpty(key), "a.descript like {0} or a.email like {0} or a.img_url like {0} or a.tel like {0} or a.telphone like {0}", string.Concat("%", key, "%"));
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
		public ActionResult Edit([FromQuery] uint Id) {
			Order_refundInfo item = Order_refund.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Order_id, [FromForm] uint? Productitem_id, [FromForm] string Descript, [FromForm] string Email, [FromForm] string Img_url, [FromForm] IFormFile Img_url_file, [FromForm] Order_refundSTATE? State, [FromForm] string Tel, [FromForm] string Telphone, [FromForm] decimal? Wealth) {
			Order_refundInfo item = new Order_refundInfo();
			if (Img_url_file != null) {
				item.Img_url = $"/upload/{Guid.NewGuid().ToString()}.png";
				using (FileStream fs = new FileStream(System.IO.Path.Combine(AppContext.BaseDirectory, item.Img_url), FileMode.Create)) Img_url_file.CopyTo(fs);
			} else
				item.Img_url = Img_url;
			item.Order_id = Order_id;
			item.Productitem_id = Productitem_id;
			item.Create_time = DateTime.Now;
			item.Descript = Descript;
			item.Email = Email;
			item.State = State;
			item.Tel = Tel;
			item.Telphone = Telphone;
			item.Wealth = Wealth;
			item = Order_refund.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] uint? Order_id, [FromForm] uint? Productitem_id, [FromForm] string Descript, [FromForm] string Email, [FromForm] string Img_url, [FromForm] IFormFile Img_url_file, [FromForm] Order_refundSTATE? State, [FromForm] string Tel, [FromForm] string Telphone, [FromForm] decimal? Wealth) {
			Order_refundInfo item = Order_refund.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			if (!string.IsNullOrEmpty(item.Img_url) && (item.Img_url != Img_url || Img_url_file != null)) {
				string path = System.IO.Path.Combine(AppContext.BaseDirectory, item.Img_url);
				if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
			}
			if (Img_url_file != null) {
				item.Img_url = $"/upload/{Guid.NewGuid().ToString()}.png";
				using (FileStream fs = new FileStream(System.IO.Path.Combine(AppContext.BaseDirectory, item.Img_url), FileMode.Create)) Img_url_file.CopyTo(fs);
			} else
				item.Img_url = Img_url;
			item.Order_id = Order_id;
			item.Productitem_id = Productitem_id;
			item.Create_time = DateTime.Now;
			item.Descript = Descript;
			item.Email = Email;
			item.State = State;
			item.Tel = Tel;
			item.Telphone = Telphone;
			item.Wealth = Wealth;
			int affrows = Order_refund.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Order_refund.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
