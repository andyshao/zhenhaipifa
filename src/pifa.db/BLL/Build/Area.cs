using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Area {

		protected static readonly pifa.DAL.Area dal = new pifa.DAL.Area();
		protected static readonly int itemCacheTimeout;

		static Area() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Area"], out itemCacheTimeout))
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

		public static int Update(AreaInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Area.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Area.SqlUpdateBuild UpdateDiy(AreaInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Area.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Area.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Area.SqlUpdateBuild(); }
		}

		public static AreaInfo Insert(uint? Parent_id, string Name) {
			return Insert(new AreaInfo {
				Parent_id = Parent_id, 
				Name = Name});
		}
		public static AreaInfo Insert(AreaInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(AreaInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Area_", item.Id));
		}
		#endregion

		public static AreaInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("pifa_BLL_Area_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new AreaInfo(value); } catch { }
			AreaInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<AreaInfo> GetItems() {
			return Select.ToList();
		}
		public static AreaSelectBuild Select {
			get { return new AreaSelectBuild(dal); }
		}
		public static List<AreaInfo> GetItemsByParent_id(params uint?[] Parent_id) {
			return Select.WhereParent_id(Parent_id).ToList();
		}
		public static List<AreaInfo> GetItemsByParent_id(uint?[] Parent_id, int limit) {
			return Select.WhereParent_id(Parent_id).Limit(limit).ToList();
		}
		public static AreaSelectBuild SelectByParent_id(params uint?[] Parent_id) {
			return Select.WhereParent_id(Parent_id);
		}
		public static AreaSelectBuild SelectByCategory(params CategoryInfo[] items) {
			return Select.WhereCategory(items);
		}
		public static AreaSelectBuild SelectByCategory_id(params uint[] ids) {
			return Select.WhereCategory_id(ids);
		}
	}
	public partial class AreaSelectBuild : SelectBuild<AreaInfo, AreaSelectBuild> {
		public AreaSelectBuild WhereParent_id(params uint?[] Parent_id) {
			return this.Where1Or("a.`Parent_id` = {0}", Parent_id);
		}
		public AreaSelectBuild WhereCategory(params CategoryInfo[] items) {
			if (items == null) return this;
			return WhereCategory_id(items.Where<CategoryInfo>(a => a != null).Select<CategoryInfo, uint>(a => a.Id.Value).ToArray());
		}
		public AreaSelectBuild WhereCategory_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `area_id` FROM `area_category` WHERE `area_id` = a.`id` AND `category_id` IN ({0}) )", string.Join<uint>(",", ids))) as AreaSelectBuild;
		}
		public AreaSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public AreaSelectBuild WhereName(params string[] Name) {
			return this.Where1Or("a.`name` = {0}", Name);
		}
		public AreaSelectBuild WhereNameLike(params string[] Name) {
			if (Name == null) return this;
			return this.Where1Or(@"a.`name` LIKE {0}", Name.Select(a => "%" + a + "%").ToArray());
		}
		protected new AreaSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as AreaSelectBuild;
		}
		public AreaSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}