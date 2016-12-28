using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Shopstat {

		protected static readonly pifa.DAL.Shopstat dal = new pifa.DAL.Shopstat();
		protected static readonly int itemCacheTimeout;

		static Shopstat() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Shopstat"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Shop_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Shop_id));
			return dal.Delete(Shop_id);
		}

		public static int Update(ShopstatInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Shopstat.SqlUpdateBuild UpdateDiy(uint? Shop_id) {
			return UpdateDiy(null, Shop_id);
		}
		public static pifa.DAL.Shopstat.SqlUpdateBuild UpdateDiy(ShopstatInfo item, uint? Shop_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Shop_id));
			return new pifa.DAL.Shopstat.SqlUpdateBuild(item, Shop_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Shopstat.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Shopstat.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Shopstat.Insert(ShopstatInfo item)
		/// </summary>
		[Obsolete]
		public static ShopstatInfo Insert(uint? Shop_id, uint? Today_fav, uint? Today_session, uint? Today_share, uint? Total_fav, uint? Total_session, uint? Total_share) {
			return Insert(new ShopstatInfo {
				Shop_id = Shop_id, 
				Today_fav = Today_fav, 
				Today_session = Today_session, 
				Today_share = Today_share, 
				Total_fav = Total_fav, 
				Total_session = Total_session, 
				Total_share = Total_share});
		}
		public static ShopstatInfo Insert(ShopstatInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(ShopstatInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Shopstat_", item.Shop_id));
		}
		#endregion

		public static ShopstatInfo GetItem(uint? Shop_id) {
			if (Shop_id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Shop_id);
			string key = string.Concat("pifa_BLL_Shopstat_", Shop_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new ShopstatInfo(value); } catch { }
			ShopstatInfo item = dal.GetItem(Shop_id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<ShopstatInfo> GetItems() {
			return Select.ToList();
		}
		public static ShopstatSelectBuild Select {
			get { return new ShopstatSelectBuild(dal); }
		}
		public static List<ShopstatInfo> GetItemsByShop_id(params uint?[] Shop_id) {
			return Select.WhereShop_id(Shop_id).ToList();
		}
		public static List<ShopstatInfo> GetItemsByShop_id(uint?[] Shop_id, int limit) {
			return Select.WhereShop_id(Shop_id).Limit(limit).ToList();
		}
		public static ShopstatSelectBuild SelectByShop_id(params uint?[] Shop_id) {
			return Select.WhereShop_id(Shop_id);
		}
	}
	public partial class ShopstatSelectBuild : SelectBuild<ShopstatInfo, ShopstatSelectBuild> {
		public ShopstatSelectBuild WhereShop_id(params uint?[] Shop_id) {
			return this.Where1Or("a.`Shop_id` = {0}", Shop_id);
		}
		public ShopstatSelectBuild WhereToday_fav(params uint?[] Today_fav) {
			return this.Where1Or("a.`today_fav` = {0}", Today_fav);
		}
		public ShopstatSelectBuild WhereToday_session(params uint?[] Today_session) {
			return this.Where1Or("a.`today_session` = {0}", Today_session);
		}
		public ShopstatSelectBuild WhereToday_share(params uint?[] Today_share) {
			return this.Where1Or("a.`today_share` = {0}", Today_share);
		}
		public ShopstatSelectBuild WhereTotal_fav(params uint?[] Total_fav) {
			return this.Where1Or("a.`total_fav` = {0}", Total_fav);
		}
		public ShopstatSelectBuild WhereTotal_session(params uint?[] Total_session) {
			return this.Where1Or("a.`total_session` = {0}", Total_session);
		}
		public ShopstatSelectBuild WhereTotal_share(params uint?[] Total_share) {
			return this.Where1Or("a.`total_share` = {0}", Total_share);
		}
		protected new ShopstatSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as ShopstatSelectBuild;
		}
		public ShopstatSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}