using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

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

		#region 独创的序列化，反序列化
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
		public Order_addressInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 6, StringSplitOptions.None);
			if (ret.Length != 6) throw new Exception("格式不正确，Order_addressInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Order_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Address = ret[1].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[2]) != 0) _Name = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) _Tel = ret[3].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[4]) != 0) _Telphone = ret[4].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[5]) != 0) _Zip = ret[5].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Order_id") ? string.Empty : string.Format(", Order_id : {0}", Order_id == null ? "null" : Order_id.ToString()), 
				__jsonIgnore.ContainsKey("Address") ? string.Empty : string.Format(", Address : {0}", Address == null ? "null" : string.Format("'{0}'", Address.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Name") ? string.Empty : string.Format(", Name : {0}", Name == null ? "null" : string.Format("'{0}'", Name.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Tel") ? string.Empty : string.Format(", Tel : {0}", Tel == null ? "null" : string.Format("'{0}'", Tel.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Telphone") ? string.Empty : string.Format(", Telphone : {0}", Telphone == null ? "null" : string.Format("'{0}'", Telphone.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Zip") ? string.Empty : string.Format(", Zip : {0}", Zip == null ? "null" : string.Format("'{0}'", Zip.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Order_id")) ht["Order_id"] = Order_id;
			if (!__jsonIgnore.ContainsKey("Address")) ht["Address"] = Address;
			if (!__jsonIgnore.ContainsKey("Name")) ht["Name"] = Name;
			if (!__jsonIgnore.ContainsKey("Tel")) ht["Tel"] = Tel;
			if (!__jsonIgnore.ContainsKey("Telphone")) ht["Telphone"] = Telphone;
			if (!__jsonIgnore.ContainsKey("Zip")) ht["Zip"] = Zip;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(Order_addressInfo).GetField("JsonIgnore");
						__jsonIgnore = new Dictionary<string, bool>();
						if (field != null) {
							string[] fs = string.Concat(field.GetValue(null)).Split(',');
							foreach (string f in fs) if (!string.IsNullOrEmpty(f)) __jsonIgnore[f] = true;
						}
					}
				}
			}
		}
		public override bool Equals(object obj) {
			Order_addressInfo item = obj as Order_addressInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(Order_addressInfo op1, Order_addressInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(Order_addressInfo op1, Order_addressInfo op2) {
			return !(op1 == op2);
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
		public uint? Order_id {
			get { return _Order_id; }
			set {
				if (_Order_id != value) _obj_order = null;
				_Order_id = value;
			}
		}
		public OrderInfo Obj_order {
			get {
				if (_obj_order == null) _obj_order = Order.GetItem(_Order_id);
				return _obj_order;
			}
			internal set { _obj_order = value; }
		}
		/// <summary>
		/// 收货地址
		/// </summary>
		public string Address {
			get { return _Address; }
			set { _Address = value; }
		}
		/// <summary>
		/// 收件人
		/// </summary>
		public string Name {
			get { return _Name; }
			set { _Name = value; }
		}
		/// <summary>
		/// 电话
		/// </summary>
		public string Tel {
			get { return _Tel; }
			set { _Tel = value; }
		}
		/// <summary>
		/// 联系电话
		/// </summary>
		public string Telphone {
			get { return _Telphone; }
			set { _Telphone = value; }
		}
		/// <summary>
		/// 邮编
		/// </summary>
		public string Zip {
			get { return _Zip; }
			set { _Zip = value; }
		}
		#endregion

		public pifa.DAL.Order_address.SqlUpdateBuild UpdateDiy {
			get { return Order_address.UpdateDiy(this, _Order_id); }
		}
	}
}

