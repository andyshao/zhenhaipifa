using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Rentsublet {

		protected static readonly pifa.DAL.Rentsublet dal = new pifa.DAL.Rentsublet();
		protected static readonly int itemCacheTimeout;

		static Rentsublet() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Rentsublet"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByMarket_id(uint? Market_id) {
			return dal.DeleteByMarket_id(Market_id);
		}

		public static int Update(RentsubletInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Rentsublet.SqlUpdateBuild UpdateDiy(uint Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Rentsublet.SqlUpdateBuild UpdateDiy(RentsubletInfo item, uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Rentsublet.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Rentsublet.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Rentsublet.SqlUpdateBuild(); }
		}

		public static RentsubletInfo Insert(uint? Market_id, DateTime? Create_time, decimal? Price, RentsubletTYPE? Type) {
			return Insert(new RentsubletInfo {
				Market_id = Market_id, 
				Create_time = Create_time, 
				Price = Price, 
				Type = Type});
		}
		public static RentsubletInfo Insert(RentsubletInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(RentsubletInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Rentsublet_", item.Id));
		}
		#endregion

		public static RentsubletInfo GetItem(uint Id) {
			if (itemCacheTimeout <= 0) return Select.WhereId(Id).ToOne();
			string key = string.Concat("pifa_BLL_Rentsublet_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return RentsubletInfo.Parse(value); } catch { }
			RentsubletInfo item = Select.WhereId(Id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<RentsubletInfo> GetItems() {
			return Select.ToList();
		}
		public static RentsubletSelectBuild Select {
			get { return new RentsubletSelectBuild(dal); }
		}
		public static List<RentsubletInfo> GetItemsByMarket_id(params uint?[] Market_id) {
			return Select.WhereMarket_id(Market_id).ToList();
		}
		public static List<RentsubletInfo> GetItemsByMarket_id(uint?[] Market_id, int limit) {
			return Select.WhereMarket_id(Market_id).Limit(limit).ToList();
		}
		public static RentsubletSelectBuild SelectByMarket_id(params uint?[] Market_id) {
			return Select.WhereMarket_id(Market_id);
		}
		public static RentsubletSelectBuild SelectByFranchising(params FranchisingInfo[] items) {
			return Select.WhereFranchising(items);
		}
		public static RentsubletSelectBuild SelectByFranchising_id(params uint[] ids) {
			return Select.WhereFranchising_id(ids);
		}
	}
	public partial class RentsubletSelectBuild : SelectBuild<RentsubletInfo, RentsubletSelectBuild> {
		public RentsubletSelectBuild WhereMarket_id(params uint?[] Market_id) {
			return this.Where1Or("a.`market_id` = {0}", Market_id);
		}
		public RentsubletSelectBuild WhereFranchising(params FranchisingInfo[] items) {
			if (items == null) return this;
			return WhereFranchising_id(items.Where<FranchisingInfo>(a => a != null).Select<FranchisingInfo, uint>(a => a.Id.Value).ToArray());
		}
		public RentsubletSelectBuild WhereFranchising_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `rentsublet_id` FROM `rentsublet_franchising` WHERE `rentsublet_id` = a.`id` AND `franchising_id` IN ({0}) )", string.Join<uint>(",", ids))) as RentsubletSelectBuild;
		}
		public RentsubletSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public RentsubletSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as RentsubletSelectBuild;
		}
		public RentsubletSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as RentsubletSelectBuild;
		}
		public RentsubletSelectBuild WherePrice(params decimal?[] Price) {
			return this.Where1Or("a.`price` = {0}", Price);
		}
		public RentsubletSelectBuild WherePriceRange(decimal? begin) {
			return base.Where("a.`price` >= {0}", begin) as RentsubletSelectBuild;
		}
		public RentsubletSelectBuild WherePriceRange(decimal? begin, decimal? end) {
			if (end == null) return WherePriceRange(begin);
			return base.Where("a.`price` between {0} and {1}", begin, end) as RentsubletSelectBuild;
		}
		public RentsubletSelectBuild WhereType_IN(params RentsubletTYPE?[] Types) {
			return this.Where1Or("a.`type` = {0}", Types);
		}
		public RentsubletSelectBuild WhereType(RentsubletTYPE Type1) {
			return this.WhereType_IN(Type1);
		}
		#region WhereType
		public RentsubletSelectBuild WhereType(RentsubletTYPE Type1, RentsubletTYPE Type2) {
			return this.WhereType_IN(Type1, Type2);
		}
		public RentsubletSelectBuild WhereType(RentsubletTYPE Type1, RentsubletTYPE Type2, RentsubletTYPE Type3) {
			return this.WhereType_IN(Type1, Type2, Type3);
		}
		public RentsubletSelectBuild WhereType(RentsubletTYPE Type1, RentsubletTYPE Type2, RentsubletTYPE Type3, RentsubletTYPE Type4) {
			return this.WhereType_IN(Type1, Type2, Type3, Type4);
		}
		public RentsubletSelectBuild WhereType(RentsubletTYPE Type1, RentsubletTYPE Type2, RentsubletTYPE Type3, RentsubletTYPE Type4, RentsubletTYPE Type5) {
			return this.WhereType_IN(Type1, Type2, Type3, Type4, Type5);
		}
		#endregion
		protected new RentsubletSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as RentsubletSelectBuild;
		}
		public RentsubletSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}