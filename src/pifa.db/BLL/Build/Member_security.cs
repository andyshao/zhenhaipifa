using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Member_security {

		protected static readonly pifa.DAL.Member_security dal = new pifa.DAL.Member_security();
		protected static readonly int itemCacheTimeout;

		static Member_security() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Member_security"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Member_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Member_id));
			return dal.Delete(Member_id);
		}
		public static int DeleteByMember_id(uint? Member_id) {
			return dal.DeleteByMember_id(Member_id);
		}

		public static int Update(Member_securityInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Member_security.SqlUpdateBuild UpdateDiy(uint Member_id) {
			return UpdateDiy(null, Member_id);
		}
		public static pifa.DAL.Member_security.SqlUpdateBuild UpdateDiy(Member_securityInfo item, uint Member_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Member_id));
			return new pifa.DAL.Member_security.SqlUpdateBuild(item, Member_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Member_security.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Member_security.SqlUpdateBuild(); }
		}

		public static Member_securityInfo Insert(uint? Member_id, string Password) {
			return Insert(new Member_securityInfo {
				Member_id = Member_id, 
				Password = Password});
		}
		public static Member_securityInfo Insert(Member_securityInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Member_securityInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Member_security_", item.Member_id));
		}
		#endregion

		public static Member_securityInfo GetItem(uint Member_id) {
			if (itemCacheTimeout <= 0) return Select.WhereMember_id(Member_id).ToOne();
			string key = string.Concat("pifa_BLL_Member_security_", Member_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return Member_securityInfo.Parse(value); } catch { }
			Member_securityInfo item = Select.WhereMember_id(Member_id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Member_securityInfo> GetItems() {
			return Select.ToList();
		}
		public static Member_securitySelectBuild Select {
			get { return new Member_securitySelectBuild(dal); }
		}
		public static List<Member_securityInfo> GetItemsByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id).ToList();
		}
		public static List<Member_securityInfo> GetItemsByMember_id(uint?[] Member_id, int limit) {
			return Select.WhereMember_id(Member_id).Limit(limit).ToList();
		}
		public static Member_securitySelectBuild SelectByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id);
		}
	}
	public partial class Member_securitySelectBuild : SelectBuild<Member_securityInfo, Member_securitySelectBuild> {
		public Member_securitySelectBuild WhereMember_id(params uint?[] Member_id) {
			return this.Where1Or("a.`member_id` = {0}", Member_id);
		}
		public Member_securitySelectBuild WherePassword(params string[] Password) {
			return this.Where1Or("a.`password` = {0}", Password);
		}
		public Member_securitySelectBuild WherePasswordLike(params string[] Password) {
			if (Password == null || Password.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`password` LIKE {0}", Password.Select(a => "%" + a + "%").ToArray());
		}
		protected new Member_securitySelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Member_securitySelectBuild;
		}
		public Member_securitySelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}