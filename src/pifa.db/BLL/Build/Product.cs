using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Product {

		protected static readonly pifa.DAL.Product dal = new pifa.DAL.Product();
		protected static readonly int itemCacheTimeout;

		static Product() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Product"], out itemCacheTimeout))
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
		public static int DeleteByShop_id(uint? Shop_id) {
			return dal.DeleteByShop_id(Shop_id);
		}

		public static int Update(ProductInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Product.SqlUpdateBuild UpdateDiy(uint Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Product.SqlUpdateBuild UpdateDiy(ProductInfo item, uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Product.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Product.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Product.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Product.Insert(ProductInfo item)
		/// </summary>
		[Obsolete]
		public static ProductInfo Insert(uint? Category_id, uint? Shop_id, DateTime? Create_time, ProductICON? Icon, decimal? Price, uint? Stock, string Title, string Unit) {
			return Insert(new ProductInfo {
				Category_id = Category_id, 
				Shop_id = Shop_id, 
				Create_time = Create_time, 
				Icon = Icon, 
				Price = Price, 
				Stock = Stock, 
				Title = Title, 
				Unit = Unit});
		}
		public static ProductInfo Insert(ProductInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(ProductInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Product_", item.Id));
		}
		#endregion

		public static ProductInfo GetItem(uint Id) {
			if (itemCacheTimeout <= 0) return Select.WhereId(Id).ToOne();
			string key = string.Concat("pifa_BLL_Product_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return ProductInfo.Parse(value); } catch { }
			ProductInfo item = Select.WhereId(Id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<ProductInfo> GetItems() {
			return Select.ToList();
		}
		public static ProductSelectBuild Select {
			get { return new ProductSelectBuild(dal); }
		}
		public static List<ProductInfo> GetItemsByCategory_id(params uint?[] Category_id) {
			return Select.WhereCategory_id(Category_id).ToList();
		}
		public static List<ProductInfo> GetItemsByCategory_id(uint?[] Category_id, int limit) {
			return Select.WhereCategory_id(Category_id).Limit(limit).ToList();
		}
		public static ProductSelectBuild SelectByCategory_id(params uint?[] Category_id) {
			return Select.WhereCategory_id(Category_id);
		}
		public static List<ProductInfo> GetItemsByShop_id(params uint?[] Shop_id) {
			return Select.WhereShop_id(Shop_id).ToList();
		}
		public static List<ProductInfo> GetItemsByShop_id(uint?[] Shop_id, int limit) {
			return Select.WhereShop_id(Shop_id).Limit(limit).ToList();
		}
		public static ProductSelectBuild SelectByShop_id(params uint?[] Shop_id) {
			return Select.WhereShop_id(Shop_id);
		}
		public static ProductSelectBuild SelectByMember(params MemberInfo[] items) {
			return Select.WhereMember(items);
		}
		public static ProductSelectBuild SelectByMember_id(params uint[] ids) {
			return Select.WhereMember_id(ids);
		}
		public static ProductSelectBuild SelectByAttr(params PattrInfo[] items) {
			return Select.WhereAttr(items);
		}
		public static ProductSelectBuild SelectByAttr_id(params uint[] ids) {
			return Select.WhereAttr_id(ids);
		}
	}
	public partial class ProductSelectBuild : SelectBuild<ProductInfo, ProductSelectBuild> {
		public ProductSelectBuild WhereCategory_id(params uint?[] Category_id) {
			return this.Where1Or("a.`category_id` = {0}", Category_id);
		}
		public ProductSelectBuild WhereShop_id(params uint?[] Shop_id) {
			return this.Where1Or("a.`shop_id` = {0}", Shop_id);
		}
		public ProductSelectBuild WhereMember(params MemberInfo[] items) {
			if (items == null) return this;
			return WhereMember_id(items.Where<MemberInfo>(a => a != null).Select<MemberInfo, uint>(a => a.Id.Value).ToArray());
		}
		public ProductSelectBuild WhereMember_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `product_id` FROM `member_product` WHERE `product_id` = a.`id` AND `member_id` IN ({0}) )", string.Join<uint>(",", ids))) as ProductSelectBuild;
		}
		public ProductSelectBuild WhereAttr(params PattrInfo[] items) {
			if (items == null) return this;
			return WhereAttr_id(items.Where<PattrInfo>(a => a != null).Select<PattrInfo, uint>(a => a.Id.Value).ToArray());
		}
		public ProductSelectBuild WhereAttr_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `product_id` FROM `product_attr` WHERE `product_id` = a.`id` AND `pattr_id` IN ({0}) )", string.Join<uint>(",", ids))) as ProductSelectBuild;
		}
		public ProductSelectBuild WhereId(params uint[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public ProductSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as ProductSelectBuild;
		}
		public ProductSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as ProductSelectBuild;
		}
		public ProductSelectBuild WhereIcon_IN(params ProductICON[] Icons) {
			return this.Where1Or("(a.`icon` & {0}) = {0}", Icons);
		}
		public ProductSelectBuild WhereIcon(ProductICON Icon1) {
			return this.WhereIcon_IN(Icon1);
		}
		#region WhereIcon
		public ProductSelectBuild WhereIcon(ProductICON Icon1, ProductICON Icon2) {
			return this.WhereIcon_IN(Icon1, Icon2);
		}
		public ProductSelectBuild WhereIcon(ProductICON Icon1, ProductICON Icon2, ProductICON Icon3) {
			return this.WhereIcon_IN(Icon1, Icon2, Icon3);
		}
		public ProductSelectBuild WhereIcon(ProductICON Icon1, ProductICON Icon2, ProductICON Icon3, ProductICON Icon4) {
			return this.WhereIcon_IN(Icon1, Icon2, Icon3, Icon4);
		}
		public ProductSelectBuild WhereIcon(ProductICON Icon1, ProductICON Icon2, ProductICON Icon3, ProductICON Icon4, ProductICON Icon5) {
			return this.WhereIcon_IN(Icon1, Icon2, Icon3, Icon4, Icon5);
		}
		#endregion
		public ProductSelectBuild WherePrice(params decimal?[] Price) {
			return this.Where1Or("a.`price` = {0}", Price);
		}
		public ProductSelectBuild WherePriceRange(decimal? begin) {
			return base.Where("a.`price` >= {0}", begin) as ProductSelectBuild;
		}
		public ProductSelectBuild WherePriceRange(decimal? begin, decimal? end) {
			if (end == null) return WherePriceRange(begin);
			return base.Where("a.`price` between {0} and {1}", begin, end) as ProductSelectBuild;
		}
		public ProductSelectBuild WhereStock(params uint?[] Stock) {
			return this.Where1Or("a.`stock` = {0}", Stock);
		}
		public ProductSelectBuild WhereTitle(params string[] Title) {
			return this.Where1Or("a.`title` = {0}", Title);
		}
		public ProductSelectBuild WhereTitleLike(params string[] Title) {
			if (Title == null || Title.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`title` LIKE {0}", Title.Select(a => "%" + a + "%").ToArray());
		}
		public ProductSelectBuild WhereUnit(params string[] Unit) {
			return this.Where1Or("a.`unit` = {0}", Unit);
		}
		public ProductSelectBuild WhereUnitLike(params string[] Unit) {
			if (Unit == null || Unit.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`unit` LIKE {0}", Unit.Select(a => "%" + a + "%").ToArray());
		}
		protected new ProductSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as ProductSelectBuild;
		}
		public ProductSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}