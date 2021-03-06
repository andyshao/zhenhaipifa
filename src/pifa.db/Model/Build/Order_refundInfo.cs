﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class Order_refundInfo {
		#region fields
		private uint? _Id;
		private uint? _Order_id;
		private OrderInfo _obj_order;
		private uint? _Productitem_id;
		private ProductitemInfo _obj_productitem;
		private DateTime? _Create_time;
		private string _Descript;
		private string _Email;
		private string _Img_url;
		private Order_refundSTATE? _State;
		private string _Tel;
		private string _Telphone;
		private decimal? _Wealth;
		#endregion

		public Order_refundInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Order_refund(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Order_id == null ? "null" : _Order_id.ToString(), "|",
				_Productitem_id == null ? "null" : _Productitem_id.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Descript == null ? "null" : _Descript.Replace("|", StringifySplit), "|",
				_Email == null ? "null" : _Email.Replace("|", StringifySplit), "|",
				_Img_url == null ? "null" : _Img_url.Replace("|", StringifySplit), "|",
				_State == null ? "null" : _State.ToInt64().ToString(), "|",
				_Tel == null ? "null" : _Tel.Replace("|", StringifySplit), "|",
				_Telphone == null ? "null" : _Telphone.Replace("|", StringifySplit), "|",
				_Wealth == null ? "null" : _Wealth.ToString());
		}
		public static Order_refundInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 11, StringSplitOptions.None);
			if (ret.Length != 11) throw new Exception("格式不正确，Order_refundInfo：" + stringify);
			Order_refundInfo item = new Order_refundInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Order_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Productitem_id = uint.Parse(ret[2]);
			if (string.Compare("null", ret[3]) != 0) item.Create_time = new DateTime(long.Parse(ret[3]));
			if (string.Compare("null", ret[4]) != 0) item.Descript = ret[4].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[5]) != 0) item.Email = ret[5].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[6]) != 0) item.Img_url = ret[6].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[7]) != 0) item.State = (Order_refundSTATE)long.Parse(ret[7]);
			if (string.Compare("null", ret[8]) != 0) item.Tel = ret[8].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[9]) != 0) item.Telphone = ret[9].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[10]) != 0) item.Wealth = decimal.Parse(ret[10]);
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(Order_refundInfo).GetField("JsonIgnore");
			Dictionary<string, bool> ret = new Dictionary<string, bool>();
			if (field != null) string.Concat(field.GetValue(null)).Split(',').ToList().ForEach(f => {
				if (!string.IsNullOrEmpty(f)) ret[f] = true;
			});
			return ret;
		});
		private static Dictionary<string, bool> __jsonIgnore => __jsonIgnoreLazy.Value;
		public override string ToString() {
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Order_id") ? string.Empty : string.Format(", Order_id : {0}", Order_id == null ? "null" : Order_id.ToString()), 
				__jsonIgnore.ContainsKey("Productitem_id") ? string.Empty : string.Format(", Productitem_id : {0}", Productitem_id == null ? "null" : Productitem_id.ToString()), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Descript") ? string.Empty : string.Format(", Descript : {0}", Descript == null ? "null" : string.Format("'{0}'", Descript.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Email") ? string.Empty : string.Format(", Email : {0}", Email == null ? "null" : string.Format("'{0}'", Email.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Img_url") ? string.Empty : string.Format(", Img_url : {0}", Img_url == null ? "null" : string.Format("'{0}'", Img_url.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("State") ? string.Empty : string.Format(", State : {0}", State == null ? "null" : string.Format("'{0}'", State.ToDescriptionOrString().Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Tel") ? string.Empty : string.Format(", Tel : {0}", Tel == null ? "null" : string.Format("'{0}'", Tel.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Telphone") ? string.Empty : string.Format(", Telphone : {0}", Telphone == null ? "null" : string.Format("'{0}'", Telphone.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Wealth") ? string.Empty : string.Format(", Wealth : {0}", Wealth == null ? "null" : Wealth.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Order_id")) ht["Order_id"] = Order_id;
			if (allField || !__jsonIgnore.ContainsKey("Productitem_id")) ht["Productitem_id"] = Productitem_id;
			if (allField || !__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (allField || !__jsonIgnore.ContainsKey("Descript")) ht["Descript"] = Descript;
			if (allField || !__jsonIgnore.ContainsKey("Email")) ht["Email"] = Email;
			if (allField || !__jsonIgnore.ContainsKey("Img_url")) ht["Img_url"] = Img_url;
			if (allField || !__jsonIgnore.ContainsKey("State")) ht["State"] = State?.ToDescriptionOrString();
			if (allField || !__jsonIgnore.ContainsKey("Tel")) ht["Tel"] = Tel;
			if (allField || !__jsonIgnore.ContainsKey("Telphone")) ht["Telphone"] = Telphone;
			if (allField || !__jsonIgnore.ContainsKey("Wealth")) ht["Wealth"] = Wealth;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		[JsonProperty] public uint? Id {
			get { return _Id; }
			set { _Id = value; }
		}
		/// <summary>
		/// 订单
		/// </summary>
		[JsonProperty] public uint? Order_id {
			get { return _Order_id; }
			set {
				if (_Order_id != value) _obj_order = null;
				_Order_id = value;
			}
		}
		public OrderInfo Obj_order {
			get {
				if (_obj_order == null) _obj_order = Order.GetItem(_Order_id.Value);
				return _obj_order;
			}
			internal set { _obj_order = value; }
		}
		/// <summary>
		/// 商品项
		/// </summary>
		[JsonProperty] public uint? Productitem_id {
			get { return _Productitem_id; }
			set {
				if (_Productitem_id != value) _obj_productitem = null;
				_Productitem_id = value;
			}
		}
		public ProductitemInfo Obj_productitem {
			get {
				if (_obj_productitem == null) _obj_productitem = Productitem.GetItem(_Productitem_id.Value);
				return _obj_productitem;
			}
			internal set { _obj_productitem = value; }
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		[JsonProperty] public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 说明
		/// </summary>
		[JsonProperty] public string Descript {
			get { return _Descript; }
			set { _Descript = value; }
		}
		/// <summary>
		/// 邮箱
		/// </summary>
		[JsonProperty] public string Email {
			get { return _Email; }
			set { _Email = value; }
		}
		/// <summary>
		/// 图片
		/// </summary>
		[JsonProperty] public string Img_url {
			get { return _Img_url; }
			set { _Img_url = value; }
		}
		/// <summary>
		/// 状态
		/// </summary>
		[JsonProperty] public Order_refundSTATE? State {
			get { return _State; }
			set { _State = value; }
		}
		/// <summary>
		/// 电话
		/// </summary>
		[JsonProperty] public string Tel {
			get { return _Tel; }
			set { _Tel = value; }
		}
		/// <summary>
		/// 手机
		/// </summary>
		[JsonProperty] public string Telphone {
			get { return _Telphone; }
			set { _Telphone = value; }
		}
		/// <summary>
		/// 退款金额
		/// </summary>
		[JsonProperty] public decimal? Wealth {
			get { return _Wealth; }
			set { _Wealth = value; }
		}
		#endregion

		public pifa.DAL.Order_refund.SqlUpdateBuild UpdateDiy {
			get { return Order_refund.UpdateDiy(this, _Id.Value); }
		}
		public Order_refundInfo Save() {
			if (this.Id != null) {
				Order_refund.Update(this);
				return this;
			}
			this.Create_time = DateTime.Now;
			return Order_refund.Insert(this);
		}
	}
	public enum Order_refundSTATE {
		/// <summary>
		/// ????
		/// </summary>
		[Description("????")]
		Unknow1 = 1, 
		/// <summary>
		/// ????
		/// </summary>
		[Description("????")]
		Unknow2, 
		/// <summary>
		/// ????
		/// </summary>
		[Description("????")]
		Unknow3, 
		/// <summary>
		/// ??
		/// </summary>
		[Description("??")]
		Unknow4, 
		/// <summary>
		/// ??
		/// </summary>
		[Description("??")]
		Unknow5, 
		/// <summary>
		/// ??
		/// </summary>
		[Description("??")]
		Unknow6, 
		/// <summary>
		/// ??
		/// </summary>
		[Description("??")]
		Unknow7
	}
}

