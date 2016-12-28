using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class Order_productitemInfo {
		#region fields
		private uint? _Order_id;
		private OrderInfo _obj_order;
		private uint? _Productitem_id;
		private ProductitemInfo _obj_productitem;
		private uint? _Number;
		private decimal? _Price;
		private Order_productitemSTATE? _State;
		private string _Title;
		#endregion

		public Order_productitemInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Order_productitem(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Order_id == null ? "null" : _Order_id.ToString(), "|",
				_Productitem_id == null ? "null" : _Productitem_id.ToString(), "|",
				_Number == null ? "null" : _Number.ToString(), "|",
				_Price == null ? "null" : _Price.ToString(), "|",
				_State == null ? "null" : _State.ToInt64().ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit));
		}
		public Order_productitemInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 6, StringSplitOptions.None);
			if (ret.Length != 6) throw new Exception("格式不正确，Order_productitemInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Order_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Productitem_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _Number = uint.Parse(ret[2]);
			if (string.Compare("null", ret[3]) != 0) _Price = decimal.Parse(ret[3]);
			if (string.Compare("null", ret[4]) != 0) _State = (Order_productitemSTATE)long.Parse(ret[4]);
			if (string.Compare("null", ret[5]) != 0) _Title = ret[5].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Order_id") ? string.Empty : string.Format(", Order_id : {0}", Order_id == null ? "null" : Order_id.ToString()), 
				__jsonIgnore.ContainsKey("Productitem_id") ? string.Empty : string.Format(", Productitem_id : {0}", Productitem_id == null ? "null" : Productitem_id.ToString()), 
				__jsonIgnore.ContainsKey("Number") ? string.Empty : string.Format(", Number : {0}", Number == null ? "null" : Number.ToString()), 
				__jsonIgnore.ContainsKey("Price") ? string.Empty : string.Format(", Price : {0}", Price == null ? "null" : Price.ToString()), 
				__jsonIgnore.ContainsKey("State") ? string.Empty : string.Format(", State : {0}", State == null ? "null" : string.Format("'{0}'", State.ToDescriptionOrString().Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Order_id")) ht["Order_id"] = Order_id;
			if (!__jsonIgnore.ContainsKey("Productitem_id")) ht["Productitem_id"] = Productitem_id;
			if (!__jsonIgnore.ContainsKey("Number")) ht["Number"] = Number;
			if (!__jsonIgnore.ContainsKey("Price")) ht["Price"] = Price;
			if (!__jsonIgnore.ContainsKey("State")) ht["State"] = State?.ToDescriptionOrString();
			if (!__jsonIgnore.ContainsKey("Title")) ht["Title"] = Title;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(Order_productitemInfo).GetField("JsonIgnore");
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
			Order_productitemInfo item = obj as Order_productitemInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(Order_productitemInfo op1, Order_productitemInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(Order_productitemInfo op1, Order_productitemInfo op2) {
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
		/// 产品项
		/// </summary>
		public uint? Productitem_id {
			get { return _Productitem_id; }
			set {
				if (_Productitem_id != value) _obj_productitem = null;
				_Productitem_id = value;
			}
		}
		public ProductitemInfo Obj_productitem {
			get {
				if (_obj_productitem == null) _obj_productitem = Productitem.GetItem(_Productitem_id);
				return _obj_productitem;
			}
			internal set { _obj_productitem = value; }
		}
		/// <summary>
		/// 数量
		/// </summary>
		public uint? Number {
			get { return _Number; }
			set { _Number = value; }
		}
		/// <summary>
		/// 价格
		/// </summary>
		public decimal? Price {
			get { return _Price; }
			set { _Price = value; }
		}
		/// <summary>
		/// 状态
		/// </summary>
		public Order_productitemSTATE? State {
			get { return _State; }
			set { _State = value; }
		}
		/// <summary>
		/// 商品名称
		/// </summary>
		public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
		#endregion

		public pifa.DAL.Order_productitem.SqlUpdateBuild UpdateDiy {
			get { return Order_productitem.UpdateDiy(this, _Order_id, _Productitem_id); }
		}
	}
	public enum Order_productitemSTATE {
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
		/// ???
		/// </summary>
		[Description("???")]
		Unknow3, 
		/// <summary>
		/// ????
		/// </summary>
		[Description("????")]
		Unknow4, 
		/// <summary>
		/// ????
		/// </summary>
		[Description("????")]
		Unknow5, 
		/// <summary>
		/// ???
		/// </summary>
		[Description("???")]
		Unknow6
	}
}

