using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class News_newstag {

		protected static readonly pifa.DAL.News_newstag dal = new pifa.DAL.News_newstag();
		protected static readonly int itemCacheTimeout;

		static News_newstag() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_News_newstag"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? News_id, uint? Newstag_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(News_id, Newstag_id));
			return dal.Delete(News_id, Newstag_id);
		}
		public static int DeleteByNewstag_id(uint? Newstag_id) {
			return dal.DeleteByNewstag_id(Newstag_id);
		}
		public static int DeleteByNews_id(uint? News_id) {
			return dal.DeleteByNews_id(News_id);
		}

		public static int Update(News_newstagInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.News_newstag.SqlUpdateBuild UpdateDiy(uint? News_id, uint? Newstag_id) {
			return UpdateDiy(null, News_id, Newstag_id);
		}
		public static pifa.DAL.News_newstag.SqlUpdateBuild UpdateDiy(News_newstagInfo item, uint? News_id, uint? Newstag_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(News_id, Newstag_id));
			return new pifa.DAL.News_newstag.SqlUpdateBuild(item, News_id, Newstag_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.News_newstag.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.News_newstag.SqlUpdateBuild(); }
		}

		public static News_newstagInfo Insert(uint? News_id, uint? Newstag_id) {
			return Insert(new News_newstagInfo {
				News_id = News_id, 
				Newstag_id = Newstag_id});
		}
		public static News_newstagInfo Insert(News_newstagInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(News_newstagInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_News_newstag_", item.News_id, "_,_", item.Newstag_id));
		}
		#endregion

		public static News_newstagInfo GetItem(uint? News_id, uint? Newstag_id) {
			if (News_id == null || Newstag_id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(News_id, Newstag_id);
			string key = string.Concat("pifa_BLL_News_newstag_", News_id, "_,_", Newstag_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new News_newstagInfo(value); } catch { }
			News_newstagInfo item = dal.GetItem(News_id, Newstag_id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<News_newstagInfo> GetItems() {
			return Select.ToList();
		}
		public static News_newstagSelectBuild Select {
			get { return new News_newstagSelectBuild(dal); }
		}
		public static List<News_newstagInfo> GetItemsByNewstag_id(params uint?[] Newstag_id) {
			return Select.WhereNewstag_id(Newstag_id).ToList();
		}
		public static List<News_newstagInfo> GetItemsByNewstag_id(uint?[] Newstag_id, int limit) {
			return Select.WhereNewstag_id(Newstag_id).Limit(limit).ToList();
		}
		public static News_newstagSelectBuild SelectByNewstag_id(params uint?[] Newstag_id) {
			return Select.WhereNewstag_id(Newstag_id);
		}
		public static List<News_newstagInfo> GetItemsByNews_id(params uint?[] News_id) {
			return Select.WhereNews_id(News_id).ToList();
		}
		public static List<News_newstagInfo> GetItemsByNews_id(uint?[] News_id, int limit) {
			return Select.WhereNews_id(News_id).Limit(limit).ToList();
		}
		public static News_newstagSelectBuild SelectByNews_id(params uint?[] News_id) {
			return Select.WhereNews_id(News_id);
		}
	}
	public partial class News_newstagSelectBuild : SelectBuild<News_newstagInfo, News_newstagSelectBuild> {
		public News_newstagSelectBuild WhereNewstag_id(params uint?[] Newstag_id) {
			return this.Where1Or("a.`Newstag_id` = {0}", Newstag_id);
		}
		public News_newstagSelectBuild WhereNews_id(params uint?[] News_id) {
			return this.Where1Or("a.`News_id` = {0}", News_id);
		}
		protected new News_newstagSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as News_newstagSelectBuild;
		}
		public News_newstagSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}