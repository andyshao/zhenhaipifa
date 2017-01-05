using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Productdesc : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`productdesc`";
			internal static readonly string Field = "a.`product_id`, a.`content`";
			internal static readonly string Sort = "a.`product_id`";
			public static readonly string Delete = "DELETE FROM `productdesc` WHERE ";
			public static readonly string Insert = "INSERT INTO `productdesc`(`product_id`, `content`) VALUES(?product_id, ?content)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(ProductdescInfo item) {
			return new MySqlParameter[] {
				GetParameter("?product_id", MySqlDbType.UInt32, 10, item.Product_id), 
				GetParameter("?content", MySqlDbType.Text, -1, item.Content)};
		}
		public ProductdescInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as ProductdescInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			ProductdescInfo item = new ProductdescInfo();
				if (!dr.IsDBNull(++index)) item.Product_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Content = dr.GetString(index);
			return item;
		}
		#endregion

		public int Delete(uint Product_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`product_id` = ?product_id"), 
				GetParameter("?product_id", MySqlDbType.UInt32, 10, Product_id));
		}
		public int DeleteByProduct_id(uint? Product_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`product_id` = ?product_id"), 
				GetParameter("?product_id", MySqlDbType.UInt32, 10, Product_id));
		}

		public int Update(ProductdescInfo item) {
			return new SqlUpdateBuild(null, item.Product_id.Value)
				.SetContent(item.Content).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected ProductdescInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(ProductdescInfo item, uint Product_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`product_id` = {0}", Product_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Productdesc.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Productdesc.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetContent(string value) {
				if (_item != null) _item.Content = value;
				return this.Set("`content`", $"?content_{_parameters.Count}", 
					GetParameter($"?content_{{_parameters.Count}}", MySqlDbType.Text, -1, value));
			}
		}
		#endregion

		public ProductdescInfo Insert(ProductdescInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

	}
}