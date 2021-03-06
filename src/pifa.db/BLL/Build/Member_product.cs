﻿using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Member_product {

		protected static readonly pifa.DAL.Member_product dal = new pifa.DAL.Member_product();
		protected static readonly int itemCacheTimeout;

		static Member_product() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Member_product"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Member_id, uint Product_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Member_id, Product_id));
			return dal.Delete(Member_id, Product_id);
		}
		public static int DeleteByMember_id(uint? Member_id) {
			return dal.DeleteByMember_id(Member_id);
		}
		public static int DeleteByProduct_id(uint? Product_id) {
			return dal.DeleteByProduct_id(Product_id);
		}

		public static int Update(Member_productInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Member_product.SqlUpdateBuild UpdateDiy(uint Member_id, uint Product_id) {
			return UpdateDiy(null, Member_id, Product_id);
		}
		public static pifa.DAL.Member_product.SqlUpdateBuild UpdateDiy(Member_productInfo item, uint Member_id, uint Product_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Member_id, Product_id));
			return new pifa.DAL.Member_product.SqlUpdateBuild(item, Member_id, Product_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Member_product.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Member_product.SqlUpdateBuild(); }
		}

		public static Member_productInfo Insert(uint? Member_id, uint? Product_id, DateTime? Create_time) {
			return Insert(new Member_productInfo {
				Member_id = Member_id, 
				Product_id = Product_id, 
				Create_time = Create_time});
		}
		public static Member_productInfo Insert(Member_productInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Member_productInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Member_product_", item.Member_id, "_,_", item.Product_id));
		}
		#endregion

		public static Member_productInfo GetItem(uint Member_id, uint Product_id) {
			if (itemCacheTimeout <= 0) return Select.WhereMember_id(Member_id).WhereProduct_id(Product_id).ToOne();
			string key = string.Concat("pifa_BLL_Member_product_", Member_id, "_,_", Product_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return Member_productInfo.Parse(value); } catch { }
			Member_productInfo item = Select.WhereMember_id(Member_id).WhereProduct_id(Product_id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Member_productInfo> GetItems() {
			return Select.ToList();
		}
		public static Member_productSelectBuild Select {
			get { return new Member_productSelectBuild(dal); }
		}
		public static List<Member_productInfo> GetItemsByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id).ToList();
		}
		public static List<Member_productInfo> GetItemsByMember_id(uint?[] Member_id, int limit) {
			return Select.WhereMember_id(Member_id).Limit(limit).ToList();
		}
		public static Member_productSelectBuild SelectByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id);
		}
		public static List<Member_productInfo> GetItemsByProduct_id(params uint?[] Product_id) {
			return Select.WhereProduct_id(Product_id).ToList();
		}
		public static List<Member_productInfo> GetItemsByProduct_id(uint?[] Product_id, int limit) {
			return Select.WhereProduct_id(Product_id).Limit(limit).ToList();
		}
		public static Member_productSelectBuild SelectByProduct_id(params uint?[] Product_id) {
			return Select.WhereProduct_id(Product_id);
		}
	}
	public partial class Member_productSelectBuild : SelectBuild<Member_productInfo, Member_productSelectBuild> {
		public Member_productSelectBuild WhereMember_id(params uint?[] Member_id) {
			return this.Where1Or("a.`member_id` = {0}", Member_id);
		}
		public Member_productSelectBuild WhereProduct_id(params uint?[] Product_id) {
			return this.Where1Or("a.`product_id` = {0}", Product_id);
		}
		public Member_productSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as Member_productSelectBuild;
		}
		public Member_productSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as Member_productSelectBuild;
		}
		protected new Member_productSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Member_productSelectBuild;
		}
		public Member_productSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}