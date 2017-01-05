using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class ShopsecurityInfo {
		#region fields
		private uint? _Shop_id;
		private ShopInfo _obj_shop;
		private string _Idcard;
		private string _Idcard_img1;
		private string _Idcard_img2;
		private string _License_img;
		#endregion

		public ShopsecurityInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Shopsecurity(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Shop_id == null ? "null" : _Shop_id.ToString(), "|",
				_Idcard == null ? "null" : _Idcard.Replace("|", StringifySplit), "|",
				_Idcard_img1 == null ? "null" : _Idcard_img1.Replace("|", StringifySplit), "|",
				_Idcard_img2 == null ? "null" : _Idcard_img2.Replace("|", StringifySplit), "|",
				_License_img == null ? "null" : _License_img.Replace("|", StringifySplit));
		}
		public static ShopsecurityInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 5, StringSplitOptions.None);
			if (ret.Length != 5) throw new Exception("格式不正确，ShopsecurityInfo：" + stringify);
			ShopsecurityInfo item = new ShopsecurityInfo();
			if (string.Compare("null", ret[0]) != 0) item.Shop_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Idcard = ret[1].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[2]) != 0) item.Idcard_img1 = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) item.Idcard_img2 = ret[3].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[4]) != 0) item.License_img = ret[4].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(ShopsecurityInfo).GetField("JsonIgnore");
			Dictionary<string, bool> ret = new Dictionary<string, bool>();
			if (field != null) string.Concat(field.GetValue(null)).Split(',').ToList().ForEach(f => {
				if (!string.IsNullOrEmpty(f)) ret[f] = true;
			});
			return ret;
		});
		private static Dictionary<string, bool> __jsonIgnore => __jsonIgnoreLazy.Value;
		public override string ToString() {
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Shop_id") ? string.Empty : string.Format(", Shop_id : {0}", Shop_id == null ? "null" : Shop_id.ToString()), 
				__jsonIgnore.ContainsKey("Idcard") ? string.Empty : string.Format(", Idcard : {0}", Idcard == null ? "null" : string.Format("'{0}'", Idcard.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Idcard_img1") ? string.Empty : string.Format(", Idcard_img1 : {0}", Idcard_img1 == null ? "null" : string.Format("'{0}'", Idcard_img1.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Idcard_img2") ? string.Empty : string.Format(", Idcard_img2 : {0}", Idcard_img2 == null ? "null" : string.Format("'{0}'", Idcard_img2.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("License_img") ? string.Empty : string.Format(", License_img : {0}", License_img == null ? "null" : string.Format("'{0}'", License_img.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Shop_id")) ht["Shop_id"] = Shop_id;
			if (allField || !__jsonIgnore.ContainsKey("Idcard")) ht["Idcard"] = Idcard;
			if (allField || !__jsonIgnore.ContainsKey("Idcard_img1")) ht["Idcard_img1"] = Idcard_img1;
			if (allField || !__jsonIgnore.ContainsKey("Idcard_img2")) ht["Idcard_img2"] = Idcard_img2;
			if (allField || !__jsonIgnore.ContainsKey("License_img")) ht["License_img"] = License_img;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
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
		/// <summary>
		/// 身份证
		/// </summary>
		[JsonProperty] public string Idcard {
			get { return _Idcard; }
			set { _Idcard = value; }
		}
		/// <summary>
		/// 身份证正面照
		/// </summary>
		[JsonProperty] public string Idcard_img1 {
			get { return _Idcard_img1; }
			set { _Idcard_img1 = value; }
		}
		/// <summary>
		/// 身份证背面照
		/// </summary>
		[JsonProperty] public string Idcard_img2 {
			get { return _Idcard_img2; }
			set { _Idcard_img2 = value; }
		}
		/// <summary>
		/// 经营执照
		/// </summary>
		[JsonProperty] public string License_img {
			get { return _License_img; }
			set { _License_img = value; }
		}
		#endregion

		public pifa.DAL.Shopsecurity.SqlUpdateBuild UpdateDiy {
			get { return Shopsecurity.UpdateDiy(this, _Shop_id.Value); }
		}
		public ShopsecurityInfo Save() {
			if (this.Shop_id != null) {
				Shopsecurity.Update(this);
				return this;
			}
			return Shopsecurity.Insert(this);
		}
	}
}

