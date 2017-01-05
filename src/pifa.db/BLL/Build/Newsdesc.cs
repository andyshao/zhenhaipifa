using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Newsdesc {

		protected static readonly pifa.DAL.Newsdesc dal = new pifa.DAL.Newsdesc();
		protected static readonly int itemCacheTimeout;

		static Newsdesc() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Newsdesc"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint News_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(News_id));
			return dal.Delete(News_id);
		}
		public static int DeleteByNews_id(uint? News_id) {
			return dal.DeleteByNews_id(News_id);
		}

		public static int Update(NewsdescInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Newsdesc.SqlUpdateBuild UpdateDiy(uint News_id) {
			return UpdateDiy(null, News_id);
		}
		public static pifa.DAL.Newsdesc.SqlUpdateBuild UpdateDiy(NewsdescInfo item, uint News_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(News_id));
			return new pifa.DAL.Newsdesc.SqlUpdateBuild(item, News_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Newsdesc.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Newsdesc.SqlUpdateBuild(); }
		}

		public static NewsdescInfo Insert(uint? News_id, string Content) {
			return Insert(new NewsdescInfo {
				News_id = News_id, 
				Content = Content});
		}
		public static NewsdescInfo Insert(NewsdescInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(NewsdescInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Newsdesc_", item.News_id));
		}
		#endregion

		public static NewsdescInfo GetItem(uint News_id) {
			if (itemCacheTimeout <= 0) return Select.WhereNews_id(News_id).ToOne();
			string key = string.Concat("pifa_BLL_Newsdesc_", News_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return NewsdescInfo.Parse(value); } catch { }
			NewsdescInfo item = Select.WhereNews_id(News_id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<NewsdescInfo> GetItems() {
			return Select.ToList();
		}
		public static NewsdescSelectBuild Select {
			get { return new NewsdescSelectBuild(dal); }
		}
		public static List<NewsdescInfo> GetItemsByNews_id(params uint?[] News_id) {
			return Select.WhereNews_id(News_id).ToList();
		}
		public static List<NewsdescInfo> GetItemsByNews_id(uint?[] News_id, int limit) {
			return Select.WhereNews_id(News_id).Limit(limit).ToList();
		}
		public static NewsdescSelectBuild SelectByNews_id(params uint?[] News_id) {
			return Select.WhereNews_id(News_id);
		}
	}
	public partial class NewsdescSelectBuild : SelectBuild<NewsdescInfo, NewsdescSelectBuild> {
		public NewsdescSelectBuild WhereNews_id(params uint?[] News_id) {
			return this.Where1Or("a.`news_id` = {0}", News_id);
		}
		public NewsdescSelectBuild WhereContentLike(params string[] Content) {
			if (Content == null || Content.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`content` LIKE {0}", Content.Select(a => "%" + a + "%").ToArray());
		}
		protected new NewsdescSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as NewsdescSelectBuild;
		}
		public NewsdescSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}