﻿using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Member_shop : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`member_shop`";
			internal static readonly string Field = "a.`member_id`, a.`shop_id`, a.`create_time`";
			internal static readonly string Sort = "a.`member_id`, a.`shop_id`";
			public static readonly string Delete = "DELETE FROM `member_shop` WHERE ";
			public static readonly string Insert = "INSERT INTO `member_shop`(`member_id`, `shop_id`, `create_time`) VALUES(?member_id, ?shop_id, ?create_time)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Member_shopInfo item) {
			return new MySqlParameter[] {
				GetParameter("?member_id", MySqlDbType.UInt32, 10, item.Member_id), 
				GetParameter("?shop_id", MySqlDbType.UInt32, 10, item.Shop_id), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time)};
		}
		public Member_shopInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Member_shopInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			Member_shopInfo item = new Member_shopInfo();
				if (!dr.IsDBNull(++index)) item.Member_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Shop_id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Create_time = (DateTime?)dr.GetDateTime(index);
			return item;
		}
		#endregion

		public int Delete(uint Member_id, uint Shop_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`member_id` = ?member_id AND `shop_id` = ?shop_id"), 
				GetParameter("?member_id", MySqlDbType.UInt32, 10, Member_id), 
				GetParameter("?shop_id", MySqlDbType.UInt32, 10, Shop_id));
		}
		public int DeleteByMember_id(uint? Member_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`member_id` = ?member_id"), 
				GetParameter("?member_id", MySqlDbType.UInt32, 10, Member_id));
		}
		public int DeleteByShop_id(uint? Shop_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`shop_id` = ?shop_id"), 
				GetParameter("?shop_id", MySqlDbType.UInt32, 10, Shop_id));
		}

		public int Update(Member_shopInfo item) {
			return new SqlUpdateBuild(null, item.Member_id.Value, item.Shop_id.Value)
				.SetCreate_time(item.Create_time).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Member_shopInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Member_shopInfo item, uint Member_id, uint Shop_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`member_id` = {0} AND `shop_id` = {1}", Member_id, Shop_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Member_shop.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Member_shop.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", $"?create_time_{_parameters.Count}", 
					GetParameter($"?create_time_{{_parameters.Count}}", MySqlDbType.DateTime, -1, value));
			}
		}
		#endregion

		public Member_shopInfo Insert(Member_shopInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

	}
}