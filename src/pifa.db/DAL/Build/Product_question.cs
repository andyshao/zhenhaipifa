using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Product_question : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`product_question`";
			internal static readonly string Field = "a.`id`, a.`member_id`, a.`parent_id`, a.`product_id`, a.`content`, a.`create_time`, a.`email`, a.`name`, a.`state`+0";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `product_question` WHERE ";
			public static readonly string Insert = "INSERT INTO `product_question`(`member_id`, `parent_id`, `product_id`, `content`, `create_time`, `email`, `name`, `state`) VALUES(?member_id, ?parent_id, ?product_id, ?content, ?create_time, ?email, ?name, ?state); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Product_questionInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?member_id", MySqlDbType.UInt32, 10, item.Member_id), 
				GetParameter("?parent_id", MySqlDbType.UInt32, 10, item.Parent_id), 
				GetParameter("?product_id", MySqlDbType.UInt32, 10, item.Product_id), 
				GetParameter("?content", MySqlDbType.VarChar, 255, item.Content), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?email", MySqlDbType.VarChar, 255, item.Email), 
				GetParameter("?name", MySqlDbType.VarChar, 255, item.Name), 
				GetParameter("?state", MySqlDbType.Enum, -1, item.State?.ToInt64())};
		}
		public Product_questionInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Product_questionInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new Product_questionInfo {
				Id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Member_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Parent_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Product_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Content = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Create_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				Email = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Name = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				State = dr.IsDBNull(++index) ? null : (Product_questionSTATE?)dr.GetInt64(index)};
		}
		public SelectBuild<Product_questionInfo> Select {
			get { return SelectBuild<Product_questionInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}
		public int DeleteByMember_id(uint? Member_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`member_id` = ?member_id"), 
				GetParameter("?member_id", MySqlDbType.UInt32, 10, Member_id));
		}
		public int DeleteByProduct_id(uint? Product_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`product_id` = ?product_id"), 
				GetParameter("?product_id", MySqlDbType.UInt32, 10, Product_id));
		}
		public int DeleteByParent_id(uint? Parent_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`parent_id` = ?parent_id"), 
				GetParameter("?parent_id", MySqlDbType.UInt32, 10, Parent_id));
		}

		public int Update(Product_questionInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetMember_id(item.Member_id)
				.SetParent_id(item.Parent_id)
				.SetProduct_id(item.Product_id)
				.SetContent(item.Content)
				.SetCreate_time(item.Create_time)
				.SetEmail(item.Email)
				.SetName(item.Name)
				.SetState(item.State).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Product_questionInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Product_questionInfo item, uint? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Product_question.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Product_question.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetMember_id(uint? value) {
				if (_item != null) _item.Member_id = value;
				return this.Set("`member_id`", string.Concat("?member_id_", _parameters.Count), 
					GetParameter(string.Concat("?member_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetParent_id(uint? value) {
				if (_item != null) _item.Parent_id = value;
				return this.Set("`parent_id`", string.Concat("?parent_id_", _parameters.Count), 
					GetParameter(string.Concat("?parent_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetProduct_id(uint? value) {
				if (_item != null) _item.Product_id = value;
				return this.Set("`product_id`", string.Concat("?product_id_", _parameters.Count), 
					GetParameter(string.Concat("?product_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetContent(string value) {
				if (_item != null) _item.Content = value;
				return this.Set("`content`", string.Concat("?content_", _parameters.Count), 
					GetParameter(string.Concat("?content_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", string.Concat("?create_time_", _parameters.Count), 
					GetParameter(string.Concat("?create_time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetEmail(string value) {
				if (_item != null) _item.Email = value;
				return this.Set("`email`", string.Concat("?email_", _parameters.Count), 
					GetParameter(string.Concat("?email_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetName(string value) {
				if (_item != null) _item.Name = value;
				return this.Set("`name`", string.Concat("?name_", _parameters.Count), 
					GetParameter(string.Concat("?name_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetState(Product_questionSTATE? value) {
				if (_item != null) _item.State = value;
				return this.Set("`state`", string.Concat("?state_", _parameters.Count), 
					GetParameter(string.Concat("?state_", _parameters.Count), MySqlDbType.Enum, -1, value?.ToInt64()));
			}
		}
		#endregion

		public Product_questionInfo Insert(Product_questionInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public Product_questionInfo GetItem(uint? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
	}
}