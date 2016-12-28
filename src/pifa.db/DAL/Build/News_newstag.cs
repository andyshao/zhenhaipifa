using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class News_newstag : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`news_newstag`";
			internal static readonly string Field = "a.`news_id`, a.`newstag_id`";
			internal static readonly string Sort = "a.`news_id`, a.`newstag_id`";
			public static readonly string Delete = "DELETE FROM `news_newstag` WHERE ";
			public static readonly string Insert = "INSERT INTO `news_newstag`(`news_id`, `newstag_id`) VALUES(?news_id, ?newstag_id)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(News_newstagInfo item) {
			return new MySqlParameter[] {
				GetParameter("?news_id", MySqlDbType.UInt32, 10, item.News_id), 
				GetParameter("?newstag_id", MySqlDbType.UInt32, 10, item.Newstag_id)};
		}
		public News_newstagInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as News_newstagInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new News_newstagInfo {
				News_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Newstag_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index)};
		}
		public SelectBuild<News_newstagInfo> Select {
			get { return SelectBuild<News_newstagInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? News_id, uint? Newstag_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`news_id` = ?news_id AND `newstag_id` = ?newstag_id"), 
				GetParameter("?news_id", MySqlDbType.UInt32, 10, News_id), 
				GetParameter("?newstag_id", MySqlDbType.UInt32, 10, Newstag_id));
		}
		public int DeleteByNewstag_id(uint? Newstag_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`newstag_id` = ?newstag_id"), 
				GetParameter("?newstag_id", MySqlDbType.UInt32, 10, Newstag_id));
		}
		public int DeleteByNews_id(uint? News_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`news_id` = ?news_id"), 
				GetParameter("?news_id", MySqlDbType.UInt32, 10, News_id));
		}

		public int Update(News_newstagInfo item) {
			return new SqlUpdateBuild(null, item.News_id, item.Newstag_id).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected News_newstagInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(News_newstagInfo item, uint? News_id, uint? Newstag_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`news_id` = {0} AND `newstag_id` = {1}", News_id, Newstag_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.News_newstag.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.News_newstag.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
		}
		#endregion

		public News_newstagInfo Insert(News_newstagInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

		public News_newstagInfo GetItem(uint? News_id, uint? Newstag_id) {
			return this.Select.Where("a.`news_id` = {0} AND a.`newstag_id` = {1}", News_id, Newstag_id).ToOne();
		}
	}
}