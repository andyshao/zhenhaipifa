using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class Order_addressInfo {
		#region fields
		private uint? _Order_id;
		private OrderInfo _obj_order;
		private string _Address;
		private string _Name;
		private string _Tel;
		private string _Telphone;
		private string _Zip;
		#endregion

		public Order_addressInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Order_address(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Order_id == null ? "null" : _Order_id.ToString(), "|",
				_Address == null ? "null" : _Address.Replace("|", StringifySplit), "|",
				_Name == null ? "null" : _Name.Replace("|", StringifySplit), "|",
				_Tel == null ? "null" : _Tel.Replace("|", StringifySplit), "|",
				_Telphone == null ? "null" : _Telphone.Replace("|", StringifySplit), "|",
				_Zip == null ? "null" : _Zip.Replace("|", StringifySplit));
		}
		public static Order_addressInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 6, StringSplitOptions.None);
			if (ret.Length != 6) throw new Exception("格式不正确，Order_addressInfo：" + stringify);
			Order_addressInfo item = new Order_addressInfo();
			if (string.Compare("null", ret[0]) != 0) item.Order_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Address = ret[1].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[2]) != 0) item.Name = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) item.Tel = ret[3].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[4]) != 0) item.Telphone = ret[4].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[5]) != 0) item.Zip = ret[5].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(Order_addressInfo).GetField("JsonIgnore");
			Dictionary<string, bool> ret = new Dictionary<string, bool>();
			if (field != null) string.Concat(field.GetValue(null)).Split(',').ToList().ForEach(f => {
				if (!string.IsNullOrEmpty(f)) ret[f] = true;
			});
			return ret;
		});
		private static Dictionary<string, bool> __jsonIgnore => __jsonIgnoreLazy.Value;
		public override string ToString() {
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Order_id") ? string.Empty : string.Format(", Order_id : {0}", Order_id == null ? "null" : Order_id.ToString()), 
				__jsonIgnore.ContainsKey("Address") ? string.Empty : string.Format(", Address : {0}", Address == null ? "null" : string.Format("'{0}'", Address.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Name") ? string.Empty : string.Format(", Name : {0}", Name == null ? "null" : string.Format("'{0}'", Name.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Tel") ? string.Empty : string.Format(", Tel : {0}", Tel == null ? "null" : string.Format("'{0}'", Tel.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Telphone") ? string.Empty : string.Format(", Telphone : {0}", Telphone == null ? "null" : string.Format("'{0}'", Telphone.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Zip") ? string.Empty : string.Format(", Zip : {0}", Zip == null ? "null" : string.Format("'{0}'", Zip.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Order_id")) ht["Order_id"] = Order_id;
			if (allField || !__jsonIgnore.ContainsKey("Address")) ht["Address"] = Address;
			if (allField || !__jsonIgnore.ContainsKey("Name")) ht["Name"] = Name;
			if (allField || !__jsonIgnore.ContainsKey("Tel")) ht["Tel"] = Tel;
			if (allField || !__jsonIgnore.ContainsKey("Telphone")) ht["Telphone"] = Telphone;
			if (allField || !__jsonIgnore.ContainsKey("Zip")) ht["Zip"] = Zip;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
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
		/// 收货地址
		/// </summary>
		[JsonProperty] public string Address {
			get { return _Address; }
			set { _Address = value; }
		}
		/// <summary>
		/// 收件人
		/// </summary>
		[JsonProperty] public string Name {
			get { return _Name; }
			set { _Name = value; }
		}
		/// <summary>
		/// 电话
		/// </summary>
		[JsonProperty] public string Tel {
			get { return _Tel; }
			set { _Tel = value; }
		}
		/// <summary>
		/// 联系电话
		/// </summary>
		[JsonProperty] public string Telphone {
			get { return _Telphone; }
			set { _Telphone = value; }
		}
		/// <summary>
		/// 邮编
		/// </summary>
		[JsonProperty] public string Zip {
			get { return _Zip; }
			set { _Zip = value; }
		}
		#endregion

		public pifa.DAL.Order_address.SqlUpdateBuild UpdateDiy {
			get { return Order_address.UpdateDiy(this, _Order_id.Value); }
		}
		public Order_addressInfo Save() {
			if (this.Order_id != null) {
				Order_address.Update(this);
				return this;
			}
			return Order_address.Insert(this);
		}
	}
}

