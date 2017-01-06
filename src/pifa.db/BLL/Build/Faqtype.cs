using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Faqtype {

		protected static readonly pifa.DAL.Faqtype dal = new pifa.DAL.Faqtype();
		protected static readonly int itemCacheTimeout;

		static Faqtype() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Faqtype"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}

		public static int Update(FaqtypeInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Faqtype.SqlUpdateBuild UpdateDiy(uint Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Faqtype.SqlUpdateBuild UpdateDiy(FaqtypeInfo item, uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Faqtype.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Faqtype.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Faqtype.SqlUpdateBuild(); }
		}

		public static FaqtypeInfo Insert(byte? Sort, string Title) {
			return Insert(new FaqtypeInfo {
				Sort = Sort, 
				Title = Title});
		}
		public static FaqtypeInfo Insert(FaqtypeInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(FaqtypeInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Faqtype_", item.Id));
		}
		#endregion

		public static FaqtypeInfo GetItem(uint Id) {
			if (itemCacheTimeout <= 0) return Select.WhereId(Id).ToOne();
			string key = string.Concat("pifa_BLL_Faqtype_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return FaqtypeInfo.Parse(value); } catch { }
			FaqtypeInfo item = Select.WhereId(Id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<FaqtypeInfo> GetItems() {
			return Select.ToList();
		}
		public static FaqtypeSelectBuild Select {
			get { return new FaqtypeSelectBuild(dal); }
		}
	}
	public partial class FaqtypeSelectBuild : SelectBuild<FaqtypeInfo, FaqtypeSelectBuild> {
		public FaqtypeSelectBuild WhereId(params uint[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public FaqtypeSelectBuild WhereSort(params byte?[] Sort) {
			return this.Where1Or("a.`sort` = {0}", Sort);
		}
		public FaqtypeSelectBuild WhereTitle(params string[] Title) {
			return this.Where1Or("a.`title` = {0}", Title);
		}
		public FaqtypeSelectBuild WhereTitleLike(params string[] Title) {
			if (Title == null || Title.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`title` LIKE {0}", Title.Select(a => "%" + a + "%").ToArray());
		}
		protected new FaqtypeSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as FaqtypeSelectBuild;
		}
		public FaqtypeSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}