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
	public class Product_commentController : BaseAdminController {
		public Product_commentController(ILogger<Product_commentController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Member_id, [FromQuery] uint?[] Order_id, [FromQuery] uint?[] Product_id, [FromQuery] uint?[] Productitem_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Product_comment.Select
				.Where(!string.IsNullOrEmpty(key), "a.content like {0} or a.nickname like {0} or a.title like {0} or a.upload_image_url like {0}", string.Concat("%", key, "%"));
			if (Member_id.Length > 0) select.WhereMember_id(Member_id);
			if (Order_id.Length > 0) select.WhereOrder_id(Order_id);
			if (Product_id.Length > 0) select.WhereProduct_id(Product_id);
			if (Productitem_id.Length > 0) select.WhereProductitem_id(Productitem_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Member>("b", "b.id = a.member_id")
				.InnerJoin<Order>("c", "c.id = a.order_id")
				.InnerJoin<Product>("d", "d.id = a.product_id")
				.InnerJoin<Productitem>("e", "e.id = a.productitem_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_member", items.Select<Product_commentInfo, MemberInfo>(a => a.Obj_member).ToBson(), 
				"items_order", items.Select<Product_commentInfo, OrderInfo>(a => a.Obj_order).ToBson(), 
				"items_product", items.Select<Product_commentInfo, ProductInfo>(a => a.Obj_product).ToBson(), 
				"items_productitem", items.Select<Product_commentInfo, ProductitemInfo>(a => a.Obj_productitem).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			Product_commentInfo item = Product_comment.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Member_id, [FromForm] uint? Order_id, [FromForm] uint? Product_id, [FromForm] uint? Productitem_id, [FromForm] string Content, [FromForm] string Nickname, [FromForm] byte? Star_price, [FromForm] byte? Star_quality, [FromForm] byte? Star_value, [FromForm] Product_commentSTATE? State, [FromForm] string Title, [FromForm] string Upload_image_url) {
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

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] uint? Member_id, [FromForm] uint? Order_id, [FromForm] uint? Product_id, [FromForm] uint? Productitem_id, [FromForm] string Content, [FromForm] string Nickname, [FromForm] byte? Star_price, [FromForm] byte? Star_quality, [FromForm] byte? Star_value, [FromForm] Product_commentSTATE? State, [FromForm] string Title, [FromForm] string Upload_image_url) {
			Product_commentInfo item = new Product_commentInfo();
			item.Id = Id;
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
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Product_comment.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
