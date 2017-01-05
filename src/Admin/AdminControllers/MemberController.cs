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
	public class MemberController : BaseAdminController {
		public MemberController(ILogger<MemberController> logger) : base(logger) { }

		[HttpGet]
		public ActionResult List([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] int limit = 20, [FromQuery] int page = 1) {
			var select = Member.Select
				.Where(!string.IsNullOrEmpty(key), "a.email like {0} or a.telphone like {0} or a.username like {0}", string.Concat("%", key, "%"));
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
			MemberInfo item = Member.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			ViewBag.item = item;
			return View();
		}

		/***************************************** POST *****************************************/
		[HttpPost(@"add")]
		[ValidateAntiForgeryToken]
		public APIReturn _Add([FromForm] uint? Id, [FromForm] string Email, [FromForm] DateTime? Lastlogin_time, [FromForm] string Telphone, [FromForm] string Username) {
			MemberInfo item = new MemberInfo();
			item.Id = Id;
			item.Create_time = DateTime.Now;
			item.Email = Email;
			item.Lastlogin_time = Lastlogin_time;
			item.Telphone = Telphone;
			item.Username = Username;
			item = Member.Insert(item);
			return APIReturn.成功.SetData("item", item.ToBson());
		}
		[HttpPost(@"edit")]
		[ValidateAntiForgeryToken]
		public APIReturn _Edit([FromQuery] uint Id, [FromForm] string Email, [FromForm] DateTime? Lastlogin_time, [FromForm] string Telphone, [FromForm] string Username) {
			MemberInfo item = Member.GetItem(Id);
			if (item == null) return APIReturn.记录不存在_或者没有权限;
			item.Create_time = DateTime.Now;
			item.Email = Email;
			item.Lastlogin_time = Lastlogin_time;
			item.Telphone = Telphone;
			item.Username = Username;
			int affrows = Member.Update(item);
			if (affrows > 0) return APIReturn.成功.SetMessage($"更新成功，影响行数：{affrows}");
			return APIReturn.失败;
		}

		[HttpPost("del")]
		[ValidateAntiForgeryToken]
		public APIReturn _Del([FromForm] uint[] ids) {
			int affrows = 0;
			foreach (uint id in ids)
				affrows += Member.Delete(id);
			if (affrows > 0) return APIReturn.成功.SetMessage($"删除成功，影响行数：{affrows}");
			return APIReturn.失败;
		}
	}
}
