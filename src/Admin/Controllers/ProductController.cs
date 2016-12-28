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
	public class ProductController : BaseAdminController {
		public ProductController(ILogger<ProductController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Category_id, [FromQuery] uint?[] Shop_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Product.Select
				.Where(!string.IsNullOrEmpty(key), "a.title like {0} or a.unit like {0}", string.Concat("%", key, "%"));
			if (Category_id.Length > 0) select.WhereCategory_id(Category_id);
			if (Shop_id.Length > 0) select.WhereShop_id(Shop_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Category>("b", "b.id = a.category_id")
				.InnerJoin<Shop>("c", "c.id = a.shop_id").Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count, 
				"items_category", items.Select<ProductInfo, CategoryInfo>(a => a.Obj_category).ToBson(), 
				"items_shop", items.Select<ProductInfo, ShopInfo>(a => a.Obj_shop).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			ProductInfo item = Product.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Category_id, [FromForm] uint? Shop_id, [FromForm] ProductICON[] Icon, [FromForm] decimal? Price, [FromForm] uint? Stock, [FromForm] string Title, [FromForm] string Unit) {
			ProductInfo item = new ProductInfo();
			item.Category_id = Category_id;
			item.Shop_id = Shop_id;
			item.Create_time = DateTime.Now;
			item.Icon = null;
			Icon?.ToList().ForEach(a => item.Icon = (item.Icon ?? 0) | a);
			item.Price = Price;
			item.Stock = Stock;
			item.Title = Title;
			item.Unit = Unit;
			item = Product.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] uint? Category_id, [FromForm] uint? Shop_id, [FromForm] ProductICON[] Icon, [FromForm] decimal? Price, [FromForm] uint? Stock, [FromForm] string Title, [FromForm] string Unit) {
			ProductInfo item = new ProductInfo();
			item.Id = Id;
			item.Category_id = Category_id;
			item.Shop_id = Shop_id;
			item.Create_time = DateTime.Now;
			item.Icon = null;
			Icon?.ToList().ForEach(a => item.Icon = (item.Icon ?? 0) | a);
			item.Price = Price;
			item.Stock = Stock;
			item.Title = Title;
			item.Unit = Unit;
			int affrows = Product.Update(item);
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Product.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
