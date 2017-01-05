using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Product_attr {

		protected static readonly pifa.DAL.Product_attr dal = new pifa.DAL.Product_attr();
		protected static readonly int itemCacheTimeout;

		static Product_attr() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Product_attr"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Pattr_id, uint Product_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Pattr_id, Product_id));
			return dal.Delete(Pattr_id, Product_id);
		}
		public static int DeleteByPattr_id(uint? Pattr_id) {
			return dal.DeleteByPattr_id(Pattr_id);
		}
		public static int DeleteByProduct_id(uint? Product_id) {
			return dal.DeleteByProduct_id(Product_id);
		}

		public static int Update(Product_attrInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Product_attr.SqlUpdateBuild UpdateDiy(uint Pattr_id, uint Product_id) {
			return UpdateDiy(null, Pattr_id, Product_id);
		}
		public static pifa.DAL.Product_attr.SqlUpdateBuild UpdateDiy(Product_attrInfo item, uint Pattr_id, uint Product_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Pattr_id, Product_id));
			return new pifa.DAL.Product_attr.SqlUpdateBuild(item, Pattr_id, Product_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Product_attr.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Product_attr.SqlUpdateBuild(); }
		}

		public static Product_attrInfo Insert(uint? Pattr_id, uint? Product_id, string Value) {
			return Insert(new Product_attrInfo {
				Pattr_id = Pattr_id, 
				Product_id = Product_id, 
				Value = Value});
		}
		public static Product_attrInfo Insert(Product_attrInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Product_attrInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Product_attr_", item.Pattr_id, "_,_", item.Product_id));
		}
		#endregion

		public static Product_attrInfo GetItem(uint Pattr_id, uint Product_id) {
			if (itemCacheTimeout <= 0) return Select.WherePattr_id(Pattr_id).WhereProduct_id(Product_id).ToOne();
			string key = string.Concat("pifa_BLL_Product_attr_", Pattr_id, "_,_", Product_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return Product_attrInfo.Parse(value); } catch { }
			Product_attrInfo item = Select.WherePattr_id(Pattr_id).WhereProduct_id(Product_id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Product_attrInfo> GetItems() {
			return Select.ToList();
		}
		public static Product_attrSelectBuild Select {
			get { return new Product_attrSelectBuild(dal); }
		}
		public static List<Product_attrInfo> GetItemsByPattr_id(params uint?[] Pattr_id) {
			return Select.WherePattr_id(Pattr_id).ToList();
		}
		public static List<Product_attrInfo> GetItemsByPattr_id(uint?[] Pattr_id, int limit) {
			return Select.WherePattr_id(Pattr_id).Limit(limit).ToList();
		}
		public static Product_attrSelectBuild SelectByPattr_id(params uint?[] Pattr_id) {
			return Select.WherePattr_id(Pattr_id);
		}
		public static List<Product_attrInfo> GetItemsByProduct_id(params uint?[] Product_id) {
			return Select.WhereProduct_id(Product_id).ToList();
		}
		public static List<Product_attrInfo> GetItemsByProduct_id(uint?[] Product_id, int limit) {
			return Select.WhereProduct_id(Product_id).Limit(limit).ToList();
		}
		public static Product_attrSelectBuild SelectByProduct_id(params uint?[] Product_id) {
			return Select.WhereProduct_id(Product_id);
		}
	}
	public partial class Product_attrSelectBuild : SelectBuild<Product_attrInfo, Product_attrSelectBuild> {
		public Product_attrSelectBuild WherePattr_id(params uint?[] Pattr_id) {
			return this.Where1Or("a.`pattr_id` = {0}", Pattr_id);
		}
		public Product_attrSelectBuild WhereProduct_id(params uint?[] Product_id) {
			return this.Where1Or("a.`product_id` = {0}", Product_id);
		}
		public Product_attrSelectBuild WhereValue(params string[] Value) {
			return this.Where1Or("a.`value` = {0}", Value);
		}
		public Product_attrSelectBuild WhereValueLike(params string[] Value) {
			if (Value == null || Value.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`value` LIKE {0}", Value.Select(a => "%" + a + "%").ToArray());
		}
		protected new Product_attrSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Product_attrSelectBuild;
		}
		public Product_attrSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}