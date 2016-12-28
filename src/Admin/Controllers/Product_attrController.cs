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
	public class Product_attrController : BaseAdminController {
		public Product_attrController(ILogger<Product_attrController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Pattr_id, [FromQuery] uint?[] Product_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Product_attr.Select
				.Where(!string.IsNullOrEmpty(key), "a.value like {0}", string.Concat("%", key, "%"));
			if (Pattr_id.Length > 0) select.WherePattr_id(Pattr_id);
			if (Product_id.Length > 0) select.WhereProduct_id(Product_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Pattr>("b", "b.id = a.pattr_id")
				.InnerJoin<Product>("c", "c.id = a.product_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_pattr", items.Select<Product_attrInfo, PattrInfo>(a => a.Obj_pattr).ToBson(), 
				"items_product", items.Select<Product_attrInfo, ProductInfo>(a => a.Obj_product).ToBson());
		}

		[HttpGet(@"{Pattr_id}/{Product_id}/")]
		public APIReturn Get_item(uint? Pattr_id, uint? Product_id) {
			Product_attrInfo item = Product_attr.GetItem(Pattr_id, Product_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Pattr_id, [FromForm] uint? Product_id, [FromForm] string Value) {
			Product_attrInfo item = new Product_attrInfo();
			item.Pattr_id = Pattr_id;
			item.Product_id = Product_id;
			item.Value = Value;
			item = Product_attr.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Pattr_id}/{Product_id}/")]
		public APIReturn Put_update(uint? Pattr_id, uint? Product_id, [FromForm] string Value) {
			Product_attrInfo item = new Product_attrInfo();
			item.Pattr_id = Pattr_id;
			item.Product_id = Product_id;
			item.Value = Value;
			int affrows = Product_attr.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Pattr_id}/{Product_id}/")]
		public APIReturn Delete_delete(uint? Pattr_id, uint? Product_id) {
			int affrows = Product_attr.Delete(Pattr_id, Product_id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
