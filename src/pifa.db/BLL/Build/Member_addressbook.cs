using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Member_addressbook {

		protected static readonly pifa.DAL.Member_addressbook dal = new pifa.DAL.Member_addressbook();
		protected static readonly int itemCacheTimeout;

		static Member_addressbook() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Member_addressbook"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByMember_id(uint? Member_id) {
			return dal.DeleteByMember_id(Member_id);
		}

		public static int Update(Member_addressbookInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Member_addressbook.SqlUpdateBuild UpdateDiy(uint Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Member_addressbook.SqlUpdateBuild UpdateDiy(Member_addressbookInfo item, uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Member_addressbook.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Member_addressbook.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Member_addressbook.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Member_addressbook.Insert(Member_addressbookInfo item)
		/// </summary>
		[Obsolete]
		public static Member_addressbookInfo Insert(uint? Member_id, string Address, DateTime? Create_time, bool? Is_default, string Name, string Tel, string Telphone, string Zip) {
			return Insert(new Member_addressbookInfo {
				Member_id = Member_id, 
				Address = Address, 
				Create_time = Create_time, 
				Is_default = Is_default, 
				Name = Name, 
				Tel = Tel, 
				Telphone = Telphone, 
				Zip = Zip});
		}
		public static Member_addressbookInfo Insert(Member_addressbookInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Member_addressbookInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Member_addressbook_", item.Id));
		}
		#endregion

		public static Member_addressbookInfo GetItem(uint Id) {
			if (itemCacheTimeout <= 0) return Select.WhereId(Id).ToOne();
			string key = string.Concat("pifa_BLL_Member_addressbook_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return Member_addressbookInfo.Parse(value); } catch { }
			Member_addressbookInfo item = Select.WhereId(Id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Member_addressbookInfo> GetItems() {
			return Select.ToList();
		}
		public static Member_addressbookSelectBuild Select {
			get { return new Member_addressbookSelectBuild(dal); }
		}
		public static List<Member_addressbookInfo> GetItemsByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id).ToList();
		}
		public static List<Member_addressbookInfo> GetItemsByMember_id(uint?[] Member_id, int limit) {
			return Select.WhereMember_id(Member_id).Limit(limit).ToList();
		}
		public static Member_addressbookSelectBuild SelectByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id);
		}
	}
	public partial class Member_addressbookSelectBuild : SelectBuild<Member_addressbookInfo, Member_addressbookSelectBuild> {
		public Member_addressbookSelectBuild WhereMember_id(params uint?[] Member_id) {
			return this.Where1Or("a.`member_id` = {0}", Member_id);
		}
		public Member_addressbookSelectBuild WhereId(params uint[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public Member_addressbookSelectBuild WhereAddress(params string[] Address) {
			return this.Where1Or("a.`address` = {0}", Address);
		}
		public Member_addressbookSelectBuild WhereAddressLike(params string[] Address) {
			if (Address == null || Address.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`address` LIKE {0}", Address.Select(a => "%" + a + "%").ToArray());
		}
		public Member_addressbookSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as Member_addressbookSelectBuild;
		}
		public Member_addressbookSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as Member_addressbookSelectBuild;
		}
		public Member_addressbookSelectBuild WhereIs_default(params bool?[] Is_default) {
			return this.Where1Or("a.`is_default` = {0}", Is_default);
		}
		public Member_addressbookSelectBuild WhereName(params string[] Name) {
			return this.Where1Or("a.`name` = {0}", Name);
		}
		public Member_addressbookSelectBuild WhereNameLike(params string[] Name) {
			if (Name == null || Name.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`name` LIKE {0}", Name.Select(a => "%" + a + "%").ToArray());
		}
		public Member_addressbookSelectBuild WhereTel(params string[] Tel) {
			return this.Where1Or("a.`tel` = {0}", Tel);
		}
		public Member_addressbookSelectBuild WhereTelLike(params string[] Tel) {
			if (Tel == null || Tel.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`tel` LIKE {0}", Tel.Select(a => "%" + a + "%").ToArray());
		}
		public Member_addressbookSelectBuild WhereTelphone(params string[] Telphone) {
			return this.Where1Or("a.`telphone` = {0}", Telphone);
		}
		public Member_addressbookSelectBuild WhereTelphoneLike(params string[] Telphone) {
			if (Telphone == null || Telphone.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`telphone` LIKE {0}", Telphone.Select(a => "%" + a + "%").ToArray());
		}
		public Member_addressbookSelectBuild WhereZip(params string[] Zip) {
			return this.Where1Or("a.`zip` = {0}", Zip);
		}
		public Member_addressbookSelectBuild WhereZipLike(params string[] Zip) {
			if (Zip == null || Zip.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`zip` LIKE {0}", Zip.Select(a => "%" + a + "%").ToArray());
		}
		protected new Member_addressbookSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Member_addressbookSelectBuild;
		}
		public Member_addressbookSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}