using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class MarketdescInfo {
		#region fields
		private uint? _Market_id;
		private MarketInfo _obj_market;
		private string _Content;
		private string _Url;
		#endregion

		public MarketdescInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Marketdesc(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Market_id == null ? "null" : _Market_id.ToString(), "|",
				_Content == null ? "null" : _Content.Replace("|", StringifySplit), "|",
				_Url == null ? "null" : _Url.Replace("|", StringifySplit));
		}
		public MarketdescInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 3, StringSplitOptions.None);
			if (ret.Length != 3) throw new Exception("格式不正确，MarketdescInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Market_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Content = ret[1].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[2]) != 0) _Url = ret[2].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Market_id") ? string.Empty : string.Format(", Market_id : {0}", Market_id == null ? "null" : Market_id.ToString()), 
				__jsonIgnore.ContainsKey("Content") ? string.Empty : string.Format(", Content : {0}", Content == null ? "null" : string.Format("'{0}'", Content.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Url") ? string.Empty : string.Format(", Url : {0}", Url == null ? "null" : string.Format("'{0}'", Url.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Market_id")) ht["Market_id"] = Market_id;
			if (!__jsonIgnore.ContainsKey("Content")) ht["Content"] = Content;
			if (!__jsonIgnore.ContainsKey("Url")) ht["Url"] = Url;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(MarketdescInfo).GetField("JsonIgnore");
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
			MarketdescInfo item = obj as MarketdescInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(MarketdescInfo op1, MarketdescInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(MarketdescInfo op1, MarketdescInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public uint? Market_id {
			get { return _Market_id; }
			set {
				if (_Market_id != value) _obj_market = null;
				_Market_id = value;
			}
		}
		public MarketInfo Obj_market {
			get {
				if (_obj_market == null) _obj_market = Market.GetItem(_Market_id);
				return _obj_market;
			}
			internal set { _obj_market = value; }
		}
		/// <summary>
		/// 市场简介
		/// </summary>
		public string Content {
			get { return _Content; }
			set { _Content = value; }
		}
		/// <summary>
		/// 自有平台连接
		/// </summary>
		public string Url {
			get { return _Url; }
			set { _Url = value; }
		}
		#endregion

		public pifa.DAL.Marketdesc.SqlUpdateBuild UpdateDiy {
			get { return Marketdesc.UpdateDiy(this, _Market_id); }
		}
	}
}

