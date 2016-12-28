using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Pattr : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`pattr`";
			internal static readonly string Field = "a.`id`, a.`category_id`, a.`parent_id`, a.`is_filter`, a.`name`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `pattr` WHERE ";
			public static readonly string Insert = "INSERT INTO `pattr`(`category_id`, `parent_id`, `is_filter`, `name`) VALUES(?category_id, ?parent_id, ?is_filter, ?name); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(PattrInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?category_id", MySqlDbType.UInt32, 10, item.Category_id), 
				GetParameter("?parent_id", MySqlDbType.UInt32, 10, item.Parent_id), 
				GetParameter("?is_filter", MySqlDbType.Bit, 1, item.Is_filter), 
				GetParameter("?name", MySqlDbType.VarChar, 255, item.Name)};
		}
		public PattrInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as PattrInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new PattrInfo {
				Id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Category_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Parent_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Is_filter = dr.IsDBNull(++index) ? null : (bool?)dr.GetBoolean(index), 
				Name = dr.IsDBNull(++index) ? null : dr.GetString(index)};
		}
		public SelectBuild<PattrInfo> Select {
			get { return SelectBuild<PattrInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}
		public int DeleteByCategory_id(uint? Category_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`category_id` = ?category_id"), 
				GetParameter("?category_id", MySqlDbType.UInt32, 10, Category_id));
		}
		public int DeleteByParent_id(uint? Parent_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`parent_id` = ?parent_id"), 
				GetParameter("?parent_id", MySqlDbType.UInt32, 10, Parent_id));
		}

		public int Update(PattrInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetCategory_id(item.Category_id)
				.SetParent_id(item.Parent_id)
				.SetIs_filter(item.Is_filter)
				.SetName(item.Name).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected PattrInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(PattrInfo item, uint? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Pattr.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Pattr.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetCategory_id(uint? value) {
				if (_item != null) _item.Category_id = value;
				return this.Set("`category_id`", string.Concat("?category_id_", _parameters.Count), 
					GetParameter(string.Concat("?category_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetParent_id(uint? value) {
				if (_item != null) _item.Parent_id = value;
				return this.Set("`parent_id`", string.Concat("?parent_id_", _parameters.Count), 
					GetParameter(string.Concat("?parent_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetIs_filter(bool? value) {
				if (_item != null) _item.Is_filter = value;
				return this.Set("`is_filter`", string.Concat("?is_filter_", _parameters.Count), 
					GetParameter(string.Concat("?is_filter_", _parameters.Count), MySqlDbType.Bit, 1, value));
			}
			public SqlUpdateBuild SetName(string value) {
				if (_item != null) _item.Name = value;
				return this.Set("`name`", string.Concat("?name_", _parameters.Count), 
					GetParameter(string.Concat("?name_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
		}
		#endregion

		public PattrInfo Insert(PattrInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public PattrInfo GetItem(uint? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
	}
}