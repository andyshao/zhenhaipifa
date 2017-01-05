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
using System.Text;
using System.Security.Cryptography;

namespace Admin.Routes {
	[Route("")]
	public class SignController : BaseController {
		public SignController(ILogger<SignController> logger) : base(logger) { }

		/// <summary>
		/// 注册
		/// </summary>
		/// <param name="model"></param>
		/// <param name="vcode"></param>
		/// <returns></returns>
		[HttpPost("/register")]
		public APIReturn register(RegisterModel model, [FromForm] string vcode) {
			if (string.IsNullOrEmpty(vcode) || Session.GetString("_vcode") != vcode) return APIReturn.参数错误.SetMessage("验证码不正确");
			if (Member.GetItemByUsername(model.Username) != null) return APIReturn.用户名已被占用;
			SqlHelper.Transaction(() => {
				var item = Member.Insert(new MemberInfo {
					Create_time = DateTime.Now,
					Email = $"{model.Username}@@empty",
					Lastlogin_time = DateTime.Now,
					Telphone = $"{model.Username}@@empty",
					Username = model.Username
				});
				string sha256 = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(model.Password)));
				item.AddSecurity(sha256);
			});
			return APIReturn.成功;
		}
		/// <summary>
		/// 登陆
		/// </summary>
		/// <param name="model"></param>
		/// <param name="vcode"></param>
		/// <returns></returns>
		[HttpPost("/login")]
		public APIReturn login(RegisterModel model, [FromForm] string vcode) {
			var item = Member.Select.Where("a.username = {0} or a.telphone = {0} or a.email = {0}", model.Username).ToOne();
			if (item == null) return APIReturn.用户不存在;
			var security = Member_security.GetItem(item.Id.Value);
			string sha256 = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(model.Password)));
			if (security.Password != sha256) return APIReturn.用户名或密码错误;
			Session.SetInt32("_memberid", (int)item.Id);
			return APIReturn.成功;
		}
	}
}
