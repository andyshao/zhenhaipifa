using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Franchising {

		protected static readonly pifa.DAL.Franchising dal = new pifa.DAL.Franchising();
		protected static readonly int itemCacheTimeout;

		static Franchising() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Franchising"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}

		public static int Update(FranchisingInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Franchising.SqlUpdateBuild UpdateDiy(uint Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Franchising.SqlUpdateBuild UpdateDiy(FranchisingInfo item, uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Franchising.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Franchising.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Franchising.SqlUpdateBuild(); }
		}

		public static FranchisingInfo Insert(string Title) {
			return Insert(new FranchisingInfo {
				Title = Title});
		}
		public static FranchisingInfo Insert(FranchisingInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(FranchisingInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Franchising_", item.Id));
		}
		#endregion

		public static FranchisingInfo GetItem(uint Id) {
			if (itemCacheTimeout <= 0) return Select.WhereId(Id).ToOne();
			string key = string.Concat("pifa_BLL_Franchising_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return FranchisingInfo.Parse(value); } catch { }
			FranchisingInfo item = Select.WhereId(Id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<FranchisingInfo> GetItems() {
			return Select.ToList();
		}
		public static FranchisingSelectBuild Select {
			get { return new FranchisingSelectBuild(dal); }
		}
		public static FranchisingSelectBuild SelectByFactory(params FactoryInfo[] items) {
			return Select.WhereFactory(items);
		}
		public static FranchisingSelectBuild SelectByFactory_id(params uint[] ids) {
			return Select.WhereFactory_id(ids);
		}
		public static FranchisingSelectBuild SelectByRentsublet(params RentsubletInfo[] items) {
			return Select.WhereRentsublet(items);
		}
		public static FranchisingSelectBuild SelectByRentsublet_id(params uint[] ids) {
			return Select.WhereRentsublet_id(ids);
		}
		public static FranchisingSelectBuild SelectByShop(params ShopInfo[] items) {
			return Select.WhereShop(items);
		}
		public static FranchisingSelectBuild SelectByShop_id(params uint[] ids) {
			return Select.WhereShop_id(ids);
		}
	}
	public partial class FranchisingSelectBuild : SelectBuild<FranchisingInfo, FranchisingSelectBuild> {
		public FranchisingSelectBuild WhereFactory(params FactoryInfo[] items) {
			if (items == null) return this;
			return WhereFactory_id(items.Where<FactoryInfo>(a => a != null).Select<FactoryInfo, uint>(a => a.Id.Value).ToArray());
		}
		public FranchisingSelectBuild WhereFactory_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `franchising_id` FROM `factory_franchising` WHERE `franchising_id` = a.`id` AND `factory_id` IN ({0}) )", string.Join<uint>(",", ids))) as FranchisingSelectBuild;
		}
		public FranchisingSelectBuild WhereRentsublet(params RentsubletInfo[] items) {
			if (items == null) return this;
			return WhereRentsublet_id(items.Where<RentsubletInfo>(a => a != null).Select<RentsubletInfo, uint>(a => a.Id.Value).ToArray());
		}
		public FranchisingSelectBuild WhereRentsublet_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `franchising_id` FROM `rentsublet_franchising` WHERE `franchising_id` = a.`id` AND `rentsublet_id` IN ({0}) )", string.Join<uint>(",", ids))) as FranchisingSelectBuild;
		}
		public FranchisingSelectBuild WhereShop(params ShopInfo[] items) {
			if (items == null) return this;
			return WhereShop_id(items.Where<ShopInfo>(a => a != null).Select<ShopInfo, uint>(a => a.Id.Value).ToArray());
		}
		public FranchisingSelectBuild WhereShop_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `franchising_id` FROM `shop_franchising` WHERE `franchising_id` = a.`id` AND `shop_id` IN ({0}) )", string.Join<uint>(",", ids))) as FranchisingSelectBuild;
		}
		public FranchisingSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public FranchisingSelectBuild WhereTitle(params string[] Title) {
			return this.Where1Or("a.`title` = {0}", Title);
		}
		public FranchisingSelectBuild WhereTitleLike(params string[] Title) {
			if (Title == null || Title.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`title` LIKE {0}", Title.Select(a => "%" + a + "%").ToArray());
		}
		protected new FranchisingSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as FranchisingSelectBuild;
		}
		public FranchisingSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}