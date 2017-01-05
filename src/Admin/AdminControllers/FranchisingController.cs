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
	public class FranchisingController : BaseAdminController {
		public FranchisingController(ILogger<FranchisingController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint[] Factory_id, [FromQuery] uint[] Rentsublet_id, [FromQuery] uint[] Shop_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Franchising.Select
				.Where(!string.IsNullOrEmpty(key), "a.title like {0}", string.Concat("%", key, "%"));
			if (Factory_id.Length > 0) select.WhereFactory_id(Factory_id);
			if (Rentsublet_id.Length > 0) select.WhereRentsublet_id(Rentsublet_id);
			if (Shop_id.Length > 0) select.WhereShop_id(Shop_id);
			int count;
			var items = select.Count(out count).Skip((page - 1) * limit).Limit(limit).ToList();
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
			FranchisingInfo item = Franchising.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] string Title, [FromForm] uint[] mn_Factory, [FromForm] uint[] mn_Rentsublet, [FromForm] uint[] mn_Shop) {
			FranchisingInfo item = new FranchisingInfo();
			item.Title = Title;
			item = Franchising.Insert(item);
			//关联 Factory
			foreach (uint mn_Factory_in in mn_Factory)
				item.FlagFactory(mn_Factory_in);
			//关联 Rentsublet
			foreach (uint mn_Rentsublet_in in mn_Rentsublet)
				item.FlagRentsublet(mn_Rentsublet_in);
			//关联 Shop
			foreach (uint mn_Shop_in in mn_Shop)
				item.FlagShop(mn_Shop_in);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] string Title, [FromForm] uint[] mn_Factory, [FromForm] uint[] mn_Rentsublet, [FromForm] uint[] mn_Shop) {
			FranchisingInfo item = Franchising.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Title = Title;
			int affrows = Franchising.Update(item);
			//关联 Factory
			if (mn_Factory.Length == 0) {
				item.UnflagFactoryALL();
			} else {
				List<uint> mn_Factory_list = mn_Factory.ToList();
				foreach (var Obj_factory in item.Obj_factorys) {
					int idx = mn_Factory_list.FindIndex(a => a == Obj_factory.Id);
					if (idx == -1) item.UnflagFactory(Obj_factory.Id);
					else mn_Factory_list.RemoveAt(idx);
				}
				mn_Factory_list.ForEach(a => item.FlagFactory(a));
			}
			//关联 Rentsublet
			if (mn_Rentsublet.Length == 0) {
				item.UnflagRentsubletALL();
			} else {
				List<uint> mn_Rentsublet_list = mn_Rentsublet.ToList();
				foreach (var Obj_rentsublet in item.Obj_rentsublets) {
					int idx = mn_Rentsublet_list.FindIndex(a => a == Obj_rentsublet.Id);
					if (idx == -1) item.UnflagRentsublet(Obj_rentsublet.Id);
					else mn_Rentsublet_list.RemoveAt(idx);
				}
				mn_Rentsublet_list.ForEach(a => item.FlagRentsublet(a));
			}
			//关联 Shop
			if (mn_Shop.Length == 0) {
				item.UnflagShopALL();
			} else {
				List<uint> mn_Shop_list = mn_Shop.ToList();
				foreach (var Obj_shop in item.Obj_shops) {
					int idx = mn_Shop_list.FindIndex(a => a == Obj_shop.Id);
					if (idx == -1) item.UnflagShop(Obj_shop.Id);
					else mn_Shop_list.RemoveAt(idx);
				}
				mn_Shop_list.ForEach(a => item.FlagShop(a));
			}
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Franchising.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
