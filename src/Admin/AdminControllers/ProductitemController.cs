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
	public class ProductitemController : BaseAdminController {
		public ProductitemController(ILogger<ProductitemController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Product_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Productitem.Select
				.Where(!string.IsNullOrEmpty(key), "a.img_url like {0} or a.name like {0}", string.Concat("%", key, "%"));
			if (Product_id.Length > 0) select.WhereProduct_id(Product_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Product>("b", "b.id = a.product_id").Skip((page - 1) * limit).Limit(limit).ToList();
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
			ProductitemInfo item = Productitem.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Product_id, [FromForm] string Img_url, [FromForm] IFormFile Img_url_file, [FromForm] string Name, [FromForm] decimal? Original_price, [FromForm] decimal? Price, [FromForm] uint? Stock) {
			ProductitemInfo item = new ProductitemInfo();
			if (Img_url_file != null) {
				item.Img_url = $"/upload/{Guid.NewGuid().ToString()}.png";
				using (FileStream fs = new FileStream(System.IO.Path.Combine(AppContext.BaseDirectory, item.Img_url), FileMode.Create)) Img_url_file.CopyTo(fs);
			} else
				item.Img_url = Img_url;
			item.Product_id = Product_id;
			item.Name = Name;
			item.Original_price = Original_price;
			item.Price = Price;
			item.Stock = Stock;
			item = Productitem.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] uint? Product_id, [FromForm] string Img_url, [FromForm] IFormFile Img_url_file, [FromForm] string Name, [FromForm] decimal? Original_price, [FromForm] decimal? Price, [FromForm] uint? Stock) {
			ProductitemInfo item = Productitem.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			if (!string.IsNullOrEmpty(item.Img_url) && (item.Img_url != Img_url || Img_url_file != null)) {
				string path = System.IO.Path.Combine(AppContext.BaseDirectory, item.Img_url);
				if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
			}
			if (Img_url_file != null) {
				item.Img_url = $"/upload/{Guid.NewGuid().ToString()}.png";
				using (FileStream fs = new FileStream(System.IO.Path.Combine(AppContext.BaseDirectory, item.Img_url), FileMode.Create)) Img_url_file.CopyTo(fs);
			} else
				item.Img_url = Img_url;
			item.Product_id = Product_id;
			item.Name = Name;
			item.Original_price = Original_price;
			item.Price = Price;
			item.Stock = Stock;
			int affrows = Productitem.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Productitem.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
