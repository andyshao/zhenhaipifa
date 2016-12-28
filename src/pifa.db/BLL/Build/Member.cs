using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Member {

		protected static readonly pifa.DAL.Member dal = new pifa.DAL.Member();
		protected static readonly int itemCacheTimeout;

		static Member() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Member"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByUsername(string Username) {
			return dal.DeleteByUsername(Username);
		}
		public static int DeleteByTelphone(string Telphone) {
			return dal.DeleteByTelphone(Telphone);
		}
		public static int DeleteByEmail(string Email) {
			return dal.DeleteByEmail(Email);
		}

		public static int Update(MemberInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Member.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Member.SqlUpdateBuild UpdateDiy(MemberInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Member.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Member.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Member.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Member.Insert(MemberInfo item)
		/// </summary>
		[Obsolete]
		public static MemberInfo Insert(uint? Id, DateTime? Create_time, string Email, DateTime? Lastlogin_time, string Telphone, string Username) {
			return Insert(new MemberInfo {
				Id = Id, 
				Create_time = Create_time, 
				Email = Email, 
				Lastlogin_time = Lastlogin_time, 
				Telphone = Telphone, 
				Username = Username});
		}
		public static MemberInfo Insert(MemberInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(MemberInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Member_", item.Id));
			RedisHelper.Remove(string.Concat("pifa_BLL_MemberByUsername_", item.Username));
			RedisHelper.Remove(string.Concat("pifa_BLL_MemberByTelphone_", item.Telphone));
			RedisHelper.Remove(string.Concat("pifa_BLL_MemberByEmail_", item.Email));
		}
		#endregion

		public static MemberInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("pifa_BLL_Member_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new MemberInfo(value); } catch { }
			MemberInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}
		public static MemberInfo GetItemByUsername(string Username) {
			if (Username == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItemByUsername(Username);
			string key = string.Concat("pifa_BLL_MemberByUsername_", Username);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new MemberInfo(value); } catch { }
			MemberInfo item = dal.GetItemByUsername(Username);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}
		public static MemberInfo GetItemByTelphone(string Telphone) {
			if (Telphone == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItemByTelphone(Telphone);
			string key = string.Concat("pifa_BLL_MemberByTelphone_", Telphone);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new MemberInfo(value); } catch { }
			MemberInfo item = dal.GetItemByTelphone(Telphone);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}
		public static MemberInfo GetItemByEmail(string Email) {
			if (Email == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItemByEmail(Email);
			string key = string.Concat("pifa_BLL_MemberByEmail_", Email);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new MemberInfo(value); } catch { }
			MemberInfo item = dal.GetItemByEmail(Email);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<MemberInfo> GetItems() {
			return Select.ToList();
		}
		public static MemberSelectBuild Select {
			get { return new MemberSelectBuild(dal); }
		}
		public static MemberSelectBuild SelectByMarket(params MarketInfo[] items) {
			return Select.WhereMarket(items);
		}
		public static MemberSelectBuild SelectByMarket_id(params uint[] ids) {
			return Select.WhereMarket_id(ids);
		}
		public static MemberSelectBuild SelectByProduct(params ProductInfo[] items) {
			return Select.WhereProduct(items);
		}
		public static MemberSelectBuild SelectByProduct_id(params uint[] ids) {
			return Select.WhereProduct_id(ids);
		}
		public static MemberSelectBuild SelectByShop(params ShopInfo[] items) {
			return Select.WhereShop(items);
		}
		public static MemberSelectBuild SelectByShop_id(params uint[] ids) {
			return Select.WhereShop_id(ids);
		}
	}
	public partial class MemberSelectBuild : SelectBuild<MemberInfo, MemberSelectBuild> {
		public MemberSelectBuild WhereMarket(params MarketInfo[] items) {
			if (items == null) return this;
			return WhereMarket_id(items.Where<MarketInfo>(a => a != null).Select<MarketInfo, uint>(a => a.Id.Value).ToArray());
		}
		public MemberSelectBuild WhereMarket_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `member_id` FROM `member_market` WHERE `member_id` = a.`id` AND `market_id` IN ({0}) )", string.Join<uint>(",", ids))) as MemberSelectBuild;
		}
		public MemberSelectBuild WhereProduct(params ProductInfo[] items) {
			if (items == null) return this;
			return WhereProduct_id(items.Where<ProductInfo>(a => a != null).Select<ProductInfo, uint>(a => a.Id.Value).ToArray());
		}
		public MemberSelectBuild WhereProduct_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `member_id` FROM `member_product` WHERE `member_id` = a.`id` AND `product_id` IN ({0}) )", string.Join<uint>(",", ids))) as MemberSelectBuild;
		}
		public MemberSelectBuild WhereShop(params ShopInfo[] items) {
			if (items == null) return this;
			return WhereShop_id(items.Where<ShopInfo>(a => a != null).Select<ShopInfo, uint>(a => a.Id.Value).ToArray());
		}
		public MemberSelectBuild WhereShop_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `member_id` FROM `member_shop` WHERE `member_id` = a.`id` AND `shop_id` IN ({0}) )", string.Join<uint>(",", ids))) as MemberSelectBuild;
		}
		public MemberSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public MemberSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as MemberSelectBuild;
		}
		public MemberSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as MemberSelectBuild;
		}
		public MemberSelectBuild WhereEmail(params string[] Email) {
			return this.Where1Or("a.`email` = {0}", Email);
		}
		public MemberSelectBuild WhereEmailLike(params string[] Email) {
			if (Email == null) return this;
			return this.Where1Or(@"a.`email` LIKE {0}", Email.Select(a => "%" + a + "%").ToArray());
		}
		public MemberSelectBuild WhereLastlogin_timeRange(DateTime? begin) {
			return base.Where("a.`lastlogin_time` >= {0}", begin) as MemberSelectBuild;
		}
		public MemberSelectBuild WhereLastlogin_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereLastlogin_timeRange(begin);
			return base.Where("a.`lastlogin_time` between {0} and {1}", begin, end) as MemberSelectBuild;
		}
		public MemberSelectBuild WhereTelphone(params string[] Telphone) {
			return this.Where1Or("a.`telphone` = {0}", Telphone);
		}
		public MemberSelectBuild WhereTelphoneLike(params string[] Telphone) {
			if (Telphone == null) return this;
			return this.Where1Or(@"a.`telphone` LIKE {0}", Telphone.Select(a => "%" + a + "%").ToArray());
		}
		public MemberSelectBuild WhereUsername(params string[] Username) {
			return this.Where1Or("a.`username` = {0}", Username);
		}
		public MemberSelectBuild WhereUsernameLike(params string[] Username) {
			if (Username == null) return this;
			return this.Where1Or(@"a.`username` LIKE {0}", Username.Select(a => "%" + a + "%").ToArray());
		}
		protected new MemberSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as MemberSelectBuild;
		}
		public MemberSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}