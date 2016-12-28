using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Shop : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`shop`";
			internal static readonly string Field = "a.`id`, a.`markettype_id`, a.`member_id`, a.`address`, a.`area`, a.`code`, a.`create_time`, a.`fax`, a.`func_switch`+0, a.`icon`+0, a.`kefu`, a.`main_business`, a.`nickname`, a.`state`+0, a.`title`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `shop` WHERE ";
			public static readonly string Insert = "INSERT INTO `shop`(`markettype_id`, `member_id`, `address`, `area`, `code`, `create_time`, `fax`, `func_switch`, `icon`, `kefu`, `main_business`, `nickname`, `state`, `title`) VALUES(?markettype_id, ?member_id, ?address, ?area, ?code, ?create_time, ?fax, ?func_switch, ?icon, ?kefu, ?main_business, ?nickname, ?state, ?title); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(ShopInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?markettype_id", MySqlDbType.UInt32, 10, item.Markettype_id), 
				GetParameter("?member_id", MySqlDbType.UInt32, 10, item.Member_id), 
				GetParameter("?address", MySqlDbType.VarChar, 255, item.Address), 
				GetParameter("?area", MySqlDbType.Decimal, 12, item.Area), 
				GetParameter("?code", MySqlDbType.VarChar, 32, item.Code), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?fax", MySqlDbType.VarChar, 64, item.Fax), 
				GetParameter("?func_switch", MySqlDbType.Set, -1, item.Func_switch?.ToInt64()), 
				GetParameter("?icon", MySqlDbType.Set, -1, item.Icon?.ToInt64()), 
				GetParameter("?kefu", MySqlDbType.VarChar, 255, item.Kefu), 
				GetParameter("?main_business", MySqlDbType.VarChar, 255, item.Main_business), 
				GetParameter("?nickname", MySqlDbType.VarChar, 64, item.Nickname), 
				GetParameter("?state", MySqlDbType.Enum, -1, item.State?.ToInt64()), 
				GetParameter("?title", MySqlDbType.VarChar, 255, item.Title)};
		}
		public ShopInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as ShopInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new ShopInfo {
				Id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Markettype_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Member_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Address = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Area = dr.IsDBNull(++index) ? null : (decimal?)dr.GetDecimal(index), 
				Code = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Create_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				Fax = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Func_switch = dr.IsDBNull(++index) ? null : (ShopFUNC_SWITCH?)dr.GetInt64(index), 
				Icon = dr.IsDBNull(++index) ? null : (ShopICON?)dr.GetInt64(index), 
				Kefu = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Main_business = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Nickname = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				State = dr.IsDBNull(++index) ? null : (ShopSTATE?)dr.GetInt64(index), 
				Title = dr.IsDBNull(++index) ? null : dr.GetString(index)};
		}
		public SelectBuild<ShopInfo> Select {
			get { return SelectBuild<ShopInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}
		public int DeleteByCode(string Code) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`code` = ?code"), 
				GetParameter("?code", MySqlDbType.VarChar, 32, Code));
		}
		public int DeleteByMarkettype_id(uint? Markettype_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`markettype_id` = ?markettype_id"), 
				GetParameter("?markettype_id", MySqlDbType.UInt32, 10, Markettype_id));
		}
		public int DeleteByMember_id(uint? Member_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`member_id` = ?member_id"), 
				GetParameter("?member_id", MySqlDbType.UInt32, 10, Member_id));
		}

		public int Update(ShopInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetMarkettype_id(item.Markettype_id)
				.SetMember_id(item.Member_id)
				.SetAddress(item.Address)
				.SetArea(item.Area)
				.SetCode(item.Code)
				.SetCreate_time(item.Create_time)
				.SetFax(item.Fax)
				.SetFunc_switch(item.Func_switch)
				.SetIcon(item.Icon)
				.SetKefu(item.Kefu)
				.SetMain_business(item.Main_business)
				.SetNickname(item.Nickname)
				.SetState(item.State)
				.SetTitle(item.Title).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected ShopInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(ShopInfo item, uint? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Shop.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Shop.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetMarkettype_id(uint? value) {
				if (_item != null) _item.Markettype_id = value;
				return this.Set("`markettype_id`", string.Concat("?markettype_id_", _parameters.Count), 
					GetParameter(string.Concat("?markettype_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetMember_id(uint? value) {
				if (_item != null) _item.Member_id = value;
				return this.Set("`member_id`", string.Concat("?member_id_", _parameters.Count), 
					GetParameter(string.Concat("?member_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetAddress(string value) {
				if (_item != null) _item.Address = value;
				return this.Set("`address`", string.Concat("?address_", _parameters.Count), 
					GetParameter(string.Concat("?address_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetArea(decimal? value) {
				if (_item != null) _item.Area = value;
				return this.Set("`area`", string.Concat("?area_", _parameters.Count), 
					GetParameter(string.Concat("?area_", _parameters.Count), MySqlDbType.Decimal, 12, value));
			}
			public SqlUpdateBuild SetAreaIncrement(decimal value) {
				if (_item != null) _item.Area += value;
				return this.Set("`area`", string.Concat("`area` + ?area_", _parameters.Count), 
					GetParameter(string.Concat("?area_", _parameters.Count), MySqlDbType.Decimal, 12, value));
			}
			public SqlUpdateBuild SetCode(string value) {
				if (_item != null) _item.Code = value;
				return this.Set("`code`", string.Concat("?code_", _parameters.Count), 
					GetParameter(string.Concat("?code_", _parameters.Count), MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", string.Concat("?create_time_", _parameters.Count), 
					GetParameter(string.Concat("?create_time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetFax(string value) {
				if (_item != null) _item.Fax = value;
				return this.Set("`fax`", string.Concat("?fax_", _parameters.Count), 
					GetParameter(string.Concat("?fax_", _parameters.Count), MySqlDbType.VarChar, 64, value));
			}
			public SqlUpdateBuild SetFunc_switch(ShopFUNC_SWITCH? value) {
				if (_item != null) _item.Func_switch = value;
				return this.Set("`func_switch`", string.Concat("?func_switch_", _parameters.Count), 
					GetParameter(string.Concat("?func_switch_", _parameters.Count), MySqlDbType.Set, -1, value?.ToInt64()));
			}
			public SqlUpdateBuild SetFunc_switchFlag(ShopFUNC_SWITCH value, bool isUnFlag = false) {
				if (_item != null) _item.Func_switch = isUnFlag ? ((_item.Func_switch ?? 0) ^ value) : ((_item.Func_switch ?? 0) | value);
				return this.Set("`func_switch`", string.Format("ifnull(`func_switch`+0,0) {1} ?func_switch_{0}", _parameters.Count, isUnFlag ? '^' : '|'), 
					GetParameter(string.Concat("?func_switch_", _parameters.Count), MySqlDbType.Set, -1, value.ToInt64()));
			}
			public SqlUpdateBuild SetFunc_switchUnFlag(ShopFUNC_SWITCH value) {
				return this.SetFunc_switchFlag(value, true);
			}
			public SqlUpdateBuild SetIcon(ShopICON? value) {
				if (_item != null) _item.Icon = value;
				return this.Set("`icon`", string.Concat("?icon_", _parameters.Count), 
					GetParameter(string.Concat("?icon_", _parameters.Count), MySqlDbType.Set, -1, value?.ToInt64()));
			}
			public SqlUpdateBuild SetIconFlag(ShopICON value, bool isUnFlag = false) {
				if (_item != null) _item.Icon = isUnFlag ? ((_item.Icon ?? 0) ^ value) : ((_item.Icon ?? 0) | value);
				return this.Set("`icon`", string.Format("ifnull(`icon`+0,0) {1} ?icon_{0}", _parameters.Count, isUnFlag ? '^' : '|'), 
					GetParameter(string.Concat("?icon_", _parameters.Count), MySqlDbType.Set, -1, value.ToInt64()));
			}
			public SqlUpdateBuild SetIconUnFlag(ShopICON value) {
				return this.SetIconFlag(value, true);
			}
			public SqlUpdateBuild SetKefu(string value) {
				if (_item != null) _item.Kefu = value;
				return this.Set("`kefu`", string.Concat("?kefu_", _parameters.Count), 
					GetParameter(string.Concat("?kefu_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetMain_business(string value) {
				if (_item != null) _item.Main_business = value;
				return this.Set("`main_business`", string.Concat("?main_business_", _parameters.Count), 
					GetParameter(string.Concat("?main_business_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetNickname(string value) {
				if (_item != null) _item.Nickname = value;
				return this.Set("`nickname`", string.Concat("?nickname_", _parameters.Count), 
					GetParameter(string.Concat("?nickname_", _parameters.Count), MySqlDbType.VarChar, 64, value));
			}
			public SqlUpdateBuild SetState(ShopSTATE? value) {
				if (_item != null) _item.State = value;
				return this.Set("`state`", string.Concat("?state_", _parameters.Count), 
					GetParameter(string.Concat("?state_", _parameters.Count), MySqlDbType.Enum, -1, value?.ToInt64()));
			}
			public SqlUpdateBuild SetTitle(string value) {
				if (_item != null) _item.Title = value;
				return this.Set("`title`", string.Concat("?title_", _parameters.Count), 
					GetParameter(string.Concat("?title_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
		}
		#endregion

		public ShopInfo Insert(ShopInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public ShopInfo GetItem(uint? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
		public ShopInfo GetItemByCode(string Code) {
			return this.Select.Where("a.`code` = {0}", Code).ToOne();
		}
	}
}