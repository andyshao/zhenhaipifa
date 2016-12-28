using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Faq {

		protected static readonly pifa.DAL.Faq dal = new pifa.DAL.Faq();
		protected static readonly int itemCacheTimeout;

		static Faq() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Faq"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByFaqtype_id(uint? Faqtype_id) {
			return dal.DeleteByFaqtype_id(Faqtype_id);
		}

		public static int Update(FaqInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Faq.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Faq.SqlUpdateBuild UpdateDiy(FaqInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Faq.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Faq.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Faq.SqlUpdateBuild(); }
		}

		public static FaqInfo Insert(uint? Faqtype_id, DateTime? Create_time, string Title) {
			return Insert(new FaqInfo {
				Faqtype_id = Faqtype_id, 
				Create_time = Create_time, 
				Title = Title});
		}
		public static FaqInfo Insert(FaqInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(FaqInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Faq_", item.Id));
		}
		#endregion

		public static FaqInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("pifa_BLL_Faq_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new FaqInfo(value); } catch { }
			FaqInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<FaqInfo> GetItems() {
			return Select.ToList();
		}
		public static FaqSelectBuild Select {
			get { return new FaqSelectBuild(dal); }
		}
		public static List<FaqInfo> GetItemsByFaqtype_id(params uint?[] Faqtype_id) {
			return Select.WhereFaqtype_id(Faqtype_id).ToList();
		}
		public static List<FaqInfo> GetItemsByFaqtype_id(uint?[] Faqtype_id, int limit) {
			return Select.WhereFaqtype_id(Faqtype_id).Limit(limit).ToList();
		}
		public static FaqSelectBuild SelectByFaqtype_id(params uint?[] Faqtype_id) {
			return Select.WhereFaqtype_id(Faqtype_id);
		}
	}
	public partial class FaqSelectBuild : SelectBuild<FaqInfo, FaqSelectBuild> {
		public FaqSelectBuild WhereFaqtype_id(params uint?[] Faqtype_id) {
			return this.Where1Or("a.`Faqtype_id` = {0}", Faqtype_id);
		}
		public FaqSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public FaqSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as FaqSelectBuild;
		}
		public FaqSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as FaqSelectBuild;
		}
		public FaqSelectBuild WhereTitle(params string[] Title) {
			return this.Where1Or("a.`title` = {0}", Title);
		}
		public FaqSelectBuild WhereTitleLike(params string[] Title) {
			if (Title == null) return this;
			return this.Where1Or(@"a.`title` LIKE {0}", Title.Select(a => "%" + a + "%").ToArray());
		}
		protected new FaqSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as FaqSelectBuild;
		}
		public FaqSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}