using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Order_address {

		protected static readonly pifa.DAL.Order_address dal = new pifa.DAL.Order_address();
		protected static readonly int itemCacheTimeout;

		static Order_address() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Order_address"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Order_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Order_id));
			return dal.Delete(Order_id);
		}

		public static int Update(Order_addressInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Order_address.SqlUpdateBuild UpdateDiy(uint? Order_id) {
			return UpdateDiy(null, Order_id);
		}
		public static pifa.DAL.Order_address.SqlUpdateBuild UpdateDiy(Order_addressInfo item, uint? Order_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Order_id));
			return new pifa.DAL.Order_address.SqlUpdateBuild(item, Order_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Order_address.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Order_address.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Order_address.Insert(Order_addressInfo item)
		/// </summary>
		[Obsolete]
		public static Order_addressInfo Insert(uint? Order_id, string Address, string Name, string Tel, string Telphone, string Zip) {
			return Insert(new Order_addressInfo {
				Order_id = Order_id, 
				Address = Address, 
				Name = Name, 
				Tel = Tel, 
				Telphone = Telphone, 
				Zip = Zip});
		}
		public static Order_addressInfo Insert(Order_addressInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Order_addressInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Order_address_", item.Order_id));
		}
		#endregion

		public static Order_addressInfo GetItem(uint? Order_id) {
			if (Order_id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Order_id);
			string key = string.Concat("pifa_BLL_Order_address_", Order_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new Order_addressInfo(value); } catch { }
			Order_addressInfo item = dal.GetItem(Order_id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Order_addressInfo> GetItems() {
			return Select.ToList();
		}
		public static Order_addressSelectBuild Select {
			get { return new Order_addressSelectBuild(dal); }
		}
		public static List<Order_addressInfo> GetItemsByOrder_id(params uint?[] Order_id) {
			return Select.WhereOrder_id(Order_id).ToList();
		}
		public static List<Order_addressInfo> GetItemsByOrder_id(uint?[] Order_id, int limit) {
			return Select.WhereOrder_id(Order_id).Limit(limit).ToList();
		}
		public static Order_addressSelectBuild SelectByOrder_id(params uint?[] Order_id) {
			return Select.WhereOrder_id(Order_id);
		}
	}
	public partial class Order_addressSelectBuild : SelectBuild<Order_addressInfo, Order_addressSelectBuild> {
		public Order_addressSelectBuild WhereOrder_id(params uint?[] Order_id) {
			return this.Where1Or("a.`Order_id` = {0}", Order_id);
		}
		public Order_addressSelectBuild WhereAddress(params string[] Address) {
			return this.Where1Or("a.`address` = {0}", Address);
		}
		public Order_addressSelectBuild WhereAddressLike(params string[] Address) {
			if (Address == null) return this;
			return this.Where1Or(@"a.`address` LIKE {0}", Address.Select(a => "%" + a + "%").ToArray());
		}
		public Order_addressSelectBuild WhereName(params string[] Name) {
			return this.Where1Or("a.`name` = {0}", Name);
		}
		public Order_addressSelectBuild WhereNameLike(params string[] Name) {
			if (Name == null) return this;
			return this.Where1Or(@"a.`name` LIKE {0}", Name.Select(a => "%" + a + "%").ToArray());
		}
		public Order_addressSelectBuild WhereTel(params string[] Tel) {
			return this.Where1Or("a.`tel` = {0}", Tel);
		}
		public Order_addressSelectBuild WhereTelLike(params string[] Tel) {
			if (Tel == null) return this;
			return this.Where1Or(@"a.`tel` LIKE {0}", Tel.Select(a => "%" + a + "%").ToArray());
		}
		public Order_addressSelectBuild WhereTelphone(params string[] Telphone) {
			return this.Where1Or("a.`telphone` = {0}", Telphone);
		}
		public Order_addressSelectBuild WhereTelphoneLike(params string[] Telphone) {
			if (Telphone == null) return this;
			return this.Where1Or(@"a.`telphone` LIKE {0}", Telphone.Select(a => "%" + a + "%").ToArray());
		}
		public Order_addressSelectBuild WhereZip(params string[] Zip) {
			return this.Where1Or("a.`zip` = {0}", Zip);
		}
		public Order_addressSelectBuild WhereZipLike(params string[] Zip) {
			if (Zip == null) return this;
			return this.Where1Or(@"a.`zip` LIKE {0}", Zip.Select(a => "%" + a + "%").ToArray());
		}
		protected new Order_addressSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Order_addressSelectBuild;
		}
		public Order_addressSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}