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
	public class ProductdescController : BaseAdminController {
		public ProductdescController(ILogger<ProductdescController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Product_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Productdesc.Select
				.Where(!string.IsNullOrEmpty(key), "a.content like {0}", string.Concat("%", key, "%"));
			if (Product_id.Length > 0) select.WhereProduct_id(Product_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Product>("b", "b.id = a.product_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_product", items.Select<ProductdescInfo, ProductInfo>(a => a.Obj_product).ToBson());
		}

		[HttpGet(@"{Product_id}/")]
		public APIReturn Get_item(uint? Product_id) {
			ProductdescInfo item = Productdesc.GetItem(Product_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Product_id, [FromForm] string Content) {
			ProductdescInfo item = new ProductdescInfo();
			item.Product_id = Product_id;
			item.Content = Content;
			item = Productdesc.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Product_id}/")]
		public APIReturn Put_update(uint? Product_id, [FromForm] string Content) {
			ProductdescInfo item = new ProductdescInfo();
			item.Product_id = Product_id;
			item.Content = Content;
			int affrows = Productdesc.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Product_id}/")]
		public APIReturn Delete_delete(uint? Product_id) {
			int affrows = Productdesc.Delete(Product_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
