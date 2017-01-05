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
	public class Product_commentController : BaseAdminController {
		public Product_commentController(ILogger<Product_commentController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Member_id, [FromQuery] uint?[] Order_id, [FromQuery] uint?[] Product_id, [FromQuery] uint?[] Productitem_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Product_comment.Select
				.Where(!string.IsNullOrEmpty(key), "a.content like {0} or a.nickname like {0} or a.title like {0} or a.upload_image_url like {0}", string.Concat("%", key, "%"));
			if (Member_id.Length > 0) select.WhereMember_id(Member_id);
			if (Order_id.Length > 0) select.WhereOrder_id(Order_id);
			if (Product_id.Length > 0) select.WhereProduct_id(Product_id);
			if (Productitem_id.Length > 0) select.WhereProductitem_id(Productitem_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Member>("b", "b.id = a.member_id")
				.LeftJoin<Order>("c", "c.id = a.order_id")
				.LeftJoin<Product>("d", "d.id = a.product_id")
				.LeftJoin<Productitem>("e", "e.id = a.productitem_id").Skip((page - 1) * limit).Limit(limit).ToList();
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
			Product_commentInfo item = Product_comment.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Member_id, [FromForm] uint? Order_id, [FromForm] uint? Product_id, [FromForm] uint? Productitem_id, [FromForm] string Content, [FromForm] string Nickname, [FromForm] byte? Star_price, [FromForm] byte? Star_quality, [FromForm] byte? Star_value, [FromForm] Product_commentSTATE? State, [FromForm] string Title, [FromForm] string Upload_image_url) {
			Product_commentInfo item = new Product_commentInfo();
			item.Member_id = Member_id;
			item.Order_id = Order_id;
			item.Product_id = Product_id;
			item.Productitem_id = Productitem_id;
			item.Content = Content;
			item.Create_time = DateTime.Now;
			item.Nickname = Nickname;
			item.Star_price = Star_price;
			item.Star_quality = Star_quality;
			item.Star_value = Star_value;
			item.State = State;
			item.Title = Title;
			item.Upload_image_url = Upload_image_url;
			item = Product_comment.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] uint? Member_id, [FromForm] uint? Order_id, [FromForm] uint? Product_id, [FromForm] uint? Productitem_id, [FromForm] string Content, [FromForm] string Nickname, [FromForm] byte? Star_price, [FromForm] byte? Star_quality, [FromForm] byte? Star_value, [FromForm] Product_commentSTATE? State, [FromForm] string Title, [FromForm] string Upload_image_url) {
			Product_commentInfo item = Product_comment.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Member_id = Member_id;
			item.Order_id = Order_id;
			item.Product_id = Product_id;
			item.Productitem_id = Productitem_id;
			item.Content = Content;
			item.Create_time = DateTime.Now;
			item.Nickname = Nickname;
			item.Star_price = Star_price;
			item.Star_quality = Star_quality;
			item.Star_value = Star_value;
			item.State = State;
			item.Title = Title;
			item.Upload_image_url = Upload_image_url;
			int affrows = Product_comment.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Product_comment.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
