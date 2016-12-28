using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Order : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`order`";
			internal static readonly string Field = "a.`id`, a.`member_id`, a.`code`, a.`create_time`, a.`express_code`, a.`express_name`, a.`paymethod`, a.`remark`, a.`state`+0, a.`total_express_price`, a.`total_original_price`, a.`total_price`, a.`update_time`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `order` WHERE ";
			public static readonly string Insert = "INSERT INTO `order`(`member_id`, `code`, `create_time`, `express_code`, `express_name`, `paymethod`, `remark`, `state`, `total_express_price`, `total_original_price`, `total_price`, `update_time`) VALUES(?member_id, ?code, ?create_time, ?express_code, ?express_name, ?paymethod, ?remark, ?state, ?total_express_price, ?total_original_price, ?total_price, ?update_time); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(OrderInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?member_id", MySqlDbType.UInt32, 10, item.Member_id), 
				GetParameter("?code", MySqlDbType.VarChar, 32, item.Code), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?express_code", MySqlDbType.VarChar, 64, item.Express_code), 
				GetParameter("?express_name", MySqlDbType.VarChar, 255, item.Express_name), 
				GetParameter("?paymethod", MySqlDbType.VarChar, 32, item.Paymethod), 
				GetParameter("?remark", MySqlDbType.VarChar, 255, item.Remark), 
				GetParameter("?state", MySqlDbType.Enum, -1, item.State?.ToInt64()), 
				GetParameter("?total_express_price", MySqlDbType.Decimal, 10, item.Total_express_price), 
				GetParameter("?total_original_price", MySqlDbType.Decimal, 10, item.Total_original_price), 
				GetParameter("?total_price", MySqlDbType.Decimal, 10, item.Total_price), 
				GetParameter("?update_time", MySqlDbType.DateTime, -1, item.Update_time)};
		}
		public OrderInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as OrderInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new OrderInfo {
				Id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Member_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Code = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Create_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				Express_code = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Express_name = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Paymethod = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Remark = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				State = dr.IsDBNull(++index) ? null : (OrderSTATE?)dr.GetInt64(index), 
				Total_express_price = dr.IsDBNull(++index) ? null : (decimal?)dr.GetDecimal(index), 
				Total_original_price = dr.IsDBNull(++index) ? null : (decimal?)dr.GetDecimal(index), 
				Total_price = dr.IsDBNull(++index) ? null : (decimal?)dr.GetDecimal(index), 
				Update_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index)};
		}
		public SelectBuild<OrderInfo> Select {
			get { return SelectBuild<OrderInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}
		public int DeleteByCode(string Code) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`code` = ?code"), 
				GetParameter("?code", MySqlDbType.VarChar, 32, Code));
		}
		public int DeleteByMember_id(uint? Member_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`member_id` = ?member_id"), 
				GetParameter("?member_id", MySqlDbType.UInt32, 10, Member_id));
		}

		public int Update(OrderInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetMember_id(item.Member_id)
				.SetCode(item.Code)
				.SetCreate_time(item.Create_time)
				.SetExpress_code(item.Express_code)
				.SetExpress_name(item.Express_name)
				.SetPaymethod(item.Paymethod)
				.SetRemark(item.Remark)
				.SetState(item.State)
				.SetTotal_express_price(item.Total_express_price)
				.SetTotal_original_price(item.Total_original_price)
				.SetTotal_price(item.Total_price)
				.SetUpdate_time(item.Update_time).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected OrderInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(OrderInfo item, uint? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Order.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Order.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetMember_id(uint? value) {
				if (_item != null) _item.Member_id = value;
				return this.Set("`member_id`", string.Concat("?member_id_", _parameters.Count), 
					GetParameter(string.Concat("?member_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetCode(string value) {
				if (_item != null) _item.Code = value;
				return this.Set("`code`", string.Concat("?code_", _parameters.Count), 
					GetParameter(string.Concat("?code_", _parameters.Count), MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", string.Concat("?create_time_", _parameters.Count), 
					GetParameter(string.Concat("?create_time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetExpress_code(string value) {
				if (_item != null) _item.Express_code = value;
				return this.Set("`express_code`", string.Concat("?express_code_", _parameters.Count), 
					GetParameter(string.Concat("?express_code_", _parameters.Count), MySqlDbType.VarChar, 64, value));
			}
			public SqlUpdateBuild SetExpress_name(string value) {
				if (_item != null) _item.Express_name = value;
				return this.Set("`express_name`", string.Concat("?express_name_", _parameters.Count), 
					GetParameter(string.Concat("?express_name_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetPaymethod(string value) {
				if (_item != null) _item.Paymethod = value;
				return this.Set("`paymethod`", string.Concat("?paymethod_", _parameters.Count), 
					GetParameter(string.Concat("?paymethod_", _parameters.Count), MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetRemark(string value) {
				if (_item != null) _item.Remark = value;
				return this.Set("`remark`", string.Concat("?remark_", _parameters.Count), 
					GetParameter(string.Concat("?remark_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetState(OrderSTATE? value) {
				if (_item != null) _item.State = value;
				return this.Set("`state`", string.Concat("?state_", _parameters.Count), 
					GetParameter(string.Concat("?state_", _parameters.Count), MySqlDbType.Enum, -1, value?.ToInt64()));
			}
			public SqlUpdateBuild SetTotal_express_price(decimal? value) {
				if (_item != null) _item.Total_express_price = value;
				return this.Set("`total_express_price`", string.Concat("?total_express_price_", _parameters.Count), 
					GetParameter(string.Concat("?total_express_price_", _parameters.Count), MySqlDbType.Decimal, 10, value));
			}
			public SqlUpdateBuild SetTotal_express_priceIncrement(decimal value) {
				if (_item != null) _item.Total_express_price += value;
				return this.Set("`total_express_price`", string.Concat("`total_express_price` + ?total_express_price_", _parameters.Count), 
					GetParameter(string.Concat("?total_express_price_", _parameters.Count), MySqlDbType.Decimal, 10, value));
			}
			public SqlUpdateBuild SetTotal_original_price(decimal? value) {
				if (_item != null) _item.Total_original_price = value;
				return this.Set("`total_original_price`", string.Concat("?total_original_price_", _parameters.Count), 
					GetParameter(string.Concat("?total_original_price_", _parameters.Count), MySqlDbType.Decimal, 10, value));
			}
			public SqlUpdateBuild SetTotal_original_priceIncrement(decimal value) {
				if (_item != null) _item.Total_original_price += value;
				return this.Set("`total_original_price`", string.Concat("`total_original_price` + ?total_original_price_", _parameters.Count), 
					GetParameter(string.Concat("?total_original_price_", _parameters.Count), MySqlDbType.Decimal, 10, value));
			}
			public SqlUpdateBuild SetTotal_price(decimal? value) {
				if (_item != null) _item.Total_price = value;
				return this.Set("`total_price`", string.Concat("?total_price_", _parameters.Count), 
					GetParameter(string.Concat("?total_price_", _parameters.Count), MySqlDbType.Decimal, 10, value));
			}
			public SqlUpdateBuild SetTotal_priceIncrement(decimal value) {
				if (_item != null) _item.Total_price += value;
				return this.Set("`total_price`", string.Concat("`total_price` + ?total_price_", _parameters.Count), 
					GetParameter(string.Concat("?total_price_", _parameters.Count), MySqlDbType.Decimal, 10, value));
			}
			public SqlUpdateBuild SetUpdate_time(DateTime? value) {
				if (_item != null) _item.Update_time = value;
				return this.Set("`update_time`", string.Concat("?update_time_", _parameters.Count), 
					GetParameter(string.Concat("?update_time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
		}
		#endregion

		public OrderInfo Insert(OrderInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public OrderInfo GetItem(uint? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
		public OrderInfo GetItemByCode(string Code) {
			return this.Select.Where("a.`code` = {0}", Code).ToOne();
		}
	}
}