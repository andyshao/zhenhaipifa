using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Rentsublet_franchising {

		protected static readonly pifa.DAL.Rentsublet_franchising dal = new pifa.DAL.Rentsublet_franchising();
		protected static readonly int itemCacheTimeout;

		static Rentsublet_franchising() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Rentsublet_franchising"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Franchising_id, uint? Rentsublet_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Franchising_id, Rentsublet_id));
			return dal.Delete(Franchising_id, Rentsublet_id);
		}
		public static int DeleteByFranchising_id(uint? Franchising_id) {
			return dal.DeleteByFranchising_id(Franchising_id);
		}
		public static int DeleteByRentsublet_id(uint? Rentsublet_id) {
			return dal.DeleteByRentsublet_id(Rentsublet_id);
		}

		public static int Update(Rentsublet_franchisingInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Rentsublet_franchising.SqlUpdateBuild UpdateDiy(uint? Franchising_id, uint? Rentsublet_id) {
			return UpdateDiy(null, Franchising_id, Rentsublet_id);
		}
		public static pifa.DAL.Rentsublet_franchising.SqlUpdateBuild UpdateDiy(Rentsublet_franchisingInfo item, uint? Franchising_id, uint? Rentsublet_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Franchising_id, Rentsublet_id));
			return new pifa.DAL.Rentsublet_franchising.SqlUpdateBuild(item, Franchising_id, Rentsublet_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Rentsublet_franchising.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Rentsublet_franchising.SqlUpdateBuild(); }
		}

		public static Rentsublet_franchisingInfo Insert(uint? Franchising_id, uint? Rentsublet_id) {
			return Insert(new Rentsublet_franchisingInfo {
				Franchising_id = Franchising_id, 
				Rentsublet_id = Rentsublet_id});
		}
		public static Rentsublet_franchisingInfo Insert(Rentsublet_franchisingInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Rentsublet_franchisingInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Rentsublet_franchising_", item.Franchising_id, "_,_", item.Rentsublet_id));
		}
		#endregion

		public static Rentsublet_franchisingInfo GetItem(uint? Franchising_id, uint? Rentsublet_id) {
			if (Franchising_id == null || Rentsublet_id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Franchising_id, Rentsublet_id);
			string key = string.Concat("pifa_BLL_Rentsublet_franchising_", Franchising_id, "_,_", Rentsublet_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new Rentsublet_franchisingInfo(value); } catch { }
			Rentsublet_franchisingInfo item = dal.GetItem(Franchising_id, Rentsublet_id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Rentsublet_franchisingInfo> GetItems() {
			return Select.ToList();
		}
		public static Rentsublet_franchisingSelectBuild Select {
			get { return new Rentsublet_franchisingSelectBuild(dal); }
		}
		public static List<Rentsublet_franchisingInfo> GetItemsByFranchising_id(params uint?[] Franchising_id) {
			return Select.WhereFranchising_id(Franchising_id).ToList();
		}
		public static List<Rentsublet_franchisingInfo> GetItemsByFranchising_id(uint?[] Franchising_id, int limit) {
			return Select.WhereFranchising_id(Franchising_id).Limit(limit).ToList();
		}
		public static Rentsublet_franchisingSelectBuild SelectByFranchising_id(params uint?[] Franchising_id) {
			return Select.WhereFranchising_id(Franchising_id);
		}
		public static List<Rentsublet_franchisingInfo> GetItemsByRentsublet_id(params uint?[] Rentsublet_id) {
			return Select.WhereRentsublet_id(Rentsublet_id).ToList();
		}
		public static List<Rentsublet_franchisingInfo> GetItemsByRentsublet_id(uint?[] Rentsublet_id, int limit) {
			return Select.WhereRentsublet_id(Rentsublet_id).Limit(limit).ToList();
		}
		public static Rentsublet_franchisingSelectBuild SelectByRentsublet_id(params uint?[] Rentsublet_id) {
			return Select.WhereRentsublet_id(Rentsublet_id);
		}
	}
	public partial class Rentsublet_franchisingSelectBuild : SelectBuild<Rentsublet_franchisingInfo, Rentsublet_franchisingSelectBuild> {
		public Rentsublet_franchisingSelectBuild WhereFranchising_id(params uint?[] Franchising_id) {
			return this.Where1Or("a.`Franchising_id` = {0}", Franchising_id);
		}
		public Rentsublet_franchisingSelectBuild WhereRentsublet_id(params uint?[] Rentsublet_id) {
			return this.Where1Or("a.`Rentsublet_id` = {0}", Rentsublet_id);
		}
		protected new Rentsublet_franchisingSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Rentsublet_franchisingSelectBuild;
		}
		public Rentsublet_franchisingSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}