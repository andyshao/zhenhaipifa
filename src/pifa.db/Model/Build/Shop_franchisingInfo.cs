using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class Shop_franchisingInfo {
		#region fields
		private uint? _Franchising_id;
		private FranchisingInfo _obj_franchising;
		private uint? _Shop_id;
		private ShopInfo _obj_shop;
		#endregion

		public Shop_franchisingInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Shop_franchising(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Franchising_id == null ? "null" : _Franchising_id.ToString(), "|",
				_Shop_id == null ? "null" : _Shop_id.ToString());
		}
		public Shop_franchisingInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，Shop_franchisingInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Franchising_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Shop_id = uint.Parse(ret[1]);
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Franchising_id") ? string.Empty : string.Format(", Franchising_id : {0}", Franchising_id == null ? "null" : Franchising_id.ToString()), 
				__jsonIgnore.ContainsKey("Shop_id") ? string.Empty : string.Format(", Shop_id : {0}", Shop_id == null ? "null" : Shop_id.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Franchising_id")) ht["Franchising_id"] = Franchising_id;
			if (!__jsonIgnore.ContainsKey("Shop_id")) ht["Shop_id"] = Shop_id;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(Shop_franchisingInfo).GetField("JsonIgnore");
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
			Shop_franchisingInfo item = obj as Shop_franchisingInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(Shop_franchisingInfo op1, Shop_franchisingInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(Shop_franchisingInfo op1, Shop_franchisingInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public uint? Franchising_id {
			get { return _Franchising_id; }
			set {
				if (_Franchising_id != value) _obj_franchising = null;
				_Franchising_id = value;
			}
		}
		public FranchisingInfo Obj_franchising {
			get {
				if (_obj_franchising == null) _obj_franchising = Franchising.GetItem(_Franchising_id);
				return _obj_franchising;
			}
			internal set { _obj_franchising = value; }
		}
		public uint? Shop_id {
			get { return _Shop_id; }
			set {
				if (_Shop_id != value) _obj_shop = null;
				_Shop_id = value;
			}
		}
		public ShopInfo Obj_shop {
			get {
				if (_obj_shop == null) _obj_shop = Shop.GetItem(_Shop_id);
				return _obj_shop;
			}
			internal set { _obj_shop = value; }
		}
		#endregion

		public pifa.DAL.Shop_franchising.SqlUpdateBuild UpdateDiy {
			get { return Shop_franchising.UpdateDiy(this, _Franchising_id, _Shop_id); }
		}
	}
}

