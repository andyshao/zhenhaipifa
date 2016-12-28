using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class News {

		protected static readonly pifa.DAL.News dal = new pifa.DAL.News();
		protected static readonly int itemCacheTimeout;

		static News() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_News"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}

		public static int Update(NewsInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.News.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.News.SqlUpdateBuild UpdateDiy(NewsInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.News.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.News.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.News.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 News.Insert(NewsInfo item)
		/// </summary>
		[Obsolete]
		public static NewsInfo Insert(DateTime? Create_time, string Intro, uint? Pv, string Source, NewsSTATE? State, string Title, DateTime? Update_time) {
			return Insert(new NewsInfo {
				Create_time = Create_time, 
				Intro = Intro, 
				Pv = Pv, 
				Source = Source, 
				State = State, 
				Title = Title, 
				Update_time = Update_time});
		}
		public static NewsInfo Insert(NewsInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(NewsInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_News_", item.Id));
		}
		#endregion

		public static NewsInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("pifa_BLL_News_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new NewsInfo(value); } catch { }
			NewsInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<NewsInfo> GetItems() {
			return Select.ToList();
		}
		public static NewsSelectBuild Select {
			get { return new NewsSelectBuild(dal); }
		}
		public static NewsSelectBuild SelectByNewstag(params NewstagInfo[] items) {
			return Select.WhereNewstag(items);
		}
		public static NewsSelectBuild SelectByNewstag_id(params uint[] ids) {
			return Select.WhereNewstag_id(ids);
		}
	}
	public partial class NewsSelectBuild : SelectBuild<NewsInfo, NewsSelectBuild> {
		public NewsSelectBuild WhereNewstag(params NewstagInfo[] items) {
			if (items == null) return this;
			return WhereNewstag_id(items.Where<NewstagInfo>(a => a != null).Select<NewstagInfo, uint>(a => a.Id.Value).ToArray());
		}
		public NewsSelectBuild WhereNewstag_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `news_id` FROM `news_newstag` WHERE `news_id` = a.`id` AND `newstag_id` IN ({0}) )", string.Join<uint>(",", ids))) as NewsSelectBuild;
		}
		public NewsSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public NewsSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as NewsSelectBuild;
		}
		public NewsSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as NewsSelectBuild;
		}
		public NewsSelectBuild WhereIntroLike(params string[] Intro) {
			if (Intro == null) return this;
			return this.Where1Or(@"a.`intro` LIKE {0}", Intro.Select(a => "%" + a + "%").ToArray());
		}
		public NewsSelectBuild WherePv(params uint?[] Pv) {
			return this.Where1Or("a.`pv` = {0}", Pv);
		}
		public NewsSelectBuild WhereSource(params string[] Source) {
			return this.Where1Or("a.`source` = {0}", Source);
		}
		public NewsSelectBuild WhereSourceLike(params string[] Source) {
			if (Source == null) return this;
			return this.Where1Or(@"a.`source` LIKE {0}", Source.Select(a => "%" + a + "%").ToArray());
		}
		public NewsSelectBuild WhereState_IN(params NewsSTATE?[] States) {
			return this.Where1Or("a.`state` = {0}", States);
		}
		public NewsSelectBuild WhereState(NewsSTATE? State1) {
			return this.WhereState_IN(State1);
		}
		#region WhereState
		public NewsSelectBuild WhereState(NewsSTATE? State1, NewsSTATE? State2) {
			return this.WhereState_IN(State1, State2);
		}
		public NewsSelectBuild WhereState(NewsSTATE? State1, NewsSTATE? State2, NewsSTATE? State3) {
			return this.WhereState_IN(State1, State2, State3);
		}
		public NewsSelectBuild WhereState(NewsSTATE? State1, NewsSTATE? State2, NewsSTATE? State3, NewsSTATE? State4) {
			return this.WhereState_IN(State1, State2, State3, State4);
		}
		public NewsSelectBuild WhereState(NewsSTATE? State1, NewsSTATE? State2, NewsSTATE? State3, NewsSTATE? State4, NewsSTATE? State5) {
			return this.WhereState_IN(State1, State2, State3, State4, State5);
		}
		#endregion
		public NewsSelectBuild WhereTitle(params string[] Title) {
			return this.Where1Or("a.`title` = {0}", Title);
		}
		public NewsSelectBuild WhereTitleLike(params string[] Title) {
			if (Title == null) return this;
			return this.Where1Or(@"a.`title` LIKE {0}", Title.Select(a => "%" + a + "%").ToArray());
		}
		public NewsSelectBuild WhereUpdate_timeRange(DateTime? begin) {
			return base.Where("a.`update_time` >= {0}", begin) as NewsSelectBuild;
		}
		public NewsSelectBuild WhereUpdate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereUpdate_timeRange(begin);
			return base.Where("a.`update_time` between {0} and {1}", begin, end) as NewsSelectBuild;
		}
		protected new NewsSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as NewsSelectBuild;
		}
		public NewsSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}