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
	public class ShopController : BaseAdminController {
		public ShopController(ILogger<ShopController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Markettype_id, [FromQuery] uint?[] Member_id, [FromQuery] uint[] Franchising_id, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Shop.Select
				.Where(!string.IsNullOrEmpty(key), "a.address like {0} or a.code like {0} or a.fax like {0} or a.kefu like {0} or a.main_business like {0} or a.nickname like {0} or a.title like {0}", string.Concat("%", key, "%"));
			if (Markettype_id.Length > 0) select.WhereMarkettype_id(Markettype_id);
			if (Member_id.Length > 0) select.WhereMember_id(Member_id);
			if (Franchising_id.Length > 0) select.WhereFranchising_id(Franchising_id);
			int count;
			var items = select.Count(out count)
				.LeftJoin<Markettype>("b", "b.id = a.markettype_id")
				.LeftJoin<Member>("c", "c.id = a.member_id").Skip((page - 1) * limit).Limit(limit).ToList();
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
			ShopInfo item = Shop.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Markettype_id, [FromForm] uint? Member_id, [FromForm] string Address, [FromForm] decimal? Area, [FromForm] string Code, [FromForm] string Fax, [FromForm] ShopFUNC_SWITCH[] Func_switch, [FromForm] ShopICON[] Icon, [FromForm] string Kefu, [FromForm] string Main_business, [FromForm] string Nickname, [FromForm] ShopSTATE? State, [FromForm] string Title, [FromForm] uint[] mn_Franchising) {
			ShopInfo item = new ShopInfo();
			item.Markettype_id = Markettype_id;
			item.Member_id = Member_id;
			item.Address = Address;
			item.Area = Area;
			item.Code = Code;
			item.Create_time = DateTime.Now;
			item.Fax = Fax;
			item.Func_switch = null;
			Func_switch?.ToList().ForEach(a => item.Func_switch = (item.Func_switch ?? 0) | a);
			item.Icon = null;
			Icon?.ToList().ForEach(a => item.Icon = (item.Icon ?? 0) | a);
			item.Kefu = Kefu;
			item.Main_business = Main_business;
			item.Nickname = Nickname;
			item.State = State;
			item.Title = Title;
			item = Shop.Insert(item);
			//关联 Franchising
			foreach (uint mn_Franchising_in in mn_Franchising)
				item.FlagFranchising(mn_Franchising_in);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] uint? Markettype_id, [FromForm] uint? Member_id, [FromForm] string Address, [FromForm] decimal? Area, [FromForm] string Code, [FromForm] string Fax, [FromForm] ShopFUNC_SWITCH[] Func_switch, [FromForm] ShopICON[] Icon, [FromForm] string Kefu, [FromForm] string Main_business, [FromForm] string Nickname, [FromForm] ShopSTATE? State, [FromForm] string Title, [FromForm] uint[] mn_Franchising) {
			ShopInfo item = Shop.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Markettype_id = Markettype_id;
			item.Member_id = Member_id;
			item.Address = Address;
			item.Area = Area;
			item.Code = Code;
			item.Create_time = DateTime.Now;
			item.Fax = Fax;
			item.Func_switch = null;
			Func_switch?.ToList().ForEach(a => item.Func_switch = (item.Func_switch ?? 0) | a);
			item.Icon = null;
			Icon?.ToList().ForEach(a => item.Icon = (item.Icon ?? 0) | a);
			item.Kefu = Kefu;
			item.Main_business = Main_business;
			item.Nickname = Nickname;
			item.State = State;
			item.Title = Title;
			int affrows = Shop.Update(item);
			//关联 Franchising
			if (mn_Franchising.Length == 0) {
				item.UnflagFranchisingALL();
			} else {
				List<uint> mn_Franchising_list = mn_Franchising.ToList();
				foreach (var Obj_franchising in item.Obj_franchisings) {
					int idx = mn_Franchising_list.FindIndex(a => a == Obj_franchising.Id);
					if (idx == -1) item.UnflagFranchising(Obj_franchising.Id);
					else mn_Franchising_list.RemoveAt(idx);
				}
				mn_Franchising_list.ForEach(a => item.FlagFranchising(a));
			}
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Shop.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
