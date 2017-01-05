using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Generator;
using pifa.BLL;
using pifa.Model;

public partial class BaseController : Controller {
	public ILogger _logger;
	public ISession Session { get { return HttpContext.Session; } }
	public HttpRequest Req { get { return Request; } }
	public HttpResponse Res { get { return Response; } }

	public pifa.Model.MemberInfo LoginUser { get; private set; }
	public BaseController(ILogger logger) { _logger = logger; }
	public override void OnActionExecuting(ActionExecutingContext context) {
		#region 参数验证
		if (context.ModelState.IsValid == false)
			foreach (var value in context.ModelState.Values)
				if (value.Errors.Any()) {
					context.Result = APIReturn.参数格式不正确.SetMessage($"参数格式不正确：{value.Errors.First().ErrorMessage}");
					return;
				}
		#endregion
		#region 初始化当前登陆账号
		var memberid = Session.GetUInt32("_memberid");
		if (memberid != null) LoginUser = Member.GetItem(memberid.Value);

		var method = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo;
		if (method.GetCustomAttribute<需要登陆Attribute>() != null && LoginUser == null)
			context.Result = new RedirectResult("/signin");
		else if (method.GetCustomAttribute<匿名访问Attribute>() == null && LoginUser == null)
			context.Result = new RedirectResult("/signin");
		ViewBag.user = LoginUser;
		#endregion
		base.OnActionExecuting(context);

		//BuildManager.
	}
	public override void OnActionExecuted(ActionExecutedContext context) {
		if (context.Exception != null) {
			#region 错误拦截，在这里记录日志
			//this.Json(new APIReturn(-1, context.Exception.Message)).ExecuteResultAsync(context).Wait();
			//context.Exception = null;
			#endregion
		}
		base.OnActionExecuted(context);
	}
}

public partial class APIReturn {
	public static APIReturn 用户名已被占用 { get { return new APIReturn(973, "用户名已被占用"); } }
	public static APIReturn 用户不存在 { get { return new APIReturn(971, "用户不存在"); } }
	public static APIReturn 用户名或密码错误 { get { return new APIReturn(972, "用户名或密码错误"); } }
	public static APIReturn 参数错误 { get { return new APIReturn(974, "参数错误"); } }
}

public static class _Extenal {
	public static uint? GetUInt32(this ISession session, string key) {
		int? ret = session.GetInt32(key);
		if (ret == null) return null;
		return Convert.ToUInt32(ret);
	}
}