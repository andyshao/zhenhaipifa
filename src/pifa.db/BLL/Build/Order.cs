using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Order {

		protected static readonly pifa.DAL.Order dal = new pifa.DAL.Order();
		protected static readonly int itemCacheTimeout;

		static Order() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Order"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByCode(string Code) {
			return dal.DeleteByCode(Code);
		}
		public static int DeleteByMember_id(uint? Member_id) {
			return dal.DeleteByMember_id(Member_id);
		}

		public static int Update(OrderInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Order.SqlUpdateBuild UpdateDiy(uint Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Order.SqlUpdateBuild UpdateDiy(OrderInfo item, uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Order.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Order.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Order.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Order.Insert(OrderInfo item)
		/// </summary>
		[Obsolete]
		public static OrderInfo Insert(uint? Member_id, string Code, DateTime? Create_time, string Express_code, string Express_name, string Paymethod, string Remark, OrderSTATE? State, decimal? Total_express_price, decimal? Total_original_price, decimal? Total_price, DateTime? Update_time) {
			return Insert(new OrderInfo {
				Member_id = Member_id, 
				Code = Code, 
				Create_time = Create_time, 
				Express_code = Express_code, 
				Express_name = Express_name, 
				Paymethod = Paymethod, 
				Remark = Remark, 
				State = State, 
				Total_express_price = Total_express_price, 
				Total_original_price = Total_original_price, 
				Total_price = Total_price, 
				Update_time = Update_time});
		}
		public static OrderInfo Insert(OrderInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(OrderInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Order_", item.Id));
			RedisHelper.Remove(string.Concat("pifa_BLL_OrderByCode_", item.Code));
		}
		#endregion

		public static OrderInfo GetItem(uint Id) {
			if (itemCacheTimeout <= 0) return Select.WhereId(Id).ToOne();
			string key = string.Concat("pifa_BLL_Order_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return OrderInfo.Parse(value); } catch { }
			OrderInfo item = Select.WhereId(Id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}
		public static OrderInfo GetItemByCode(string Code) {
			if (itemCacheTimeout <= 0) return Select.WhereCode(Code).ToOne();
			string key = string.Concat("pifa_BLL_OrderByCode_", Code);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return OrderInfo.Parse(value); } catch { }
			OrderInfo item = Select.WhereCode(Code).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<OrderInfo> GetItems() {
			return Select.ToList();
		}
		public static OrderSelectBuild Select {
			get { return new OrderSelectBuild(dal); }
		}
		public static List<OrderInfo> GetItemsByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id).ToList();
		}
		public static List<OrderInfo> GetItemsByMember_id(uint?[] Member_id, int limit) {
			return Select.WhereMember_id(Member_id).Limit(limit).ToList();
		}
		public static OrderSelectBuild SelectByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id);
		}
		public static OrderSelectBuild SelectByProductitem(params ProductitemInfo[] items) {
			return Select.WhereProductitem(items);
		}
		public static OrderSelectBuild SelectByProductitem_id(params uint[] ids) {
			return Select.WhereProductitem_id(ids);
		}
	}
	public partial class OrderSelectBuild : SelectBuild<OrderInfo, OrderSelectBuild> {
		public OrderSelectBuild WhereMember_id(params uint?[] Member_id) {
			return this.Where1Or("a.`member_id` = {0}", Member_id);
		}
		public OrderSelectBuild WhereProductitem(params ProductitemInfo[] items) {
			if (items == null) return this;
			return WhereProductitem_id(items.Where<ProductitemInfo>(a => a != null).Select<ProductitemInfo, uint>(a => a.Id.Value).ToArray());
		}
		public OrderSelectBuild WhereProductitem_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `order_id` FROM `order_productitem` WHERE `order_id` = a.`id` AND `productitem_id` IN ({0}) )", string.Join<uint>(",", ids))) as OrderSelectBuild;
		}
		public OrderSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public OrderSelectBuild WhereCode(params string[] Code) {
			return this.Where1Or("a.`code` = {0}", Code);
		}
		public OrderSelectBuild WhereCodeLike(params string[] Code) {
			if (Code == null || Code.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`code` LIKE {0}", Code.Select(a => "%" + a + "%").ToArray());
		}
		public OrderSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as OrderSelectBuild;
		}
		public OrderSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as OrderSelectBuild;
		}
		public OrderSelectBuild WhereExpress_code(params string[] Express_code) {
			return this.Where1Or("a.`express_code` = {0}", Express_code);
		}
		public OrderSelectBuild WhereExpress_codeLike(params string[] Express_code) {
			if (Express_code == null || Express_code.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`express_code` LIKE {0}", Express_code.Select(a => "%" + a + "%").ToArray());
		}
		public OrderSelectBuild WhereExpress_name(params string[] Express_name) {
			return this.Where1Or("a.`express_name` = {0}", Express_name);
		}
		public OrderSelectBuild WhereExpress_nameLike(params string[] Express_name) {
			if (Express_name == null || Express_name.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`express_name` LIKE {0}", Express_name.Select(a => "%" + a + "%").ToArray());
		}
		public OrderSelectBuild WherePaymethod(params string[] Paymethod) {
			return this.Where1Or("a.`paymethod` = {0}", Paymethod);
		}
		public OrderSelectBuild WherePaymethodLike(params string[] Paymethod) {
			if (Paymethod == null || Paymethod.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`paymethod` LIKE {0}", Paymethod.Select(a => "%" + a + "%").ToArray());
		}
		public OrderSelectBuild WhereRemark(params string[] Remark) {
			return this.Where1Or("a.`remark` = {0}", Remark);
		}
		public OrderSelectBuild WhereRemarkLike(params string[] Remark) {
			if (Remark == null || Remark.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`remark` LIKE {0}", Remark.Select(a => "%" + a + "%").ToArray());
		}
		public OrderSelectBuild WhereState_IN(params OrderSTATE?[] States) {
			return this.Where1Or("a.`state` = {0}", States);
		}
		public OrderSelectBuild WhereState(OrderSTATE State1) {
			return this.WhereState_IN(State1);
		}
		#region WhereState
		public OrderSelectBuild WhereState(OrderSTATE State1, OrderSTATE State2) {
			return this.WhereState_IN(State1, State2);
		}
		public OrderSelectBuild WhereState(OrderSTATE State1, OrderSTATE State2, OrderSTATE State3) {
			return this.WhereState_IN(State1, State2, State3);
		}
		public OrderSelectBuild WhereState(OrderSTATE State1, OrderSTATE State2, OrderSTATE State3, OrderSTATE State4) {
			return this.WhereState_IN(State1, State2, State3, State4);
		}
		public OrderSelectBuild WhereState(OrderSTATE State1, OrderSTATE State2, OrderSTATE State3, OrderSTATE State4, OrderSTATE State5) {
			return this.WhereState_IN(State1, State2, State3, State4, State5);
		}
		#endregion
		public OrderSelectBuild WhereTotal_express_price(params decimal?[] Total_express_price) {
			return this.Where1Or("a.`total_express_price` = {0}", Total_express_price);
		}
		public OrderSelectBuild WhereTotal_express_priceRange(decimal? begin) {
			return base.Where("a.`total_express_price` >= {0}", begin) as OrderSelectBuild;
		}
		public OrderSelectBuild WhereTotal_express_priceRange(decimal? begin, decimal? end) {
			if (end == null) return WhereTotal_express_priceRange(begin);
			return base.Where("a.`total_express_price` between {0} and {1}", begin, end) as OrderSelectBuild;
		}
		public OrderSelectBuild WhereTotal_original_price(params decimal?[] Total_original_price) {
			return this.Where1Or("a.`total_original_price` = {0}", Total_original_price);
		}
		public OrderSelectBuild WhereTotal_original_priceRange(decimal? begin) {
			return base.Where("a.`total_original_price` >= {0}", begin) as OrderSelectBuild;
		}
		public OrderSelectBuild WhereTotal_original_priceRange(decimal? begin, decimal? end) {
			if (end == null) return WhereTotal_original_priceRange(begin);
			return base.Where("a.`total_original_price` between {0} and {1}", begin, end) as OrderSelectBuild;
		}
		public OrderSelectBuild WhereTotal_price(params decimal?[] Total_price) {
			return this.Where1Or("a.`total_price` = {0}", Total_price);
		}
		public OrderSelectBuild WhereTotal_priceRange(decimal? begin) {
			return base.Where("a.`total_price` >= {0}", begin) as OrderSelectBuild;
		}
		public OrderSelectBuild WhereTotal_priceRange(decimal? begin, decimal? end) {
			if (end == null) return WhereTotal_priceRange(begin);
			return base.Where("a.`total_price` between {0} and {1}", begin, end) as OrderSelectBuild;
		}
		public OrderSelectBuild WhereUpdate_timeRange(DateTime? begin) {
			return base.Where("a.`update_time` >= {0}", begin) as OrderSelectBuild;
		}
		public OrderSelectBuild WhereUpdate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereUpdate_timeRange(begin);
			return base.Where("a.`update_time` between {0} and {1}", begin, end) as OrderSelectBuild;
		}
		protected new OrderSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as OrderSelectBuild;
		}
		public OrderSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}