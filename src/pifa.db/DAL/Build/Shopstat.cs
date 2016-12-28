using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Shopstat : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`shopstat`";
			internal static readonly string Field = "a.`shop_id`, a.`today_fav`, a.`today_session`, a.`today_share`, a.`total_fav`, a.`total_session`, a.`total_share`";
			internal static readonly string Sort = "a.`shop_id`";
			public static readonly string Delete = "DELETE FROM `shopstat` WHERE ";
			public static readonly string Insert = "INSERT INTO `shopstat`(`shop_id`, `today_fav`, `today_session`, `today_share`, `total_fav`, `total_session`, `total_share`) VALUES(?shop_id, ?today_fav, ?today_session, ?today_share, ?total_fav, ?total_session, ?total_share)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(ShopstatInfo item) {
			return new MySqlParameter[] {
				GetParameter("?shop_id", MySqlDbType.UInt32, 10, item.Shop_id), 
				GetParameter("?today_fav", MySqlDbType.UInt32, 10, item.Today_fav), 
				GetParameter("?today_session", MySqlDbType.UInt32, 10, item.Today_session), 
				GetParameter("?today_share", MySqlDbType.UInt32, 10, item.Today_share), 
				GetParameter("?total_fav", MySqlDbType.UInt32, 10, item.Total_fav), 
				GetParameter("?total_session", MySqlDbType.UInt32, 10, item.Total_session), 
				GetParameter("?total_share", MySqlDbType.UInt32, 10, item.Total_share)};
		}
		public ShopstatInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as ShopstatInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new ShopstatInfo {
				Shop_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Today_fav = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Today_session = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Today_share = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Total_fav = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Total_session = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Total_share = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index)};
		}
		public SelectBuild<ShopstatInfo> Select {
			get { return SelectBuild<ShopstatInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Shop_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`shop_id` = ?shop_id"), 
				GetParameter("?shop_id", MySqlDbType.UInt32, 10, Shop_id));
		}

		public int Update(ShopstatInfo item) {
			return new SqlUpdateBuild(null, item.Shop_id)
				.SetToday_fav(item.Today_fav)
				.SetToday_session(item.Today_session)
				.SetToday_share(item.Today_share)
				.SetTotal_fav(item.Total_fav)
				.SetTotal_session(item.Total_session)
				.SetTotal_share(item.Total_share).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected ShopstatInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(ShopstatInfo item, uint? Shop_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`shop_id` = {0}", Shop_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Shopstat.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Shopstat.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetToday_fav(uint? value) {
				if (_item != null) _item.Today_fav = value;
				return this.Set("`today_fav`", string.Concat("?today_fav_", _parameters.Count), 
					GetParameter(string.Concat("?today_fav_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetToday_favIncrement(int value) {
				if (_item != null) _item.Today_fav = (uint?)((int?)_item.Today_fav + value);
				return this.Set("`today_fav`", string.Concat("`today_fav` + ?today_fav_", _parameters.Count), 
					GetParameter(string.Concat("?today_fav_", _parameters.Count), MySqlDbType.Int32, 10, value));
			}
			public SqlUpdateBuild SetToday_session(uint? value) {
				if (_item != null) _item.Today_session = value;
				return this.Set("`today_session`", string.Concat("?today_session_", _parameters.Count), 
					GetParameter(string.Concat("?today_session_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetToday_sessionIncrement(int value) {
				if (_item != null) _item.Today_session = (uint?)((int?)_item.Today_session + value);
				return this.Set("`today_session`", string.Concat("`today_session` + ?today_session_", _parameters.Count), 
					GetParameter(string.Concat("?today_session_", _parameters.Count), MySqlDbType.Int32, 10, value));
			}
			public SqlUpdateBuild SetToday_share(uint? value) {
				if (_item != null) _item.Today_share = value;
				return this.Set("`today_share`", string.Concat("?today_share_", _parameters.Count), 
					GetParameter(string.Concat("?today_share_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetToday_shareIncrement(int value) {
				if (_item != null) _item.Today_share = (uint?)((int?)_item.Today_share + value);
				return this.Set("`today_share`", string.Concat("`today_share` + ?today_share_", _parameters.Count), 
					GetParameter(string.Concat("?today_share_", _parameters.Count), MySqlDbType.Int32, 10, value));
			}
			public SqlUpdateBuild SetTotal_fav(uint? value) {
				if (_item != null) _item.Total_fav = value;
				return this.Set("`total_fav`", string.Concat("?total_fav_", _parameters.Count), 
					GetParameter(string.Concat("?total_fav_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetTotal_favIncrement(int value) {
				if (_item != null) _item.Total_fav = (uint?)((int?)_item.Total_fav + value);
				return this.Set("`total_fav`", string.Concat("`total_fav` + ?total_fav_", _parameters.Count), 
					GetParameter(string.Concat("?total_fav_", _parameters.Count), MySqlDbType.Int32, 10, value));
			}
			public SqlUpdateBuild SetTotal_session(uint? value) {
				if (_item != null) _item.Total_session = value;
				return this.Set("`total_session`", string.Concat("?total_session_", _parameters.Count), 
					GetParameter(string.Concat("?total_session_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetTotal_sessionIncrement(int value) {
				if (_item != null) _item.Total_session = (uint?)((int?)_item.Total_session + value);
				return this.Set("`total_session`", string.Concat("`total_session` + ?total_session_", _parameters.Count), 
					GetParameter(string.Concat("?total_session_", _parameters.Count), MySqlDbType.Int32, 10, value));
			}
			public SqlUpdateBuild SetTotal_share(uint? value) {
				if (_item != null) _item.Total_share = value;
				return this.Set("`total_share`", string.Concat("?total_share_", _parameters.Count), 
					GetParameter(string.Concat("?total_share_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetTotal_shareIncrement(int value) {
				if (_item != null) _item.Total_share = (uint?)((int?)_item.Total_share + value);
				return this.Set("`total_share`", string.Concat("`total_share` + ?total_share_", _parameters.Count), 
					GetParameter(string.Concat("?total_share_", _parameters.Count), MySqlDbType.Int32, 10, value));
			}
		}
		#endregion

		public ShopstatInfo Insert(ShopstatInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

		public ShopstatInfo GetItem(uint? Shop_id) {
			return this.Select.Where("a.`shop_id` = {0}", Shop_id).ToOne();
		}
	}
}