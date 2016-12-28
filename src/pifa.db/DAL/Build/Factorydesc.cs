using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Factorydesc : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`factorydesc`";
			internal static readonly string Field = "a.`factory_id`, a.`address`, a.`content`, a.`url`, a.`username`";
			internal static readonly string Sort = "a.`factory_id`";
			public static readonly string Delete = "DELETE FROM `factorydesc` WHERE ";
			public static readonly string Insert = "INSERT INTO `factorydesc`(`factory_id`, `address`, `content`, `url`, `username`) VALUES(?factory_id, ?address, ?content, ?url, ?username)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(FactorydescInfo item) {
			return new MySqlParameter[] {
				GetParameter("?factory_id", MySqlDbType.UInt32, 10, item.Factory_id), 
				GetParameter("?address", MySqlDbType.VarChar, 255, item.Address), 
				GetParameter("?content", MySqlDbType.Text, -1, item.Content), 
				GetParameter("?url", MySqlDbType.VarChar, 255, item.Url), 
				GetParameter("?username", MySqlDbType.VarChar, 255, item.Username)};
		}
		public FactorydescInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as FactorydescInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new FactorydescInfo {
				Factory_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Address = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Content = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Url = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Username = dr.IsDBNull(++index) ? null : dr.GetString(index)};
		}
		public SelectBuild<FactorydescInfo> Select {
			get { return SelectBuild<FactorydescInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Factory_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`factory_id` = ?factory_id"), 
				GetParameter("?factory_id", MySqlDbType.UInt32, 10, Factory_id));
		}

		public int Update(FactorydescInfo item) {
			return new SqlUpdateBuild(null, item.Factory_id)
				.SetAddress(item.Address)
				.SetContent(item.Content)
				.SetUrl(item.Url)
				.SetUsername(item.Username).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected FactorydescInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(FactorydescInfo item, uint? Factory_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`factory_id` = {0}", Factory_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Factorydesc.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Factorydesc.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetAddress(string value) {
				if (_item != null) _item.Address = value;
				return this.Set("`address`", string.Concat("?address_", _parameters.Count), 
					GetParameter(string.Concat("?address_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetContent(string value) {
				if (_item != null) _item.Content = value;
				return this.Set("`content`", string.Concat("?content_", _parameters.Count), 
					GetParameter(string.Concat("?content_", _parameters.Count), MySqlDbType.Text, -1, value));
			}
			public SqlUpdateBuild SetUrl(string value) {
				if (_item != null) _item.Url = value;
				return this.Set("`url`", string.Concat("?url_", _parameters.Count), 
					GetParameter(string.Concat("?url_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetUsername(string value) {
				if (_item != null) _item.Username = value;
				return this.Set("`username`", string.Concat("?username_", _parameters.Count), 
					GetParameter(string.Concat("?username_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
		}
		#endregion

		public FactorydescInfo Insert(FactorydescInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

		public FactorydescInfo GetItem(uint? Factory_id) {
			return this.Select.Where("a.`factory_id` = {0}", Factory_id).ToOne();
		}
	}
}