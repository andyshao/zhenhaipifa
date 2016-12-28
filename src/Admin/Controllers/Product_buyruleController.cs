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
	public class Product_buyruleController : BaseAdminController {
		public Product_buyruleController(ILogger<Product_buyruleController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Product_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Product_buyrule.Select;
			if (Product_id.Length > 0) select.WhereProduct_id(Product_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Product>("b", "b.id = a.product_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_product", items.Select<Product_buyruleInfo, ProductInfo>(a => a.Obj_product).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			Product_buyruleInfo item = Product_buyrule.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Product_id, [FromForm] uint? Discount, [FromForm] uint? Ordering_end, [FromForm] uint? Ordering_start) {
			Product_buyruleInfo item = new Product_buyruleInfo();
			item.Product_id = Product_id;
			item.Discount = Discount;
			item.Ordering_end = Ordering_end;
			item.Ordering_start = Ordering_start;
			item = Product_buyrule.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] uint? Product_id, [FromForm] uint? Discount, [FromForm] uint? Ordering_end, [FromForm] uint? Ordering_start) {
			Product_buyruleInfo item = new Product_buyruleInfo();
			item.Id = Id;
			item.Product_id = Product_id;
			item.Discount = Discount;
			item.Ordering_end = Ordering_end;
			item.Ordering_start = Ordering_start;
			int affrows = Product_buyrule.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Product_buyrule.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
