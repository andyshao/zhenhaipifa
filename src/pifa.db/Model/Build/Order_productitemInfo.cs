using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
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

		#region 序列化，反序列化
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
		public static Order_productitemInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 6, StringSplitOptions.None);
			if (ret.Length != 6) throw new Exception("格式不正确，Order_productitemInfo：" + stringify);
			Order_productitemInfo item = new Order_productitemInfo();
			if (string.Compare("null", ret[0]) != 0) item.Order_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Productitem_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Number = uint.Parse(ret[2]);
			if (string.Compare("null", ret[3]) != 0) item.Price = decimal.Parse(ret[3]);
			if (string.Compare("null", ret[4]) != 0) item.State = (Order_productitemSTATE)long.Parse(ret[4]);
			if (string.Compare("null", ret[5]) != 0) item.Title = ret[5].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(Order_productitemInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Productitem_id") ? string.Empty : string.Format(", Productitem_id : {0}", Productitem_id == null ? "null" : Productitem_id.ToString()), 
				__jsonIgnore.ContainsKey("Number") ? string.Empty : string.Format(", Number : {0}", Number == null ? "null" : Number.ToString()), 
				__jsonIgnore.ContainsKey("Price") ? string.Empty : string.Format(", Price : {0}", Price == null ? "null" : Price.ToString()), 
				__jsonIgnore.ContainsKey("State") ? string.Empty : string.Format(", State : {0}", State == null ? "null" : string.Format("'{0}'", State.ToDescriptionOrString().Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Order_id")) ht["Order_id"] = Order_id;
			if (allField || !__jsonIgnore.ContainsKey("Productitem_id")) ht["Productitem_id"] = Productitem_id;
			if (allField || !__jsonIgnore.ContainsKey("Number")) ht["Number"] = Number;
			if (allField || !__jsonIgnore.ContainsKey("Price")) ht["Price"] = Price;
			if (allField || !__jsonIgnore.ContainsKey("State")) ht["State"] = State?.ToDescriptionOrString();
			if (allField || !__jsonIgnore.ContainsKey("Title")) ht["Title"] = Title;
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
		/// 产品项
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
		/// 数量
		/// </summary>
		[JsonProperty] public uint? Number {
			get { return _Number; }
			set { _Number = value; }
		}
		/// <summary>
		/// 价格
		/// </summary>
		[JsonProperty] public decimal? Price {
			get { return _Price; }
			set { _Price = value; }
		}
		/// <summary>
		/// 状态
		/// </summary>
		[JsonProperty] public Order_productitemSTATE? State {
			get { return _State; }
			set { _State = value; }
		}
		/// <summary>
		/// 商品名称
		/// </summary>
		[JsonProperty] public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
		#endregion

		public pifa.DAL.Order_productitem.SqlUpdateBuild UpdateDiy {
			get { return Order_productitem.UpdateDiy(this, _Order_id.Value, _Productitem_id.Value); }
		}
		public Order_productitemInfo Save() {
			if (this.Order_id != null && this.Productitem_id != null) {
				Order_productitem.Update(this);
				return this;
			}
			return Order_productitem.Insert(this);
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

