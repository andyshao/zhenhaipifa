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
	public class FranchisingController : BaseAdminController {
		public FranchisingController(ILogger<FranchisingController> logger) : base(logger) { }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint[] Factory_id, [FromQuery] uint[] Rentsublet_id, [FromQuery] uint[] Shop_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Franchising.Select
				.Where(!string.IsNullOrEmpty(key), "a.title like {0}", string.Concat("%", key, "%"));
			if (Factory_id.Length > 0) select.WhereFactory_id(Factory_id);
			if (Rentsublet_id.Length > 0) select.WhereRentsublet_id(Rentsublet_id);
			if (Shop_id.Length > 0) select.WhereShop_id(Shop_id);
			int count;
			var items = select.Count(out count).Skip(skip).Limit(limit).ToList();
			return APIReturn.成功.SetData("items", items.ToBson(), "count", count);
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			FranchisingInfo item = Franchising.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			return APIReturn.成功.SetData("item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] string Title, [FromForm] uint[] mn_Factory, [FromForm] uint[] mn_Rentsublet, [FromForm] uint[] mn_Shop) {
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

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] string Title, [FromForm] uint[] mn_Factory, [FromForm] uint[] mn_Rentsublet, [FromForm] uint[] mn_Shop) {
			FranchisingInfo item = new FranchisingInfo();
			item.Id = Id;
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
			if (affrows > 0) return APIReturn.成功;
			return APIReturn.失败;
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Franchising.Delete(Id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
