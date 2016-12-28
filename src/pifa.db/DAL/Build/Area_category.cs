using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Area_category : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`area_category`";
			internal static readonly string Field = "a.`area_id`, a.`category_id`";
			internal static readonly string Sort = "a.`area_id`, a.`category_id`";
			public static readonly string Delete = "DELETE FROM `area_category` WHERE ";
			public static readonly string Insert = "INSERT INTO `area_category`(`area_id`, `category_id`) VALUES(?area_id, ?category_id)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Area_categoryInfo item) {
			return new MySqlParameter[] {
				GetParameter("?area_id", MySqlDbType.UInt32, 10, item.Area_id), 
				GetParameter("?category_id", MySqlDbType.UInt32, 10, item.Category_id)};
		}
		public Area_categoryInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Area_categoryInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new Area_categoryInfo {
				Area_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Category_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index)};
		}
		public SelectBuild<Area_categoryInfo> Select {
			get { return SelectBuild<Area_categoryInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Area_id, uint? Category_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`area_id` = ?area_id AND `category_id` = ?category_id"), 
				GetParameter("?area_id", MySqlDbType.UInt32, 10, Area_id), 
				GetParameter("?category_id", MySqlDbType.UInt32, 10, Category_id));
		}
		public int DeleteByArea_id(uint? Area_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`area_id` = ?area_id"), 
				GetParameter("?area_id", MySqlDbType.UInt32, 10, Area_id));
		}
		public int DeleteByCategory_id(uint? Category_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`category_id` = ?category_id"), 
				GetParameter("?category_id", MySqlDbType.UInt32, 10, Category_id));
		}

		public int Update(Area_categoryInfo item) {
			return new SqlUpdateBuild(null, item.Area_id, item.Category_id).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Area_categoryInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Area_categoryInfo item, uint? Area_id, uint? Category_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`area_id` = {0} AND `category_id` = {1}", Area_id, Category_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Area_category.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Area_category.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
		}
		#endregion

		public Area_categoryInfo Insert(Area_categoryInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

		public Area_categoryInfo GetItem(uint? Area_id, uint? Category_id) {
			return this.Select.Where("a.`area_id` = {0} AND a.`category_id` = {1}", Area_id, Category_id).ToOne();
		}
	}
}