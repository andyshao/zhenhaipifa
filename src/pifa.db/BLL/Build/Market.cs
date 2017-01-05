using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Market {

		protected static readonly pifa.DAL.Market dal = new pifa.DAL.Market();
		protected static readonly int itemCacheTimeout;

		static Market() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Market"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByArea_id(uint? Area_id) {
			return dal.DeleteByArea_id(Area_id);
		}

		public static int Update(MarketInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Market.SqlUpdateBuild UpdateDiy(uint Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Market.SqlUpdateBuild UpdateDiy(MarketInfo item, uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Market.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Market.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Market.SqlUpdateBuild(); }
		}

		public static MarketInfo Insert(uint? Area_id, DateTime? Create_time, string Title) {
			return Insert(new MarketInfo {
				Area_id = Area_id, 
				Create_time = Create_time, 
				Title = Title});
		}
		public static MarketInfo Insert(MarketInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(MarketInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Market_", item.Id));
		}
		#endregion

		public static MarketInfo GetItem(uint Id) {
			if (itemCacheTimeout <= 0) return Select.WhereId(Id).ToOne();
			string key = string.Concat("pifa_BLL_Market_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return MarketInfo.Parse(value); } catch { }
			MarketInfo item = Select.WhereId(Id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<MarketInfo> GetItems() {
			return Select.ToList();
		}
		public static MarketSelectBuild Select {
			get { return new MarketSelectBuild(dal); }
		}
		public static List<MarketInfo> GetItemsByArea_id(params uint?[] Area_id) {
			return Select.WhereArea_id(Area_id).ToList();
		}
		public static List<MarketInfo> GetItemsByArea_id(uint?[] Area_id, int limit) {
			return Select.WhereArea_id(Area_id).Limit(limit).ToList();
		}
		public static MarketSelectBuild SelectByArea_id(params uint?[] Area_id) {
			return Select.WhereArea_id(Area_id);
		}
		public static MarketSelectBuild SelectByMember(params MemberInfo[] items) {
			return Select.WhereMember(items);
		}
		public static MarketSelectBuild SelectByMember_id(params uint[] ids) {
			return Select.WhereMember_id(ids);
		}
	}
	public partial class MarketSelectBuild : SelectBuild<MarketInfo, MarketSelectBuild> {
		public MarketSelectBuild WhereArea_id(params uint?[] Area_id) {
			return this.Where1Or("a.`area_id` = {0}", Area_id);
		}
		public MarketSelectBuild WhereMember(params MemberInfo[] items) {
			if (items == null) return this;
			return WhereMember_id(items.Where<MemberInfo>(a => a != null).Select<MemberInfo, uint>(a => a.Id.Value).ToArray());
		}
		public MarketSelectBuild WhereMember_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `market_id` FROM `member_market` WHERE `market_id` = a.`id` AND `member_id` IN ({0}) )", string.Join<uint>(",", ids))) as MarketSelectBuild;
		}
		public MarketSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public MarketSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as MarketSelectBuild;
		}
		public MarketSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as MarketSelectBuild;
		}
		public MarketSelectBuild WhereTitle(params string[] Title) {
			return this.Where1Or("a.`title` = {0}", Title);
		}
		public MarketSelectBuild WhereTitleLike(params string[] Title) {
			if (Title == null || Title.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`title` LIKE {0}", Title.Select(a => "%" + a + "%").ToArray());
		}
		protected new MarketSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as MarketSelectBuild;
		}
		public MarketSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}