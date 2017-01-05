using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class Factory_franchisingInfo {
		#region fields
		private uint? _Factory_id;
		private FactoryInfo _obj_factory;
		private uint? _Franchising_id;
		private FranchisingInfo _obj_franchising;
		#endregion

		public Factory_franchisingInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Factory_franchising(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Factory_id == null ? "null" : _Factory_id.ToString(), "|",
				_Franchising_id == null ? "null" : _Franchising_id.ToString());
		}
		public static Factory_franchisingInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，Factory_franchisingInfo：" + stringify);
			Factory_franchisingInfo item = new Factory_franchisingInfo();
			if (string.Compare("null", ret[0]) != 0) item.Factory_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Franchising_id = uint.Parse(ret[1]);
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(Factory_franchisingInfo).GetField("JsonIgnore");
			Dictionary<string, bool> ret = new Dictionary<string, bool>();
			if (field != null) string.Concat(field.GetValue(null)).Split(',').ToList().ForEach(f => {
				if (!string.IsNullOrEmpty(f)) ret[f] = true;
			});
			return ret;
		});
		private static Dictionary<string, bool> __jsonIgnore => __jsonIgnoreLazy.Value;
		public override string ToString() {
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Factory_id") ? string.Empty : string.Format(", Factory_id : {0}", Factory_id == null ? "null" : Factory_id.ToString()), 
				__jsonIgnore.ContainsKey("Franchising_id") ? string.Empty : string.Format(", Franchising_id : {0}", Franchising_id == null ? "null" : Franchising_id.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Factory_id")) ht["Factory_id"] = Factory_id;
			if (allField || !__jsonIgnore.ContainsKey("Franchising_id")) ht["Franchising_id"] = Franchising_id;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		[JsonProperty] public uint? Factory_id {
			get { return _Factory_id; }
			set {
				if (_Factory_id != value) _obj_factory = null;
				_Factory_id = value;
			}
		}
		public FactoryInfo Obj_factory {
			get {
				if (_obj_factory == null) _obj_factory = Factory.GetItem(_Factory_id.Value);
				return _obj_factory;
			}
			internal set { _obj_factory = value; }
		}
		[JsonProperty] public uint? Franchising_id {
			get { return _Franchising_id; }
			set {
				if (_Franchising_id != value) _obj_franchising = null;
				_Franchising_id = value;
			}
		}
		public FranchisingInfo Obj_franchising {
			get {
				if (_obj_franchising == null) _obj_franchising = Franchising.GetItem(_Franchising_id.Value);
				return _obj_franchising;
			}
			internal set { _obj_franchising = value; }
		}
		#endregion

		public pifa.DAL.Factory_franchising.SqlUpdateBuild UpdateDiy {
			get { return Factory_franchising.UpdateDiy(this, _Factory_id.Value, _Franchising_id.Value); }
		}
	}
}

