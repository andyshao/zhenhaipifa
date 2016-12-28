using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Product_buyrule : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`product_buyrule`";
			internal static readonly string Field = "a.`id`, a.`product_id`, a.`discount`, a.`ordering_end`, a.`ordering_start`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `product_buyrule` WHERE ";
			public static readonly string Insert = "INSERT INTO `product_buyrule`(`product_id`, `discount`, `ordering_end`, `ordering_start`) VALUES(?product_id, ?discount, ?ordering_end, ?ordering_start); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Product_buyruleInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?product_id", MySqlDbType.UInt32, 10, item.Product_id), 
				GetParameter("?discount", MySqlDbType.UInt32, 10, item.Discount), 
				GetParameter("?ordering_end", MySqlDbType.UInt32, 10, item.Ordering_end), 
				GetParameter("?ordering_start", MySqlDbType.UInt32, 10, item.Ordering_start)};
		}
		public Product_buyruleInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Product_buyruleInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new Product_buyruleInfo {
				Id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Product_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Discount = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Ordering_end = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Ordering_start = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index)};
		}
		public SelectBuild<Product_buyruleInfo> Select {
			get { return SelectBuild<Product_buyruleInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}
		public int DeleteByProduct_id(uint? Product_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`product_id` = ?product_id"), 
				GetParameter("?product_id", MySqlDbType.UInt32, 10, Product_id));
		}

		public int Update(Product_buyruleInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetProduct_id(item.Product_id)
				.SetDiscount(item.Discount)
				.SetOrdering_end(item.Ordering_end)
				.SetOrdering_start(item.Ordering_start).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Product_buyruleInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Product_buyruleInfo item, uint? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Product_buyrule.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Product_buyrule.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetProduct_id(uint? value) {
				if (_item != null) _item.Product_id = value;
				return this.Set("`product_id`", string.Concat("?product_id_", _parameters.Count), 
					GetParameter(string.Concat("?product_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetDiscount(uint? value) {
				if (_item != null) _item.Discount = value;
				return this.Set("`discount`", string.Concat("?discount_", _parameters.Count), 
					GetParameter(string.Concat("?discount_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetDiscountIncrement(int value) {
				if (_item != null) _item.Discount = (uint?)((int?)_item.Discount + value);
				return this.Set("`discount`", string.Concat("`discount` + ?discount_", _parameters.Count), 
					GetParameter(string.Concat("?discount_", _parameters.Count), MySqlDbType.Int32, 10, value));
			}
			public SqlUpdateBuild SetOrdering_end(uint? value) {
				if (_item != null) _item.Ordering_end = value;
				return this.Set("`ordering_end`", string.Concat("?ordering_end_", _parameters.Count), 
					GetParameter(string.Concat("?ordering_end_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetOrdering_endIncrement(int value) {
				if (_item != null) _item.Ordering_end = (uint?)((int?)_item.Ordering_end + value);
				return this.Set("`ordering_end`", string.Concat("`ordering_end` + ?ordering_end_", _parameters.Count), 
					GetParameter(string.Concat("?ordering_end_", _parameters.Count), MySqlDbType.Int32, 10, value));
			}
			public SqlUpdateBuild SetOrdering_start(uint? value) {
				if (_item != null) _item.Ordering_start = value;
				return this.Set("`ordering_start`", string.Concat("?ordering_start_", _parameters.Count), 
					GetParameter(string.Concat("?ordering_start_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetOrdering_startIncrement(int value) {
				if (_item != null) _item.Ordering_start = (uint?)((int?)_item.Ordering_start + value);
				return this.Set("`ordering_start`", string.Concat("`ordering_start` + ?ordering_start_", _parameters.Count), 
					GetParameter(string.Concat("?ordering_start_", _parameters.Count), MySqlDbType.Int32, 10, value));
			}
		}
		#endregion

		public Product_buyruleInfo Insert(Product_buyruleInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public Product_buyruleInfo GetItem(uint? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
	}
}