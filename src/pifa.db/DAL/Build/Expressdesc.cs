using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Expressdesc : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`expressdesc`";
			internal static readonly string Field = "a.`express_id`, a.`content`";
			internal static readonly string Sort = "a.`express_id`";
			public static readonly string Delete = "DELETE FROM `expressdesc` WHERE ";
			public static readonly string Insert = "INSERT INTO `expressdesc`(`express_id`, `content`) VALUES(?express_id, ?content)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(ExpressdescInfo item) {
			return new MySqlParameter[] {
				GetParameter("?express_id", MySqlDbType.UInt32, 10, item.Express_id), 
				GetParameter("?content", MySqlDbType.Text, -1, item.Content)};
		}
		public ExpressdescInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as ExpressdescInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			ExpressdescInfo item = new ExpressdescInfo();
				if (!dr.IsDBNull(++index)) item.Express_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Content = dr.GetString(index);
			return item;
		}
		#endregion

		public int Delete(uint Express_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`express_id` = ?express_id"), 
				GetParameter("?express_id", MySqlDbType.UInt32, 10, Express_id));
		}
		public int DeleteByExpress_id(uint? Express_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`express_id` = ?express_id"), 
				GetParameter("?express_id", MySqlDbType.UInt32, 10, Express_id));
		}

		public int Update(ExpressdescInfo item) {
			return new SqlUpdateBuild(null, item.Express_id.Value)
				.SetContent(item.Content).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected ExpressdescInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(ExpressdescInfo item, uint Express_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`express_id` = {0}", Express_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Expressdesc.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Expressdesc.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetContent(string value) {
				if (_item != null) _item.Content = value;
				return this.Set("`content`", $"?content_{_parameters.Count}", 
					GetParameter($"?content_{{_parameters.Count}}", MySqlDbType.Text, -1, value));
			}
		}
		#endregion

		public ExpressdescInfo Insert(ExpressdescInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

	}
}