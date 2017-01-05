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
	public class Product_questionController : BaseAdminController {
		public Product_questionController(ILogger<Product_questionController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Member_id, [FromQuery] uint?[] Parent_id, [FromQuery] uint?[] Product_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Product_question.Select
				.Where(!string.IsNullOrEmpty(key), "a.content like {0} or a.email like {0} or a.name like {0}", string.Concat("%", key, "%"));
			if (Member_id.Length > 0) select.WhereMember_id(Member_id);
			if (Parent_id.Length > 0) select.WhereParent_id(Parent_id);
			if (Product_id.Length > 0) select.WhereProduct_id(Product_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Member>("b", "b.id = a.member_id")
				.LeftJoin<Product>("c", "c.id = a.product_id").Skip((page - 1) * limit).Limit(limit).ToList();
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
			Product_questionInfo item = Product_question.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Member_id, [FromForm] uint? Parent_id, [FromForm] uint? Product_id, [FromForm] string Content, [FromForm] string Email, [FromForm] string Name, [FromForm] Product_questionSTATE? State) {
			Product_questionInfo item = new Product_questionInfo();
			item.Member_id = Member_id;
			item.Parent_id = Parent_id;
			item.Product_id = Product_id;
			item.Content = Content;
			item.Create_time = DateTime.Now;
			item.Email = Email;
			item.Name = Name;
			item.State = State;
			item = Product_question.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] uint? Member_id, [FromForm] uint? Parent_id, [FromForm] uint? Product_id, [FromForm] string Content, [FromForm] string Email, [FromForm] string Name, [FromForm] Product_questionSTATE? State) {
			Product_questionInfo item = Product_question.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Member_id = Member_id;
			item.Parent_id = Parent_id;
			item.Product_id = Product_id;
			item.Content = Content;
			item.Create_time = DateTime.Now;
			item.Email = Email;
			item.Name = Name;
			item.State = State;
			int affrows = Product_question.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Product_question.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
