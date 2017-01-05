using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class NewsdescInfo {
		#region fields
		private uint? _News_id;
		private NewsInfo _obj_news;
		private string _Content;
		#endregion

		public NewsdescInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Newsdesc(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_News_id == null ? "null" : _News_id.ToString(), "|",
				_Content == null ? "null" : _Content.Replace("|", StringifySplit));
		}
		public static NewsdescInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，NewsdescInfo：" + stringify);
			NewsdescInfo item = new NewsdescInfo();
			if (string.Compare("null", ret[0]) != 0) item.News_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Content = ret[1].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(NewsdescInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Content") ? string.Empty : string.Format(", Content : {0}", Content == null ? "null" : string.Format("'{0}'", Content.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("News_id")) ht["News_id"] = News_id;
			if (allField || !__jsonIgnore.ContainsKey("Content")) ht["Content"] = Content;
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
		/// <summary>
		/// 正文
		/// </summary>
		[JsonProperty] public string Content {
			get { return _Content; }
			set { _Content = value; }
		}
		#endregion

		public pifa.DAL.Newsdesc.SqlUpdateBuild UpdateDiy {
			get { return Newsdesc.UpdateDiy(this, _News_id.Value); }
		}
		public NewsdescInfo Save() {
			if (this.News_id != null) {
				Newsdesc.Update(this);
				return this;
			}
			return Newsdesc.Insert(this);
		}
	}
}

