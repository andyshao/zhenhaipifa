using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Order_refund {

		protected static readonly pifa.DAL.Order_refund dal = new pifa.DAL.Order_refund();
		protected static readonly int itemCacheTimeout;

		static Order_refund() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Order_refund"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByOrder_id(uint? Order_id) {
			return dal.DeleteByOrder_id(Order_id);
		}
		public static int DeleteByProductitem_id(uint? Productitem_id) {
			return dal.DeleteByProductitem_id(Productitem_id);
		}

		public static int Update(Order_refundInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Order_refund.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Order_refund.SqlUpdateBuild UpdateDiy(Order_refundInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Order_refund.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Order_refund.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Order_refund.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Order_refund.Insert(Order_refundInfo item)
		/// </summary>
		[Obsolete]
		public static Order_refundInfo Insert(uint? Order_id, uint? Productitem_id, DateTime? Create_time, string Descript, string Email, string Img_url, Order_refundSTATE? State, string Tel, string Telphone, decimal? Wealth) {
			return Insert(new Order_refundInfo {
				Order_id = Order_id, 
				Productitem_id = Productitem_id, 
				Create_time = Create_time, 
				Descript = Descript, 
				Email = Email, 
				Img_url = Img_url, 
				State = State, 
				Tel = Tel, 
				Telphone = Telphone, 
				Wealth = Wealth});
		}
		public static Order_refundInfo Insert(Order_refundInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Order_refundInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Order_refund_", item.Id));
		}
		#endregion

		public static Order_refundInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("pifa_BLL_Order_refund_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new Order_refundInfo(value); } catch { }
			Order_refundInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Order_refundInfo> GetItems() {
			return Select.ToList();
		}
		public static Order_refundSelectBuild Select {
			get { return new Order_refundSelectBuild(dal); }
		}
		public static List<Order_refundInfo> GetItemsByOrder_id(params uint?[] Order_id) {
			return Select.WhereOrder_id(Order_id).ToList();
		}
		public static List<Order_refundInfo> GetItemsByOrder_id(uint?[] Order_id, int limit) {
			return Select.WhereOrder_id(Order_id).Limit(limit).ToList();
		}
		public static Order_refundSelectBuild SelectByOrder_id(params uint?[] Order_id) {
			return Select.WhereOrder_id(Order_id);
		}
		public static List<Order_refundInfo> GetItemsByProductitem_id(params uint?[] Productitem_id) {
			return Select.WhereProductitem_id(Productitem_id).ToList();
		}
		public static List<Order_refundInfo> GetItemsByProductitem_id(uint?[] Productitem_id, int limit) {
			return Select.WhereProductitem_id(Productitem_id).Limit(limit).ToList();
		}
		public static Order_refundSelectBuild SelectByProductitem_id(params uint?[] Productitem_id) {
			return Select.WhereProductitem_id(Productitem_id);
		}
	}
	public partial class Order_refundSelectBuild : SelectBuild<Order_refundInfo, Order_refundSelectBuild> {
		public Order_refundSelectBuild WhereOrder_id(params uint?[] Order_id) {
			return this.Where1Or("a.`Order_id` = {0}", Order_id);
		}
		public Order_refundSelectBuild WhereProductitem_id(params uint?[] Productitem_id) {
			return this.Where1Or("a.`Productitem_id` = {0}", Productitem_id);
		}
		public Order_refundSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public Order_refundSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as Order_refundSelectBuild;
		}
		public Order_refundSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as Order_refundSelectBuild;
		}
		public Order_refundSelectBuild WhereDescriptLike(params string[] Descript) {
			if (Descript == null) return this;
			return this.Where1Or(@"a.`descript` LIKE {0}", Descript.Select(a => "%" + a + "%").ToArray());
		}
		public Order_refundSelectBuild WhereEmail(params string[] Email) {
			return this.Where1Or("a.`email` = {0}", Email);
		}
		public Order_refundSelectBuild WhereEmailLike(params string[] Email) {
			if (Email == null) return this;
			return this.Where1Or(@"a.`email` LIKE {0}", Email.Select(a => "%" + a + "%").ToArray());
		}
		public Order_refundSelectBuild WhereImg_url(params string[] Img_url) {
			return this.Where1Or("a.`img_url` = {0}", Img_url);
		}
		public Order_refundSelectBuild WhereImg_urlLike(params string[] Img_url) {
			if (Img_url == null) return this;
			return this.Where1Or(@"a.`img_url` LIKE {0}", Img_url.Select(a => "%" + a + "%").ToArray());
		}
		public Order_refundSelectBuild WhereState_IN(params Order_refundSTATE?[] States) {
			return this.Where1Or("a.`state` = {0}", States);
		}
		public Order_refundSelectBuild WhereState(Order_refundSTATE? State1) {
			return this.WhereState_IN(State1);
		}
		#region WhereState
		public Order_refundSelectBuild WhereState(Order_refundSTATE? State1, Order_refundSTATE? State2) {
			return this.WhereState_IN(State1, State2);
		}
		public Order_refundSelectBuild WhereState(Order_refundSTATE? State1, Order_refundSTATE? State2, Order_refundSTATE? State3) {
			return this.WhereState_IN(State1, State2, State3);
		}
		public Order_refundSelectBuild WhereState(Order_refundSTATE? State1, Order_refundSTATE? State2, Order_refundSTATE? State3, Order_refundSTATE? State4) {
			return this.WhereState_IN(State1, State2, State3, State4);
		}
		public Order_refundSelectBuild WhereState(Order_refundSTATE? State1, Order_refundSTATE? State2, Order_refundSTATE? State3, Order_refundSTATE? State4, Order_refundSTATE? State5) {
			return this.WhereState_IN(State1, State2, State3, State4, State5);
		}
		#endregion
		public Order_refundSelectBuild WhereTel(params string[] Tel) {
			return this.Where1Or("a.`tel` = {0}", Tel);
		}
		public Order_refundSelectBuild WhereTelLike(params string[] Tel) {
			if (Tel == null) return this;
			return this.Where1Or(@"a.`tel` LIKE {0}", Tel.Select(a => "%" + a + "%").ToArray());
		}
		public Order_refundSelectBuild WhereTelphone(params string[] Telphone) {
			return this.Where1Or("a.`telphone` = {0}", Telphone);
		}
		public Order_refundSelectBuild WhereTelphoneLike(params string[] Telphone) {
			if (Telphone == null) return this;
			return this.Where1Or(@"a.`telphone` LIKE {0}", Telphone.Select(a => "%" + a + "%").ToArray());
		}
		public Order_refundSelectBuild WhereWealth(params decimal?[] Wealth) {
			return this.Where1Or("a.`wealth` = {0}", Wealth);
		}
		public Order_refundSelectBuild WhereWealthRange(decimal? begin) {
			return base.Where("a.`wealth` >= {0}", begin) as Order_refundSelectBuild;
		}
		public Order_refundSelectBuild WhereWealthRange(decimal? begin, decimal? end) {
			if (end == null) return WhereWealthRange(begin);
			return base.Where("a.`wealth` between {0} and {1}", begin, end) as Order_refundSelectBuild;
		}
		protected new Order_refundSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Order_refundSelectBuild;
		}
		public Order_refundSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}