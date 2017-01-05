using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Factory : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`factory`";
			internal static readonly string Field = "a.`id`, a.`area_id`, a.`capacity`, a.`create_time`, a.`main_business`, a.`min_order`, a.`process_cost`, a.`sampling_period`, a.`sampling_price`, a.`telphone`, a.`title`, a.`turn_single_time`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `factory` WHERE ";
			public static readonly string Insert = "INSERT INTO `factory`(`area_id`, `capacity`, `create_time`, `main_business`, `min_order`, `process_cost`, `sampling_period`, `sampling_price`, `telphone`, `title`, `turn_single_time`) VALUES(?area_id, ?capacity, ?create_time, ?main_business, ?min_order, ?process_cost, ?sampling_period, ?sampling_price, ?telphone, ?title, ?turn_single_time); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(FactoryInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?area_id", MySqlDbType.UInt32, 10, item.Area_id), 
				GetParameter("?capacity", MySqlDbType.VarChar, 32, item.Capacity), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?main_business", MySqlDbType.VarChar, 255, item.Main_business), 
				GetParameter("?min_order", MySqlDbType.VarChar, 16, item.Min_order), 
				GetParameter("?process_cost", MySqlDbType.VarChar, 16, item.Process_cost), 
				GetParameter("?sampling_period", MySqlDbType.VarChar, 16, item.Sampling_period), 
				GetParameter("?sampling_price", MySqlDbType.VarChar, 16, item.Sampling_price), 
				GetParameter("?telphone", MySqlDbType.VarChar, 32, item.Telphone), 
				GetParameter("?title", MySqlDbType.VarChar, 255, item.Title), 
				GetParameter("?turn_single_time", MySqlDbType.VarChar, 16, item.Turn_single_time)};
		}
		public FactoryInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as FactoryInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			FactoryInfo item = new FactoryInfo();
				if (!dr.IsDBNull(++index)) item.Id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Area_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Capacity = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Create_time = (DateTime?)dr.GetDateTime(index);
				if (!dr.IsDBNull(++index)) item.Main_business = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Min_order = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Process_cost = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Sampling_period = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Sampling_price = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Telphone = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Title = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Turn_single_time = dr.GetString(index);
			return item;
		}
		#endregion

		public int Delete(uint Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}
		public int DeleteByArea_id(uint? Area_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`area_id` = ?area_id"), 
				GetParameter("?area_id", MySqlDbType.UInt32, 10, Area_id));
		}

		public int Update(FactoryInfo item) {
			return new SqlUpdateBuild(null, item.Id.Value)
				.SetArea_id(item.Area_id)
				.SetCapacity(item.Capacity)
				.SetCreate_time(item.Create_time)
				.SetMain_business(item.Main_business)
				.SetMin_order(item.Min_order)
				.SetProcess_cost(item.Process_cost)
				.SetSampling_period(item.Sampling_period)
				.SetSampling_price(item.Sampling_price)
				.SetTelphone(item.Telphone)
				.SetTitle(item.Title)
				.SetTurn_single_time(item.Turn_single_time).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected FactoryInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(FactoryInfo item, uint Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Factory.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Factory.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetArea_id(uint? value) {
				if (_item != null) _item.Area_id = value;
				return this.Set("`area_id`", $"?area_id_{_parameters.Count}", 
					GetParameter($"?area_id_{{_parameters.Count}}", MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetCapacity(string value) {
				if (_item != null) _item.Capacity = value;
				return this.Set("`capacity`", $"?capacity_{_parameters.Count}", 
					GetParameter($"?capacity_{{_parameters.Count}}", MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", $"?create_time_{_parameters.Count}", 
					GetParameter($"?create_time_{{_parameters.Count}}", MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetMain_business(string value) {
				if (_item != null) _item.Main_business = value;
				return this.Set("`main_business`", $"?main_business_{_parameters.Count}", 
					GetParameter($"?main_business_{{_parameters.Count}}", MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetMin_order(string value) {
				if (_item != null) _item.Min_order = value;
				return this.Set("`min_order`", $"?min_order_{_parameters.Count}", 
					GetParameter($"?min_order_{{_parameters.Count}}", MySqlDbType.VarChar, 16, value));
			}
			public SqlUpdateBuild SetProcess_cost(string value) {
				if (_item != null) _item.Process_cost = value;
				return this.Set("`process_cost`", $"?process_cost_{_parameters.Count}", 
					GetParameter($"?process_cost_{{_parameters.Count}}", MySqlDbType.VarChar, 16, value));
			}
			public SqlUpdateBuild SetSampling_period(string value) {
				if (_item != null) _item.Sampling_period = value;
				return this.Set("`sampling_period`", $"?sampling_period_{_parameters.Count}", 
					GetParameter($"?sampling_period_{{_parameters.Count}}", MySqlDbType.VarChar, 16, value));
			}
			public SqlUpdateBuild SetSampling_price(string value) {
				if (_item != null) _item.Sampling_price = value;
				return this.Set("`sampling_price`", $"?sampling_price_{_parameters.Count}", 
					GetParameter($"?sampling_price_{{_parameters.Count}}", MySqlDbType.VarChar, 16, value));
			}
			public SqlUpdateBuild SetTelphone(string value) {
				if (_item != null) _item.Telphone = value;
				return this.Set("`telphone`", $"?telphone_{_parameters.Count}", 
					GetParameter($"?telphone_{{_parameters.Count}}", MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetTitle(string value) {
				if (_item != null) _item.Title = value;
				return this.Set("`title`", $"?title_{_parameters.Count}", 
					GetParameter($"?title_{{_parameters.Count}}", MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetTurn_single_time(string value) {
				if (_item != null) _item.Turn_single_time = value;
				return this.Set("`turn_single_time`", $"?turn_single_time_{_parameters.Count}", 
					GetParameter($"?turn_single_time_{{_parameters.Count}}", MySqlDbType.VarChar, 16, value));
			}
		}
		#endregion

		public FactoryInfo Insert(FactoryInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

	}
}