using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Productitem : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`productitem`";
			internal static readonly string Field = "a.`id`, a.`product_id`, a.`img_url`, a.`name`, a.`original_price`, a.`price`, a.`stock`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `productitem` WHERE ";
			public static readonly string Insert = "INSERT INTO `productitem`(`product_id`, `img_url`, `name`, `original_price`, `price`, `stock`) VALUES(?product_id, ?img_url, ?name, ?original_price, ?price, ?stock); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(ProductitemInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?product_id", MySqlDbType.UInt32, 10, item.Product_id), 
				GetParameter("?img_url", MySqlDbType.VarChar, 255, item.Img_url), 
				GetParameter("?name", MySqlDbType.VarChar, 255, item.Name), 
				GetParameter("?original_price", MySqlDbType.Decimal, 10, item.Original_price), 
				GetParameter("?price", MySqlDbType.Decimal, 10, item.Price), 
				GetParameter("?stock", MySqlDbType.UInt32, 10, item.Stock)};
		}
		public ProductitemInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as ProductitemInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new ProductitemInfo {
				Id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Product_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Img_url = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Name = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Original_price = dr.IsDBNull(++index) ? null : (decimal?)dr.GetDecimal(index), 
				Price = dr.IsDBNull(++index) ? null : (decimal?)dr.GetDecimal(index), 
				Stock = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index)};
		}
		public SelectBuild<ProductitemInfo> Select {
			get { return SelectBuild<ProductitemInfo>.From(this, SqlHelper.Instance); }
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

		public int Update(ProductitemInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetProduct_id(item.Product_id)
				.SetImg_url(item.Img_url)
				.SetName(item.Name)
				.SetOriginal_price(item.Original_price)
				.SetPrice(item.Price)
				.SetStock(item.Stock).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected ProductitemInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(ProductitemInfo item, uint? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Productitem.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Productitem.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetProduct_id(uint? value) {
				if (_item != null) _item.Product_id = value;
				return this.Set("`product_id`", string.Concat("?product_id_", _parameters.Count), 
					GetParameter(string.Concat("?product_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetImg_url(string value) {
				if (_item != null) _item.Img_url = value;
				return this.Set("`img_url`", string.Concat("?img_url_", _parameters.Count), 
					GetParameter(string.Concat("?img_url_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetName(string value) {
				if (_item != null) _item.Name = value;
				return this.Set("`name`", string.Concat("?name_", _parameters.Count), 
					GetParameter(string.Concat("?name_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetOriginal_price(decimal? value) {
				if (_item != null) _item.Original_price = value;
				return this.Set("`original_price`", string.Concat("?original_price_", _parameters.Count), 
					GetParameter(string.Concat("?original_price_", _parameters.Count), MySqlDbType.Decimal, 10, value));
			}
			public SqlUpdateBuild SetOriginal_priceIncrement(decimal value) {
				if (_item != null) _item.Original_price += value;
				return this.Set("`original_price`", string.Concat("`original_price` + ?original_price_", _parameters.Count), 
					GetParameter(string.Concat("?original_price_", _parameters.Count), MySqlDbType.Decimal, 10, value));
			}
			public SqlUpdateBuild SetPrice(decimal? value) {
				if (_item != null) _item.Price = value;
				return this.Set("`price`", string.Concat("?price_", _parameters.Count), 
					GetParameter(string.Concat("?price_", _parameters.Count), MySqlDbType.Decimal, 10, value));
			}
			public SqlUpdateBuild SetPriceIncrement(decimal value) {
				if (_item != null) _item.Price += value;
				return this.Set("`price`", string.Concat("`price` + ?price_", _parameters.Count), 
					GetParameter(string.Concat("?price_", _parameters.Count), MySqlDbType.Decimal, 10, value));
			}
			public SqlUpdateBuild SetStock(uint? value) {
				if (_item != null) _item.Stock = value;
				return this.Set("`stock`", string.Concat("?stock_", _parameters.Count), 
					GetParameter(string.Concat("?stock_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetStockIncrement(int value) {
				if (_item != null) _item.Stock = (uint?)((int?)_item.Stock + value);
				return this.Set("`stock`", string.Concat("`stock` + ?stock_", _parameters.Count), 
					GetParameter(string.Concat("?stock_", _parameters.Count), MySqlDbType.Int32, 10, value));
			}
		}
		#endregion

		public ProductitemInfo Insert(ProductitemInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public ProductitemInfo GetItem(uint? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
	}
}