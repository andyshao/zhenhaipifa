using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Express {

		protected static readonly pifa.DAL.Express dal = new pifa.DAL.Express();
		protected static readonly int itemCacheTimeout;

		static Express() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Express"], out itemCacheTimeout))
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

		public static int Update(ExpressInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Express.SqlUpdateBuild UpdateDiy(uint Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Express.SqlUpdateBuild UpdateDiy(ExpressInfo item, uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Express.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Express.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Express.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Express.Insert(ExpressInfo item)
		/// </summary>
		[Obsolete]
		public static ExpressInfo Insert(uint? Area_id, string Address, DateTime? Create_time, string Service_features, string Telphone, string Title) {
			return Insert(new ExpressInfo {
				Area_id = Area_id, 
				Address = Address, 
				Create_time = Create_time, 
				Service_features = Service_features, 
				Telphone = Telphone, 
				Title = Title});
		}
		public static ExpressInfo Insert(ExpressInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(ExpressInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Express_", item.Id));
		}
		#endregion

		public static ExpressInfo GetItem(uint Id) {
			if (itemCacheTimeout <= 0) return Select.WhereId(Id).ToOne();
			string key = string.Concat("pifa_BLL_Express_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return ExpressInfo.Parse(value); } catch { }
			ExpressInfo item = Select.WhereId(Id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<ExpressInfo> GetItems() {
			return Select.ToList();
		}
		public static ExpressSelectBuild Select {
			get { return new ExpressSelectBuild(dal); }
		}
		public static List<ExpressInfo> GetItemsByArea_id(params uint?[] Area_id) {
			return Select.WhereArea_id(Area_id).ToList();
		}
		public static List<ExpressInfo> GetItemsByArea_id(uint?[] Area_id, int limit) {
			return Select.WhereArea_id(Area_id).Limit(limit).ToList();
		}
		public static ExpressSelectBuild SelectByArea_id(params uint?[] Area_id) {
			return Select.WhereArea_id(Area_id);
		}
	}
	public partial class ExpressSelectBuild : SelectBuild<ExpressInfo, ExpressSelectBuild> {
		public ExpressSelectBuild WhereArea_id(params uint?[] Area_id) {
			return this.Where1Or("a.`area_id` = {0}", Area_id);
		}
		public ExpressSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public ExpressSelectBuild WhereAddress(params string[] Address) {
			return this.Where1Or("a.`address` = {0}", Address);
		}
		public ExpressSelectBuild WhereAddressLike(params string[] Address) {
			if (Address == null || Address.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`address` LIKE {0}", Address.Select(a => "%" + a + "%").ToArray());
		}
		public ExpressSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as ExpressSelectBuild;
		}
		public ExpressSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as ExpressSelectBuild;
		}
		public ExpressSelectBuild WhereService_features(params string[] Service_features) {
			return this.Where1Or("a.`service_features` = {0}", Service_features);
		}
		public ExpressSelectBuild WhereService_featuresLike(params string[] Service_features) {
			if (Service_features == null || Service_features.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`service_features` LIKE {0}", Service_features.Select(a => "%" + a + "%").ToArray());
		}
		public ExpressSelectBuild WhereTelphone(params string[] Telphone) {
			return this.Where1Or("a.`telphone` = {0}", Telphone);
		}
		public ExpressSelectBuild WhereTelphoneLike(params string[] Telphone) {
			if (Telphone == null || Telphone.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`telphone` LIKE {0}", Telphone.Select(a => "%" + a + "%").ToArray());
		}
		public ExpressSelectBuild WhereTitle(params string[] Title) {
			return this.Where1Or("a.`title` = {0}", Title);
		}
		public ExpressSelectBuild WhereTitleLike(params string[] Title) {
			if (Title == null || Title.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`title` LIKE {0}", Title.Select(a => "%" + a + "%").ToArray());
		}
		protected new ExpressSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as ExpressSelectBuild;
		}
		public ExpressSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}