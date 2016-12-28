using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using pifa.BLL;
using pifa.Model;

namespace pifa.Admin.Controllers {
	[Route("api/[controller]")]
	[Obsolete]
	public class SysController : Controller {
		[HttpGet(@"connection")]
		public object Get_connection() {
			List<Hashtable> ret = new List<Hashtable>();
			foreach (var conn in SqlHelper.Instance.Pool.AllConnections) {
				ret.Add(new Hashtable() {
						{ "数据库", conn.SqlConnection.Database },
						{ "状态", conn.SqlConnection.State },
						{ "最后活动", conn.LastActive },
						{ "获取次数", conn.UseSum }
					});
			}
			return new {
				FreeConnections = SqlHelper.Instance.Pool.FreeConnections.Count,
				AllConnections = SqlHelper.Instance.Pool.AllConnections.Count,
				List = ret
			};
		}
		[HttpGet(@"connection/redis")]
		public object Get_connection_redis() {
			List<Hashtable> ret = new List<Hashtable>();
			foreach (var conn in RedisHelper.Instance.AllConnections) {
				ret.Add(new Hashtable() {
						{ "数据库", conn.Client.ClientGetName() },
						{ "最后活动", conn.LastActive },
						{ "获取次数", conn.UseSum }
					});
			}
			return new {
				FreeConnections = RedisHelper.Instance.FreeConnections.Count,
				AllConnections = RedisHelper.Instance.AllConnections.Count,
				List = ret
			};
		}

		[HttpGet(@"init_sysdir")]
		public APIReturn Get_init_sysdir() {
			/*
			if (Sysdir.SelectByParent_id(null).Count() > 0)
				return new APIReturn(-33, "本系统已经初始化过，页面没经过任何操作退出。");

			SysdirInfo dir1, dir2, dir3;
			dir1 = Sysdir.Insert(null, DateTime.Now, "运营管理", 1, null);

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "area", 1, "/area/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/area/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/area/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/area/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/area/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "area_category", 2, "/area_category/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/area_category/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/area_category/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/area_category/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/area_category/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "category", 3, "/category/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/category/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/category/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/category/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/category/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "express", 4, "/express/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/express/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/express/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/express/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/express/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "expressdesc", 5, "/expressdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/expressdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/expressdesc/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/expressdesc/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/expressdesc/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "factory", 6, "/factory/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/factory/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/factory/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/factory/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/factory/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "factory_franchising", 7, "/factory_franchising/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/factory_franchising/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/factory_franchising/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/factory_franchising/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/factory_franchising/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "factorydesc", 8, "/factorydesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/factorydesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/factorydesc/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/factorydesc/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/factorydesc/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "faq", 9, "/faq/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/faq/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/faq/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/faq/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/faq/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "faqdesc", 10, "/faqdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/faqdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/faqdesc/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/faqdesc/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/faqdesc/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "faqtype", 11, "/faqtype/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/faqtype/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/faqtype/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/faqtype/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/faqtype/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "franchising", 12, "/franchising/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/franchising/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/franchising/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/franchising/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/franchising/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "market", 13, "/market/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/market/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/market/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/market/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/market/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "marketdesc", 14, "/marketdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/marketdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/marketdesc/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/marketdesc/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/marketdesc/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "markettype", 15, "/markettype/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/markettype/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/markettype/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/markettype/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/markettype/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "markettype_category", 16, "/markettype_category/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/markettype_category/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/markettype_category/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/markettype_category/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/markettype_category/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "member", 17, "/member/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/member/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/member/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/member/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/member/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "member_addressbook", 18, "/member_addressbook/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/member_addressbook/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/member_addressbook/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/member_addressbook/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/member_addressbook/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "member_fav_market", 19, "/member_fav_market/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/member_fav_market/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/member_fav_market/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/member_fav_market/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/member_fav_market/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "member_fav_product", 20, "/member_fav_product/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/member_fav_product/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/member_fav_product/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/member_fav_product/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/member_fav_product/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "member_fav_shop", 21, "/member_fav_shop/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/member_fav_shop/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/member_fav_shop/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/member_fav_shop/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/member_fav_shop/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "member_security", 22, "/member_security/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/member_security/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/member_security/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/member_security/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/member_security/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "news", 23, "/news/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/news/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/news/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/news/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/news/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "news_newstag", 24, "/news_newstag/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/news_newstag/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/news_newstag/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/news_newstag/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/news_newstag/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "newsdesc", 25, "/newsdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/newsdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/newsdesc/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/newsdesc/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/newsdesc/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "newstag", 26, "/newstag/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/newstag/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/newstag/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/newstag/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/newstag/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "order", 27, "/order/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/order/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/order/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/order/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/order/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "order_address", 28, "/order_address/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/order_address/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/order_address/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/order_address/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/order_address/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "order_productitem", 29, "/order_productitem/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/order_productitem/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/order_productitem/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/order_productitem/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/order_productitem/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "order_refund", 30, "/order_refund/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/order_refund/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/order_refund/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/order_refund/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/order_refund/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "pattr", 31, "/pattr/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/pattr/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/pattr/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/pattr/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/pattr/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "product", 32, "/product/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/product/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/product/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/product/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/product/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "product_attr", 33, "/product_attr/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/product_attr/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/product_attr/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/product_attr/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/product_attr/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "product_buyrule", 34, "/product_buyrule/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/product_buyrule/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/product_buyrule/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/product_buyrule/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/product_buyrule/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "product_comment", 35, "/product_comment/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/product_comment/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/product_comment/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/product_comment/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/product_comment/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "product_question", 36, "/product_question/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/product_question/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/product_question/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/product_question/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/product_question/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "productdesc", 37, "/productdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/productdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/productdesc/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/productdesc/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/productdesc/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "productitem", 38, "/productitem/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/productitem/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/productitem/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/productitem/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/productitem/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "rentsublet", 39, "/rentsublet/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/rentsublet/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/rentsublet/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/rentsublet/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/rentsublet/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "rentsublet_franchising", 40, "/rentsublet_franchising/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/rentsublet_franchising/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/rentsublet_franchising/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/rentsublet_franchising/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/rentsublet_franchising/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "shop", 41, "/shop/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/shop/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/shop/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/shop/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/shop/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "shop_franchising", 42, "/shop_franchising/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/shop_franchising/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/shop_franchising/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/shop_franchising/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/shop_franchising/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "shop_friendly_links", 43, "/shop_friendly_links/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/shop_friendly_links/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/shop_friendly_links/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/shop_friendly_links/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/shop_friendly_links/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "shopsecurity", 44, "/shopsecurity/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/shopsecurity/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/shopsecurity/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/shopsecurity/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/shopsecurity/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "shopstat", 45, "/shopstat/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/shopstat/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/shopstat/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/shopstat/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/shopstat/del.aspx");
			*/
			return new APIReturn(0, "管理目录已初始化完成。");
		}
	}
}
