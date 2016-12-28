using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class Rentsublet_franchisingInfo {
		#region fields
		private uint? _Franchising_id;
		private FranchisingInfo _obj_franchising;
		private uint? _Rentsublet_id;
		private RentsubletInfo _obj_rentsublet;
		#endregion

		public Rentsublet_franchisingInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Rentsublet_franchising(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Franchising_id == null ? "null" : _Franchising_id.ToString(), "|",
				_Rentsublet_id == null ? "null" : _Rentsublet_id.ToString());
		}
		public Rentsublet_franchisingInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，Rentsublet_franchisingInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Franchising_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Rentsublet_id = uint.Parse(ret[1]);
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Franchising_id") ? string.Empty : string.Format(", Franchising_id : {0}", Franchising_id == null ? "null" : Franchising_id.ToString()), 
				__jsonIgnore.ContainsKey("Rentsublet_id") ? string.Empty : string.Format(", Rentsublet_id : {0}", Rentsublet_id == null ? "null" : Rentsublet_id.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Franchising_id")) ht["Franchising_id"] = Franchising_id;
			if (!__jsonIgnore.ContainsKey("Rentsublet_id")) ht["Rentsublet_id"] = Rentsublet_id;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(Rentsublet_franchisingInfo).GetField("JsonIgnore");
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
			Rentsublet_franchisingInfo item = obj as Rentsublet_franchisingInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(Rentsublet_franchisingInfo op1, Rentsublet_franchisingInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(Rentsublet_franchisingInfo op1, Rentsublet_franchisingInfo op2) {
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
		public uint? Rentsublet_id {
			get { return _Rentsublet_id; }
			set {
				if (_Rentsublet_id != value) _obj_rentsublet = null;
				_Rentsublet_id = value;
			}
		}
		public RentsubletInfo Obj_rentsublet {
			get {
				if (_obj_rentsublet == null) _obj_rentsublet = Rentsublet.GetItem(_Rentsublet_id);
				return _obj_rentsublet;
			}
			internal set { _obj_rentsublet = value; }
		}
		#endregion

		public pifa.DAL.Rentsublet_franchising.SqlUpdateBuild UpdateDiy {
			get { return Rentsublet_franchising.UpdateDiy(this, _Franchising_id, _Rentsublet_id); }
		}
	}
}

