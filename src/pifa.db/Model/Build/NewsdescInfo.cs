using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class NewsdescInfo {
		#region fields
		private uint? _News_id;
		private NewsInfo _obj_news;
		private string _Content;
		#endregion

		public NewsdescInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Newsdesc(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_News_id == null ? "null" : _News_id.ToString(), "|",
				_Content == null ? "null" : _Content.Replace("|", StringifySplit));
		}
		public NewsdescInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，NewsdescInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _News_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Content = ret[1].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("News_id") ? string.Empty : string.Format(", News_id : {0}", News_id == null ? "null" : News_id.ToString()), 
				__jsonIgnore.ContainsKey("Content") ? string.Empty : string.Format(", Content : {0}", Content == null ? "null" : string.Format("'{0}'", Content.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("News_id")) ht["News_id"] = News_id;
			if (!__jsonIgnore.ContainsKey("Content")) ht["Content"] = Content;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(NewsdescInfo).GetField("JsonIgnore");
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
			NewsdescInfo item = obj as NewsdescInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(NewsdescInfo op1, NewsdescInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(NewsdescInfo op1, NewsdescInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public uint? News_id {
			get { return _News_id; }
			set {
				if (_News_id != value) _obj_news = null;
				_News_id = value;
			}
		}
		public NewsInfo Obj_news {
			get {
				if (_obj_news == null) _obj_news = News.GetItem(_News_id);
				return _obj_news;
			}
			internal set { _obj_news = value; }
		}
		/// <summary>
		/// 正文
		/// </summary>
		public string Content {
			get { return _Content; }
			set { _Content = value; }
		}
		#endregion

		public pifa.DAL.Newsdesc.SqlUpdateBuild UpdateDiy {
			get { return Newsdesc.UpdateDiy(this, _News_id); }
		}
	}
}

