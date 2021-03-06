﻿using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Newstag : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`newstag`";
			internal static readonly string Field = "a.`id`, a.`create_time`, a.`name`, a.`total_news`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `newstag` WHERE ";
			public static readonly string Insert = "INSERT INTO `newstag`(`create_time`, `name`, `total_news`) VALUES(?create_time, ?name, ?total_news); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(NewstagInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?name", MySqlDbType.VarChar, 64, item.Name), 
				GetParameter("?total_news", MySqlDbType.UInt32, 10, item.Total_news)};
		}
		public NewstagInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as NewstagInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			NewstagInfo item = new NewstagInfo();
				if (!dr.IsDBNull(++index)) item.Id = (uint?)dr.GetInt32(index);
				if (!dr.IsDBNull(++index)) item.Create_time = (DateTime?)dr.GetDateTime(index);
				if (!dr.IsDBNull(++index)) item.Name = dr.GetString(index);
				if (!dr.IsDBNull(++index)) item.Total_news = (uint?)dr.GetInt32(index);
			return item;
		}
		#endregion

		public int Delete(uint Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}

		public int Update(NewstagInfo item) {
			return new SqlUpdateBuild(null, item.Id.Value)
				.SetCreate_time(item.Create_time)
				.SetName(item.Name)
				.SetTotal_news(item.Total_news).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected NewstagInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(NewstagInfo item, uint Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Newstag.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Newstag.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", $"?create_time_{_parameters.Count}", 
					GetParameter($"?create_time_{{_parameters.Count}}", MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetName(string value) {
				if (_item != null) _item.Name = value;
				return this.Set("`name`", $"?name_{_parameters.Count}", 
					GetParameter($"?name_{{_parameters.Count}}", MySqlDbType.VarChar, 64, value));
			}
			public SqlUpdateBuild SetTotal_news(uint? value) {
				if (_item != null) _item.Total_news = value;
				return this.Set("`total_news`", $"?total_news_{_parameters.Count}", 
					GetParameter($"?total_news_{{_parameters.Count}}", MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetTotal_newsIncrement(int value) {
				if (_item != null) _item.Total_news = (uint?)((int?)_item.Total_news + value);
				return this.Set("`total_news`", "`total_news` + ?total_news_{_parameters.Count}", 
					GetParameter($"?total_news_{{_parameters.Count}}", MySqlDbType.Int32, 10, value));
			}
		}
		#endregion

		public NewstagInfo Insert(NewstagInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

	}
}