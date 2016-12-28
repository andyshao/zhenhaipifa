using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Shop {

		protected static readonly pifa.DAL.Shop dal = new pifa.DAL.Shop();
		protected static readonly int itemCacheTimeout;

		static Shop() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Shop"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByCode(string Code) {
			return dal.DeleteByCode(Code);
		}
		public static int DeleteByMarkettype_id(uint? Markettype_id) {
			return dal.DeleteByMarkettype_id(Markettype_id);
		}
		public static int DeleteByMember_id(uint? Member_id) {
			return dal.DeleteByMember_id(Member_id);
		}

		public static int Update(ShopInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Shop.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Shop.SqlUpdateBuild UpdateDiy(ShopInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Shop.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Shop.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Shop.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Shop.Insert(ShopInfo item)
		/// </summary>
		[Obsolete]
		public static ShopInfo Insert(uint? Markettype_id, uint? Member_id, string Address, decimal? Area, string Code, DateTime? Create_time, string Fax, ShopFUNC_SWITCH? Func_switch, ShopICON? Icon, string Kefu, string Main_business, string Nickname, ShopSTATE? State, string Title) {
			return Insert(new ShopInfo {
				Markettype_id = Markettype_id, 
				Member_id = Member_id, 
				Address = Address, 
				Area = Area, 
				Code = Code, 
				Create_time = Create_time, 
				Fax = Fax, 
				Func_switch = Func_switch, 
				Icon = Icon, 
				Kefu = Kefu, 
				Main_business = Main_business, 
				Nickname = Nickname, 
				State = State, 
				Title = Title});
		}
		public static ShopInfo Insert(ShopInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(ShopInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Shop_", item.Id));
			RedisHelper.Remove(string.Concat("pifa_BLL_ShopByCode_", item.Code));
		}
		#endregion

		public static ShopInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("pifa_BLL_Shop_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new ShopInfo(value); } catch { }
			ShopInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}
		public static ShopInfo GetItemByCode(string Code) {
			if (Code == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItemByCode(Code);
			string key = string.Concat("pifa_BLL_ShopByCode_", Code);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new ShopInfo(value); } catch { }
			ShopInfo item = dal.GetItemByCode(Code);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<ShopInfo> GetItems() {
			return Select.ToList();
		}
		public static ShopSelectBuild Select {
			get { return new ShopSelectBuild(dal); }
		}
		public static List<ShopInfo> GetItemsByMarkettype_id(params uint?[] Markettype_id) {
			return Select.WhereMarkettype_id(Markettype_id).ToList();
		}
		public static List<ShopInfo> GetItemsByMarkettype_id(uint?[] Markettype_id, int limit) {
			return Select.WhereMarkettype_id(Markettype_id).Limit(limit).ToList();
		}
		public static ShopSelectBuild SelectByMarkettype_id(params uint?[] Markettype_id) {
			return Select.WhereMarkettype_id(Markettype_id);
		}
		public static List<ShopInfo> GetItemsByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id).ToList();
		}
		public static List<ShopInfo> GetItemsByMember_id(uint?[] Member_id, int limit) {
			return Select.WhereMember_id(Member_id).Limit(limit).ToList();
		}
		public static ShopSelectBuild SelectByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id);
		}
		public static ShopSelectBuild SelectByMember_fav(params MemberInfo[] items) {
			return Select.WhereMember_fav(items);
		}
		public static ShopSelectBuild SelectByMember_fav_id(params uint[] ids) {
			return Select.WhereMember_fav_id(ids);
		}
		public static ShopSelectBuild SelectByFranchising(params FranchisingInfo[] items) {
			return Select.WhereFranchising(items);
		}
		public static ShopSelectBuild SelectByFranchising_id(params uint[] ids) {
			return Select.WhereFranchising_id(ids);
		}
	}
	public partial class ShopSelectBuild : SelectBuild<ShopInfo, ShopSelectBuild> {
		public ShopSelectBuild WhereMarkettype_id(params uint?[] Markettype_id) {
			return this.Where1Or("a.`Markettype_id` = {0}", Markettype_id);
		}
		public ShopSelectBuild WhereMember_id(params uint?[] Member_id) {
			return this.Where1Or("a.`Member_id` = {0}", Member_id);
		}
		public ShopSelectBuild WhereMember_fav(params MemberInfo[] items) {
			if (items == null) return this;
			return WhereMember_fav_id(items.Where<MemberInfo>(a => a != null).Select<MemberInfo, uint>(a => a.Id.Value).ToArray());
		}
		public ShopSelectBuild WhereMember_fav_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `shop_id` FROM `member_fav_shop` WHERE `shop_id` = a.`id` AND `member_id` IN ({0}) )", string.Join<uint>(",", ids))) as ShopSelectBuild;
		}
		public ShopSelectBuild WhereFranchising(params FranchisingInfo[] items) {
			if (items == null) return this;
			return WhereFranchising_id(items.Where<FranchisingInfo>(a => a != null).Select<FranchisingInfo, uint>(a => a.Id.Value).ToArray());
		}
		public ShopSelectBuild WhereFranchising_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `shop_id` FROM `shop_franchising` WHERE `shop_id` = a.`id` AND `franchising_id` IN ({0}) )", string.Join<uint>(",", ids))) as ShopSelectBuild;
		}
		public ShopSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public ShopSelectBuild WhereAddress(params string[] Address) {
			return this.Where1Or("a.`address` = {0}", Address);
		}
		public ShopSelectBuild WhereAddressLike(params string[] Address) {
			if (Address == null) return this;
			return this.Where1Or(@"a.`address` LIKE {0}", Address.Select(a => "%" + a + "%").ToArray());
		}
		public ShopSelectBuild WhereArea(params decimal?[] Area) {
			return this.Where1Or("a.`area` = {0}", Area);
		}
		public ShopSelectBuild WhereAreaRange(decimal? begin) {
			return base.Where("a.`area` >= {0}", begin) as ShopSelectBuild;
		}
		public ShopSelectBuild WhereAreaRange(decimal? begin, decimal? end) {
			if (end == null) return WhereAreaRange(begin);
			return base.Where("a.`area` between {0} and {1}", begin, end) as ShopSelectBuild;
		}
		public ShopSelectBuild WhereCode(params string[] Code) {
			return this.Where1Or("a.`code` = {0}", Code);
		}
		public ShopSelectBuild WhereCodeLike(params string[] Code) {
			if (Code == null) return this;
			return this.Where1Or(@"a.`code` LIKE {0}", Code.Select(a => "%" + a + "%").ToArray());
		}
		public ShopSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as ShopSelectBuild;
		}
		public ShopSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as ShopSelectBuild;
		}
		public ShopSelectBuild WhereFax(params string[] Fax) {
			return this.Where1Or("a.`fax` = {0}", Fax);
		}
		public ShopSelectBuild WhereFaxLike(params string[] Fax) {
			if (Fax == null) return this;
			return this.Where1Or(@"a.`fax` LIKE {0}", Fax.Select(a => "%" + a + "%").ToArray());
		}
		public ShopSelectBuild WhereFunc_switch_IN(params ShopFUNC_SWITCH?[] Func_switchs) {
			return this.Where1Or("(a.`func_switch` & {0}) = {0}", Func_switchs);
		}
		public ShopSelectBuild WhereFunc_switch(ShopFUNC_SWITCH? Func_switch1) {
			return this.WhereFunc_switch_IN(Func_switch1);
		}
		#region WhereFunc_switch
		public ShopSelectBuild WhereFunc_switch(ShopFUNC_SWITCH? Func_switch1, ShopFUNC_SWITCH? Func_switch2) {
			return this.WhereFunc_switch_IN(Func_switch1, Func_switch2);
		}
		public ShopSelectBuild WhereFunc_switch(ShopFUNC_SWITCH? Func_switch1, ShopFUNC_SWITCH? Func_switch2, ShopFUNC_SWITCH? Func_switch3) {
			return this.WhereFunc_switch_IN(Func_switch1, Func_switch2, Func_switch3);
		}
		public ShopSelectBuild WhereFunc_switch(ShopFUNC_SWITCH? Func_switch1, ShopFUNC_SWITCH? Func_switch2, ShopFUNC_SWITCH? Func_switch3, ShopFUNC_SWITCH? Func_switch4) {
			return this.WhereFunc_switch_IN(Func_switch1, Func_switch2, Func_switch3, Func_switch4);
		}
		public ShopSelectBuild WhereFunc_switch(ShopFUNC_SWITCH? Func_switch1, ShopFUNC_SWITCH? Func_switch2, ShopFUNC_SWITCH? Func_switch3, ShopFUNC_SWITCH? Func_switch4, ShopFUNC_SWITCH? Func_switch5) {
			return this.WhereFunc_switch_IN(Func_switch1, Func_switch2, Func_switch3, Func_switch4, Func_switch5);
		}
		#endregion
		public ShopSelectBuild WhereIcon_IN(params ShopICON?[] Icons) {
			return this.Where1Or("(a.`icon` & {0}) = {0}", Icons);
		}
		public ShopSelectBuild WhereIcon(ShopICON? Icon1) {
			return this.WhereIcon_IN(Icon1);
		}
		#region WhereIcon
		public ShopSelectBuild WhereIcon(ShopICON? Icon1, ShopICON? Icon2) {
			return this.WhereIcon_IN(Icon1, Icon2);
		}
		public ShopSelectBuild WhereIcon(ShopICON? Icon1, ShopICON? Icon2, ShopICON? Icon3) {
			return this.WhereIcon_IN(Icon1, Icon2, Icon3);
		}
		public ShopSelectBuild WhereIcon(ShopICON? Icon1, ShopICON? Icon2, ShopICON? Icon3, ShopICON? Icon4) {
			return this.WhereIcon_IN(Icon1, Icon2, Icon3, Icon4);
		}
		public ShopSelectBuild WhereIcon(ShopICON? Icon1, ShopICON? Icon2, ShopICON? Icon3, ShopICON? Icon4, ShopICON? Icon5) {
			return this.WhereIcon_IN(Icon1, Icon2, Icon3, Icon4, Icon5);
		}
		#endregion
		public ShopSelectBuild WhereKefu(params string[] Kefu) {
			return this.Where1Or("a.`kefu` = {0}", Kefu);
		}
		public ShopSelectBuild WhereKefuLike(params string[] Kefu) {
			if (Kefu == null) return this;
			return this.Where1Or(@"a.`kefu` LIKE {0}", Kefu.Select(a => "%" + a + "%").ToArray());
		}
		public ShopSelectBuild WhereMain_business(params string[] Main_business) {
			return this.Where1Or("a.`main_business` = {0}", Main_business);
		}
		public ShopSelectBuild WhereMain_businessLike(params string[] Main_business) {
			if (Main_business == null) return this;
			return this.Where1Or(@"a.`main_business` LIKE {0}", Main_business.Select(a => "%" + a + "%").ToArray());
		}
		public ShopSelectBuild WhereNickname(params string[] Nickname) {
			return this.Where1Or("a.`nickname` = {0}", Nickname);
		}
		public ShopSelectBuild WhereNicknameLike(params string[] Nickname) {
			if (Nickname == null) return this;
			return this.Where1Or(@"a.`nickname` LIKE {0}", Nickname.Select(a => "%" + a + "%").ToArray());
		}
		public ShopSelectBuild WhereState_IN(params ShopSTATE?[] States) {
			return this.Where1Or("a.`state` = {0}", States);
		}
		public ShopSelectBuild WhereState(ShopSTATE? State1) {
			return this.WhereState_IN(State1);
		}
		#region WhereState
		public ShopSelectBuild WhereState(ShopSTATE? State1, ShopSTATE? State2) {
			return this.WhereState_IN(State1, State2);
		}
		public ShopSelectBuild WhereState(ShopSTATE? State1, ShopSTATE? State2, ShopSTATE? State3) {
			return this.WhereState_IN(State1, State2, State3);
		}
		public ShopSelectBuild WhereState(ShopSTATE? State1, ShopSTATE? State2, ShopSTATE? State3, ShopSTATE? State4) {
			return this.WhereState_IN(State1, State2, State3, State4);
		}
		public ShopSelectBuild WhereState(ShopSTATE? State1, ShopSTATE? State2, ShopSTATE? State3, ShopSTATE? State4, ShopSTATE? State5) {
			return this.WhereState_IN(State1, State2, State3, State4, State5);
		}
		#endregion
		public ShopSelectBuild WhereTitle(params string[] Title) {
			return this.Where1Or("a.`title` = {0}", Title);
		}
		public ShopSelectBuild WhereTitleLike(params string[] Title) {
			if (Title == null) return this;
			return this.Where1Or(@"a.`title` LIKE {0}", Title.Select(a => "%" + a + "%").ToArray());
		}
		protected new ShopSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as ShopSelectBuild;
		}
		public ShopSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}