using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Product_buyrule {

		protected static readonly pifa.DAL.Product_buyrule dal = new pifa.DAL.Product_buyrule();
		protected static readonly int itemCacheTimeout;

		static Product_buyrule() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Product_buyrule"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByProduct_id(uint? Product_id) {
			return dal.DeleteByProduct_id(Product_id);
		}

		public static int Update(Product_buyruleInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Product_buyrule.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Product_buyrule.SqlUpdateBuild UpdateDiy(Product_buyruleInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Product_buyrule.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Product_buyrule.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Product_buyrule.SqlUpdateBuild(); }
		}

		public static Product_buyruleInfo Insert(uint? Product_id, uint? Discount, uint? Ordering_end, uint? Ordering_start) {
			return Insert(new Product_buyruleInfo {
				Product_id = Product_id, 
				Discount = Discount, 
				Ordering_end = Ordering_end, 
				Ordering_start = Ordering_start});
		}
		public static Product_buyruleInfo Insert(Product_buyruleInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Product_buyruleInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Product_buyrule_", item.Id));
		}
		#endregion

		public static Product_buyruleInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("pifa_BLL_Product_buyrule_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new Product_buyruleInfo(value); } catch { }
			Product_buyruleInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Product_buyruleInfo> GetItems() {
			return Select.ToList();
		}
		public static Product_buyruleSelectBuild Select {
			get { return new Product_buyruleSelectBuild(dal); }
		}
		public static List<Product_buyruleInfo> GetItemsByProduct_id(params uint?[] Product_id) {
			return Select.WhereProduct_id(Product_id).ToList();
		}
		public static List<Product_buyruleInfo> GetItemsByProduct_id(uint?[] Product_id, int limit) {
			return Select.WhereProduct_id(Product_id).Limit(limit).ToList();
		}
		public static Product_buyruleSelectBuild SelectByProduct_id(params uint?[] Product_id) {
			return Select.WhereProduct_id(Product_id);
		}
	}
	public partial class Product_buyruleSelectBuild : SelectBuild<Product_buyruleInfo, Product_buyruleSelectBuild> {
		public Product_buyruleSelectBuild WhereProduct_id(params uint?[] Product_id) {
			return this.Where1Or("a.`Product_id` = {0}", Product_id);
		}
		public Product_buyruleSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public Product_buyruleSelectBuild WhereDiscount(params uint?[] Discount) {
			return this.Where1Or("a.`discount` = {0}", Discount);
		}
		public Product_buyruleSelectBuild WhereOrdering_end(params uint?[] Ordering_end) {
			return this.Where1Or("a.`ordering_end` = {0}", Ordering_end);
		}
		public Product_buyruleSelectBuild WhereOrdering_start(params uint?[] Ordering_start) {
			return this.Where1Or("a.`ordering_start` = {0}", Ordering_start);
		}
		protected new Product_buyruleSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Product_buyruleSelectBuild;
		}
		public Product_buyruleSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}