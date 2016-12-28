using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Factory {

		protected static readonly pifa.DAL.Factory dal = new pifa.DAL.Factory();
		protected static readonly int itemCacheTimeout;

		static Factory() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Factory"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByArea_id(uint? Area_id) {
			return dal.DeleteByArea_id(Area_id);
		}

		public static int Update(FactoryInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Factory.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Factory.SqlUpdateBuild UpdateDiy(FactoryInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Factory.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Factory.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Factory.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Factory.Insert(FactoryInfo item)
		/// </summary>
		[Obsolete]
		public static FactoryInfo Insert(uint? Area_id, string Capacity, DateTime? Create_time, string Main_business, string Min_order, string Process_cost, string Sampling_period, string Sampling_price, string Telphone, string Title, string Turn_single_time) {
			return Insert(new FactoryInfo {
				Area_id = Area_id, 
				Capacity = Capacity, 
				Create_time = Create_time, 
				Main_business = Main_business, 
				Min_order = Min_order, 
				Process_cost = Process_cost, 
				Sampling_period = Sampling_period, 
				Sampling_price = Sampling_price, 
				Telphone = Telphone, 
				Title = Title, 
				Turn_single_time = Turn_single_time});
		}
		public static FactoryInfo Insert(FactoryInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(FactoryInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Factory_", item.Id));
		}
		#endregion

		public static FactoryInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("pifa_BLL_Factory_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new FactoryInfo(value); } catch { }
			FactoryInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<FactoryInfo> GetItems() {
			return Select.ToList();
		}
		public static FactorySelectBuild Select {
			get { return new FactorySelectBuild(dal); }
		}
		public static List<FactoryInfo> GetItemsByArea_id(params uint?[] Area_id) {
			return Select.WhereArea_id(Area_id).ToList();
		}
		public static List<FactoryInfo> GetItemsByArea_id(uint?[] Area_id, int limit) {
			return Select.WhereArea_id(Area_id).Limit(limit).ToList();
		}
		public static FactorySelectBuild SelectByArea_id(params uint?[] Area_id) {
			return Select.WhereArea_id(Area_id);
		}
		public static FactorySelectBuild SelectByFranchising(params FranchisingInfo[] items) {
			return Select.WhereFranchising(items);
		}
		public static FactorySelectBuild SelectByFranchising_id(params uint[] ids) {
			return Select.WhereFranchising_id(ids);
		}
	}
	public partial class FactorySelectBuild : SelectBuild<FactoryInfo, FactorySelectBuild> {
		public FactorySelectBuild WhereArea_id(params uint?[] Area_id) {
			return this.Where1Or("a.`Area_id` = {0}", Area_id);
		}
		public FactorySelectBuild WhereFranchising(params FranchisingInfo[] items) {
			if (items == null) return this;
			return WhereFranchising_id(items.Where<FranchisingInfo>(a => a != null).Select<FranchisingInfo, uint>(a => a.Id.Value).ToArray());
		}
		public FactorySelectBuild WhereFranchising_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `factory_id` FROM `factory_franchising` WHERE `factory_id` = a.`id` AND `franchising_id` IN ({0}) )", string.Join<uint>(",", ids))) as FactorySelectBuild;
		}
		public FactorySelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public FactorySelectBuild WhereCapacity(params string[] Capacity) {
			return this.Where1Or("a.`capacity` = {0}", Capacity);
		}
		public FactorySelectBuild WhereCapacityLike(params string[] Capacity) {
			if (Capacity == null) return this;
			return this.Where1Or(@"a.`capacity` LIKE {0}", Capacity.Select(a => "%" + a + "%").ToArray());
		}
		public FactorySelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as FactorySelectBuild;
		}
		public FactorySelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as FactorySelectBuild;
		}
		public FactorySelectBuild WhereMain_business(params string[] Main_business) {
			return this.Where1Or("a.`main_business` = {0}", Main_business);
		}
		public FactorySelectBuild WhereMain_businessLike(params string[] Main_business) {
			if (Main_business == null) return this;
			return this.Where1Or(@"a.`main_business` LIKE {0}", Main_business.Select(a => "%" + a + "%").ToArray());
		}
		public FactorySelectBuild WhereMin_order(params string[] Min_order) {
			return this.Where1Or("a.`min_order` = {0}", Min_order);
		}
		public FactorySelectBuild WhereMin_orderLike(params string[] Min_order) {
			if (Min_order == null) return this;
			return this.Where1Or(@"a.`min_order` LIKE {0}", Min_order.Select(a => "%" + a + "%").ToArray());
		}
		public FactorySelectBuild WhereProcess_cost(params string[] Process_cost) {
			return this.Where1Or("a.`process_cost` = {0}", Process_cost);
		}
		public FactorySelectBuild WhereProcess_costLike(params string[] Process_cost) {
			if (Process_cost == null) return this;
			return this.Where1Or(@"a.`process_cost` LIKE {0}", Process_cost.Select(a => "%" + a + "%").ToArray());
		}
		public FactorySelectBuild WhereSampling_period(params string[] Sampling_period) {
			return this.Where1Or("a.`sampling_period` = {0}", Sampling_period);
		}
		public FactorySelectBuild WhereSampling_periodLike(params string[] Sampling_period) {
			if (Sampling_period == null) return this;
			return this.Where1Or(@"a.`sampling_period` LIKE {0}", Sampling_period.Select(a => "%" + a + "%").ToArray());
		}
		public FactorySelectBuild WhereSampling_price(params string[] Sampling_price) {
			return this.Where1Or("a.`sampling_price` = {0}", Sampling_price);
		}
		public FactorySelectBuild WhereSampling_priceLike(params string[] Sampling_price) {
			if (Sampling_price == null) return this;
			return this.Where1Or(@"a.`sampling_price` LIKE {0}", Sampling_price.Select(a => "%" + a + "%").ToArray());
		}
		public FactorySelectBuild WhereTelphone(params string[] Telphone) {
			return this.Where1Or("a.`telphone` = {0}", Telphone);
		}
		public FactorySelectBuild WhereTelphoneLike(params string[] Telphone) {
			if (Telphone == null) return this;
			return this.Where1Or(@"a.`telphone` LIKE {0}", Telphone.Select(a => "%" + a + "%").ToArray());
		}
		public FactorySelectBuild WhereTitle(params string[] Title) {
			return this.Where1Or("a.`title` = {0}", Title);
		}
		public FactorySelectBuild WhereTitleLike(params string[] Title) {
			if (Title == null) return this;
			return this.Where1Or(@"a.`title` LIKE {0}", Title.Select(a => "%" + a + "%").ToArray());
		}
		public FactorySelectBuild WhereTurn_single_time(params string[] Turn_single_time) {
			return this.Where1Or("a.`turn_single_time` = {0}", Turn_single_time);
		}
		public FactorySelectBuild WhereTurn_single_timeLike(params string[] Turn_single_time) {
			if (Turn_single_time == null) return this;
			return this.Where1Or(@"a.`turn_single_time` LIKE {0}", Turn_single_time.Select(a => "%" + a + "%").ToArray());
		}
		protected new FactorySelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as FactorySelectBuild;
		}
		public FactorySelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}