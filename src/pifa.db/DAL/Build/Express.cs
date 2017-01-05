using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Express : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`express`";
			internal static readonly string Field = "a.`id`, a.`area_id`, a.`address`, a.`create_time`, a.`service_features`, a.`telphone`, a.`title`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `express` WHERE ";
			public static readonly string Insert = "INSERT INTO `express`(`area_id`, `address`, `create_time`, `service_features`, `telphone`, `title`) VALUES(?area_id, ?address, ?create_time, ?service_features, ?telphone, ?title); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(ExpressInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?area_id", MySqlDbType.UInt32, 10, item.Area_id), 
				GetParameter("?address", MySqlDbType.VarChar, 255, item.Address), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?service_features", MySqlDbType.VarChar, 255, item.Service_features), 
				GetParameter("?telphone", MySqlDbType.VarChar, 255, item.Telphone), 
				GetParameter("?title", MySqlDbType.VarChar, 255, item.Title)};
		}
		public ExpressInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as ExpressInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			ExpressInfo item = new ExpressInfo();
				if (!dr.IsDBNull(++index)) item.Id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Area_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Address = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Create_time = (DateTime?)dr.GetDateTime(index);
				if (!dr.IsDBNull(++index)) item.Service_features = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Telphone = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Title = dr.GetString(index);
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

		public int Update(ExpressInfo item) {
			return new SqlUpdateBuild(null, item.Id.Value)
				.SetArea_id(item.Area_id)
				.SetAddress(item.Address)
				.SetCreate_time(item.Create_time)
				.SetService_features(item.Service_features)
				.SetTelphone(item.Telphone)
				.SetTitle(item.Title).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected ExpressInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(ExpressInfo item, uint Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Express.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Express.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetArea_id(uint? value) {
				if (_item != null) _item.Area_id = value;
				return this.Set("`area_id`", $"?area_id_{_parameters.Count}", 
					GetParameter($"?area_id_{{_parameters.Count}}", MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetAddress(string value) {
				if (_item != null) _item.Address = value;
				return this.Set("`address`", $"?address_{_parameters.Count}", 
					GetParameter($"?address_{{_parameters.Count}}", MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", $"?create_time_{_parameters.Count}", 
					GetParameter($"?create_time_{{_parameters.Count}}", MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetService_features(string value) {
				if (_item != null) _item.Service_features = value;
				return this.Set("`service_features`", $"?service_features_{_parameters.Count}", 
					GetParameter($"?service_features_{{_parameters.Count}}", MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetTelphone(string value) {
				if (_item != null) _item.Telphone = value;
				return this.Set("`telphone`", $"?telphone_{_parameters.Count}", 
					GetParameter($"?telphone_{{_parameters.Count}}", MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetTitle(string value) {
				if (_item != null) _item.Title = value;
				return this.Set("`title`", $"?title_{_parameters.Count}", 
					GetParameter($"?title_{{_parameters.Count}}", MySqlDbType.VarChar, 255, value));
			}
		}
		#endregion

		public ExpressInfo Insert(ExpressInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

	}
}