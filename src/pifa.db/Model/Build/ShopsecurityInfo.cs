using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

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

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Shopsecurity(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Shop_id == null ? "null" : _Shop_id.ToString(), "|",
				_Idcard == null ? "null" : _Idcard.Replace("|", StringifySplit), "|",
				_Idcard_img1 == null ? "null" : _Idcard_img1.Replace("|", StringifySplit), "|",
				_Idcard_img2 == null ? "null" : _Idcard_img2.Replace("|", StringifySplit), "|",
				_License_img == null ? "null" : _License_img.Replace("|", StringifySplit));
		}
		public ShopsecurityInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 5, StringSplitOptions.None);
			if (ret.Length != 5) throw new Exception("格式不正确，ShopsecurityInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Shop_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Idcard = ret[1].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[2]) != 0) _Idcard_img1 = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) _Idcard_img2 = ret[3].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[4]) != 0) _License_img = ret[4].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Shop_id") ? string.Empty : string.Format(", Shop_id : {0}", Shop_id == null ? "null" : Shop_id.ToString()), 
				__jsonIgnore.ContainsKey("Idcard") ? string.Empty : string.Format(", Idcard : {0}", Idcard == null ? "null" : string.Format("'{0}'", Idcard.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Idcard_img1") ? string.Empty : string.Format(", Idcard_img1 : {0}", Idcard_img1 == null ? "null" : string.Format("'{0}'", Idcard_img1.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Idcard_img2") ? string.Empty : string.Format(", Idcard_img2 : {0}", Idcard_img2 == null ? "null" : string.Format("'{0}'", Idcard_img2.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("License_img") ? string.Empty : string.Format(", License_img : {0}", License_img == null ? "null" : string.Format("'{0}'", License_img.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Shop_id")) ht["Shop_id"] = Shop_id;
			if (!__jsonIgnore.ContainsKey("Idcard")) ht["Idcard"] = Idcard;
			if (!__jsonIgnore.ContainsKey("Idcard_img1")) ht["Idcard_img1"] = Idcard_img1;
			if (!__jsonIgnore.ContainsKey("Idcard_img2")) ht["Idcard_img2"] = Idcard_img2;
			if (!__jsonIgnore.ContainsKey("License_img")) ht["License_img"] = License_img;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(ShopsecurityInfo).GetField("JsonIgnore");
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
			ShopsecurityInfo item = obj as ShopsecurityInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(ShopsecurityInfo op1, ShopsecurityInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(ShopsecurityInfo op1, ShopsecurityInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
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
		/// <summary>
		/// 身份证
		/// </summary>
		public string Idcard {
			get { return _Idcard; }
			set { _Idcard = value; }
		}
		/// <summary>
		/// 身份证正面照
		/// </summary>
		public string Idcard_img1 {
			get { return _Idcard_img1; }
			set { _Idcard_img1 = value; }
		}
		/// <summary>
		/// 身份证背面照
		/// </summary>
		public string Idcard_img2 {
			get { return _Idcard_img2; }
			set { _Idcard_img2 = value; }
		}
		/// <summary>
		/// 经营执照
		/// </summary>
		public string License_img {
			get { return _License_img; }
			set { _License_img = value; }
		}
		#endregion

		public pifa.DAL.Shopsecurity.SqlUpdateBuild UpdateDiy {
			get { return Shopsecurity.UpdateDiy(this, _Shop_id); }
		}
	}
}

