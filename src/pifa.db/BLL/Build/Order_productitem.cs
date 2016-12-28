using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Order_productitem {

		protected static readonly pifa.DAL.Order_productitem dal = new pifa.DAL.Order_productitem();
		protected static readonly int itemCacheTimeout;

		static Order_productitem() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Order_productitem"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Order_id, uint? Productitem_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Order_id, Productitem_id));
			return dal.Delete(Order_id, Productitem_id);
		}
		public static int DeleteByOrder_id(uint? Order_id) {
			return dal.DeleteByOrder_id(Order_id);
		}
		public static int DeleteByProductitem_id(uint? Productitem_id) {
			return dal.DeleteByProductitem_id(Productitem_id);
		}

		public static int Update(Order_productitemInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Order_productitem.SqlUpdateBuild UpdateDiy(uint? Order_id, uint? Productitem_id) {
			return UpdateDiy(null, Order_id, Productitem_id);
		}
		public static pifa.DAL.Order_productitem.SqlUpdateBuild UpdateDiy(Order_productitemInfo item, uint? Order_id, uint? Productitem_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Order_id, Productitem_id));
			return new pifa.DAL.Order_productitem.SqlUpdateBuild(item, Order_id, Productitem_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Order_productitem.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Order_productitem.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Order_productitem.Insert(Order_productitemInfo item)
		/// </summary>
		[Obsolete]
		public static Order_productitemInfo Insert(uint? Order_id, uint? Productitem_id, uint? Number, decimal? Price, Order_productitemSTATE? State, string Title) {
			return Insert(new Order_productitemInfo {
				Order_id = Order_id, 
				Productitem_id = Productitem_id, 
				Number = Number, 
				Price = Price, 
				State = State, 
				Title = Title});
		}
		public static Order_productitemInfo Insert(Order_productitemInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Order_productitemInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Order_productitem_", item.Order_id, "_,_", item.Productitem_id));
		}
		#endregion

		public static Order_productitemInfo GetItem(uint? Order_id, uint? Productitem_id) {
			if (Order_id == null || Productitem_id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Order_id, Productitem_id);
			string key = string.Concat("pifa_BLL_Order_productitem_", Order_id, "_,_", Productitem_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new Order_productitemInfo(value); } catch { }
			Order_productitemInfo item = dal.GetItem(Order_id, Productitem_id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Order_productitemInfo> GetItems() {
			return Select.ToList();
		}
		public static Order_productitemSelectBuild Select {
			get { return new Order_productitemSelectBuild(dal); }
		}
		public static List<Order_productitemInfo> GetItemsByOrder_id(params uint?[] Order_id) {
			return Select.WhereOrder_id(Order_id).ToList();
		}
		public static List<Order_productitemInfo> GetItemsByOrder_id(uint?[] Order_id, int limit) {
			return Select.WhereOrder_id(Order_id).Limit(limit).ToList();
		}
		public static Order_productitemSelectBuild SelectByOrder_id(params uint?[] Order_id) {
			return Select.WhereOrder_id(Order_id);
		}
		public static List<Order_productitemInfo> GetItemsByProductitem_id(params uint?[] Productitem_id) {
			return Select.WhereProductitem_id(Productitem_id).ToList();
		}
		public static List<Order_productitemInfo> GetItemsByProductitem_id(uint?[] Productitem_id, int limit) {
			return Select.WhereProductitem_id(Productitem_id).Limit(limit).ToList();
		}
		public static Order_productitemSelectBuild SelectByProductitem_id(params uint?[] Productitem_id) {
			return Select.WhereProductitem_id(Productitem_id);
		}
	}
	public partial class Order_productitemSelectBuild : SelectBuild<Order_productitemInfo, Order_productitemSelectBuild> {
		public Order_productitemSelectBuild WhereOrder_id(params uint?[] Order_id) {
			return this.Where1Or("a.`Order_id` = {0}", Order_id);
		}
		public Order_productitemSelectBuild WhereProductitem_id(params uint?[] Productitem_id) {
			return this.Where1Or("a.`Productitem_id` = {0}", Productitem_id);
		}
		public Order_productitemSelectBuild WhereNumber(params uint?[] Number) {
			return this.Where1Or("a.`number` = {0}", Number);
		}
		public Order_productitemSelectBuild WherePrice(params decimal?[] Price) {
			return this.Where1Or("a.`price` = {0}", Price);
		}
		public Order_productitemSelectBuild WherePriceRange(decimal? begin) {
			return base.Where("a.`price` >= {0}", begin) as Order_productitemSelectBuild;
		}
		public Order_productitemSelectBuild WherePriceRange(decimal? begin, decimal? end) {
			if (end == null) return WherePriceRange(begin);
			return base.Where("a.`price` between {0} and {1}", begin, end) as Order_productitemSelectBuild;
		}
		public Order_productitemSelectBuild WhereState_IN(params Order_productitemSTATE?[] States) {
			return this.Where1Or("a.`state` = {0}", States);
		}
		public Order_productitemSelectBuild WhereState(Order_productitemSTATE? State1) {
			return this.WhereState_IN(State1);
		}
		#region WhereState
		public Order_productitemSelectBuild WhereState(Order_productitemSTATE? State1, Order_productitemSTATE? State2) {
			return this.WhereState_IN(State1, State2);
		}
		public Order_productitemSelectBuild WhereState(Order_productitemSTATE? State1, Order_productitemSTATE? State2, Order_productitemSTATE? State3) {
			return this.WhereState_IN(State1, State2, State3);
		}
		public Order_productitemSelectBuild WhereState(Order_productitemSTATE? State1, Order_productitemSTATE? State2, Order_productitemSTATE? State3, Order_productitemSTATE? State4) {
			return this.WhereState_IN(State1, State2, State3, State4);
		}
		public Order_productitemSelectBuild WhereState(Order_productitemSTATE? State1, Order_productitemSTATE? State2, Order_productitemSTATE? State3, Order_productitemSTATE? State4, Order_productitemSTATE? State5) {
			return this.WhereState_IN(State1, State2, State3, State4, State5);
		}
		#endregion
		public Order_productitemSelectBuild WhereTitle(params string[] Title) {
			return this.Where1Or("a.`title` = {0}", Title);
		}
		public Order_productitemSelectBuild WhereTitleLike(params string[] Title) {
			if (Title == null) return this;
			return this.Where1Or(@"a.`title` LIKE {0}", Title.Select(a => "%" + a + "%").ToArray());
		}
		protected new Order_productitemSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Order_productitemSelectBuild;
		}
		public Order_productitemSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}