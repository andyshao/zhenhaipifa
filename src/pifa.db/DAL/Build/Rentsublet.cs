using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Rentsublet : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`rentsublet`";
			internal static readonly string Field = "a.`id`, a.`market_id`, a.`create_time`, a.`price`, a.`type`+0";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `rentsublet` WHERE ";
			public static readonly string Insert = "INSERT INTO `rentsublet`(`market_id`, `create_time`, `price`, `type`) VALUES(?market_id, ?create_time, ?price, ?type); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(RentsubletInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?market_id", MySqlDbType.UInt32, 10, item.Market_id), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?price", MySqlDbType.Decimal, 10, item.Price), 
				GetParameter("?type", MySqlDbType.Enum, -1, item.Type?.ToInt64())};
		}
		public RentsubletInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as RentsubletInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new RentsubletInfo {
				Id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Market_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Create_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				Price = dr.IsDBNull(++index) ? null : (decimal?)dr.GetDecimal(index), 
				Type = dr.IsDBNull(++index) ? null : (RentsubletTYPE?)dr.GetInt64(index)};
		}
		public SelectBuild<RentsubletInfo> Select {
			get { return SelectBuild<RentsubletInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}
		public int DeleteByMarket_id(uint? Market_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`market_id` = ?market_id"), 
				GetParameter("?market_id", MySqlDbType.UInt32, 10, Market_id));
		}

		public int Update(RentsubletInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetMarket_id(item.Market_id)
				.SetCreate_time(item.Create_time)
				.SetPrice(item.Price)
				.SetType(item.Type).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected RentsubletInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(RentsubletInfo item, uint? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Rentsublet.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Rentsublet.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetMarket_id(uint? value) {
				if (_item != null) _item.Market_id = value;
				return this.Set("`market_id`", string.Concat("?market_id_", _parameters.Count), 
					GetParameter(string.Concat("?market_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", string.Concat("?create_time_", _parameters.Count), 
					GetParameter(string.Concat("?create_time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
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
			public SqlUpdateBuild SetType(RentsubletTYPE? value) {
				if (_item != null) _item.Type = value;
				return this.Set("`type`", string.Concat("?type_", _parameters.Count), 
					GetParameter(string.Concat("?type_", _parameters.Count), MySqlDbType.Enum, -1, value?.ToInt64()));
			}
		}
		#endregion

		public RentsubletInfo Insert(RentsubletInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public RentsubletInfo GetItem(uint? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
	}
}