using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Product_question {

		protected static readonly pifa.DAL.Product_question dal = new pifa.DAL.Product_question();
		protected static readonly int itemCacheTimeout;

		static Product_question() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Product_question"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByMember_id(uint? Member_id) {
			return dal.DeleteByMember_id(Member_id);
		}
		public static int DeleteByProduct_id(uint? Product_id) {
			return dal.DeleteByProduct_id(Product_id);
		}
		public static int DeleteByParent_id(uint? Parent_id) {
			return dal.DeleteByParent_id(Parent_id);
		}

		public static int Update(Product_questionInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Product_question.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Product_question.SqlUpdateBuild UpdateDiy(Product_questionInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Product_question.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Product_question.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Product_question.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Product_question.Insert(Product_questionInfo item)
		/// </summary>
		[Obsolete]
		public static Product_questionInfo Insert(uint? Member_id, uint? Parent_id, uint? Product_id, string Content, DateTime? Create_time, string Email, string Name, Product_questionSTATE? State) {
			return Insert(new Product_questionInfo {
				Member_id = Member_id, 
				Parent_id = Parent_id, 
				Product_id = Product_id, 
				Content = Content, 
				Create_time = Create_time, 
				Email = Email, 
				Name = Name, 
				State = State});
		}
		public static Product_questionInfo Insert(Product_questionInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Product_questionInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Product_question_", item.Id));
		}
		#endregion

		public static Product_questionInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("pifa_BLL_Product_question_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new Product_questionInfo(value); } catch { }
			Product_questionInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Product_questionInfo> GetItems() {
			return Select.ToList();
		}
		public static Product_questionSelectBuild Select {
			get { return new Product_questionSelectBuild(dal); }
		}
		public static List<Product_questionInfo> GetItemsByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id).ToList();
		}
		public static List<Product_questionInfo> GetItemsByMember_id(uint?[] Member_id, int limit) {
			return Select.WhereMember_id(Member_id).Limit(limit).ToList();
		}
		public static Product_questionSelectBuild SelectByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id);
		}
		public static List<Product_questionInfo> GetItemsByProduct_id(params uint?[] Product_id) {
			return Select.WhereProduct_id(Product_id).ToList();
		}
		public static List<Product_questionInfo> GetItemsByProduct_id(uint?[] Product_id, int limit) {
			return Select.WhereProduct_id(Product_id).Limit(limit).ToList();
		}
		public static Product_questionSelectBuild SelectByProduct_id(params uint?[] Product_id) {
			return Select.WhereProduct_id(Product_id);
		}
		public static List<Product_questionInfo> GetItemsByParent_id(params uint?[] Parent_id) {
			return Select.WhereParent_id(Parent_id).ToList();
		}
		public static List<Product_questionInfo> GetItemsByParent_id(uint?[] Parent_id, int limit) {
			return Select.WhereParent_id(Parent_id).Limit(limit).ToList();
		}
		public static Product_questionSelectBuild SelectByParent_id(params uint?[] Parent_id) {
			return Select.WhereParent_id(Parent_id);
		}
	}
	public partial class Product_questionSelectBuild : SelectBuild<Product_questionInfo, Product_questionSelectBuild> {
		public Product_questionSelectBuild WhereMember_id(params uint?[] Member_id) {
			return this.Where1Or("a.`Member_id` = {0}", Member_id);
		}
		public Product_questionSelectBuild WhereProduct_id(params uint?[] Product_id) {
			return this.Where1Or("a.`Product_id` = {0}", Product_id);
		}
		public Product_questionSelectBuild WhereParent_id(params uint?[] Parent_id) {
			return this.Where1Or("a.`Parent_id` = {0}", Parent_id);
		}
		public Product_questionSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public Product_questionSelectBuild WhereContent(params string[] Content) {
			return this.Where1Or("a.`content` = {0}", Content);
		}
		public Product_questionSelectBuild WhereContentLike(params string[] Content) {
			if (Content == null) return this;
			return this.Where1Or(@"a.`content` LIKE {0}", Content.Select(a => "%" + a + "%").ToArray());
		}
		public Product_questionSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as Product_questionSelectBuild;
		}
		public Product_questionSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as Product_questionSelectBuild;
		}
		public Product_questionSelectBuild WhereEmail(params string[] Email) {
			return this.Where1Or("a.`email` = {0}", Email);
		}
		public Product_questionSelectBuild WhereEmailLike(params string[] Email) {
			if (Email == null) return this;
			return this.Where1Or(@"a.`email` LIKE {0}", Email.Select(a => "%" + a + "%").ToArray());
		}
		public Product_questionSelectBuild WhereName(params string[] Name) {
			return this.Where1Or("a.`name` = {0}", Name);
		}
		public Product_questionSelectBuild WhereNameLike(params string[] Name) {
			if (Name == null) return this;
			return this.Where1Or(@"a.`name` LIKE {0}", Name.Select(a => "%" + a + "%").ToArray());
		}
		public Product_questionSelectBuild WhereState_IN(params Product_questionSTATE?[] States) {
			return this.Where1Or("a.`state` = {0}", States);
		}
		public Product_questionSelectBuild WhereState(Product_questionSTATE? State1) {
			return this.WhereState_IN(State1);
		}
		#region WhereState
		public Product_questionSelectBuild WhereState(Product_questionSTATE? State1, Product_questionSTATE? State2) {
			return this.WhereState_IN(State1, State2);
		}
		public Product_questionSelectBuild WhereState(Product_questionSTATE? State1, Product_questionSTATE? State2, Product_questionSTATE? State3) {
			return this.WhereState_IN(State1, State2, State3);
		}
		public Product_questionSelectBuild WhereState(Product_questionSTATE? State1, Product_questionSTATE? State2, Product_questionSTATE? State3, Product_questionSTATE? State4) {
			return this.WhereState_IN(State1, State2, State3, State4);
		}
		public Product_questionSelectBuild WhereState(Product_questionSTATE? State1, Product_questionSTATE? State2, Product_questionSTATE? State3, Product_questionSTATE? State4, Product_questionSTATE? State5) {
			return this.WhereState_IN(State1, State2, State3, State4, State5);
		}
		#endregion
		protected new Product_questionSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Product_questionSelectBuild;
		}
		public Product_questionSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}