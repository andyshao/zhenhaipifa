using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Shop_franchising {

		protected static readonly pifa.DAL.Shop_franchising dal = new pifa.DAL.Shop_franchising();
		protected static readonly int itemCacheTimeout;

		static Shop_franchising() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Shop_franchising"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Franchising_id, uint Shop_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Franchising_id, Shop_id));
			return dal.Delete(Franchising_id, Shop_id);
		}
		public static int DeleteByFranchising_id(uint? Franchising_id) {
			return dal.DeleteByFranchising_id(Franchising_id);
		}
		public static int DeleteByShop_id(uint? Shop_id) {
			return dal.DeleteByShop_id(Shop_id);
		}

		public static int Update(Shop_franchisingInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Shop_franchising.SqlUpdateBuild UpdateDiy(uint Franchising_id, uint Shop_id) {
			return UpdateDiy(null, Franchising_id, Shop_id);
		}
		public static pifa.DAL.Shop_franchising.SqlUpdateBuild UpdateDiy(Shop_franchisingInfo item, uint Franchising_id, uint Shop_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Franchising_id, Shop_id));
			return new pifa.DAL.Shop_franchising.SqlUpdateBuild(item, Franchising_id, Shop_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Shop_franchising.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Shop_franchising.SqlUpdateBuild(); }
		}

		public static Shop_franchisingInfo Insert(uint? Franchising_id, uint? Shop_id) {
			return Insert(new Shop_franchisingInfo {
				Franchising_id = Franchising_id, 
				Shop_id = Shop_id});
		}
		public static Shop_franchisingInfo Insert(Shop_franchisingInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Shop_franchisingInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Shop_franchising_", item.Franchising_id, "_,_", item.Shop_id));
		}
		#endregion

		public static Shop_franchisingInfo GetItem(uint Franchising_id, uint Shop_id) {
			if (itemCacheTimeout <= 0) return Select.WhereFranchising_id(Franchising_id).WhereShop_id(Shop_id).ToOne();
			string key = string.Concat("pifa_BLL_Shop_franchising_", Franchising_id, "_,_", Shop_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return Shop_franchisingInfo.Parse(value); } catch { }
			Shop_franchisingInfo item = Select.WhereFranchising_id(Franchising_id).WhereShop_id(Shop_id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Shop_franchisingInfo> GetItems() {
			return Select.ToList();
		}
		public static Shop_franchisingSelectBuild Select {
			get { return new Shop_franchisingSelectBuild(dal); }
		}
		public static List<Shop_franchisingInfo> GetItemsByFranchising_id(params uint?[] Franchising_id) {
			return Select.WhereFranchising_id(Franchising_id).ToList();
		}
		public static List<Shop_franchisingInfo> GetItemsByFranchising_id(uint?[] Franchising_id, int limit) {
			return Select.WhereFranchising_id(Franchising_id).Limit(limit).ToList();
		}
		public static Shop_franchisingSelectBuild SelectByFranchising_id(params uint?[] Franchising_id) {
			return Select.WhereFranchising_id(Franchising_id);
		}
		public static List<Shop_franchisingInfo> GetItemsByShop_id(params uint?[] Shop_id) {
			return Select.WhereShop_id(Shop_id).ToList();
		}
		public static List<Shop_franchisingInfo> GetItemsByShop_id(uint?[] Shop_id, int limit) {
			return Select.WhereShop_id(Shop_id).Limit(limit).ToList();
		}
		public static Shop_franchisingSelectBuild SelectByShop_id(params uint?[] Shop_id) {
			return Select.WhereShop_id(Shop_id);
		}
	}
	public partial class Shop_franchisingSelectBuild : SelectBuild<Shop_franchisingInfo, Shop_franchisingSelectBuild> {
		public Shop_franchisingSelectBuild WhereFranchising_id(params uint?[] Franchising_id) {
			return this.Where1Or("a.`franchising_id` = {0}", Franchising_id);
		}
		public Shop_franchisingSelectBuild WhereShop_id(params uint?[] Shop_id) {
			return this.Where1Or("a.`shop_id` = {0}", Shop_id);
		}
		protected new Shop_franchisingSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Shop_franchisingSelectBuild;
		}
		public Shop_franchisingSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}