using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Factory_franchising {

		protected static readonly pifa.DAL.Factory_franchising dal = new pifa.DAL.Factory_franchising();
		protected static readonly int itemCacheTimeout;

		static Factory_franchising() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Factory_franchising"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Factory_id, uint Franchising_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Factory_id, Franchising_id));
			return dal.Delete(Factory_id, Franchising_id);
		}
		public static int DeleteByFactory_id(uint? Factory_id) {
			return dal.DeleteByFactory_id(Factory_id);
		}
		public static int DeleteByFranchising_id(uint? Franchising_id) {
			return dal.DeleteByFranchising_id(Franchising_id);
		}

		public static int Update(Factory_franchisingInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Factory_franchising.SqlUpdateBuild UpdateDiy(uint Factory_id, uint Franchising_id) {
			return UpdateDiy(null, Factory_id, Franchising_id);
		}
		public static pifa.DAL.Factory_franchising.SqlUpdateBuild UpdateDiy(Factory_franchisingInfo item, uint Factory_id, uint Franchising_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Factory_id, Franchising_id));
			return new pifa.DAL.Factory_franchising.SqlUpdateBuild(item, Factory_id, Franchising_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Factory_franchising.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Factory_franchising.SqlUpdateBuild(); }
		}

		public static Factory_franchisingInfo Insert(uint? Factory_id, uint? Franchising_id) {
			return Insert(new Factory_franchisingInfo {
				Factory_id = Factory_id, 
				Franchising_id = Franchising_id});
		}
		public static Factory_franchisingInfo Insert(Factory_franchisingInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Factory_franchisingInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Factory_franchising_", item.Factory_id, "_,_", item.Franchising_id));
		}
		#endregion

		public static Factory_franchisingInfo GetItem(uint Factory_id, uint Franchising_id) {
			if (itemCacheTimeout <= 0) return Select.WhereFactory_id(Factory_id).WhereFranchising_id(Franchising_id).ToOne();
			string key = string.Concat("pifa_BLL_Factory_franchising_", Factory_id, "_,_", Franchising_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return Factory_franchisingInfo.Parse(value); } catch { }
			Factory_franchisingInfo item = Select.WhereFactory_id(Factory_id).WhereFranchising_id(Franchising_id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Factory_franchisingInfo> GetItems() {
			return Select.ToList();
		}
		public static Factory_franchisingSelectBuild Select {
			get { return new Factory_franchisingSelectBuild(dal); }
		}
		public static List<Factory_franchisingInfo> GetItemsByFactory_id(params uint?[] Factory_id) {
			return Select.WhereFactory_id(Factory_id).ToList();
		}
		public static List<Factory_franchisingInfo> GetItemsByFactory_id(uint?[] Factory_id, int limit) {
			return Select.WhereFactory_id(Factory_id).Limit(limit).ToList();
		}
		public static Factory_franchisingSelectBuild SelectByFactory_id(params uint?[] Factory_id) {
			return Select.WhereFactory_id(Factory_id);
		}
		public static List<Factory_franchisingInfo> GetItemsByFranchising_id(params uint?[] Franchising_id) {
			return Select.WhereFranchising_id(Franchising_id).ToList();
		}
		public static List<Factory_franchisingInfo> GetItemsByFranchising_id(uint?[] Franchising_id, int limit) {
			return Select.WhereFranchising_id(Franchising_id).Limit(limit).ToList();
		}
		public static Factory_franchisingSelectBuild SelectByFranchising_id(params uint?[] Franchising_id) {
			return Select.WhereFranchising_id(Franchising_id);
		}
	}
	public partial class Factory_franchisingSelectBuild : SelectBuild<Factory_franchisingInfo, Factory_franchisingSelectBuild> {
		public Factory_franchisingSelectBuild WhereFactory_id(params uint?[] Factory_id) {
			return this.Where1Or("a.`factory_id` = {0}", Factory_id);
		}
		public Factory_franchisingSelectBuild WhereFranchising_id(params uint?[] Franchising_id) {
			return this.Where1Or("a.`franchising_id` = {0}", Franchising_id);
		}
		protected new Factory_franchisingSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Factory_franchisingSelectBuild;
		}
		public Factory_franchisingSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}