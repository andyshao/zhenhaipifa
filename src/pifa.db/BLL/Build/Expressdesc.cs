using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Expressdesc {

		protected static readonly pifa.DAL.Expressdesc dal = new pifa.DAL.Expressdesc();
		protected static readonly int itemCacheTimeout;

		static Expressdesc() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Expressdesc"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Express_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Express_id));
			return dal.Delete(Express_id);
		}

		public static int Update(ExpressdescInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Expressdesc.SqlUpdateBuild UpdateDiy(uint? Express_id) {
			return UpdateDiy(null, Express_id);
		}
		public static pifa.DAL.Expressdesc.SqlUpdateBuild UpdateDiy(ExpressdescInfo item, uint? Express_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Express_id));
			return new pifa.DAL.Expressdesc.SqlUpdateBuild(item, Express_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Expressdesc.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Expressdesc.SqlUpdateBuild(); }
		}

		public static ExpressdescInfo Insert(uint? Express_id, string Content) {
			return Insert(new ExpressdescInfo {
				Express_id = Express_id, 
				Content = Content});
		}
		public static ExpressdescInfo Insert(ExpressdescInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(ExpressdescInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Expressdesc_", item.Express_id));
		}
		#endregion

		public static ExpressdescInfo GetItem(uint? Express_id) {
			if (Express_id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Express_id);
			string key = string.Concat("pifa_BLL_Expressdesc_", Express_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new ExpressdescInfo(value); } catch { }
			ExpressdescInfo item = dal.GetItem(Express_id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<ExpressdescInfo> GetItems() {
			return Select.ToList();
		}
		public static ExpressdescSelectBuild Select {
			get { return new ExpressdescSelectBuild(dal); }
		}
		public static List<ExpressdescInfo> GetItemsByExpress_id(params uint?[] Express_id) {
			return Select.WhereExpress_id(Express_id).ToList();
		}
		public static List<ExpressdescInfo> GetItemsByExpress_id(uint?[] Express_id, int limit) {
			return Select.WhereExpress_id(Express_id).Limit(limit).ToList();
		}
		public static ExpressdescSelectBuild SelectByExpress_id(params uint?[] Express_id) {
			return Select.WhereExpress_id(Express_id);
		}
	}
	public partial class ExpressdescSelectBuild : SelectBuild<ExpressdescInfo, ExpressdescSelectBuild> {
		public ExpressdescSelectBuild WhereExpress_id(params uint?[] Express_id) {
			return this.Where1Or("a.`Express_id` = {0}", Express_id);
		}
		public ExpressdescSelectBuild WhereContentLike(params string[] Content) {
			if (Content == null) return this;
			return this.Where1Or(@"a.`content` LIKE {0}", Content.Select(a => "%" + a + "%").ToArray());
		}
		protected new ExpressdescSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as ExpressdescSelectBuild;
		}
		public ExpressdescSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}