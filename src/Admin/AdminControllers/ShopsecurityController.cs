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
	public class ShopsecurityController : BaseAdminController {
		public ShopsecurityController(ILogger<ShopsecurityController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Shop_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Shopsecurity.Select
				.Where(!string.IsNullOrEmpty(key), "a.idcard like {0} or a.idcard_img1 like {0} or a.idcard_img2 like {0} or a.license_img like {0}", string.Concat("%", key, "%"));
			if (Shop_id.Length > 0) select.WhereShop_id(Shop_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Shop>("b", "b.id = a.shop_id").Skip((page - 1) * limit).Limit(limit).ToList();
			ViewBag.items = items;
			ViewBag.count = count;
			return View();
		}

		[HttpGet(@"add")]
		public ActionResult Edit() {
			return View();
		}
		[HttpGet(@"edit")]
		public ActionResult Edit([FromQuery] uint Shop_id) {
			ShopsecurityInfo item = Shopsecurity.GetItem(Shop_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Shop_id, [FromForm] string Idcard, [FromForm] string Idcard_img1, [FromForm] string Idcard_img2, [FromForm] string License_img, [FromForm] IFormFile License_img_file) {
			ShopsecurityInfo item = new ShopsecurityInfo();
			item.Shop_id = Shop_id;
			if (License_img_file != null) {
				item.License_img = $"/upload/{Guid.NewGuid().ToString()}.png";
				using (FileStream fs = new FileStream(System.IO.Path.Combine(AppContext.BaseDirectory, item.License_img), FileMode.Create)) License_img_file.CopyTo(fs);
			} else
				item.License_img = License_img;
			item.Idcard = Idcard;
			item.Idcard_img1 = Idcard_img1;
			item.Idcard_img2 = Idcard_img2;
			item = Shopsecurity.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Shop_id, [FromForm] string Idcard, [FromForm] string Idcard_img1, [FromForm] string Idcard_img2, [FromForm] string License_img, [FromForm] IFormFile License_img_file) {
			ShopsecurityInfo item = Shopsecurity.GetItem(Shop_id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			if (!string.IsNullOrEmpty(item.License_img) && (item.License_img != License_img || License_img_file != null)) {
				string path = System.IO.Path.Combine(AppContext.BaseDirectory, item.License_img);
				if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
			}
			if (License_img_file != null) {
				item.License_img = $"/upload/{Guid.NewGuid().ToString()}.png";
				using (FileStream fs = new FileStream(System.IO.Path.Combine(AppContext.BaseDirectory, item.License_img), FileMode.Create)) License_img_file.CopyTo(fs);
			} else
				item.License_img = License_img;
			item.Idcard = Idcard;
			item.Idcard_img1 = Idcard_img1;
			item.Idcard_img2 = Idcard_img2;
			int affrows = Shopsecurity.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Shopsecurity.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
