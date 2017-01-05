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
	public class OrderController : BaseAdminController {
		public OrderController(ILogger<OrderController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Member_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Order.Select
				.Where(!string.IsNullOrEmpty(key), "a.code like {0} or a.express_code like {0} or a.express_name like {0} or a.paymethod like {0} or a.remark like {0}", string.Concat("%", key, "%"));
			if (Member_id.Length > 0) select.WhereMember_id(Member_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Member>("b", "b.id = a.member_id").Skip((page - 1) * limit).Limit(limit).ToList();
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
			OrderInfo item = Order.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Member_id, [FromForm] string Code, [FromForm] string Express_code, [FromForm] string Express_name, [FromForm] string Paymethod, [FromForm] string Remark, [FromForm] OrderSTATE? State, [FromForm] decimal? Total_express_price, [FromForm] decimal? Total_original_price, [FromForm] decimal? Total_price) {
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
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] uint? Member_id, [FromForm] string Code, [FromForm] string Express_code, [FromForm] string Express_name, [FromForm] string Paymethod, [FromForm] string Remark, [FromForm] OrderSTATE? State, [FromForm] decimal? Total_express_price, [FromForm] decimal? Total_original_price, [FromForm] decimal? Total_price) {
			OrderInfo item = Order.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
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
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Order.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
