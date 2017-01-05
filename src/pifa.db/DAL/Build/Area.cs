using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Area : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`area`";
			internal static readonly string Field = "a.`id`, a.`parent_id`, a.`name`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `area` WHERE ";
			public static readonly string Insert = "INSERT INTO `area`(`parent_id`, `name`) VALUES(?parent_id, ?name); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(AreaInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?parent_id", MySqlDbType.UInt32, 10, item.Parent_id), 
				GetParameter("?name", MySqlDbType.VarChar, 255, item.Name)};
		}
		public AreaInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as AreaInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			AreaInfo item = new AreaInfo();
				if (!dr.IsDBNull(++index)) item.Id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Parent_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Name = dr.GetString(index);
			return item;
		}
		#endregion

		public int Delete(uint Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}
		public int DeleteByParent_id(uint? Parent_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`parent_id` = ?parent_id"), 
				GetParameter("?parent_id", MySqlDbType.UInt32, 10, Parent_id));
		}

		public int Update(AreaInfo item) {
			return new SqlUpdateBuild(null, item.Id.Value)
				.SetParent_id(item.Parent_id)
				.SetName(item.Name).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected AreaInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(AreaInfo item, uint Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Area.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Area.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetParent_id(uint? value) {
				if (_item != null) _item.Parent_id = value;
				return this.Set("`parent_id`", $"?parent_id_{_parameters.Count}", 
					GetParameter($"?parent_id_{{_parameters.Count}}", MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetName(string value) {
				if (_item != null) _item.Name = value;
				return this.Set("`name`", $"?name_{_parameters.Count}", 
					GetParameter($"?name_{{_parameters.Count}}", MySqlDbType.VarChar, 255, value));
			}
		}
		#endregion

		public AreaInfo Insert(AreaInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

	}
}