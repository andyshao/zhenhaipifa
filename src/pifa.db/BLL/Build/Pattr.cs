using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Pattr {

		protected static readonly pifa.DAL.Pattr dal = new pifa.DAL.Pattr();
		protected static readonly int itemCacheTimeout;

		static Pattr() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Pattr"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByCategory_id(uint? Category_id) {
			return dal.DeleteByCategory_id(Category_id);
		}
		public static int DeleteByParent_id(uint? Parent_id) {
			return dal.DeleteByParent_id(Parent_id);
		}

		public static int Update(PattrInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Pattr.SqlUpdateBuild UpdateDiy(uint Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Pattr.SqlUpdateBuild UpdateDiy(PattrInfo item, uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Pattr.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Pattr.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Pattr.SqlUpdateBuild(); }
		}

		public static PattrInfo Insert(uint? Category_id, uint? Parent_id, bool? Is_filter, string Name) {
			return Insert(new PattrInfo {
				Category_id = Category_id, 
				Parent_id = Parent_id, 
				Is_filter = Is_filter, 
				Name = Name});
		}
		public static PattrInfo Insert(PattrInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(PattrInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Pattr_", item.Id));
		}
		#endregion

		public static PattrInfo GetItem(uint Id) {
			if (itemCacheTimeout <= 0) return Select.WhereId(Id).ToOne();
			string key = string.Concat("pifa_BLL_Pattr_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return PattrInfo.Parse(value); } catch { }
			PattrInfo item = Select.WhereId(Id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<PattrInfo> GetItems() {
			return Select.ToList();
		}
		public static PattrSelectBuild Select {
			get { return new PattrSelectBuild(dal); }
		}
		public static List<PattrInfo> GetItemsByCategory_id(params uint?[] Category_id) {
			return Select.WhereCategory_id(Category_id).ToList();
		}
		public static List<PattrInfo> GetItemsByCategory_id(uint?[] Category_id, int limit) {
			return Select.WhereCategory_id(Category_id).Limit(limit).ToList();
		}
		public static PattrSelectBuild SelectByCategory_id(params uint?[] Category_id) {
			return Select.WhereCategory_id(Category_id);
		}
		public static List<PattrInfo> GetItemsByParent_id(params uint?[] Parent_id) {
			return Select.WhereParent_id(Parent_id).ToList();
		}
		public static List<PattrInfo> GetItemsByParent_id(uint?[] Parent_id, int limit) {
			return Select.WhereParent_id(Parent_id).Limit(limit).ToList();
		}
		public static PattrSelectBuild SelectByParent_id(params uint?[] Parent_id) {
			return Select.WhereParent_id(Parent_id);
		}
		public static PattrSelectBuild SelectByProduct_attr(params ProductInfo[] items) {
			return Select.WhereProduct_attr(items);
		}
		public static PattrSelectBuild SelectByProduct_attr_id(params uint[] ids) {
			return Select.WhereProduct_attr_id(ids);
		}
	}
	public partial class PattrSelectBuild : SelectBuild<PattrInfo, PattrSelectBuild> {
		public PattrSelectBuild WhereCategory_id(params uint?[] Category_id) {
			return this.Where1Or("a.`category_id` = {0}", Category_id);
		}
		public PattrSelectBuild WhereParent_id(params uint?[] Parent_id) {
			return this.Where1Or("a.`parent_id` = {0}", Parent_id);
		}
		public PattrSelectBuild WhereProduct_attr(params ProductInfo[] items) {
			if (items == null) return this;
			return WhereProduct_attr_id(items.Where<ProductInfo>(a => a != null).Select<ProductInfo, uint>(a => a.Id.Value).ToArray());
		}
		public PattrSelectBuild WhereProduct_attr_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `pattr_id` FROM `product_attr` WHERE `pattr_id` = a.`id` AND `product_id` IN ({0}) )", string.Join<uint>(",", ids))) as PattrSelectBuild;
		}
		public PattrSelectBuild WhereId(params uint[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public PattrSelectBuild WhereIs_filter(params bool?[] Is_filter) {
			return this.Where1Or("a.`is_filter` = {0}", Is_filter);
		}
		public PattrSelectBuild WhereName(params string[] Name) {
			return this.Where1Or("a.`name` = {0}", Name);
		}
		public PattrSelectBuild WhereNameLike(params string[] Name) {
			if (Name == null || Name.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`name` LIKE {0}", Name.Select(a => "%" + a + "%").ToArray());
		}
		protected new PattrSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as PattrSelectBuild;
		}
		public PattrSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}