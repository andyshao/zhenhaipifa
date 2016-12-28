using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Member_market {

		protected static readonly pifa.DAL.Member_market dal = new pifa.DAL.Member_market();
		protected static readonly int itemCacheTimeout;

		static Member_market() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Member_market"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Market_id, uint? Member_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Market_id, Member_id));
			return dal.Delete(Market_id, Member_id);
		}
		public static int DeleteByMarket_id(uint? Market_id) {
			return dal.DeleteByMarket_id(Market_id);
		}
		public static int DeleteByMember_id(uint? Member_id) {
			return dal.DeleteByMember_id(Member_id);
		}

		public static int Update(Member_marketInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Member_market.SqlUpdateBuild UpdateDiy(uint? Market_id, uint? Member_id) {
			return UpdateDiy(null, Market_id, Member_id);
		}
		public static pifa.DAL.Member_market.SqlUpdateBuild UpdateDiy(Member_marketInfo item, uint? Market_id, uint? Member_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Market_id, Member_id));
			return new pifa.DAL.Member_market.SqlUpdateBuild(item, Market_id, Member_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Member_market.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Member_market.SqlUpdateBuild(); }
		}

		public static Member_marketInfo Insert(uint? Market_id, uint? Member_id, DateTime? Create_time) {
			return Insert(new Member_marketInfo {
				Market_id = Market_id, 
				Member_id = Member_id, 
				Create_time = Create_time});
		}
		public static Member_marketInfo Insert(Member_marketInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Member_marketInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Member_market_", item.Market_id, "_,_", item.Member_id));
		}
		#endregion

		public static Member_marketInfo GetItem(uint? Market_id, uint? Member_id) {
			if (Market_id == null || Member_id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Market_id, Member_id);
			string key = string.Concat("pifa_BLL_Member_market_", Market_id, "_,_", Member_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new Member_marketInfo(value); } catch { }
			Member_marketInfo item = dal.GetItem(Market_id, Member_id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Member_marketInfo> GetItems() {
			return Select.ToList();
		}
		public static Member_marketSelectBuild Select {
			get { return new Member_marketSelectBuild(dal); }
		}
		public static List<Member_marketInfo> GetItemsByMarket_id(params uint?[] Market_id) {
			return Select.WhereMarket_id(Market_id).ToList();
		}
		public static List<Member_marketInfo> GetItemsByMarket_id(uint?[] Market_id, int limit) {
			return Select.WhereMarket_id(Market_id).Limit(limit).ToList();
		}
		public static Member_marketSelectBuild SelectByMarket_id(params uint?[] Market_id) {
			return Select.WhereMarket_id(Market_id);
		}
		public static List<Member_marketInfo> GetItemsByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id).ToList();
		}
		public static List<Member_marketInfo> GetItemsByMember_id(uint?[] Member_id, int limit) {
			return Select.WhereMember_id(Member_id).Limit(limit).ToList();
		}
		public static Member_marketSelectBuild SelectByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id);
		}
	}
	public partial class Member_marketSelectBuild : SelectBuild<Member_marketInfo, Member_marketSelectBuild> {
		public Member_marketSelectBuild WhereMarket_id(params uint?[] Market_id) {
			return this.Where1Or("a.`Market_id` = {0}", Market_id);
		}
		public Member_marketSelectBuild WhereMember_id(params uint?[] Member_id) {
			return this.Where1Or("a.`Member_id` = {0}", Member_id);
		}
		public Member_marketSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as Member_marketSelectBuild;
		}
		public Member_marketSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as Member_marketSelectBuild;
		}
		protected new Member_marketSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Member_marketSelectBuild;
		}
		public Member_marketSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}