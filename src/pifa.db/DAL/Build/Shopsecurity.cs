using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Shopsecurity : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`shopsecurity`";
			internal static readonly string Field = "a.`shop_id`, a.`idcard`, a.`idcard_img1`, a.`idcard_img2`, a.`license_img`";
			internal static readonly string Sort = "a.`shop_id`";
			public static readonly string Delete = "DELETE FROM `shopsecurity` WHERE ";
			public static readonly string Insert = "INSERT INTO `shopsecurity`(`shop_id`, `idcard`, `idcard_img1`, `idcard_img2`, `license_img`) VALUES(?shop_id, ?idcard, ?idcard_img1, ?idcard_img2, ?license_img)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(ShopsecurityInfo item) {
			return new MySqlParameter[] {
				GetParameter("?shop_id", MySqlDbType.UInt32, 10, item.Shop_id), 
				GetParameter("?idcard", MySqlDbType.VarChar, 32, item.Idcard), 
				GetParameter("?idcard_img1", MySqlDbType.VarChar, 255, item.Idcard_img1), 
				GetParameter("?idcard_img2", MySqlDbType.VarChar, 255, item.Idcard_img2), 
				GetParameter("?license_img", MySqlDbType.VarChar, 255, item.License_img)};
		}
		public ShopsecurityInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as ShopsecurityInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new ShopsecurityInfo {
				Shop_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Idcard = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Idcard_img1 = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Idcard_img2 = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				License_img = dr.IsDBNull(++index) ? null : dr.GetString(index)};
		}
		public SelectBuild<ShopsecurityInfo> Select {
			get { return SelectBuild<ShopsecurityInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Shop_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`shop_id` = ?shop_id"), 
				GetParameter("?shop_id", MySqlDbType.UInt32, 10, Shop_id));
		}
		public int DeleteByIdcard(string Idcard) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`idcard` = ?idcard"), 
				GetParameter("?idcard", MySqlDbType.VarChar, 32, Idcard));
		}

		public int Update(ShopsecurityInfo item) {
			return new SqlUpdateBuild(null, item.Shop_id)
				.SetIdcard(item.Idcard)
				.SetIdcard_img1(item.Idcard_img1)
				.SetIdcard_img2(item.Idcard_img2)
				.SetLicense_img(item.License_img).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected ShopsecurityInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(ShopsecurityInfo item, uint? Shop_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`shop_id` = {0}", Shop_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Shopsecurity.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Shopsecurity.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetIdcard(string value) {
				if (_item != null) _item.Idcard = value;
				return this.Set("`idcard`", string.Concat("?idcard_", _parameters.Count), 
					GetParameter(string.Concat("?idcard_", _parameters.Count), MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetIdcard_img1(string value) {
				if (_item != null) _item.Idcard_img1 = value;
				return this.Set("`idcard_img1`", string.Concat("?idcard_img1_", _parameters.Count), 
					GetParameter(string.Concat("?idcard_img1_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetIdcard_img2(string value) {
				if (_item != null) _item.Idcard_img2 = value;
				return this.Set("`idcard_img2`", string.Concat("?idcard_img2_", _parameters.Count), 
					GetParameter(string.Concat("?idcard_img2_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetLicense_img(string value) {
				if (_item != null) _item.License_img = value;
				return this.Set("`license_img`", string.Concat("?license_img_", _parameters.Count), 
					GetParameter(string.Concat("?license_img_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
		}
		#endregion

		public ShopsecurityInfo Insert(ShopsecurityInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

		public ShopsecurityInfo GetItem(uint? Shop_id) {
			return this.Select.Where("a.`shop_id` = {0}", Shop_id).ToOne();
		}
		public ShopsecurityInfo GetItemByIdcard(string Idcard) {
			return this.Select.Where("a.`idcard` = {0}", Idcard).ToOne();
		}
	}
}