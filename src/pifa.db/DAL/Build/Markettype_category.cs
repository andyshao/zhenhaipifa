using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Markettype_category : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`markettype_category`";
			internal static readonly string Field = "a.`category_id`, a.`markettype_id`";
			internal static readonly string Sort = "a.`category_id`, a.`markettype_id`";
			public static readonly string Delete = "DELETE FROM `markettype_category` WHERE ";
			public static readonly string Insert = "INSERT INTO `markettype_category`(`category_id`, `markettype_id`) VALUES(?category_id, ?markettype_id)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Markettype_categoryInfo item) {
			return new MySqlParameter[] {
				GetParameter("?category_id", MySqlDbType.UInt32, 10, item.Category_id), 
				GetParameter("?markettype_id", MySqlDbType.UInt32, 10, item.Markettype_id)};
		}
		public Markettype_categoryInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Markettype_categoryInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			Markettype_categoryInfo item = new Markettype_categoryInfo();
				if (!dr.IsDBNull(++index)) item.Category_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Markettype_id = (uint?)dr.GetInt32(index);
			return item;
		}
		#endregion

		public int Delete(uint Category_id, uint Markettype_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`category_id` = ?category_id AND `markettype_id` = ?markettype_id"), 
				GetParameter("?category_id", MySqlDbType.UInt32, 10, Category_id), 
				GetParameter("?markettype_id", MySqlDbType.UInt32, 10, Markettype_id));
		}
		public int DeleteByCategory_id(uint? Category_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`category_id` = ?category_id"), 
				GetParameter("?category_id", MySqlDbType.UInt32, 10, Category_id));
		}
		public int DeleteByMarkettype_id(uint? Markettype_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`markettype_id` = ?markettype_id"), 
				GetParameter("?markettype_id", MySqlDbType.UInt32, 10, Markettype_id));
		}

		public int Update(Markettype_categoryInfo item) {
			return new SqlUpdateBuild(null, item.Category_id.Value, item.Markettype_id.Value).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Markettype_categoryInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Markettype_categoryInfo item, uint Category_id, uint Markettype_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`category_id` = {0} AND `markettype_id` = {1}", Category_id, Markettype_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Markettype_category.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Markettype_category.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
		}
		#endregion

		public Markettype_categoryInfo Insert(Markettype_categoryInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

	}
}