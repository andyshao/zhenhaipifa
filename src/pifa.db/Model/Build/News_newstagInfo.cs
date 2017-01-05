using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class News_newstagInfo {
		#region fields
		private uint? _News_id;
		private NewsInfo _obj_news;
		private uint? _Newstag_id;
		private NewstagInfo _obj_newstag;
		#endregion

		public News_newstagInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<News_newstag(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_News_id == null ? "null" : _News_id.ToString(), "|",
				_Newstag_id == null ? "null" : _Newstag_id.ToString());
		}
		public static News_newstagInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，News_newstagInfo：" + stringify);
			News_newstagInfo item = new News_newstagInfo();
			if (string.Compare("null", ret[0]) != 0) item.News_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Newstag_id = uint.Parse(ret[1]);
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(News_newstagInfo).GetField("JsonIgnore");
			Dictionary<string, bool> ret = new Dictionary<string, bool>();
			if (field != null) string.Concat(field.GetValue(null)).Split(',').ToList().ForEach(f => {
				if (!string.IsNullOrEmpty(f)) ret[f] = true;
			});
			return ret;
		});
		private static Dictionary<string, bool> __jsonIgnore => __jsonIgnoreLazy.Value;
		public override string ToString() {
			string json = string.Concat(
				__jsonIgnore.ContainsKey("News_id") ? string.Empty : string.Format(", News_id : {0}", News_id == null ? "null" : News_id.ToString()), 
				__jsonIgnore.ContainsKey("Newstag_id") ? string.Empty : string.Format(", Newstag_id : {0}", Newstag_id == null ? "null" : Newstag_id.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("News_id")) ht["News_id"] = News_id;
			if (allField || !__jsonIgnore.ContainsKey("Newstag_id")) ht["Newstag_id"] = Newstag_id;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		[JsonProperty] public uint? News_id {
			get { return _News_id; }
			set {
				if (_News_id != value) _obj_news = null;
				_News_id = value;
			}
		}
		public NewsInfo Obj_news {
			get {
				if (_obj_news == null) _obj_news = News.GetItem(_News_id.Value);
				return _obj_news;
			}
			internal set { _obj_news = value; }
		}
		[JsonProperty] public uint? Newstag_id {
			get { return _Newstag_id; }
			set {
				if (_Newstag_id != value) _obj_newstag = null;
				_Newstag_id = value;
			}
		}
		public NewstagInfo Obj_newstag {
			get {
				if (_obj_newstag == null) _obj_newstag = Newstag.GetItem(_Newstag_id.Value);
				return _obj_newstag;
			}
			internal set { _obj_newstag = value; }
		}
		#endregion

		public pifa.DAL.News_newstag.SqlUpdateBuild UpdateDiy {
			get { return News_newstag.UpdateDiy(this, _News_id.Value, _Newstag_id.Value); }
		}
	}
}

