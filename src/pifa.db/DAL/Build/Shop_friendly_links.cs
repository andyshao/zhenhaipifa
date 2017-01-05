using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Shop_friendly_links : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`shop_friendly_links`";
			internal static readonly string Field = "a.`id`, a.`shop_id`, a.`create_time`, a.`sort`, a.`title`, a.`url`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `shop_friendly_links` WHERE ";
			public static readonly string Insert = "INSERT INTO `shop_friendly_links`(`shop_id`, `create_time`, `sort`, `title`, `url`) VALUES(?shop_id, ?create_time, ?sort, ?title, ?url); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Shop_friendly_linksInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?shop_id", MySqlDbType.UInt32, 10, item.Shop_id), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?sort", MySqlDbType.UByte, 3, item.Sort), 
				GetParameter("?title", MySqlDbType.VarChar, 255, item.Title), 
				GetParameter("?url", MySqlDbType.VarChar, 255, item.Url)};
		}
		public Shop_friendly_linksInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Shop_friendly_linksInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			Shop_friendly_linksInfo item = new Shop_friendly_linksInfo();
				if (!dr.IsDBNull(++index)) item.Id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Shop_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Create_time = (DateTime?)dr.GetDateTime(index);
				if (!dr.IsDBNull(++index)) item.Sort = (byte?)dr.GetByte(index);
				if (!dr.IsDBNull(++index)) item.Title = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Url = dr.GetString(index);
			return item;
		}
		#endregion

		public int Delete(uint Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}
		public int DeleteByShop_id(uint? Shop_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`shop_id` = ?shop_id"), 
				GetParameter("?shop_id", MySqlDbType.UInt32, 10, Shop_id));
		}

		public int Update(Shop_friendly_linksInfo item) {
			return new SqlUpdateBuild(null, item.Id.Value)
				.SetShop_id(item.Shop_id)
				.SetCreate_time(item.Create_time)
				.SetSort(item.Sort)
				.SetTitle(item.Title)
				.SetUrl(item.Url).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Shop_friendly_linksInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Shop_friendly_linksInfo item, uint Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Shop_friendly_links.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Shop_friendly_links.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
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
			public SqlUpdateBuild SetUrl(string value) {
				if (_item != null) _item.Url = value;
				return this.Set("`url`", $"?url_{_parameters.Count}", 
					GetParameter($"?url_{{_parameters.Count}}", MySqlDbType.VarChar, 255, value));
			}
		}
		#endregion

		public Shop_friendly_linksInfo Insert(Shop_friendly_linksInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

	}
}