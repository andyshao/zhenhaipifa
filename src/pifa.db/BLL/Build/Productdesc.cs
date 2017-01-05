using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Productdesc {

		protected static readonly pifa.DAL.Productdesc dal = new pifa.DAL.Productdesc();
		protected static readonly int itemCacheTimeout;

		static Productdesc() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Productdesc"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Product_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Product_id));
			return dal.Delete(Product_id);
		}
		public static int DeleteByProduct_id(uint? Product_id) {
			return dal.DeleteByProduct_id(Product_id);
		}

		public static int Update(ProductdescInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Productdesc.SqlUpdateBuild UpdateDiy(uint Product_id) {
			return UpdateDiy(null, Product_id);
		}
		public static pifa.DAL.Productdesc.SqlUpdateBuild UpdateDiy(ProductdescInfo item, uint Product_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Product_id));
			return new pifa.DAL.Productdesc.SqlUpdateBuild(item, Product_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Productdesc.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Productdesc.SqlUpdateBuild(); }
		}

		public static ProductdescInfo Insert(uint? Product_id, string Content) {
			return Insert(new ProductdescInfo {
				Product_id = Product_id, 
				Content = Content});
		}
		public static ProductdescInfo Insert(ProductdescInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(ProductdescInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Productdesc_", item.Product_id));
		}
		#endregion

		public static ProductdescInfo GetItem(uint Product_id) {
			if (itemCacheTimeout <= 0) return Select.WhereProduct_id(Product_id).ToOne();
			string key = string.Concat("pifa_BLL_Productdesc_", Product_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return ProductdescInfo.Parse(value); } catch { }
			ProductdescInfo item = Select.WhereProduct_id(Product_id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<ProductdescInfo> GetItems() {
			return Select.ToList();
		}
		public static ProductdescSelectBuild Select {
			get { return new ProductdescSelectBuild(dal); }
		}
		public static List<ProductdescInfo> GetItemsByProduct_id(params uint?[] Product_id) {
			return Select.WhereProduct_id(Product_id).ToList();
		}
		public static List<ProductdescInfo> GetItemsByProduct_id(uint?[] Product_id, int limit) {
			return Select.WhereProduct_id(Product_id).Limit(limit).ToList();
		}
		public static ProductdescSelectBuild SelectByProduct_id(params uint?[] Product_id) {
			return Select.WhereProduct_id(Product_id);
		}
	}
	public partial class ProductdescSelectBuild : SelectBuild<ProductdescInfo, ProductdescSelectBuild> {
		public ProductdescSelectBuild WhereProduct_id(params uint?[] Product_id) {
			return this.Where1Or("a.`product_id` = {0}", Product_id);
		}
		public ProductdescSelectBuild WhereContentLike(params string[] Content) {
			if (Content == null || Content.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`content` LIKE {0}", Content.Select(a => "%" + a + "%").ToArray());
		}
		protected new ProductdescSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as ProductdescSelectBuild;
		}
		public ProductdescSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}