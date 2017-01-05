using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Factory_franchising : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`factory_franchising`";
			internal static readonly string Field = "a.`factory_id`, a.`franchising_id`";
			internal static readonly string Sort = "a.`factory_id`, a.`franchising_id`";
			public static readonly string Delete = "DELETE FROM `factory_franchising` WHERE ";
			public static readonly string Insert = "INSERT INTO `factory_franchising`(`factory_id`, `franchising_id`) VALUES(?factory_id, ?franchising_id)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Factory_franchisingInfo item) {
			return new MySqlParameter[] {
				GetParameter("?factory_id", MySqlDbType.UInt32, 10, item.Factory_id), 
				GetParameter("?franchising_id", MySqlDbType.UInt32, 10, item.Franchising_id)};
		}
		public Factory_franchisingInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Factory_franchisingInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			Factory_franchisingInfo item = new Factory_franchisingInfo();
				if (!dr.IsDBNull(++index)) item.Factory_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Franchising_id = (uint?)dr.GetInt32(index);
			return item;
		}
		#endregion

		public int Delete(uint Factory_id, uint Franchising_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`factory_id` = ?factory_id AND `franchising_id` = ?franchising_id"), 
				GetParameter("?factory_id", MySqlDbType.UInt32, 10, Factory_id), 
				GetParameter("?franchising_id", MySqlDbType.UInt32, 10, Franchising_id));
		}
		public int DeleteByFactory_id(uint? Factory_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`factory_id` = ?factory_id"), 
				GetParameter("?factory_id", MySqlDbType.UInt32, 10, Factory_id));
		}
		public int DeleteByFranchising_id(uint? Franchising_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`franchising_id` = ?franchising_id"), 
				GetParameter("?franchising_id", MySqlDbType.UInt32, 10, Franchising_id));
		}

		public int Update(Factory_franchisingInfo item) {
			return new SqlUpdateBuild(null, item.Factory_id.Value, item.Franchising_id.Value).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Factory_franchisingInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Factory_franchisingInfo item, uint Factory_id, uint Franchising_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`factory_id` = {0} AND `franchising_id` = {1}", Factory_id, Franchising_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Factory_franchising.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Factory_franchising.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
		}
		#endregion

		public Factory_franchisingInfo Insert(Factory_franchisingInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

	}
}