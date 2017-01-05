using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Product_attr : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`product_attr`";
			internal static readonly string Field = "a.`pattr_id`, a.`product_id`, a.`value`";
			internal static readonly string Sort = "a.`pattr_id`, a.`product_id`";
			public static readonly string Delete = "DELETE FROM `product_attr` WHERE ";
			public static readonly string Insert = "INSERT INTO `product_attr`(`pattr_id`, `product_id`, `value`) VALUES(?pattr_id, ?product_id, ?value)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Product_attrInfo item) {
			return new MySqlParameter[] {
				GetParameter("?pattr_id", MySqlDbType.UInt32, 10, item.Pattr_id), 
				GetParameter("?product_id", MySqlDbType.UInt32, 10, item.Product_id), 
				GetParameter("?value", MySqlDbType.VarChar, 255, item.Value)};
		}
		public Product_attrInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Product_attrInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			Product_attrInfo item = new Product_attrInfo();
				if (!dr.IsDBNull(++index)) item.Pattr_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Product_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Value = dr.GetString(index);
			return item;
		}
		#endregion

		public int Delete(uint Pattr_id, uint Product_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`pattr_id` = ?pattr_id AND `product_id` = ?product_id"), 
				GetParameter("?pattr_id", MySqlDbType.UInt32, 10, Pattr_id), 
				GetParameter("?product_id", MySqlDbType.UInt32, 10, Product_id));
		}
		public int DeleteByPattr_id(uint? Pattr_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`pattr_id` = ?pattr_id"), 
				GetParameter("?pattr_id", MySqlDbType.UInt32, 10, Pattr_id));
		}
		public int DeleteByProduct_id(uint? Product_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`product_id` = ?product_id"), 
				GetParameter("?product_id", MySqlDbType.UInt32, 10, Product_id));
		}

		public int Update(Product_attrInfo item) {
			return new SqlUpdateBuild(null, item.Pattr_id.Value, item.Product_id.Value)
				.SetValue(item.Value).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Product_attrInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Product_attrInfo item, uint Pattr_id, uint Product_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`pattr_id` = {0} AND `product_id` = {1}", Pattr_id, Product_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Product_attr.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Product_attr.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetValue(string value) {
				if (_item != null) _item.Value = value;
				return this.Set("`value`", $"?value_{_parameters.Count}", 
					GetParameter($"?value_{{_parameters.Count}}", MySqlDbType.VarChar, 255, value));
			}
		}
		#endregion

		public Product_attrInfo Insert(Product_attrInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

	}
}