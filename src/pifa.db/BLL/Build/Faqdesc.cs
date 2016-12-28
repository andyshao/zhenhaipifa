using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Faqdesc {

		protected static readonly pifa.DAL.Faqdesc dal = new pifa.DAL.Faqdesc();
		protected static readonly int itemCacheTimeout;

		static Faqdesc() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Faqdesc"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Faq_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Faq_id));
			return dal.Delete(Faq_id);
		}

		public static int Update(FaqdescInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Faqdesc.SqlUpdateBuild UpdateDiy(uint? Faq_id) {
			return UpdateDiy(null, Faq_id);
		}
		public static pifa.DAL.Faqdesc.SqlUpdateBuild UpdateDiy(FaqdescInfo item, uint? Faq_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Faq_id));
			return new pifa.DAL.Faqdesc.SqlUpdateBuild(item, Faq_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Faqdesc.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Faqdesc.SqlUpdateBuild(); }
		}

		public static FaqdescInfo Insert(uint? Faq_id, string Content) {
			return Insert(new FaqdescInfo {
				Faq_id = Faq_id, 
				Content = Content});
		}
		public static FaqdescInfo Insert(FaqdescInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(FaqdescInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Faqdesc_", item.Faq_id));
		}
		#endregion

		public static FaqdescInfo GetItem(uint? Faq_id) {
			if (Faq_id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Faq_id);
			string key = string.Concat("pifa_BLL_Faqdesc_", Faq_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new FaqdescInfo(value); } catch { }
			FaqdescInfo item = dal.GetItem(Faq_id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<FaqdescInfo> GetItems() {
			return Select.ToList();
		}
		public static FaqdescSelectBuild Select {
			get { return new FaqdescSelectBuild(dal); }
		}
		public static List<FaqdescInfo> GetItemsByFaq_id(params uint?[] Faq_id) {
			return Select.WhereFaq_id(Faq_id).ToList();
		}
		public static List<FaqdescInfo> GetItemsByFaq_id(uint?[] Faq_id, int limit) {
			return Select.WhereFaq_id(Faq_id).Limit(limit).ToList();
		}
		public static FaqdescSelectBuild SelectByFaq_id(params uint?[] Faq_id) {
			return Select.WhereFaq_id(Faq_id);
		}
	}
	public partial class FaqdescSelectBuild : SelectBuild<FaqdescInfo, FaqdescSelectBuild> {
		public FaqdescSelectBuild WhereFaq_id(params uint?[] Faq_id) {
			return this.Where1Or("a.`Faq_id` = {0}", Faq_id);
		}
		public FaqdescSelectBuild WhereContentLike(params string[] Content) {
			if (Content == null) return this;
			return this.Where1Or(@"a.`content` LIKE {0}", Content.Select(a => "%" + a + "%").ToArray());
		}
		protected new FaqdescSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as FaqdescSelectBuild;
		}
		public FaqdescSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}