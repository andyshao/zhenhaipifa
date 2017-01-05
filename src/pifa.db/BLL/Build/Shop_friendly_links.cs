using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Shop_friendly_links {

		protected static readonly pifa.DAL.Shop_friendly_links dal = new pifa.DAL.Shop_friendly_links();
		protected static readonly int itemCacheTimeout;

		static Shop_friendly_links() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Shop_friendly_links"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByShop_id(uint? Shop_id) {
			return dal.DeleteByShop_id(Shop_id);
		}

		public static int Update(Shop_friendly_linksInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Shop_friendly_links.SqlUpdateBuild UpdateDiy(uint Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Shop_friendly_links.SqlUpdateBuild UpdateDiy(Shop_friendly_linksInfo item, uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Shop_friendly_links.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Shop_friendly_links.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Shop_friendly_links.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Shop_friendly_links.Insert(Shop_friendly_linksInfo item)
		/// </summary>
		[Obsolete]
		public static Shop_friendly_linksInfo Insert(uint? Shop_id, DateTime? Create_time, byte? Sort, string Title, string Url) {
			return Insert(new Shop_friendly_linksInfo {
				Shop_id = Shop_id, 
				Create_time = Create_time, 
				Sort = Sort, 
				Title = Title, 
				Url = Url});
		}
		public static Shop_friendly_linksInfo Insert(Shop_friendly_linksInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Shop_friendly_linksInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Shop_friendly_links_", item.Id));
		}
		#endregion

		public static Shop_friendly_linksInfo GetItem(uint Id) {
			if (itemCacheTimeout <= 0) return Select.WhereId(Id).ToOne();
			string key = string.Concat("pifa_BLL_Shop_friendly_links_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return Shop_friendly_linksInfo.Parse(value); } catch { }
			Shop_friendly_linksInfo item = Select.WhereId(Id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Shop_friendly_linksInfo> GetItems() {
			return Select.ToList();
		}
		public static Shop_friendly_linksSelectBuild Select {
			get { return new Shop_friendly_linksSelectBuild(dal); }
		}
		public static List<Shop_friendly_linksInfo> GetItemsByShop_id(params uint?[] Shop_id) {
			return Select.WhereShop_id(Shop_id).ToList();
		}
		public static List<Shop_friendly_linksInfo> GetItemsByShop_id(uint?[] Shop_id, int limit) {
			return Select.WhereShop_id(Shop_id).Limit(limit).ToList();
		}
		public static Shop_friendly_linksSelectBuild SelectByShop_id(params uint?[] Shop_id) {
			return Select.WhereShop_id(Shop_id);
		}
	}
	public partial class Shop_friendly_linksSelectBuild : SelectBuild<Shop_friendly_linksInfo, Shop_friendly_linksSelectBuild> {
		public Shop_friendly_linksSelectBuild WhereShop_id(params uint?[] Shop_id) {
			return this.Where1Or("a.`shop_id` = {0}", Shop_id);
		}
		public Shop_friendly_linksSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public Shop_friendly_linksSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as Shop_friendly_linksSelectBuild;
		}
		public Shop_friendly_linksSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as Shop_friendly_linksSelectBuild;
		}
		public Shop_friendly_linksSelectBuild WhereSort(params byte?[] Sort) {
			return this.Where1Or("a.`sort` = {0}", Sort);
		}
		public Shop_friendly_linksSelectBuild WhereTitle(params string[] Title) {
			return this.Where1Or("a.`title` = {0}", Title);
		}
		public Shop_friendly_linksSelectBuild WhereTitleLike(params string[] Title) {
			if (Title == null || Title.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`title` LIKE {0}", Title.Select(a => "%" + a + "%").ToArray());
		}
		public Shop_friendly_linksSelectBuild WhereUrl(params string[] Url) {
			return this.Where1Or("a.`url` = {0}", Url);
		}
		public Shop_friendly_linksSelectBuild WhereUrlLike(params string[] Url) {
			if (Url == null || Url.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`url` LIKE {0}", Url.Select(a => "%" + a + "%").ToArray());
		}
		protected new Shop_friendly_linksSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Shop_friendly_linksSelectBuild;
		}
		public Shop_friendly_linksSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}