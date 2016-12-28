﻿using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Member_fav_market : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`member_fav_market`";
			internal static readonly string Field = "a.`market_id`, a.`member_id`, a.`create_time`";
			internal static readonly string Sort = "a.`market_id`, a.`member_id`";
			public static readonly string Delete = "DELETE FROM `member_fav_market` WHERE ";
			public static readonly string Insert = "INSERT INTO `member_fav_market`(`market_id`, `member_id`, `create_time`) VALUES(?market_id, ?member_id, ?create_time)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Member_fav_marketInfo item) {
			return new MySqlParameter[] {
				GetParameter("?market_id", MySqlDbType.UInt32, 10, item.Market_id), 
				GetParameter("?member_id", MySqlDbType.UInt32, 10, item.Member_id), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time)};
		}
		public Member_fav_marketInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Member_fav_marketInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new Member_fav_marketInfo {
				Market_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Member_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Create_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index)};
		}
		public SelectBuild<Member_fav_marketInfo> Select {
			get { return SelectBuild<Member_fav_marketInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Market_id, uint? Member_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`market_id` = ?market_id AND `member_id` = ?member_id"), 
				GetParameter("?market_id", MySqlDbType.UInt32, 10, Market_id), 
				GetParameter("?member_id", MySqlDbType.UInt32, 10, Member_id));
		}
		public int DeleteByMarket_id(uint? Market_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`market_id` = ?market_id"), 
				GetParameter("?market_id", MySqlDbType.UInt32, 10, Market_id));
		}
		public int DeleteByMember_id(uint? Member_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`member_id` = ?member_id"), 
				GetParameter("?member_id", MySqlDbType.UInt32, 10, Member_id));
		}

		public int Update(Member_fav_marketInfo item) {
			return new SqlUpdateBuild(null, item.Market_id, item.Member_id)
				.SetCreate_time(item.Create_time).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Member_fav_marketInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Member_fav_marketInfo item, uint? Market_id, uint? Member_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`market_id` = {0} AND `member_id` = {1}", Market_id, Member_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Member_fav_market.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Member_fav_market.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", string.Concat("?create_time_", _parameters.Count), 
					GetParameter(string.Concat("?create_time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
		}
		#endregion

		public Member_fav_marketInfo Insert(Member_fav_marketInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

		public Member_fav_marketInfo GetItem(uint? Market_id, uint? Member_id) {
			return this.Select.Where("a.`market_id` = {0} AND a.`member_id` = {1}", Market_id, Member_id).ToOne();
		}
	}
}