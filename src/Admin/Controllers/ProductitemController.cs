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
	public class ProductitemController : BaseAdminController {
		public ProductitemController(ILogger<ProductitemController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Product_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Productitem.Select
				.Where(!string.IsNullOrEmpty(key), "a.img_url like {0} or a.name like {0}", string.Concat("%", key, "%"));
			if (Product_id.Length > 0) select.WhereProduct_id(Product_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Product>("b", "b.id = a.product_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_product", items.Select<ProductitemInfo, ProductInfo>(a => a.Obj_product).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			ProductitemInfo item = Productitem.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Product_id, [FromForm] string Img_url, [FromForm] string Name, [FromForm] decimal? Original_price, [FromForm] decimal? Price, [FromForm] uint? Stock) {
			ProductitemInfo item = new ProductitemInfo();
			item.Product_id = Product_id;
			item.Img_url = Img_url;
			item.Name = Name;
			item.Original_price = Original_price;
			item.Price = Price;
			item.Stock = Stock;
			item = Productitem.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] uint? Product_id, [FromForm] string Img_url, [FromForm] string Name, [FromForm] decimal? Original_price, [FromForm] decimal? Price, [FromForm] uint? Stock) {
			ProductitemInfo item = new ProductitemInfo();
			item.Id = Id;
			item.Product_id = Product_id;
			item.Img_url = Img_url;
			item.Name = Name;
			item.Original_price = Original_price;
			item.Price = Price;
			item.Stock = Stock;
			int affrows = Productitem.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Productitem.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
