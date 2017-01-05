using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Product : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`product`";
			internal static readonly string Field = "a.`id`, a.`category_id`, a.`shop_id`, a.`create_time`, a.`icon`+0, a.`price`, a.`stock`, a.`title`, a.`unit`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `product` WHERE ";
			public static readonly string Insert = "INSERT INTO `product`(`category_id`, `shop_id`, `create_time`, `icon`, `price`, `stock`, `title`, `unit`) VALUES(?category_id, ?shop_id, ?create_time, ?icon, ?price, ?stock, ?title, ?unit); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(ProductInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?category_id", MySqlDbType.UInt32, 10, item.Category_id), 
				GetParameter("?shop_id", MySqlDbType.UInt32, 10, item.Shop_id), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?icon", MySqlDbType.Set, -1, item.Icon?.ToInt64()), 
				GetParameter("?price", MySqlDbType.Decimal, 10, item.Price), 
				GetParameter("?stock", MySqlDbType.UInt32, 10, item.Stock), 
				GetParameter("?title", MySqlDbType.VarChar, 255, item.Title), 
				GetParameter("?unit", MySqlDbType.VarChar, 8, item.Unit)};
		}
		public ProductInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as ProductInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			ProductInfo item = new ProductInfo();
				if (!dr.IsDBNull(++index)) item.Id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Category_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Shop_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Create_time = (DateTime?)dr.GetDateTime(index);
				if (!dr.IsDBNull(++index)) item.Icon = (ProductICON?)dr.GetInt64(index);
				if (!dr.IsDBNull(++index)) item.Price = (decimal?)dr.GetDecimal(index);
				if (!dr.IsDBNull(++index)) item.Stock = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Title = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Unit = dr.GetString(index);
			return item;
		}
		#endregion

		public int Delete(uint Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}
		public int DeleteByCategory_id(uint? Category_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`category_id` = ?category_id"), 
				GetParameter("?category_id", MySqlDbType.UInt32, 10, Category_id));
		}
		public int DeleteByShop_id(uint? Shop_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`shop_id` = ?shop_id"), 
				GetParameter("?shop_id", MySqlDbType.UInt32, 10, Shop_id));
		}

		public int Update(ProductInfo item) {
			return new SqlUpdateBuild(null, item.Id.Value)
				.SetCategory_id(item.Category_id)
				.SetShop_id(item.Shop_id)
				.SetCreate_time(item.Create_time)
				.SetIcon(item.Icon)
				.SetPrice(item.Price)
				.SetStock(item.Stock)
				.SetTitle(item.Title)
				.SetUnit(item.Unit).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected ProductInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(ProductInfo item, uint Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Product.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Product.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetCategory_id(uint? value) {
				if (_item != null) _item.Category_id = value;
				return this.Set("`category_id`", $"?category_id_{_parameters.Count}", 
					GetParameter($"?category_id_{{_parameters.Count}}", MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetShop_id(uint? value) {
				if (_item != null) _item.Shop_id = value;
				return this.Set("`shop_id`", $"?shop_id_{_parameters.Count}", 
					GetParameter($"?shop_id_{{_parameters.Count}}", MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", $"?create_time_{_parameters.Count}", 
					GetParameter($"?create_time_{{_parameters.Count}}", MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetIcon(ProductICON? value) {
				if (_item != null) _item.Icon = value;
				return this.Set("`icon`", $"?icon_{_parameters.Count}", 
					GetParameter($"?icon_{{_parameters.Count}}", MySqlDbType.Set, -1, value?.ToInt64()));
			}
			public SqlUpdateBuild SetIconFlag(ProductICON value, bool isUnFlag = false) {
				if (_item != null) _item.Icon = isUnFlag ? ((_item.Icon ?? 0) ^ value) : ((_item.Icon ?? 0) | value);
				return this.Set("`icon`", "ifnull(`icon`+0,0) {(isUnFlag ? '^' : '|')} ?icon_{_parameters.Count}", 
					GetParameter(string.Concat("?icon_", _parameters.Count), MySqlDbType.Set, -1, value.ToInt64()));
			}
			public SqlUpdateBuild SetIconUnFlag(ProductICON value) {
				return this.SetIconFlag(value, true);
			}
			public SqlUpdateBuild SetPrice(decimal? value) {
				if (_item != null) _item.Price = value;
				return this.Set("`price`", $"?price_{_parameters.Count}", 
					GetParameter($"?price_{{_parameters.Count}}", MySqlDbType.Decimal, 10, value));
			}
			public SqlUpdateBuild SetPriceIncrement(decimal value) {
				if (_item != null) _item.Price += value;
				return this.Set("`price`", "`price` + ?price_{_parameters.Count}", 
					GetParameter($"?price_{{_parameters.Count}}", MySqlDbType.Decimal, 10, value));
			}
			public SqlUpdateBuild SetStock(uint? value) {
				if (_item != null) _item.Stock = value;
				return this.Set("`stock`", $"?stock_{_parameters.Count}", 
					GetParameter($"?stock_{{_parameters.Count}}", MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetStockIncrement(int value) {
				if (_item != null) _item.Stock = (uint?)((int?)_item.Stock + value);
				return this.Set("`stock`", "`stock` + ?stock_{_parameters.Count}", 
					GetParameter($"?stock_{{_parameters.Count}}", MySqlDbType.Int32, 10, value));
			}
			public SqlUpdateBuild SetTitle(string value) {
				if (_item != null) _item.Title = value;
				return this.Set("`title`", $"?title_{_parameters.Count}", 
					GetParameter($"?title_{{_parameters.Count}}", MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetUnit(string value) {
				if (_item != null) _item.Unit = value;
				return this.Set("`unit`", $"?unit_{_parameters.Count}", 
					GetParameter($"?unit_{{_parameters.Count}}", MySqlDbType.VarChar, 8, value));
			}
		}
		#endregion

		public ProductInfo Insert(ProductInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

	}
}