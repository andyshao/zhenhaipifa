using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Order_refund : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`order_refund`";
			internal static readonly string Field = "a.`id`, a.`order_id`, a.`productitem_id`, a.`create_time`, a.`descript`, a.`email`, a.`img_url`, a.`state`+0, a.`tel`, a.`telphone`, a.`wealth`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `order_refund` WHERE ";
			public static readonly string Insert = "INSERT INTO `order_refund`(`order_id`, `productitem_id`, `create_time`, `descript`, `email`, `img_url`, `state`, `tel`, `telphone`, `wealth`) VALUES(?order_id, ?productitem_id, ?create_time, ?descript, ?email, ?img_url, ?state, ?tel, ?telphone, ?wealth); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Order_refundInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?order_id", MySqlDbType.UInt32, 10, item.Order_id), 
				GetParameter("?productitem_id", MySqlDbType.UInt32, 10, item.Productitem_id), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?descript", MySqlDbType.Text, -1, item.Descript), 
				GetParameter("?email", MySqlDbType.VarChar, 64, item.Email), 
				GetParameter("?img_url", MySqlDbType.VarChar, 255, item.Img_url), 
				GetParameter("?state", MySqlDbType.Enum, -1, item.State?.ToInt64()), 
				GetParameter("?tel", MySqlDbType.VarChar, 32, item.Tel), 
				GetParameter("?telphone", MySqlDbType.VarChar, 32, item.Telphone), 
				GetParameter("?wealth", MySqlDbType.Decimal, 10, item.Wealth)};
		}
		public Order_refundInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Order_refundInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new Order_refundInfo {
				Id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Order_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Productitem_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Create_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				Descript = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Email = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Img_url = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				State = dr.IsDBNull(++index) ? null : (Order_refundSTATE?)dr.GetInt64(index), 
				Tel = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Telphone = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Wealth = dr.IsDBNull(++index) ? null : (decimal?)dr.GetDecimal(index)};
		}
		public SelectBuild<Order_refundInfo> Select {
			get { return SelectBuild<Order_refundInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}
		public int DeleteByOrder_id(uint? Order_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`order_id` = ?order_id"), 
				GetParameter("?order_id", MySqlDbType.UInt32, 10, Order_id));
		}
		public int DeleteByProductitem_id(uint? Productitem_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`productitem_id` = ?productitem_id"), 
				GetParameter("?productitem_id", MySqlDbType.UInt32, 10, Productitem_id));
		}

		public int Update(Order_refundInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetOrder_id(item.Order_id)
				.SetProductitem_id(item.Productitem_id)
				.SetCreate_time(item.Create_time)
				.SetDescript(item.Descript)
				.SetEmail(item.Email)
				.SetImg_url(item.Img_url)
				.SetState(item.State)
				.SetTel(item.Tel)
				.SetTelphone(item.Telphone)
				.SetWealth(item.Wealth).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Order_refundInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Order_refundInfo item, uint? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Order_refund.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Order_refund.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetOrder_id(uint? value) {
				if (_item != null) _item.Order_id = value;
				return this.Set("`order_id`", string.Concat("?order_id_", _parameters.Count), 
					GetParameter(string.Concat("?order_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetProductitem_id(uint? value) {
				if (_item != null) _item.Productitem_id = value;
				return this.Set("`productitem_id`", string.Concat("?productitem_id_", _parameters.Count), 
					GetParameter(string.Concat("?productitem_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", string.Concat("?create_time_", _parameters.Count), 
					GetParameter(string.Concat("?create_time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetDescript(string value) {
				if (_item != null) _item.Descript = value;
				return this.Set("`descript`", string.Concat("?descript_", _parameters.Count), 
					GetParameter(string.Concat("?descript_", _parameters.Count), MySqlDbType.Text, -1, value));
			}
			public SqlUpdateBuild SetEmail(string value) {
				if (_item != null) _item.Email = value;
				return this.Set("`email`", string.Concat("?email_", _parameters.Count), 
					GetParameter(string.Concat("?email_", _parameters.Count), MySqlDbType.VarChar, 64, value));
			}
			public SqlUpdateBuild SetImg_url(string value) {
				if (_item != null) _item.Img_url = value;
				return this.Set("`img_url`", string.Concat("?img_url_", _parameters.Count), 
					GetParameter(string.Concat("?img_url_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetState(Order_refundSTATE? value) {
				if (_item != null) _item.State = value;
				return this.Set("`state`", string.Concat("?state_", _parameters.Count), 
					GetParameter(string.Concat("?state_", _parameters.Count), MySqlDbType.Enum, -1, value?.ToInt64()));
			}
			public SqlUpdateBuild SetTel(string value) {
				if (_item != null) _item.Tel = value;
				return this.Set("`tel`", string.Concat("?tel_", _parameters.Count), 
					GetParameter(string.Concat("?tel_", _parameters.Count), MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetTelphone(string value) {
				if (_item != null) _item.Telphone = value;
				return this.Set("`telphone`", string.Concat("?telphone_", _parameters.Count), 
					GetParameter(string.Concat("?telphone_", _parameters.Count), MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetWealth(decimal? value) {
				if (_item != null) _item.Wealth = value;
				return this.Set("`wealth`", string.Concat("?wealth_", _parameters.Count), 
					GetParameter(string.Concat("?wealth_", _parameters.Count), MySqlDbType.Decimal, 10, value));
			}
			public SqlUpdateBuild SetWealthIncrement(decimal value) {
				if (_item != null) _item.Wealth += value;
				return this.Set("`wealth`", string.Concat("`wealth` + ?wealth_", _parameters.Count), 
					GetParameter(string.Concat("?wealth_", _parameters.Count), MySqlDbType.Decimal, 10, value));
			}
		}
		#endregion

		public Order_refundInfo Insert(Order_refundInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public Order_refundInfo GetItem(uint? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
	}
}