using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Markettype {

		protected static readonly pifa.DAL.Markettype dal = new pifa.DAL.Markettype();
		protected static readonly int itemCacheTimeout;

		static Markettype() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Markettype"], out itemCacheTimeout))
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
		public static int DeleteByMarket_id(uint? Market_id) {
			return dal.DeleteByMarket_id(Market_id);
		}

		public static int Update(MarkettypeInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Markettype.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Markettype.SqlUpdateBuild UpdateDiy(MarkettypeInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Markettype.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Markettype.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Markettype.SqlUpdateBuild(); }
		}

		public static MarkettypeInfo Insert(uint? Market_id, uint? Parent_id, byte? Sort, string Title) {
			return Insert(new MarkettypeInfo {
				Market_id = Market_id, 
				Parent_id = Parent_id, 
				Sort = Sort, 
				Title = Title});
		}
		public static MarkettypeInfo Insert(MarkettypeInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(MarkettypeInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Markettype_", item.Id));
		}
		#endregion

		public static MarkettypeInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("pifa_BLL_Markettype_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new MarkettypeInfo(value); } catch { }
			MarkettypeInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<MarkettypeInfo> GetItems() {
			return Select.ToList();
		}
		public static MarkettypeSelectBuild Select {
			get { return new MarkettypeSelectBuild(dal); }
		}
		public static List<MarkettypeInfo> GetItemsByParent_id(params uint?[] Parent_id) {
			return Select.WhereParent_id(Parent_id).ToList();
		}
		public static List<MarkettypeInfo> GetItemsByParent_id(uint?[] Parent_id, int limit) {
			return Select.WhereParent_id(Parent_id).Limit(limit).ToList();
		}
		public static MarkettypeSelectBuild SelectByParent_id(params uint?[] Parent_id) {
			return Select.WhereParent_id(Parent_id);
		}
		public static List<MarkettypeInfo> GetItemsByMarket_id(params uint?[] Market_id) {
			return Select.WhereMarket_id(Market_id).ToList();
		}
		public static List<MarkettypeInfo> GetItemsByMarket_id(uint?[] Market_id, int limit) {
			return Select.WhereMarket_id(Market_id).Limit(limit).ToList();
		}
		public static MarkettypeSelectBuild SelectByMarket_id(params uint?[] Market_id) {
			return Select.WhereMarket_id(Market_id);
		}
		public static MarkettypeSelectBuild SelectByCategory(params CategoryInfo[] items) {
			return Select.WhereCategory(items);
		}
		public static MarkettypeSelectBuild SelectByCategory_id(params uint[] ids) {
			return Select.WhereCategory_id(ids);
		}
	}
	public partial class MarkettypeSelectBuild : SelectBuild<MarkettypeInfo, MarkettypeSelectBuild> {
		public MarkettypeSelectBuild WhereParent_id(params uint?[] Parent_id) {
			return this.Where1Or("a.`Parent_id` = {0}", Parent_id);
		}
		public MarkettypeSelectBuild WhereMarket_id(params uint?[] Market_id) {
			return this.Where1Or("a.`Market_id` = {0}", Market_id);
		}
		public MarkettypeSelectBuild WhereCategory(params CategoryInfo[] items) {
			if (items == null) return this;
			return WhereCategory_id(items.Where<CategoryInfo>(a => a != null).Select<CategoryInfo, uint>(a => a.Id.Value).ToArray());
		}
		public MarkettypeSelectBuild WhereCategory_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `markettype_id` FROM `markettype_category` WHERE `markettype_id` = a.`id` AND `category_id` IN ({0}) )", string.Join<uint>(",", ids))) as MarkettypeSelectBuild;
		}
		public MarkettypeSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public MarkettypeSelectBuild WhereSort(params byte?[] Sort) {
			return this.Where1Or("a.`sort` = {0}", Sort);
		}
		public MarkettypeSelectBuild WhereTitle(params string[] Title) {
			return this.Where1Or("a.`title` = {0}", Title);
		}
		public MarkettypeSelectBuild WhereTitleLike(params string[] Title) {
			if (Title == null) return this;
			return this.Where1Or(@"a.`title` LIKE {0}", Title.Select(a => "%" + a + "%").ToArray());
		}
		protected new MarkettypeSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as MarkettypeSelectBuild;
		}
		public MarkettypeSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}