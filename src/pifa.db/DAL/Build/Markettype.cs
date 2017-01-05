using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Markettype : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`markettype`";
			internal static readonly string Field = "a.`id`, a.`market_id`, a.`parent_id`, a.`sort`, a.`title`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `markettype` WHERE ";
			public static readonly string Insert = "INSERT INTO `markettype`(`market_id`, `parent_id`, `sort`, `title`) VALUES(?market_id, ?parent_id, ?sort, ?title); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(MarkettypeInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?market_id", MySqlDbType.UInt32, 10, item.Market_id), 
				GetParameter("?parent_id", MySqlDbType.UInt32, 10, item.Parent_id), 
				GetParameter("?sort", MySqlDbType.UByte, 3, item.Sort), 
				GetParameter("?title", MySqlDbType.VarChar, 255, item.Title)};
		}
		public MarkettypeInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as MarkettypeInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			MarkettypeInfo item = new MarkettypeInfo();
				if (!dr.IsDBNull(++index)) item.Id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Market_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Parent_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Sort = (byte?)dr.GetByte(index);
				if (!dr.IsDBNull(++index)) item.Title = dr.GetString(index);
			return item;
		}
		#endregion

		public int Delete(uint Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}
		public int DeleteByParent_id(uint? Parent_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`parent_id` = ?parent_id"), 
				GetParameter("?parent_id", MySqlDbType.UInt32, 10, Parent_id));
		}
		public int DeleteByMarket_id(uint? Market_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`market_id` = ?market_id"), 
				GetParameter("?market_id", MySqlDbType.UInt32, 10, Market_id));
		}

		public int Update(MarkettypeInfo item) {
			return new SqlUpdateBuild(null, item.Id.Value)
				.SetMarket_id(item.Market_id)
				.SetParent_id(item.Parent_id)
				.SetSort(item.Sort)
				.SetTitle(item.Title).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected MarkettypeInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(MarkettypeInfo item, uint Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Markettype.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Markettype.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetMarket_id(uint? value) {
				if (_item != null) _item.Market_id = value;
				return this.Set("`market_id`", $"?market_id_{_parameters.Count}", 
					GetParameter($"?market_id_{{_parameters.Count}}", MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetParent_id(uint? value) {
				if (_item != null) _item.Parent_id = value;
				return this.Set("`parent_id`", $"?parent_id_{_parameters.Count}", 
					GetParameter($"?parent_id_{{_parameters.Count}}", MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetSort(byte? value) {
				if (_item != null) _item.Sort = value;
				return this.Set("`sort`", $"?sort_{_parameters.Count}", 
					GetParameter($"?sort_{{_parameters.Count}}", MySqlDbType.UByte, 3, value));
			}
			public SqlUpdateBuild SetSortIncrement(byte value) {
				if (_item != null) _item.Sort += value;
				return this.Set("`sort`", "`sort` + ?sort_{_parameters.Count}", 
					GetParameter($"?sort_{{_parameters.Count}}", MySqlDbType.Byte, 3, value));
			}
			public SqlUpdateBuild SetTitle(string value) {
				if (_item != null) _item.Title = value;
				return this.Set("`title`", $"?title_{_parameters.Count}", 
					GetParameter($"?title_{{_parameters.Count}}", MySqlDbType.VarChar, 255, value));
			}
		}
		#endregion

		public MarkettypeInfo Insert(MarkettypeInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

	}
}