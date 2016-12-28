using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class Factory_franchisingInfo {
		#region fields
		private uint? _Factory_id;
		private FactoryInfo _obj_factory;
		private uint? _Franchising_id;
		private FranchisingInfo _obj_franchising;
		#endregion

		public Factory_franchisingInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Factory_franchising(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Factory_id == null ? "null" : _Factory_id.ToString(), "|",
				_Franchising_id == null ? "null" : _Franchising_id.ToString());
		}
		public Factory_franchisingInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，Factory_franchisingInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Factory_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Franchising_id = uint.Parse(ret[1]);
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Factory_id") ? string.Empty : string.Format(", Factory_id : {0}", Factory_id == null ? "null" : Factory_id.ToString()), 
				__jsonIgnore.ContainsKey("Franchising_id") ? string.Empty : string.Format(", Franchising_id : {0}", Franchising_id == null ? "null" : Franchising_id.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Factory_id")) ht["Factory_id"] = Factory_id;
			if (!__jsonIgnore.ContainsKey("Franchising_id")) ht["Franchising_id"] = Franchising_id;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(Factory_franchisingInfo).GetField("JsonIgnore");
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
			Factory_franchisingInfo item = obj as Factory_franchisingInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(Factory_franchisingInfo op1, Factory_franchisingInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(Factory_franchisingInfo op1, Factory_franchisingInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public uint? Factory_id {
			get { return _Factory_id; }
			set {
				if (_Factory_id != value) _obj_factory = null;
				_Factory_id = value;
			}
		}
		public FactoryInfo Obj_factory {
			get {
				if (_obj_factory == null) _obj_factory = Factory.GetItem(_Factory_id);
				return _obj_factory;
			}
			internal set { _obj_factory = value; }
		}
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
		#endregion

		public pifa.DAL.Factory_franchising.SqlUpdateBuild UpdateDiy {
			get { return Factory_franchising.UpdateDiy(this, _Factory_id, _Franchising_id); }
		}
	}
}

