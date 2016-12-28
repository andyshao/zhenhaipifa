using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Markettype_category {

		protected static readonly pifa.DAL.Markettype_category dal = new pifa.DAL.Markettype_category();
		protected static readonly int itemCacheTimeout;

		static Markettype_category() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Markettype_category"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Category_id, uint? Markettype_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Category_id, Markettype_id));
			return dal.Delete(Category_id, Markettype_id);
		}
		public static int DeleteByCategory_id(uint? Category_id) {
			return dal.DeleteByCategory_id(Category_id);
		}
		public static int DeleteByMarkettype_id(uint? Markettype_id) {
			return dal.DeleteByMarkettype_id(Markettype_id);
		}

		public static int Update(Markettype_categoryInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Markettype_category.SqlUpdateBuild UpdateDiy(uint? Category_id, uint? Markettype_id) {
			return UpdateDiy(null, Category_id, Markettype_id);
		}
		public static pifa.DAL.Markettype_category.SqlUpdateBuild UpdateDiy(Markettype_categoryInfo item, uint? Category_id, uint? Markettype_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Category_id, Markettype_id));
			return new pifa.DAL.Markettype_category.SqlUpdateBuild(item, Category_id, Markettype_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Markettype_category.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Markettype_category.SqlUpdateBuild(); }
		}

		public static Markettype_categoryInfo Insert(uint? Category_id, uint? Markettype_id) {
			return Insert(new Markettype_categoryInfo {
				Category_id = Category_id, 
				Markettype_id = Markettype_id});
		}
		public static Markettype_categoryInfo Insert(Markettype_categoryInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Markettype_categoryInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Markettype_category_", item.Category_id, "_,_", item.Markettype_id));
		}
		#endregion

		public static Markettype_categoryInfo GetItem(uint? Category_id, uint? Markettype_id) {
			if (Category_id == null || Markettype_id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Category_id, Markettype_id);
			string key = string.Concat("pifa_BLL_Markettype_category_", Category_id, "_,_", Markettype_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new Markettype_categoryInfo(value); } catch { }
			Markettype_categoryInfo item = dal.GetItem(Category_id, Markettype_id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Markettype_categoryInfo> GetItems() {
			return Select.ToList();
		}
		public static Markettype_categorySelectBuild Select {
			get { return new Markettype_categorySelectBuild(dal); }
		}
		public static List<Markettype_categoryInfo> GetItemsByCategory_id(params uint?[] Category_id) {
			return Select.WhereCategory_id(Category_id).ToList();
		}
		public static List<Markettype_categoryInfo> GetItemsByCategory_id(uint?[] Category_id, int limit) {
			return Select.WhereCategory_id(Category_id).Limit(limit).ToList();
		}
		public static Markettype_categorySelectBuild SelectByCategory_id(params uint?[] Category_id) {
			return Select.WhereCategory_id(Category_id);
		}
		public static List<Markettype_categoryInfo> GetItemsByMarkettype_id(params uint?[] Markettype_id) {
			return Select.WhereMarkettype_id(Markettype_id).ToList();
		}
		public static List<Markettype_categoryInfo> GetItemsByMarkettype_id(uint?[] Markettype_id, int limit) {
			return Select.WhereMarkettype_id(Markettype_id).Limit(limit).ToList();
		}
		public static Markettype_categorySelectBuild SelectByMarkettype_id(params uint?[] Markettype_id) {
			return Select.WhereMarkettype_id(Markettype_id);
		}
	}
	public partial class Markettype_categorySelectBuild : SelectBuild<Markettype_categoryInfo, Markettype_categorySelectBuild> {
		public Markettype_categorySelectBuild WhereCategory_id(params uint?[] Category_id) {
			return this.Where1Or("a.`Category_id` = {0}", Category_id);
		}
		public Markettype_categorySelectBuild WhereMarkettype_id(params uint?[] Markettype_id) {
			return this.Where1Or("a.`Markettype_id` = {0}", Markettype_id);
		}
		protected new Markettype_categorySelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Markettype_categorySelectBuild;
		}
		public Markettype_categorySelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}