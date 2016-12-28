using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Order_address : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`order_address`";
			internal static readonly string Field = "a.`order_id`, a.`address`, a.`name`, a.`tel`, a.`telphone`, a.`zip`";
			internal static readonly string Sort = "a.`order_id`";
			public static readonly string Delete = "DELETE FROM `order_address` WHERE ";
			public static readonly string Insert = "INSERT INTO `order_address`(`order_id`, `address`, `name`, `tel`, `telphone`, `zip`) VALUES(?order_id, ?address, ?name, ?tel, ?telphone, ?zip)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Order_addressInfo item) {
			return new MySqlParameter[] {
				GetParameter("?order_id", MySqlDbType.UInt32, 10, item.Order_id), 
				GetParameter("?address", MySqlDbType.VarChar, 255, item.Address), 
				GetParameter("?name", MySqlDbType.VarChar, 32, item.Name), 
				GetParameter("?tel", MySqlDbType.VarChar, 32, item.Tel), 
				GetParameter("?telphone", MySqlDbType.VarChar, 32, item.Telphone), 
				GetParameter("?zip", MySqlDbType.VarChar, 16, item.Zip)};
		}
		public Order_addressInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Order_addressInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new Order_addressInfo {
				Order_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Address = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Name = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Tel = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Telphone = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Zip = dr.IsDBNull(++index) ? null : dr.GetString(index)};
		}
		public SelectBuild<Order_addressInfo> Select {
			get { return SelectBuild<Order_addressInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Order_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`order_id` = ?order_id"), 
				GetParameter("?order_id", MySqlDbType.UInt32, 10, Order_id));
		}

		public int Update(Order_addressInfo item) {
			return new SqlUpdateBuild(null, item.Order_id)
				.SetAddress(item.Address)
				.SetName(item.Name)
				.SetTel(item.Tel)
				.SetTelphone(item.Telphone)
				.SetZip(item.Zip).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Order_addressInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Order_addressInfo item, uint? Order_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`order_id` = {0}", Order_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Order_address.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Order_address.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetAddress(string value) {
				if (_item != null) _item.Address = value;
				return this.Set("`address`", string.Concat("?address_", _parameters.Count), 
					GetParameter(string.Concat("?address_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetName(string value) {
				if (_item != null) _item.Name = value;
				return this.Set("`name`", string.Concat("?name_", _parameters.Count), 
					GetParameter(string.Concat("?name_", _parameters.Count), MySqlDbType.VarChar, 32, value));
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
			public SqlUpdateBuild SetZip(string value) {
				if (_item != null) _item.Zip = value;
				return this.Set("`zip`", string.Concat("?zip_", _parameters.Count), 
					GetParameter(string.Concat("?zip_", _parameters.Count), MySqlDbType.VarChar, 16, value));
			}
		}
		#endregion

		public Order_addressInfo Insert(Order_addressInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

		public Order_addressInfo GetItem(uint? Order_id) {
			return this.Select.Where("a.`order_id` = {0}", Order_id).ToOne();
		}
	}
}