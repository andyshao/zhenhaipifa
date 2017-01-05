using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Factorydesc {

		protected static readonly pifa.DAL.Factorydesc dal = new pifa.DAL.Factorydesc();
		protected static readonly int itemCacheTimeout;

		static Factorydesc() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Factorydesc"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Factory_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Factory_id));
			return dal.Delete(Factory_id);
		}
		public static int DeleteByFactory_id(uint? Factory_id) {
			return dal.DeleteByFactory_id(Factory_id);
		}

		public static int Update(FactorydescInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Factorydesc.SqlUpdateBuild UpdateDiy(uint Factory_id) {
			return UpdateDiy(null, Factory_id);
		}
		public static pifa.DAL.Factorydesc.SqlUpdateBuild UpdateDiy(FactorydescInfo item, uint Factory_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Factory_id));
			return new pifa.DAL.Factorydesc.SqlUpdateBuild(item, Factory_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Factorydesc.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Factorydesc.SqlUpdateBuild(); }
		}

		public static FactorydescInfo Insert(uint? Factory_id, string Address, string Content, string Url, string Username) {
			return Insert(new FactorydescInfo {
				Factory_id = Factory_id, 
				Address = Address, 
				Content = Content, 
				Url = Url, 
				Username = Username});
		}
		public static FactorydescInfo Insert(FactorydescInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(FactorydescInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Factorydesc_", item.Factory_id));
		}
		#endregion

		public static FactorydescInfo GetItem(uint Factory_id) {
			if (itemCacheTimeout <= 0) return Select.WhereFactory_id(Factory_id).ToOne();
			string key = string.Concat("pifa_BLL_Factorydesc_", Factory_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return FactorydescInfo.Parse(value); } catch { }
			FactorydescInfo item = Select.WhereFactory_id(Factory_id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<FactorydescInfo> GetItems() {
			return Select.ToList();
		}
		public static FactorydescSelectBuild Select {
			get { return new FactorydescSelectBuild(dal); }
		}
		public static List<FactorydescInfo> GetItemsByFactory_id(params uint?[] Factory_id) {
			return Select.WhereFactory_id(Factory_id).ToList();
		}
		public static List<FactorydescInfo> GetItemsByFactory_id(uint?[] Factory_id, int limit) {
			return Select.WhereFactory_id(Factory_id).Limit(limit).ToList();
		}
		public static FactorydescSelectBuild SelectByFactory_id(params uint?[] Factory_id) {
			return Select.WhereFactory_id(Factory_id);
		}
	}
	public partial class FactorydescSelectBuild : SelectBuild<FactorydescInfo, FactorydescSelectBuild> {
		public FactorydescSelectBuild WhereFactory_id(params uint?[] Factory_id) {
			return this.Where1Or("a.`factory_id` = {0}", Factory_id);
		}
		public FactorydescSelectBuild WhereAddress(params string[] Address) {
			return this.Where1Or("a.`address` = {0}", Address);
		}
		public FactorydescSelectBuild WhereAddressLike(params string[] Address) {
			if (Address == null || Address.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`address` LIKE {0}", Address.Select(a => "%" + a + "%").ToArray());
		}
		public FactorydescSelectBuild WhereContentLike(params string[] Content) {
			if (Content == null || Content.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`content` LIKE {0}", Content.Select(a => "%" + a + "%").ToArray());
		}
		public FactorydescSelectBuild WhereUrl(params string[] Url) {
			return this.Where1Or("a.`url` = {0}", Url);
		}
		public FactorydescSelectBuild WhereUrlLike(params string[] Url) {
			if (Url == null || Url.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`url` LIKE {0}", Url.Select(a => "%" + a + "%").ToArray());
		}
		public FactorydescSelectBuild WhereUsername(params string[] Username) {
			return this.Where1Or("a.`username` = {0}", Username);
		}
		public FactorydescSelectBuild WhereUsernameLike(params string[] Username) {
			if (Username == null || Username.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`username` LIKE {0}", Username.Select(a => "%" + a + "%").ToArray());
		}
		protected new FactorydescSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as FactorydescSelectBuild;
		}
		public FactorydescSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}