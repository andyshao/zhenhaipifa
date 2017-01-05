using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Marketdesc {

		protected static readonly pifa.DAL.Marketdesc dal = new pifa.DAL.Marketdesc();
		protected static readonly int itemCacheTimeout;

		static Marketdesc() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Marketdesc"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Market_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Market_id));
			return dal.Delete(Market_id);
		}
		public static int DeleteByMarket_id(uint? Market_id) {
			return dal.DeleteByMarket_id(Market_id);
		}

		public static int Update(MarketdescInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Marketdesc.SqlUpdateBuild UpdateDiy(uint Market_id) {
			return UpdateDiy(null, Market_id);
		}
		public static pifa.DAL.Marketdesc.SqlUpdateBuild UpdateDiy(MarketdescInfo item, uint Market_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Market_id));
			return new pifa.DAL.Marketdesc.SqlUpdateBuild(item, Market_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Marketdesc.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Marketdesc.SqlUpdateBuild(); }
		}

		public static MarketdescInfo Insert(uint? Market_id, string Content, string Url) {
			return Insert(new MarketdescInfo {
				Market_id = Market_id, 
				Content = Content, 
				Url = Url});
		}
		public static MarketdescInfo Insert(MarketdescInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(MarketdescInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Marketdesc_", item.Market_id));
		}
		#endregion

		public static MarketdescInfo GetItem(uint Market_id) {
			if (itemCacheTimeout <= 0) return Select.WhereMarket_id(Market_id).ToOne();
			string key = string.Concat("pifa_BLL_Marketdesc_", Market_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return MarketdescInfo.Parse(value); } catch { }
			MarketdescInfo item = Select.WhereMarket_id(Market_id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<MarketdescInfo> GetItems() {
			return Select.ToList();
		}
		public static MarketdescSelectBuild Select {
			get { return new MarketdescSelectBuild(dal); }
		}
		public static List<MarketdescInfo> GetItemsByMarket_id(params uint?[] Market_id) {
			return Select.WhereMarket_id(Market_id).ToList();
		}
		public static List<MarketdescInfo> GetItemsByMarket_id(uint?[] Market_id, int limit) {
			return Select.WhereMarket_id(Market_id).Limit(limit).ToList();
		}
		public static MarketdescSelectBuild SelectByMarket_id(params uint?[] Market_id) {
			return Select.WhereMarket_id(Market_id);
		}
	}
	public partial class MarketdescSelectBuild : SelectBuild<MarketdescInfo, MarketdescSelectBuild> {
		public MarketdescSelectBuild WhereMarket_id(params uint?[] Market_id) {
			return this.Where1Or("a.`market_id` = {0}", Market_id);
		}
		public MarketdescSelectBuild WhereContentLike(params string[] Content) {
			if (Content == null || Content.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`content` LIKE {0}", Content.Select(a => "%" + a + "%").ToArray());
		}
		public MarketdescSelectBuild WhereUrl(params string[] Url) {
			return this.Where1Or("a.`url` = {0}", Url);
		}
		public MarketdescSelectBuild WhereUrlLike(params string[] Url) {
			if (Url == null || Url.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`url` LIKE {0}", Url.Select(a => "%" + a + "%").ToArray());
		}
		protected new MarketdescSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as MarketdescSelectBuild;
		}
		public MarketdescSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}