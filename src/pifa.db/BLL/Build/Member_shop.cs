﻿using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Member_shop {

		protected static readonly pifa.DAL.Member_shop dal = new pifa.DAL.Member_shop();
		protected static readonly int itemCacheTimeout;

		static Member_shop() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Member_shop"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Member_id, uint Shop_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Member_id, Shop_id));
			return dal.Delete(Member_id, Shop_id);
		}
		public static int DeleteByMember_id(uint? Member_id) {
			return dal.DeleteByMember_id(Member_id);
		}
		public static int DeleteByShop_id(uint? Shop_id) {
			return dal.DeleteByShop_id(Shop_id);
		}

		public static int Update(Member_shopInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Member_shop.SqlUpdateBuild UpdateDiy(uint Member_id, uint Shop_id) {
			return UpdateDiy(null, Member_id, Shop_id);
		}
		public static pifa.DAL.Member_shop.SqlUpdateBuild UpdateDiy(Member_shopInfo item, uint Member_id, uint Shop_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Member_id, Shop_id));
			return new pifa.DAL.Member_shop.SqlUpdateBuild(item, Member_id, Shop_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Member_shop.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Member_shop.SqlUpdateBuild(); }
		}

		public static Member_shopInfo Insert(uint? Member_id, uint? Shop_id, DateTime? Create_time) {
			return Insert(new Member_shopInfo {
				Member_id = Member_id, 
				Shop_id = Shop_id, 
				Create_time = Create_time});
		}
		public static Member_shopInfo Insert(Member_shopInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Member_shopInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Member_shop_", item.Member_id, "_,_", item.Shop_id));
		}
		#endregion

		public static Member_shopInfo GetItem(uint Member_id, uint Shop_id) {
			if (itemCacheTimeout <= 0) return Select.WhereMember_id(Member_id).WhereShop_id(Shop_id).ToOne();
			string key = string.Concat("pifa_BLL_Member_shop_", Member_id, "_,_", Shop_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return Member_shopInfo.Parse(value); } catch { }
			Member_shopInfo item = Select.WhereMember_id(Member_id).WhereShop_id(Shop_id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Member_shopInfo> GetItems() {
			return Select.ToList();
		}
		public static Member_shopSelectBuild Select {
			get { return new Member_shopSelectBuild(dal); }
		}
		public static List<Member_shopInfo> GetItemsByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id).ToList();
		}
		public static List<Member_shopInfo> GetItemsByMember_id(uint?[] Member_id, int limit) {
			return Select.WhereMember_id(Member_id).Limit(limit).ToList();
		}
		public static Member_shopSelectBuild SelectByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id);
		}
		public static List<Member_shopInfo> GetItemsByShop_id(params uint?[] Shop_id) {
			return Select.WhereShop_id(Shop_id).ToList();
		}
		public static List<Member_shopInfo> GetItemsByShop_id(uint?[] Shop_id, int limit) {
			return Select.WhereShop_id(Shop_id).Limit(limit).ToList();
		}
		public static Member_shopSelectBuild SelectByShop_id(params uint?[] Shop_id) {
			return Select.WhereShop_id(Shop_id);
		}
	}
	public partial class Member_shopSelectBuild : SelectBuild<Member_shopInfo, Member_shopSelectBuild> {
		public Member_shopSelectBuild WhereMember_id(params uint?[] Member_id) {
			return this.Where1Or("a.`member_id` = {0}", Member_id);
		}
		public Member_shopSelectBuild WhereShop_id(params uint?[] Shop_id) {
			return this.Where1Or("a.`shop_id` = {0}", Shop_id);
		}
		public Member_shopSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as Member_shopSelectBuild;
		}
		public Member_shopSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as Member_shopSelectBuild;
		}
		protected new Member_shopSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Member_shopSelectBuild;
		}
		public Member_shopSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}