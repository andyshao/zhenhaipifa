using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using pifa.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Routes {

	public class CommentModel {
		[FromForm]
		[MaxLength(65535, ErrorMessage = "内容过长")]
		[Required(ErrorMessage = "请填写内容")]
		public string Content { get; set; }
	}

	public class RegisterModel {
		[FromForm]
		[MaxLength(32, ErrorMessage = "登陆名过长")]
		[Required(ErrorMessage = "请填写登陆名")]
		public string Username { get; set; }
		[FromForm]
		[MaxLength(32, ErrorMessage = "密码过长")]
		[MinLength(6, ErrorMessage = "密码过短")]
		[Required(ErrorMessage = "请填写密码")]
		public string Password { get; set; }
	}
}
