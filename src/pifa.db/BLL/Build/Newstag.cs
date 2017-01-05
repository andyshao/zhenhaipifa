using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Newstag {

		protected static readonly pifa.DAL.Newstag dal = new pifa.DAL.Newstag();
		protected static readonly int itemCacheTimeout;

		static Newstag() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Newstag"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}

		public static int Update(NewstagInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Newstag.SqlUpdateBuild UpdateDiy(uint Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Newstag.SqlUpdateBuild UpdateDiy(NewstagInfo item, uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Newstag.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Newstag.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Newstag.SqlUpdateBuild(); }
		}

		public static NewstagInfo Insert(DateTime? Create_time, string Name, uint? Total_news) {
			return Insert(new NewstagInfo {
				Create_time = Create_time, 
				Name = Name, 
				Total_news = Total_news});
		}
		public static NewstagInfo Insert(NewstagInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(NewstagInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Newstag_", item.Id));
		}
		#endregion

		public static NewstagInfo GetItem(uint Id) {
			if (itemCacheTimeout <= 0) return Select.WhereId(Id).ToOne();
			string key = string.Concat("pifa_BLL_Newstag_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return NewstagInfo.Parse(value); } catch { }
			NewstagInfo item = Select.WhereId(Id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<NewstagInfo> GetItems() {
			return Select.ToList();
		}
		public static NewstagSelectBuild Select {
			get { return new NewstagSelectBuild(dal); }
		}
		public static NewstagSelectBuild SelectByNews(params NewsInfo[] items) {
			return Select.WhereNews(items);
		}
		public static NewstagSelectBuild SelectByNews_id(params uint[] ids) {
			return Select.WhereNews_id(ids);
		}
	}
	public partial class NewstagSelectBuild : SelectBuild<NewstagInfo, NewstagSelectBuild> {
		public NewstagSelectBuild WhereNews(params NewsInfo[] items) {
			if (items == null) return this;
			return WhereNews_id(items.Where<NewsInfo>(a => a != null).Select<NewsInfo, uint>(a => a.Id.Value).ToArray());
		}
		public NewstagSelectBuild WhereNews_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `newstag_id` FROM `news_newstag` WHERE `newstag_id` = a.`id` AND `news_id` IN ({0}) )", string.Join<uint>(",", ids))) as NewstagSelectBuild;
		}
		public NewstagSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public NewstagSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as NewstagSelectBuild;
		}
		public NewstagSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as NewstagSelectBuild;
		}
		public NewstagSelectBuild WhereName(params string[] Name) {
			return this.Where1Or("a.`name` = {0}", Name);
		}
		public NewstagSelectBuild WhereNameLike(params string[] Name) {
			if (Name == null || Name.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`name` LIKE {0}", Name.Select(a => "%" + a + "%").ToArray());
		}
		public NewstagSelectBuild WhereTotal_news(params uint?[] Total_news) {
			return this.Where1Or("a.`total_news` = {0}", Total_news);
		}
		protected new NewstagSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as NewstagSelectBuild;
		}
		public NewstagSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}