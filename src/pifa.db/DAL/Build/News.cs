using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class News : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`news`";
			internal static readonly string Field = "a.`id`, a.`create_time`, a.`intro`, a.`pv`, a.`source`, a.`state`+0, a.`title`, a.`update_time`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `news` WHERE ";
			public static readonly string Insert = "INSERT INTO `news`(`create_time`, `intro`, `pv`, `source`, `state`, `title`, `update_time`) VALUES(?create_time, ?intro, ?pv, ?source, ?state, ?title, ?update_time); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(NewsInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?intro", MySqlDbType.VarChar, 1000, item.Intro), 
				GetParameter("?pv", MySqlDbType.UInt32, 10, item.Pv), 
				GetParameter("?source", MySqlDbType.VarChar, 32, item.Source), 
				GetParameter("?state", MySqlDbType.Enum, -1, item.State?.ToInt64()), 
				GetParameter("?title", MySqlDbType.VarChar, 255, item.Title), 
				GetParameter("?update_time", MySqlDbType.DateTime, -1, item.Update_time)};
		}
		public NewsInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as NewsInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new NewsInfo {
				Id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Create_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				Intro = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Pv = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Source = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				State = dr.IsDBNull(++index) ? null : (NewsSTATE?)dr.GetInt64(index), 
				Title = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Update_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index)};
		}
		public SelectBuild<NewsInfo> Select {
			get { return SelectBuild<NewsInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}

		public int Update(NewsInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetCreate_time(item.Create_time)
				.SetIntro(item.Intro)
				.SetPv(item.Pv)
				.SetSource(item.Source)
				.SetState(item.State)
				.SetTitle(item.Title)
				.SetUpdate_time(item.Update_time).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected NewsInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(NewsInfo item, uint? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.News.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.News.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", string.Concat("?create_time_", _parameters.Count), 
					GetParameter(string.Concat("?create_time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetIntro(string value) {
				if (_item != null) _item.Intro = value;
				return this.Set("`intro`", string.Concat("?intro_", _parameters.Count), 
					GetParameter(string.Concat("?intro_", _parameters.Count), MySqlDbType.VarChar, 1000, value));
			}
			public SqlUpdateBuild SetPv(uint? value) {
				if (_item != null) _item.Pv = value;
				return this.Set("`pv`", string.Concat("?pv_", _parameters.Count), 
					GetParameter(string.Concat("?pv_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetPvIncrement(int value) {
				if (_item != null) _item.Pv = (uint?)((int?)_item.Pv + value);
				return this.Set("`pv`", string.Concat("`pv` + ?pv_", _parameters.Count), 
					GetParameter(string.Concat("?pv_", _parameters.Count), MySqlDbType.Int32, 10, value));
			}
			public SqlUpdateBuild SetSource(string value) {
				if (_item != null) _item.Source = value;
				return this.Set("`source`", string.Concat("?source_", _parameters.Count), 
					GetParameter(string.Concat("?source_", _parameters.Count), MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetState(NewsSTATE? value) {
				if (_item != null) _item.State = value;
				return this.Set("`state`", string.Concat("?state_", _parameters.Count), 
					GetParameter(string.Concat("?state_", _parameters.Count), MySqlDbType.Enum, -1, value?.ToInt64()));
			}
			public SqlUpdateBuild SetTitle(string value) {
				if (_item != null) _item.Title = value;
				return this.Set("`title`", string.Concat("?title_", _parameters.Count), 
					GetParameter(string.Concat("?title_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetUpdate_time(DateTime? value) {
				if (_item != null) _item.Update_time = value;
				return this.Set("`update_time`", string.Concat("?update_time_", _parameters.Count), 
					GetParameter(string.Concat("?update_time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
		}
		#endregion

		public NewsInfo Insert(NewsInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public NewsInfo GetItem(uint? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
	}
}