using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Member : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`member`";
			internal static readonly string Field = "a.`id`, a.`create_time`, a.`email`, a.`lastlogin_time`, a.`telphone`, a.`username`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `member` WHERE ";
			public static readonly string Insert = "INSERT INTO `member`(`id`, `create_time`, `email`, `lastlogin_time`, `telphone`, `username`) VALUES(?id, ?create_time, ?email, ?lastlogin_time, ?telphone, ?username)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(MemberInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?email", MySqlDbType.VarChar, 32, item.Email), 
				GetParameter("?lastlogin_time", MySqlDbType.DateTime, -1, item.Lastlogin_time), 
				GetParameter("?telphone", MySqlDbType.VarChar, 32, item.Telphone), 
				GetParameter("?username", MySqlDbType.VarChar, 32, item.Username)};
		}
		public MemberInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as MemberInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			MemberInfo item = new MemberInfo();
				if (!dr.IsDBNull(++index)) item.Id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Create_time = (DateTime?)dr.GetDateTime(index);
				if (!dr.IsDBNull(++index)) item.Email = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Lastlogin_time = (DateTime?)dr.GetDateTime(index);
				if (!dr.IsDBNull(++index)) item.Telphone = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Username = dr.GetString(index);
			return item;
		}
		#endregion

		public int Delete(uint Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}
		public int DeleteByUsername(string Username) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`username` = ?username"), 
				GetParameter("?username", MySqlDbType.VarChar, 32, Username));
		}
		public int DeleteByTelphone(string Telphone) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`telphone` = ?telphone"), 
				GetParameter("?telphone", MySqlDbType.VarChar, 32, Telphone));
		}
		public int DeleteByEmail(string Email) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`email` = ?email"), 
				GetParameter("?email", MySqlDbType.VarChar, 32, Email));
		}

		public int Update(MemberInfo item) {
			return new SqlUpdateBuild(null, item.Id.Value)
				.SetCreate_time(item.Create_time)
				.SetEmail(item.Email)
				.SetLastlogin_time(item.Lastlogin_time)
				.SetTelphone(item.Telphone)
				.SetUsername(item.Username).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected MemberInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(MemberInfo item, uint Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Member.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Member.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", $"?create_time_{_parameters.Count}", 
					GetParameter($"?create_time_{{_parameters.Count}}", MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetEmail(string value) {
				if (_item != null) _item.Email = value;
				return this.Set("`email`", $"?email_{_parameters.Count}", 
					GetParameter($"?email_{{_parameters.Count}}", MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetLastlogin_time(DateTime? value) {
				if (_item != null) _item.Lastlogin_time = value;
				return this.Set("`lastlogin_time`", $"?lastlogin_time_{_parameters.Count}", 
					GetParameter($"?lastlogin_time_{{_parameters.Count}}", MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetTelphone(string value) {
				if (_item != null) _item.Telphone = value;
				return this.Set("`telphone`", $"?telphone_{_parameters.Count}", 
					GetParameter($"?telphone_{{_parameters.Count}}", MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetUsername(string value) {
				if (_item != null) _item.Username = value;
				return this.Set("`username`", $"?username_{_parameters.Count}", 
					GetParameter($"?username_{{_parameters.Count}}", MySqlDbType.VarChar, 32, value));
			}
		}
		#endregion

		public MemberInfo Insert(MemberInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

	}
}