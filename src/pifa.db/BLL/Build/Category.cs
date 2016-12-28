using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Category {

		protected static readonly pifa.DAL.Category dal = new pifa.DAL.Category();
		protected static readonly int itemCacheTimeout;

		static Category() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Category"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByParent_id(uint? Parent_id) {
			return dal.DeleteByParent_id(Parent_id);
		}

		public static int Update(CategoryInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Category.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Category.SqlUpdateBuild UpdateDiy(CategoryInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Category.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Category.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Category.SqlUpdateBuild(); }
		}

		public static CategoryInfo Insert(uint? Parent_id, string Title) {
			return Insert(new CategoryInfo {
				Parent_id = Parent_id, 
				Title = Title});
		}
		public static CategoryInfo Insert(CategoryInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(CategoryInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Category_", item.Id));
		}
		#endregion

		public static CategoryInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("pifa_BLL_Category_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new CategoryInfo(value); } catch { }
			CategoryInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<CategoryInfo> GetItems() {
			return Select.ToList();
		}
		public static CategorySelectBuild Select {
			get { return new CategorySelectBuild(dal); }
		}
		public static List<CategoryInfo> GetItemsByParent_id(params uint?[] Parent_id) {
			return Select.WhereParent_id(Parent_id).ToList();
		}
		public static List<CategoryInfo> GetItemsByParent_id(uint?[] Parent_id, int limit) {
			return Select.WhereParent_id(Parent_id).Limit(limit).ToList();
		}
		public static CategorySelectBuild SelectByParent_id(params uint?[] Parent_id) {
			return Select.WhereParent_id(Parent_id);
		}
		public static CategorySelectBuild SelectByArea(params AreaInfo[] items) {
			return Select.WhereArea(items);
		}
		public static CategorySelectBuild SelectByArea_id(params uint[] ids) {
			return Select.WhereArea_id(ids);
		}
		public static CategorySelectBuild SelectByMarkettype(params MarkettypeInfo[] items) {
			return Select.WhereMarkettype(items);
		}
		public static CategorySelectBuild SelectByMarkettype_id(params uint[] ids) {
			return Select.WhereMarkettype_id(ids);
		}
	}
	public partial class CategorySelectBuild : SelectBuild<CategoryInfo, CategorySelectBuild> {
		public CategorySelectBuild WhereParent_id(params uint?[] Parent_id) {
			return this.Where1Or("a.`Parent_id` = {0}", Parent_id);
		}
		public CategorySelectBuild WhereArea(params AreaInfo[] items) {
			if (items == null) return this;
			return WhereArea_id(items.Where<AreaInfo>(a => a != null).Select<AreaInfo, uint>(a => a.Id.Value).ToArray());
		}
		public CategorySelectBuild WhereArea_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `category_id` FROM `area_category` WHERE `category_id` = a.`id` AND `area_id` IN ({0}) )", string.Join<uint>(",", ids))) as CategorySelectBuild;
		}
		public CategorySelectBuild WhereMarkettype(params MarkettypeInfo[] items) {
			if (items == null) return this;
			return WhereMarkettype_id(items.Where<MarkettypeInfo>(a => a != null).Select<MarkettypeInfo, uint>(a => a.Id.Value).ToArray());
		}
		public CategorySelectBuild WhereMarkettype_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `category_id` FROM `markettype_category` WHERE `category_id` = a.`id` AND `markettype_id` IN ({0}) )", string.Join<uint>(",", ids))) as CategorySelectBuild;
		}
		public CategorySelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public CategorySelectBuild WhereTitle(params string[] Title) {
			return this.Where1Or("a.`title` = {0}", Title);
		}
		public CategorySelectBuild WhereTitleLike(params string[] Title) {
			if (Title == null) return this;
			return this.Where1Or(@"a.`title` LIKE {0}", Title.Select(a => "%" + a + "%").ToArray());
		}
		protected new CategorySelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as CategorySelectBuild;
		}
		public CategorySelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}