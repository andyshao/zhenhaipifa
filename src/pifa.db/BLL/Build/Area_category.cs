using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Area_category {

		protected static readonly pifa.DAL.Area_category dal = new pifa.DAL.Area_category();
		protected static readonly int itemCacheTimeout;

		static Area_category() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Area_category"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Area_id, uint? Category_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Area_id, Category_id));
			return dal.Delete(Area_id, Category_id);
		}
		public static int DeleteByArea_id(uint? Area_id) {
			return dal.DeleteByArea_id(Area_id);
		}
		public static int DeleteByCategory_id(uint? Category_id) {
			return dal.DeleteByCategory_id(Category_id);
		}

		public static int Update(Area_categoryInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Area_category.SqlUpdateBuild UpdateDiy(uint? Area_id, uint? Category_id) {
			return UpdateDiy(null, Area_id, Category_id);
		}
		public static pifa.DAL.Area_category.SqlUpdateBuild UpdateDiy(Area_categoryInfo item, uint? Area_id, uint? Category_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Area_id, Category_id));
			return new pifa.DAL.Area_category.SqlUpdateBuild(item, Area_id, Category_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Area_category.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Area_category.SqlUpdateBuild(); }
		}

		public static Area_categoryInfo Insert(uint? Area_id, uint? Category_id) {
			return Insert(new Area_categoryInfo {
				Area_id = Area_id, 
				Category_id = Category_id});
		}
		public static Area_categoryInfo Insert(Area_categoryInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Area_categoryInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Area_category_", item.Area_id, "_,_", item.Category_id));
		}
		#endregion

		public static Area_categoryInfo GetItem(uint? Area_id, uint? Category_id) {
			if (Area_id == null || Category_id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Area_id, Category_id);
			string key = string.Concat("pifa_BLL_Area_category_", Area_id, "_,_", Category_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new Area_categoryInfo(value); } catch { }
			Area_categoryInfo item = dal.GetItem(Area_id, Category_id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Area_categoryInfo> GetItems() {
			return Select.ToList();
		}
		public static Area_categorySelectBuild Select {
			get { return new Area_categorySelectBuild(dal); }
		}
		public static List<Area_categoryInfo> GetItemsByArea_id(params uint?[] Area_id) {
			return Select.WhereArea_id(Area_id).ToList();
		}
		public static List<Area_categoryInfo> GetItemsByArea_id(uint?[] Area_id, int limit) {
			return Select.WhereArea_id(Area_id).Limit(limit).ToList();
		}
		public static Area_categorySelectBuild SelectByArea_id(params uint?[] Area_id) {
			return Select.WhereArea_id(Area_id);
		}
		public static List<Area_categoryInfo> GetItemsByCategory_id(params uint?[] Category_id) {
			return Select.WhereCategory_id(Category_id).ToList();
		}
		public static List<Area_categoryInfo> GetItemsByCategory_id(uint?[] Category_id, int limit) {
			return Select.WhereCategory_id(Category_id).Limit(limit).ToList();
		}
		public static Area_categorySelectBuild SelectByCategory_id(params uint?[] Category_id) {
			return Select.WhereCategory_id(Category_id);
		}
	}
	public partial class Area_categorySelectBuild : SelectBuild<Area_categoryInfo, Area_categorySelectBuild> {
		public Area_categorySelectBuild WhereArea_id(params uint?[] Area_id) {
			return this.Where1Or("a.`Area_id` = {0}", Area_id);
		}
		public Area_categorySelectBuild WhereCategory_id(params uint?[] Category_id) {
			return this.Where1Or("a.`Category_id` = {0}", Category_id);
		}
		protected new Area_categorySelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Area_categorySelectBuild;
		}
		public Area_categorySelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}