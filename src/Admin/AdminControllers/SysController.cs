using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using pifa.BLL;
using pifa.Model;

namespace pifa.AdminControllers {
	[Route("[controller]")]
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
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/area/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/area/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/area/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "area_category", 2, "/area_category/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/area_category/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/area_category/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/area_category/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/area_category/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "category", 3, "/category/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/category/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/category/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/category/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/category/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "express", 4, "/express/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/express/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/express/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/express/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/express/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "expressdesc", 5, "/expressdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/expressdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/expressdesc/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/expressdesc/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/expressdesc/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "factory", 6, "/factory/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/factory/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/factory/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/factory/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/factory/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "factory_franchising", 7, "/factory_franchising/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/factory_franchising/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/factory_franchising/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/factory_franchising/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/factory_franchising/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "factorydesc", 8, "/factorydesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/factorydesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/factorydesc/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/factorydesc/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/factorydesc/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "faq", 9, "/faq/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/faq/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/faq/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/faq/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/faq/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "faqdesc", 10, "/faqdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/faqdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/faqdesc/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/faqdesc/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/faqdesc/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "faqtype", 11, "/faqtype/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/faqtype/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/faqtype/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/faqtype/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/faqtype/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "franchising", 12, "/franchising/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/franchising/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/franchising/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/franchising/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/franchising/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "market", 13, "/market/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/market/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/market/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/market/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/market/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "marketdesc", 14, "/marketdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/marketdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/marketdesc/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/marketdesc/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/marketdesc/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "markettype", 15, "/markettype/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/markettype/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/markettype/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/markettype/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/markettype/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "markettype_category", 16, "/markettype_category/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/markettype_category/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/markettype_category/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/markettype_category/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/markettype_category/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "member", 17, "/member/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/member/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/member/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/member/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/member/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "member_addressbook", 18, "/member_addressbook/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/member_addressbook/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/member_addressbook/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/member_addressbook/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/member_addressbook/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "member_market", 19, "/member_market/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/member_market/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/member_market/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/member_market/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/member_market/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "member_product", 20, "/member_product/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/member_product/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/member_product/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/member_product/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/member_product/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "member_security", 21, "/member_security/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/member_security/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/member_security/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/member_security/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/member_security/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "member_shop", 22, "/member_shop/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/member_shop/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/member_shop/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/member_shop/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/member_shop/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "news", 23, "/news/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/news/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/news/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/news/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/news/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "news_newstag", 24, "/news_newstag/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/news_newstag/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/news_newstag/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/news_newstag/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/news_newstag/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "newsdesc", 25, "/newsdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/newsdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/newsdesc/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/newsdesc/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/newsdesc/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "newstag", 26, "/newstag/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/newstag/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/newstag/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/newstag/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/newstag/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "order", 27, "/order/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/order/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/order/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/order/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/order/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "order_address", 28, "/order_address/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/order_address/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/order_address/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/order_address/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/order_address/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "order_productitem", 29, "/order_productitem/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/order_productitem/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/order_productitem/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/order_productitem/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/order_productitem/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "order_refund", 30, "/order_refund/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/order_refund/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/order_refund/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/order_refund/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/order_refund/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "pattr", 31, "/pattr/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/pattr/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/pattr/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/pattr/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/pattr/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "product", 32, "/product/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/product/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/product/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/product/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/product/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "product_attr", 33, "/product_attr/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/product_attr/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/product_attr/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/product_attr/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/product_attr/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "product_buyrule", 34, "/product_buyrule/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/product_buyrule/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/product_buyrule/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/product_buyrule/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/product_buyrule/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "product_comment", 35, "/product_comment/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/product_comment/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/product_comment/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/product_comment/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/product_comment/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "product_question", 36, "/product_question/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/product_question/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/product_question/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/product_question/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/product_question/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "productdesc", 37, "/productdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/productdesc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/productdesc/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/productdesc/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/productdesc/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "productitem", 38, "/productitem/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/productitem/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/productitem/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/productitem/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/productitem/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "rentsublet", 39, "/rentsublet/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/rentsublet/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/rentsublet/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/rentsublet/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/rentsublet/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "rentsublet_franchising", 40, "/rentsublet_franchising/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/rentsublet_franchising/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/rentsublet_franchising/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/rentsublet_franchising/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/rentsublet_franchising/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "shop", 41, "/shop/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/shop/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/shop/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/shop/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/shop/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "shop_franchising", 42, "/shop_franchising/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/shop_franchising/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/shop_franchising/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/shop_franchising/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/shop_franchising/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "shop_friendly_links", 43, "/shop_friendly_links/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/shop_friendly_links/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/shop_friendly_links/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/shop_friendly_links/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/shop_friendly_links/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "shopsecurity", 44, "/shopsecurity/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/shopsecurity/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/shopsecurity/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/shopsecurity/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/shopsecurity/del");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "shopstat", 45, "/shopstat/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/shopstat/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/shopstat/add");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/shopstat/edit");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/shopstat/del");
			*/
			return new APIReturn(0, "管理目录已初始化完成。");
		}
	}
}
