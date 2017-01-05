using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Member_security : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`member_security`";
			internal static readonly string Field = "a.`member_id`, a.`password`";
			internal static readonly string Sort = "a.`member_id`";
			public static readonly string Delete = "DELETE FROM `member_security` WHERE ";
			public static readonly string Insert = "INSERT INTO `member_security`(`member_id`, `password`) VALUES(?member_id, ?password)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Member_securityInfo item) {
			return new MySqlParameter[] {
				GetParameter("?member_id", MySqlDbType.UInt32, 10, item.Member_id), 
				GetParameter("?password", MySqlDbType.VarChar, 255, item.Password)};
		}
		public Member_securityInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Member_securityInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			Member_securityInfo item = new Member_securityInfo();
				if (!dr.IsDBNull(++index)) item.Member_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Password = dr.GetString(index);
			return item;
		}
		#endregion

		public int Delete(uint Member_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`member_id` = ?member_id"), 
				GetParameter("?member_id", MySqlDbType.UInt32, 10, Member_id));
		}
		public int DeleteByMember_id(uint? Member_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`member_id` = ?member_id"), 
				GetParameter("?member_id", MySqlDbType.UInt32, 10, Member_id));
		}

		public int Update(Member_securityInfo item) {
			return new SqlUpdateBuild(null, item.Member_id.Value)
				.SetPassword(item.Password).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Member_securityInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Member_securityInfo item, uint Member_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`member_id` = {0}", Member_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Member_security.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Member_security.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetPassword(string value) {
				if (_item != null) _item.Password = value;
				return this.Set("`password`", $"?password_{_parameters.Count}", 
					GetParameter($"?password_{{_parameters.Count}}", MySqlDbType.VarChar, 255, value));
			}
		}
		#endregion

		public Member_securityInfo Insert(Member_securityInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

	}
}