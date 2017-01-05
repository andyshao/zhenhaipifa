using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Newsdesc : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`newsdesc`";
			internal static readonly string Field = "a.`news_id`, a.`content`";
			internal static readonly string Sort = "a.`news_id`";
			public static readonly string Delete = "DELETE FROM `newsdesc` WHERE ";
			public static readonly string Insert = "INSERT INTO `newsdesc`(`news_id`, `content`) VALUES(?news_id, ?content)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(NewsdescInfo item) {
			return new MySqlParameter[] {
				GetParameter("?news_id", MySqlDbType.UInt32, 10, item.News_id), 
				GetParameter("?content", MySqlDbType.Text, -1, item.Content)};
		}
		public NewsdescInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as NewsdescInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			NewsdescInfo item = new NewsdescInfo();
				if (!dr.IsDBNull(++index)) item.News_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Content = dr.GetString(index);
			return item;
		}
		#endregion

		public int Delete(uint News_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`news_id` = ?news_id"), 
				GetParameter("?news_id", MySqlDbType.UInt32, 10, News_id));
		}
		public int DeleteByNews_id(uint? News_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`news_id` = ?news_id"), 
				GetParameter("?news_id", MySqlDbType.UInt32, 10, News_id));
		}

		public int Update(NewsdescInfo item) {
			return new SqlUpdateBuild(null, item.News_id.Value)
				.SetContent(item.Content).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected NewsdescInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(NewsdescInfo item, uint News_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`news_id` = {0}", News_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Newsdesc.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Newsdesc.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
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

		public NewsdescInfo Insert(NewsdescInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

	}
}