using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class FaqdescInfo {
		#region fields
		private uint? _Faq_id;
		private FaqInfo _obj_faq;
		private string _Content;
		#endregion

		public FaqdescInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Faqdesc(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Faq_id == null ? "null" : _Faq_id.ToString(), "|",
				_Content == null ? "null" : _Content.Replace("|", StringifySplit));
		}
		public static FaqdescInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，FaqdescInfo：" + stringify);
			FaqdescInfo item = new FaqdescInfo();
			if (string.Compare("null", ret[0]) != 0) item.Faq_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Content = ret[1].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(FaqdescInfo).GetField("JsonIgnore");
			Dictionary<string, bool> ret = new Dictionary<string, bool>();
			if (field != null) string.Concat(field.GetValue(null)).Split(',').ToList().ForEach(f => {
				if (!string.IsNullOrEmpty(f)) ret[f] = true;
			});
			return ret;
		});
		private static Dictionary<string, bool> __jsonIgnore => __jsonIgnoreLazy.Value;
		public override string ToString() {
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Faq_id") ? string.Empty : string.Format(", Faq_id : {0}", Faq_id == null ? "null" : Faq_id.ToString()), 
				__jsonIgnore.ContainsKey("Content") ? string.Empty : string.Format(", Content : {0}", Content == null ? "null" : string.Format("'{0}'", Content.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Faq_id")) ht["Faq_id"] = Faq_id;
			if (allField || !__jsonIgnore.ContainsKey("Content")) ht["Content"] = Content;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		[JsonProperty] public uint? Faq_id {
			get { return _Faq_id; }
			set {
				if (_Faq_id != value) _obj_faq = null;
				_Faq_id = value;
			}
		}
		public FaqInfo Obj_faq {
			get {
				if (_obj_faq == null) _obj_faq = Faq.GetItem(_Faq_id.Value);
				return _obj_faq;
			}
			internal set { _obj_faq = value; }
		}
		/// <summary>
		/// 内容
		/// </summary>
		[JsonProperty] public string Content {
			get { return _Content; }
			set { _Content = value; }
		}
		#endregion

		public pifa.DAL.Faqdesc.SqlUpdateBuild UpdateDiy {
			get { return Faqdesc.UpdateDiy(this, _Faq_id.Value); }
		}
		public FaqdescInfo Save() {
			if (this.Faq_id != null) {
				Faqdesc.Update(this);
				return this;
			}
			return Faqdesc.Insert(this);
		}
	}
}

