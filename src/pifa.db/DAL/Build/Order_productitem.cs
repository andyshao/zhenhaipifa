using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Order_productitem : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`order_productitem`";
			internal static readonly string Field = "a.`order_id`, a.`productitem_id`, a.`number`, a.`price`, a.`state`+0, a.`title`";
			internal static readonly string Sort = "a.`order_id`, a.`productitem_id`";
			public static readonly string Delete = "DELETE FROM `order_productitem` WHERE ";
			public static readonly string Insert = "INSERT INTO `order_productitem`(`order_id`, `productitem_id`, `number`, `price`, `state`, `title`) VALUES(?order_id, ?productitem_id, ?number, ?price, ?state, ?title)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Order_productitemInfo item) {
			return new MySqlParameter[] {
				GetParameter("?order_id", MySqlDbType.UInt32, 10, item.Order_id), 
				GetParameter("?productitem_id", MySqlDbType.UInt32, 10, item.Productitem_id), 
				GetParameter("?number", MySqlDbType.UInt32, 10, item.Number), 
				GetParameter("?price", MySqlDbType.Decimal, 10, item.Price), 
				GetParameter("?state", MySqlDbType.Enum, -1, item.State?.ToInt64()), 
				GetParameter("?title", MySqlDbType.VarChar, 255, item.Title)};
		}
		public Order_productitemInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Order_productitemInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new Order_productitemInfo {
				Order_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Productitem_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Number = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Price = dr.IsDBNull(++index) ? null : (decimal?)dr.GetDecimal(index), 
				State = dr.IsDBNull(++index) ? null : (Order_productitemSTATE?)dr.GetInt64(index), 
				Title = dr.IsDBNull(++index) ? null : dr.GetString(index)};
		}
		public SelectBuild<Order_productitemInfo> Select {
			get { return SelectBuild<Order_productitemInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Order_id, uint? Productitem_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`order_id` = ?order_id AND `productitem_id` = ?productitem_id"), 
				GetParameter("?order_id", MySqlDbType.UInt32, 10, Order_id), 
				GetParameter("?productitem_id", MySqlDbType.UInt32, 10, Productitem_id));
		}
		public int DeleteByOrder_id(uint? Order_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`order_id` = ?order_id"), 
				GetParameter("?order_id", MySqlDbType.UInt32, 10, Order_id));
		}
		public int DeleteByProductitem_id(uint? Productitem_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`productitem_id` = ?productitem_id"), 
				GetParameter("?productitem_id", MySqlDbType.UInt32, 10, Productitem_id));
		}

		public int Update(Order_productitemInfo item) {
			return new SqlUpdateBuild(null, item.Order_id, item.Productitem_id)
				.SetNumber(item.Number)
				.SetPrice(item.Price)
				.SetState(item.State)
				.SetTitle(item.Title).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Order_productitemInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Order_productitemInfo item, uint? Order_id, uint? Productitem_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`order_id` = {0} AND `productitem_id` = {1}", Order_id, Productitem_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Order_productitem.SqlUpdateBuild 误修改，请必须设置 where 条件。");
				return string.Concat("UPDATE ", TSQL.Table, " SET ", _fields.Substring(1), " WHERE ", _where);
			}
			public int ExecuteNonQuery() {
				string sql = this.ToString();
				if (string.IsNullOrEmpty(sql)) return 0;
				return SqlHelper.ExecuteNonQuery(sql, _parameters.ToArray());
			}
			public SqlUpdateBuild Where(string filterFormat, params object[] values) {
				if (!string.IsNullOrEmpty(_where)) _where = string.Concat(_where, " AND ");
				_where = string.Concat(_where, "(", SqlHelper.Addslashes(filterFormat, values), ")");
				return this;
			}
			public SqlUpdateBuild Set(string field, string value, params MySqlParameter[] parms) {
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Order_productitem.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetNumber(uint? value) {
				if (_item != null) _item.Number = value;
				return this.Set("`number`", string.Concat("?number_", _parameters.Count), 
					GetParameter(string.Concat("?number_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetNumberIncrement(int value) {
				if (_item != null) _item.Number = (uint?)((int?)_item.Number + value);
				return this.Set("`number`", string.Concat("`number` + ?number_", _parameters.Count), 
					GetParameter(string.Concat("?number_", _parameters.Count), MySqlDbType.Int32, 10, value));
			}
			public SqlUpdateBuild SetPrice(decimal? value) {
				if (_item != null) _item.Price = value;
				return this.Set("`price`", string.Concat("?price_", _parameters.Count), 
					GetParameter(string.Concat("?price_", _parameters.Count), MySqlDbType.Decimal, 10, value));
			}
			public SqlUpdateBuild SetPriceIncrement(decimal value) {
				if (_item != null) _item.Price += value;
				return this.Set("`price`", string.Concat("`price` + ?price_", _parameters.Count), 
					GetParameter(string.Concat("?price_", _parameters.Count), MySqlDbType.Decimal, 10, value));
			}
			public SqlUpdateBuild SetState(Order_productitemSTATE? value) {
				if (_item != null) _item.State = value;
				return this.Set("`state`", string.Concat("?state_", _parameters.Count), 
					GetParameter(string.Concat("?state_", _parameters.Count), MySqlDbType.Enum, -1, value?.ToInt64()));
			}
			public SqlUpdateBuild SetTitle(string value) {
				if (_item != null) _item.Title = value;
				return this.Set("`title`", string.Concat("?title_", _parameters.Count), 
					GetParameter(string.Concat("?title_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
		}
		#endregion

		public Order_productitemInfo Insert(Order_productitemInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

		public Order_productitemInfo GetItem(uint? Order_id, uint? Productitem_id) {
			return this.Select.Where("a.`order_id` = {0} AND a.`productitem_id` = {1}", Order_id, Productitem_id).ToOne();
		}
	}
}