using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Faqdesc : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`faqdesc`";
			internal static readonly string Field = "a.`faq_id`, a.`content`";
			internal static readonly string Sort = "a.`faq_id`";
			public static readonly string Delete = "DELETE FROM `faqdesc` WHERE ";
			public static readonly string Insert = "INSERT INTO `faqdesc`(`faq_id`, `content`) VALUES(?faq_id, ?content)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(FaqdescInfo item) {
			return new MySqlParameter[] {
				GetParameter("?faq_id", MySqlDbType.UInt32, 10, item.Faq_id), 
				GetParameter("?content", MySqlDbType.Text, -1, item.Content)};
		}
		public FaqdescInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as FaqdescInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new FaqdescInfo {
				Faq_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Content = dr.IsDBNull(++index) ? null : dr.GetString(index)};
		}
		public SelectBuild<FaqdescInfo> Select {
			get { return SelectBuild<FaqdescInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Faq_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`faq_id` = ?faq_id"), 
				GetParameter("?faq_id", MySqlDbType.UInt32, 10, Faq_id));
		}

		public int Update(FaqdescInfo item) {
			return new SqlUpdateBuild(null, item.Faq_id)
				.SetContent(item.Content).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected FaqdescInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(FaqdescInfo item, uint? Faq_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`faq_id` = {0}", Faq_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Faqdesc.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Faqdesc.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetContent(string value) {
				if (_item != null) _item.Content = value;
				return this.Set("`content`", string.Concat("?content_", _parameters.Count), 
					GetParameter(string.Concat("?content_", _parameters.Count), MySqlDbType.Text, -1, value));
			}
		}
		#endregion

		public FaqdescInfo Insert(FaqdescInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

		public FaqdescInfo GetItem(uint? Faq_id) {
			return this.Select.Where("a.`faq_id` = {0}", Faq_id).ToOne();
		}
	}
}