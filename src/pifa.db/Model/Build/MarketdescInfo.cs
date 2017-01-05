using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class MarketdescInfo {
		#region fields
		private uint? _Market_id;
		private MarketInfo _obj_market;
		private string _Content;
		private string _Url;
		#endregion

		public MarketdescInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Marketdesc(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Market_id == null ? "null" : _Market_id.ToString(), "|",
				_Content == null ? "null" : _Content.Replace("|", StringifySplit), "|",
				_Url == null ? "null" : _Url.Replace("|", StringifySplit));
		}
		public static MarketdescInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 3, StringSplitOptions.None);
			if (ret.Length != 3) throw new Exception("格式不正确，MarketdescInfo：" + stringify);
			MarketdescInfo item = new MarketdescInfo();
			if (string.Compare("null", ret[0]) != 0) item.Market_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Content = ret[1].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[2]) != 0) item.Url = ret[2].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(MarketdescInfo).GetField("JsonIgnore");
			Dictionary<string, bool> ret = new Dictionary<string, bool>();
			if (field != null) string.Concat(field.GetValue(null)).Split(',').ToList().ForEach(f => {
				if (!string.IsNullOrEmpty(f)) ret[f] = true;
			});
			return ret;
		});
		private static Dictionary<string, bool> __jsonIgnore => __jsonIgnoreLazy.Value;
		public override string ToString() {
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Market_id") ? string.Empty : string.Format(", Market_id : {0}", Market_id == null ? "null" : Market_id.ToString()), 
				__jsonIgnore.ContainsKey("Content") ? string.Empty : string.Format(", Content : {0}", Content == null ? "null" : string.Format("'{0}'", Content.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Url") ? string.Empty : string.Format(", Url : {0}", Url == null ? "null" : string.Format("'{0}'", Url.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Market_id")) ht["Market_id"] = Market_id;
			if (allField || !__jsonIgnore.ContainsKey("Content")) ht["Content"] = Content;
			if (allField || !__jsonIgnore.ContainsKey("Url")) ht["Url"] = Url;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		[JsonProperty] public uint? Market_id {
			get { return _Market_id; }
			set {
				if (_Market_id != value) _obj_market = null;
				_Market_id = value;
			}
		}
		public MarketInfo Obj_market {
			get {
				if (_obj_market == null) _obj_market = Market.GetItem(_Market_id.Value);
				return _obj_market;
			}
			internal set { _obj_market = value; }
		}
		/// <summary>
		/// 市场简介
		/// </summary>
		[JsonProperty] public string Content {
			get { return _Content; }
			set { _Content = value; }
		}
		/// <summary>
		/// 自有平台连接
		/// </summary>
		[JsonProperty] public string Url {
			get { return _Url; }
			set { _Url = value; }
		}
		#endregion

		public pifa.DAL.Marketdesc.SqlUpdateBuild UpdateDiy {
			get { return Marketdesc.UpdateDiy(this, _Market_id.Value); }
		}
		public MarketdescInfo Save() {
			if (this.Market_id != null) {
				Marketdesc.Update(this);
				return this;
			}
			return Marketdesc.Insert(this);
		}
	}
}

