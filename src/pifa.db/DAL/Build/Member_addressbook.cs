using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Member_addressbook : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`member_addressbook`";
			internal static readonly string Field = "a.`id`, a.`member_id`, a.`address`, a.`create_time`, a.`is_default`, a.`name`, a.`tel`, a.`telphone`, a.`zip`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `member_addressbook` WHERE ";
			public static readonly string Insert = "INSERT INTO `member_addressbook`(`member_id`, `address`, `create_time`, `is_default`, `name`, `tel`, `telphone`, `zip`) VALUES(?member_id, ?address, ?create_time, ?is_default, ?name, ?tel, ?telphone, ?zip); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Member_addressbookInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?member_id", MySqlDbType.UInt32, 10, item.Member_id), 
				GetParameter("?address", MySqlDbType.VarChar, 255, item.Address), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?is_default", MySqlDbType.Bit, 1, item.Is_default), 
				GetParameter("?name", MySqlDbType.VarChar, 32, item.Name), 
				GetParameter("?tel", MySqlDbType.VarChar, 32, item.Tel), 
				GetParameter("?telphone", MySqlDbType.VarChar, 32, item.Telphone), 
				GetParameter("?zip", MySqlDbType.VarChar, 16, item.Zip)};
		}
		public Member_addressbookInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Member_addressbookInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			Member_addressbookInfo item = new Member_addressbookInfo();
				if (!dr.IsDBNull(++index)) item.Id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Member_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Address = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Create_time = (DateTime?)dr.GetDateTime(index);
				if (!dr.IsDBNull(++index)) item.Is_default = (bool?)dr.GetBoolean(index);
				if (!dr.IsDBNull(++index)) item.Name = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Tel = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Telphone = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Zip = dr.GetString(index);
			return item;
		}
		#endregion

		public int Delete(uint Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}
		public int DeleteByMember_id(uint? Member_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`member_id` = ?member_id"), 
				GetParameter("?member_id", MySqlDbType.UInt32, 10, Member_id));
		}

		public int Update(Member_addressbookInfo item) {
			return new SqlUpdateBuild(null, item.Id.Value)
				.SetMember_id(item.Member_id)
				.SetAddress(item.Address)
				.SetCreate_time(item.Create_time)
				.SetIs_default(item.Is_default)
				.SetName(item.Name)
				.SetTel(item.Tel)
				.SetTelphone(item.Telphone)
				.SetZip(item.Zip).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Member_addressbookInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Member_addressbookInfo item, uint Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Member_addressbook.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Member_addressbook.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetMember_id(uint? value) {
				if (_item != null) _item.Member_id = value;
				return this.Set("`member_id`", $"?member_id_{_parameters.Count}", 
					GetParameter($"?member_id_{{_parameters.Count}}", MySqlDbType.UInt32, 10, value));
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
			public SqlUpdateBuild SetIs_default(bool? value) {
				if (_item != null) _item.Is_default = value;
				return this.Set("`is_default`", $"?is_default_{_parameters.Count}", 
					GetParameter($"?is_default_{{_parameters.Count}}", MySqlDbType.Bit, 1, value));
			}
			public SqlUpdateBuild SetName(string value) {
				if (_item != null) _item.Name = value;
				return this.Set("`name`", $"?name_{_parameters.Count}", 
					GetParameter($"?name_{{_parameters.Count}}", MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetTel(string value) {
				if (_item != null) _item.Tel = value;
				return this.Set("`tel`", $"?tel_{_parameters.Count}", 
					GetParameter($"?tel_{{_parameters.Count}}", MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetTelphone(string value) {
				if (_item != null) _item.Telphone = value;
				return this.Set("`telphone`", $"?telphone_{_parameters.Count}", 
					GetParameter($"?telphone_{{_parameters.Count}}", MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetZip(string value) {
				if (_item != null) _item.Zip = value;
				return this.Set("`zip`", $"?zip_{_parameters.Count}", 
					GetParameter($"?zip_{{_parameters.Count}}", MySqlDbType.VarChar, 16, value));
			}
		}
		#endregion

		public Member_addressbookInfo Insert(Member_addressbookInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

	}
}