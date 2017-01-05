using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class Shop_franchisingInfo {
		#region fields
		private uint? _Franchising_id;
		private FranchisingInfo _obj_franchising;
		private uint? _Shop_id;
		private ShopInfo _obj_shop;
		#endregion

		public Shop_franchisingInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Shop_franchising(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Franchising_id == null ? "null" : _Franchising_id.ToString(), "|",
				_Shop_id == null ? "null" : _Shop_id.ToString());
		}
		public static Shop_franchisingInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，Shop_franchisingInfo：" + stringify);
			Shop_franchisingInfo item = new Shop_franchisingInfo();
			if (string.Compare("null", ret[0]) != 0) item.Franchising_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Shop_id = uint.Parse(ret[1]);
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(Shop_franchisingInfo).GetField("JsonIgnore");
			Dictionary<string, bool> ret = new Dictionary<string, bool>();
			if (field != null) string.Concat(field.GetValue(null)).Split(',').ToList().ForEach(f => {
				if (!string.IsNullOrEmpty(f)) ret[f] = true;
			});
			return ret;
		});
		private static Dictionary<string, bool> __jsonIgnore => __jsonIgnoreLazy.Value;
		public override string ToString() {
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Franchising_id") ? string.Empty : string.Format(", Franchising_id : {0}", Franchising_id == null ? "null" : Franchising_id.ToString()), 
				__jsonIgnore.ContainsKey("Shop_id") ? string.Empty : string.Format(", Shop_id : {0}", Shop_id == null ? "null" : Shop_id.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Franchising_id")) ht["Franchising_id"] = Franchising_id;
			if (allField || !__jsonIgnore.ContainsKey("Shop_id")) ht["Shop_id"] = Shop_id;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
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
		[JsonProperty] public uint? Shop_id {
			get { return _Shop_id; }
			set {
				if (_Shop_id != value) _obj_shop = null;
				_Shop_id = value;
			}
		}
		public ShopInfo Obj_shop {
			get {
				if (_obj_shop == null) _obj_shop = Shop.GetItem(_Shop_id.Value);
				return _obj_shop;
			}
			internal set { _obj_shop = value; }
		}
		#endregion

		public pifa.DAL.Shop_franchising.SqlUpdateBuild UpdateDiy {
			get { return Shop_franchising.UpdateDiy(this, _Franchising_id.Value, _Shop_id.Value); }
		}
	}
}

