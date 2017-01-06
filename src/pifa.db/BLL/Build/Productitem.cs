using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Productitem {

		protected static readonly pifa.DAL.Productitem dal = new pifa.DAL.Productitem();
		protected static readonly int itemCacheTimeout;

		static Productitem() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Productitem"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByProduct_id(uint? Product_id) {
			return dal.DeleteByProduct_id(Product_id);
		}

		public static int Update(ProductitemInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Productitem.SqlUpdateBuild UpdateDiy(uint Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Productitem.SqlUpdateBuild UpdateDiy(ProductitemInfo item, uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Productitem.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Productitem.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Productitem.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Productitem.Insert(ProductitemInfo item)
		/// </summary>
		[Obsolete]
		public static ProductitemInfo Insert(uint? Product_id, string Img_url, string Name, decimal? Original_price, decimal? Price, uint? Stock) {
			return Insert(new ProductitemInfo {
				Product_id = Product_id, 
				Img_url = Img_url, 
				Name = Name, 
				Original_price = Original_price, 
				Price = Price, 
				Stock = Stock});
		}
		public static ProductitemInfo Insert(ProductitemInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(ProductitemInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Productitem_", item.Id));
		}
		#endregion

		public static ProductitemInfo GetItem(uint Id) {
			if (itemCacheTimeout <= 0) return Select.WhereId(Id).ToOne();
			string key = string.Concat("pifa_BLL_Productitem_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return ProductitemInfo.Parse(value); } catch { }
			ProductitemInfo item = Select.WhereId(Id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<ProductitemInfo> GetItems() {
			return Select.ToList();
		}
		public static ProductitemSelectBuild Select {
			get { return new ProductitemSelectBuild(dal); }
		}
		public static List<ProductitemInfo> GetItemsByProduct_id(params uint?[] Product_id) {
			return Select.WhereProduct_id(Product_id).ToList();
		}
		public static List<ProductitemInfo> GetItemsByProduct_id(uint?[] Product_id, int limit) {
			return Select.WhereProduct_id(Product_id).Limit(limit).ToList();
		}
		public static ProductitemSelectBuild SelectByProduct_id(params uint?[] Product_id) {
			return Select.WhereProduct_id(Product_id);
		}
		public static ProductitemSelectBuild SelectByOrder(params OrderInfo[] items) {
			return Select.WhereOrder(items);
		}
		public static ProductitemSelectBuild SelectByOrder_id(params uint[] ids) {
			return Select.WhereOrder_id(ids);
		}
	}
	public partial class ProductitemSelectBuild : SelectBuild<ProductitemInfo, ProductitemSelectBuild> {
		public ProductitemSelectBuild WhereProduct_id(params uint?[] Product_id) {
			return this.Where1Or("a.`product_id` = {0}", Product_id);
		}
		public ProductitemSelectBuild WhereOrder(params OrderInfo[] items) {
			if (items == null) return this;
			return WhereOrder_id(items.Where<OrderInfo>(a => a != null).Select<OrderInfo, uint>(a => a.Id.Value).ToArray());
		}
		public ProductitemSelectBuild WhereOrder_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `productitem_id` FROM `order_productitem` WHERE `productitem_id` = a.`id` AND `order_id` IN ({0}) )", string.Join<uint>(",", ids))) as ProductitemSelectBuild;
		}
		public ProductitemSelectBuild WhereId(params uint[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public ProductitemSelectBuild WhereImg_url(params string[] Img_url) {
			return this.Where1Or("a.`img_url` = {0}", Img_url);
		}
		public ProductitemSelectBuild WhereImg_urlLike(params string[] Img_url) {
			if (Img_url == null || Img_url.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`img_url` LIKE {0}", Img_url.Select(a => "%" + a + "%").ToArray());
		}
		public ProductitemSelectBuild WhereName(params string[] Name) {
			return this.Where1Or("a.`name` = {0}", Name);
		}
		public ProductitemSelectBuild WhereNameLike(params string[] Name) {
			if (Name == null || Name.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`name` LIKE {0}", Name.Select(a => "%" + a + "%").ToArray());
		}
		public ProductitemSelectBuild WhereOriginal_price(params decimal?[] Original_price) {
			return this.Where1Or("a.`original_price` = {0}", Original_price);
		}
		public ProductitemSelectBuild WhereOriginal_priceRange(decimal? begin) {
			return base.Where("a.`original_price` >= {0}", begin) as ProductitemSelectBuild;
		}
		public ProductitemSelectBuild WhereOriginal_priceRange(decimal? begin, decimal? end) {
			if (end == null) return WhereOriginal_priceRange(begin);
			return base.Where("a.`original_price` between {0} and {1}", begin, end) as ProductitemSelectBuild;
		}
		public ProductitemSelectBuild WherePrice(params decimal?[] Price) {
			return this.Where1Or("a.`price` = {0}", Price);
		}
		public ProductitemSelectBuild WherePriceRange(decimal? begin) {
			return base.Where("a.`price` >= {0}", begin) as ProductitemSelectBuild;
		}
		public ProductitemSelectBuild WherePriceRange(decimal? begin, decimal? end) {
			if (end == null) return WherePriceRange(begin);
			return base.Where("a.`price` between {0} and {1}", begin, end) as ProductitemSelectBuild;
		}
		public ProductitemSelectBuild WhereStock(params uint?[] Stock) {
			return this.Where1Or("a.`stock` = {0}", Stock);
		}
		protected new ProductitemSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as ProductitemSelectBuild;
		}
		public ProductitemSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}