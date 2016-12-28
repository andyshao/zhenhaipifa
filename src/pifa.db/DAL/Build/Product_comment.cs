using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.DAL {

	public partial class Product_comment : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`product_comment`";
			internal static readonly string Field = "a.`id`, a.`member_id`, a.`order_id`, a.`product_id`, a.`productitem_id`, a.`content`, a.`create_time`, a.`nickname`, a.`star_price`, a.`star_quality`, a.`star_value`, a.`state`+0, a.`title`, a.`upload_image_url`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `product_comment` WHERE ";
			public static readonly string Insert = "INSERT INTO `product_comment`(`member_id`, `order_id`, `product_id`, `productitem_id`, `content`, `create_time`, `nickname`, `star_price`, `star_quality`, `star_value`, `state`, `title`, `upload_image_url`) VALUES(?member_id, ?order_id, ?product_id, ?productitem_id, ?content, ?create_time, ?nickname, ?star_price, ?star_quality, ?star_value, ?state, ?title, ?upload_image_url); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Product_commentInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?member_id", MySqlDbType.UInt32, 10, item.Member_id), 
				GetParameter("?order_id", MySqlDbType.UInt32, 10, item.Order_id), 
				GetParameter("?product_id", MySqlDbType.UInt32, 10, item.Product_id), 
				GetParameter("?productitem_id", MySqlDbType.UInt32, 10, item.Productitem_id), 
				GetParameter("?content", MySqlDbType.VarChar, 255, item.Content), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?nickname", MySqlDbType.VarChar, 255, item.Nickname), 
				GetParameter("?star_price", MySqlDbType.UByte, 3, item.Star_price), 
				GetParameter("?star_quality", MySqlDbType.UByte, 3, item.Star_quality), 
				GetParameter("?star_value", MySqlDbType.UByte, 3, item.Star_value), 
				GetParameter("?state", MySqlDbType.Enum, -1, item.State?.ToInt64()), 
				GetParameter("?title", MySqlDbType.VarChar, 255, item.Title), 
				GetParameter("?upload_image_url", MySqlDbType.VarChar, 255, item.Upload_image_url)};
		}
		public Product_commentInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Product_commentInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new Product_commentInfo {
				Id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Member_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Order_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Product_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Productitem_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Content = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Create_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				Nickname = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Star_price = dr.IsDBNull(++index) ? null : (byte?)dr.GetByte(index), 
				Star_quality = dr.IsDBNull(++index) ? null : (byte?)dr.GetByte(index), 
				Star_value = dr.IsDBNull(++index) ? null : (byte?)dr.GetByte(index), 
				State = dr.IsDBNull(++index) ? null : (Product_commentSTATE?)dr.GetInt64(index), 
				Title = dr.IsDBNull(++index) ? null : dr.GetString(index), 
				Upload_image_url = dr.IsDBNull(++index) ? null : dr.GetString(index)};
		}
		public SelectBuild<Product_commentInfo> Select {
			get { return SelectBuild<Product_commentInfo>.From(this, SqlHelper.Instance); }
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
		public int DeleteByOrder_id(uint? Order_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`order_id` = ?order_id"), 
				GetParameter("?order_id", MySqlDbType.UInt32, 10, Order_id));
		}
		public int DeleteByProductitem_id(uint? Productitem_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`productitem_id` = ?productitem_id"), 
				GetParameter("?productitem_id", MySqlDbType.UInt32, 10, Productitem_id));
		}
		public int DeleteByProduct_id(uint? Product_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`product_id` = ?product_id"), 
				GetParameter("?product_id", MySqlDbType.UInt32, 10, Product_id));
		}

		public int Update(Product_commentInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetMember_id(item.Member_id)
				.SetOrder_id(item.Order_id)
				.SetProduct_id(item.Product_id)
				.SetProductitem_id(item.Productitem_id)
				.SetContent(item.Content)
				.SetCreate_time(item.Create_time)
				.SetNickname(item.Nickname)
				.SetStar_price(item.Star_price)
				.SetStar_quality(item.Star_quality)
				.SetStar_value(item.Star_value)
				.SetState(item.State)
				.SetTitle(item.Title)
				.SetUpload_image_url(item.Upload_image_url).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Product_commentInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Product_commentInfo item, uint? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 pifa.DAL.Product_comment.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("pifa.DAL.Product_comment.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetMember_id(uint? value) {
				if (_item != null) _item.Member_id = value;
				return this.Set("`member_id`", string.Concat("?member_id_", _parameters.Count), 
					GetParameter(string.Concat("?member_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetOrder_id(uint? value) {
				if (_item != null) _item.Order_id = value;
				return this.Set("`order_id`", string.Concat("?order_id_", _parameters.Count), 
					GetParameter(string.Concat("?order_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetProduct_id(uint? value) {
				if (_item != null) _item.Product_id = value;
				return this.Set("`product_id`", string.Concat("?product_id_", _parameters.Count), 
					GetParameter(string.Concat("?product_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetProductitem_id(uint? value) {
				if (_item != null) _item.Productitem_id = value;
				return this.Set("`productitem_id`", string.Concat("?productitem_id_", _parameters.Count), 
					GetParameter(string.Concat("?productitem_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
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
			public SqlUpdateBuild SetNickname(string value) {
				if (_item != null) _item.Nickname = value;
				return this.Set("`nickname`", string.Concat("?nickname_", _parameters.Count), 
					GetParameter(string.Concat("?nickname_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetStar_price(byte? value) {
				if (_item != null) _item.Star_price = value;
				return this.Set("`star_price`", string.Concat("?star_price_", _parameters.Count), 
					GetParameter(string.Concat("?star_price_", _parameters.Count), MySqlDbType.UByte, 3, value));
			}
			public SqlUpdateBuild SetStar_priceIncrement(byte value) {
				if (_item != null) _item.Star_price += value;
				return this.Set("`star_price`", string.Concat("`star_price` + ?star_price_", _parameters.Count), 
					GetParameter(string.Concat("?star_price_", _parameters.Count), MySqlDbType.Byte, 3, value));
			}
			public SqlUpdateBuild SetStar_quality(byte? value) {
				if (_item != null) _item.Star_quality = value;
				return this.Set("`star_quality`", string.Concat("?star_quality_", _parameters.Count), 
					GetParameter(string.Concat("?star_quality_", _parameters.Count), MySqlDbType.UByte, 3, value));
			}
			public SqlUpdateBuild SetStar_qualityIncrement(byte value) {
				if (_item != null) _item.Star_quality += value;
				return this.Set("`star_quality`", string.Concat("`star_quality` + ?star_quality_", _parameters.Count), 
					GetParameter(string.Concat("?star_quality_", _parameters.Count), MySqlDbType.Byte, 3, value));
			}
			public SqlUpdateBuild SetStar_value(byte? value) {
				if (_item != null) _item.Star_value = value;
				return this.Set("`star_value`", string.Concat("?star_value_", _parameters.Count), 
					GetParameter(string.Concat("?star_value_", _parameters.Count), MySqlDbType.UByte, 3, value));
			}
			public SqlUpdateBuild SetStar_valueIncrement(byte value) {
				if (_item != null) _item.Star_value += value;
				return this.Set("`star_value`", string.Concat("`star_value` + ?star_value_", _parameters.Count), 
					GetParameter(string.Concat("?star_value_", _parameters.Count), MySqlDbType.Byte, 3, value));
			}
			public SqlUpdateBuild SetState(Product_commentSTATE? value) {
				if (_item != null) _item.State = value;
				return this.Set("`state`", string.Concat("?state_", _parameters.Count), 
					GetParameter(string.Concat("?state_", _parameters.Count), MySqlDbType.Enum, -1, value?.ToInt64()));
			}
			public SqlUpdateBuild SetTitle(string value) {
				if (_item != null) _item.Title = value;
				return this.Set("`title`", string.Concat("?title_", _parameters.Count), 
					GetParameter(string.Concat("?title_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetUpload_image_url(string value) {
				if (_item != null) _item.Upload_image_url = value;
				return this.Set("`upload_image_url`", string.Concat("?upload_image_url_", _parameters.Count), 
					GetParameter(string.Concat("?upload_image_url_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
		}
		#endregion

		public Product_commentInfo Insert(Product_commentInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public Product_commentInfo GetItem(uint? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
	}
}