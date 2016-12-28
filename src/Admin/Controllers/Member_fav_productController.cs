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
	public class Member_fav_productController : BaseAdminController {
		public Member_fav_productController(ILogger<Member_fav_productController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Member_id, [FromQuery] uint?[] Product_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Member_fav_product.Select;
			if (Member_id.Length > 0) select.WhereMember_id(Member_id);
			if (Product_id.Length > 0) select.WhereProduct_id(Product_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Member>("b", "b.id = a.member_id")
				.InnerJoin<Product>("c", "c.id = a.product_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_member", items.Select<Member_fav_productInfo, MemberInfo>(a => a.Obj_member).ToBson(), 
				"items_product", items.Select<Member_fav_productInfo, ProductInfo>(a => a.Obj_product).ToBson());
		}

		[HttpGet(@"{Member_id}/{Product_id}/")]
		public APIReturn Get_item(uint? Member_id, uint? Product_id) {
			Member_fav_productInfo item = Member_fav_product.GetItem(Member_id, Product_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Member_id, [FromForm] uint? Product_id) {
			Member_fav_productInfo item = new Member_fav_productInfo();
			item.Member_id = Member_id;
			item.Product_id = Product_id;
			item.Create_time = DateTime.Now;
			item = Member_fav_product.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Member_id}/{Product_id}/")]
		public APIReturn Put_update(uint? Member_id, uint? Product_id) {
			Member_fav_productInfo item = new Member_fav_productInfo();
			item.Member_id = Member_id;
			item.Product_id = Product_id;
			item.Create_time = DateTime.Now;
			int affrows = Member_fav_product.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Member_id}/{Product_id}/")]
		public APIReturn Delete_delete(uint? Member_id, uint? Product_id) {
			int affrows = Member_fav_product.Delete(Member_id, Product_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
